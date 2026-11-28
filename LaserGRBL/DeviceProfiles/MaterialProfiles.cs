using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;

namespace LaserGRBL.DeviceProfiles
{
	internal enum MaterialOperationKind { Engrave, Score, Cut }
	internal enum MaterialStatus { Unverified, Identified, Unsuitable }

	[DataContract]
	internal sealed class MaterialOperation
	{
		[DataMember(Name = "power", IsRequired = true)]
		internal int Power { get; set; }
		[DataMember(Name = "feedMmPerMinute", IsRequired = true)]
		internal decimal Feed { get; set; }
		[DataMember(Name = "passes", IsRequired = true)]
		internal int Passes { get; set; }
		[DataMember(Name = "tested")]
		internal bool Confirmed { get; set; }

		internal void Validate(int maxPower)
		{
			if (Power < 1 || Power > maxPower || Feed <= 0 || Feed > 100000 || Passes < 1 || Passes > 99)
				throw new InvalidDataException("Määra võimsus seadme lubatud vahemikus, kiirus üle 0 ja kuni 100000 mm/min ning 1–99 läbimist.");
		}
	}

	[DataContract]
	internal sealed class MaterialProfile
	{
		[DataMember(Name = "id", IsRequired = true)]
		internal Guid Id { get; set; }
		[DataMember(Name = "device", IsRequired = true)]
		internal string DeviceName { get; set; }
		[DataMember(Name = "deviceMaxPower", IsRequired = true)]
		internal int DeviceMaxPower { get; set; }
		[DataMember(Name = "name", IsRequired = true)]
		internal string Name { get; set; }
		[DataMember(Name = "materialStatus", IsRequired = true)]
		internal MaterialStatus Status { get; set; }
		[DataMember(Name = "notes", EmitDefaultValue = false)]
		internal string Notes { get; set; }
		[DataMember(Name = "engrave", EmitDefaultValue = false)]
		internal MaterialOperation Engrave { get; set; }
		[DataMember(Name = "score", EmitDefaultValue = false)]
		internal MaterialOperation Score { get; set; }
		[DataMember(Name = "cut", EmitDefaultValue = false)]
		internal MaterialOperation Cut { get; set; }

		internal bool BuiltIn { get; set; }
		internal bool IsUnsuitable => Status == MaterialStatus.Unsuitable || MaterialProfiles.IsUnsuitableName(Name);
		internal bool Confirmed => Status == MaterialStatus.Identified && !IsUnsuitable &&
			new[] { Engrave, Score, Cut }.Any(operation => operation != null && operation.Confirmed);

		internal MaterialOperation GetOperation(MaterialOperationKind kind)
		{
			switch (kind)
			{
				case MaterialOperationKind.Engrave: return Engrave;
				case MaterialOperationKind.Score: return Score;
				case MaterialOperationKind.Cut: return Cut;
				default: throw new ArgumentOutOfRangeException(nameof(kind));
			}
		}

		internal void Validate()
		{
			if (Id == Guid.Empty || string.IsNullOrWhiteSpace(DeviceName) || DeviceMaxPower < 1 ||
				string.IsNullOrWhiteSpace(Name) || Name.Length > 80 || Name.Any(char.IsControl) ||
				(Notes != null && Notes.Length > 600) || !Enum.IsDefined(typeof(MaterialStatus), Status))
				throw new InvalidDataException("Sisesta kehtiv materjali nimi ja vali koostise olek.");
			Engrave?.Validate(DeviceMaxPower);
			Score?.Validate(DeviceMaxPower);
			Cut?.Validate(DeviceMaxPower);
		}
	}

	internal static class MaterialProfiles
	{
		[DataContract]
		private sealed class MaterialCatalog
		{
			[DataMember(Name = "version", IsRequired = true)]
			internal int Version { get; set; }
			[DataMember(Name = "materials", IsRequired = true)]
			internal List<MaterialProfile> Materials { get; set; }
		}

		private static readonly Regex UnsuitableName = new Regex(
			@"\b(?:PVC|PVDC|vinyl|vinüül|PTFE|Teflon|chlorinated|klooritud|halogenated|halogeenitud|ABS|fiberglass|glass[- ]fibre|klaaskiud|polyvinyl[- ]chloride|polüvinüülkloriid)\b",
			RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		internal static bool IsUnsuitableName(string name) => name != null && UnsuitableName.IsMatch(name);

		internal static string GetWarning(MaterialProfile profile, MaterialOperationKind? operation = null)
		{
			if (profile != null && profile.IsUnsuitable)
				return "See materjal ei sobi laseriga töötlemiseks.";
			if (profile == null || profile.Status == MaterialStatus.Unverified)
				return "Enne laseriga töötlemist kontrolli materjali, pinnakatte ja liimi koostist.";

			MaterialOperation settings = operation.HasValue ? profile.GetOperation(operation.Value) : null;
			if (operation.HasValue && (settings == null || !settings.Confirmed))
				return "Selle toimingu seadeid pole valitud materjalil katsetatud.";
			if (!operation.HasValue && !profile.Confirmed)
				return "Selle materjali seadeid pole valitud seadmel katsetatud.";
			return null;
		}

		internal static IList<MaterialProfile> Load(string folder, string deviceName, int maxPower)
		{
			List<MaterialProfile> profiles = Read(CatalogPath(folder)).Materials
				.Where(profile => MatchesDevice(profile, deviceName, maxPower)).ToList();
			if (maxPower == 255 && string.Equals(deviceName, "Creality CV-01", StringComparison.OrdinalIgnoreCase))
				profiles.AddRange(CreateCv01Materials());
			return profiles.OrderBy(profile => profile.Name, StringComparer.CurrentCultureIgnoreCase).ToList();
		}

		internal static void Save(string folder, MaterialProfile profile, bool replace)
		{
			if (profile.BuiltIn)
				throw new InvalidOperationException("Sisseehitatud materjaliprofiili saab vaadata ja kasutada.");
			profile.Validate();
			Update(folder, profiles =>
			{
				int index = profiles.FindIndex(existing => existing.Id == profile.Id);
				if (replace != (index >= 0))
					throw new InvalidOperationException("Materjaliprofiil muutus teises aknas. Ava materjaliprofiilid uuesti ja proovi veel kord.");
				if (profiles.Any(existing => existing.Id != profile.Id && MatchesDevice(existing, profile.DeviceName, profile.DeviceMaxPower) &&
					string.Equals(existing.Name, profile.Name, StringComparison.OrdinalIgnoreCase)))
					throw new InvalidDataException("Selle nimega materjaliprofiil on seadmel juba olemas.");
				if (replace)
					profiles[index] = profile;
				else
					profiles.Add(profile);
			});
		}

		internal static void Remove(string folder, MaterialProfile profile)
		{
			if (profile.BuiltIn)
				throw new InvalidOperationException("Sisseehitatud materjaliprofiili saab vaadata ja kasutada.");
			Update(folder, profiles => profiles.RemoveAll(existing => existing.Id == profile.Id));
		}

		private static bool MatchesDevice(MaterialProfile profile, string deviceName, int maxPower)
		{
			return profile.DeviceMaxPower == maxPower && string.Equals(profile.DeviceName, deviceName, StringComparison.OrdinalIgnoreCase);
		}

		private static string CatalogPath(string folder) => Path.Combine(folder, "materials.json");

		private static MaterialCatalog Read(string path)
		{
			if (!File.Exists(path))
				return new MaterialCatalog { Version = 1, Materials = new List<MaterialProfile>() };

			using (FileStream source = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete))
			{
				var catalog = (MaterialCatalog)new DataContractJsonSerializer(typeof(MaterialCatalog)).ReadObject(source);
				if (catalog == null || catalog.Version != 1 || catalog.Materials == null ||
					catalog.Materials.Any(profile => profile == null) ||
					catalog.Materials.Select(profile => profile.Id).Distinct().Count() != catalog.Materials.Count)
					throw new InvalidDataException("Materjaliprofiilide fail on vigane või tundmatu versiooniga.");
				foreach (MaterialProfile profile in catalog.Materials)
					profile.Validate();
				return catalog;
			}
		}

		private static void Update(string folder, Action<List<MaterialProfile>> change)
		{
			Directory.CreateDirectory(folder);
			string path = CatalogPath(folder);
			using (new FileStream(path + ".lock", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
			{
				MaterialCatalog catalog = Read(path);
				change(catalog.Materials);
				string staging = Path.Combine(folder, Guid.NewGuid().ToString("N") + ".tmp");
				try
				{
					using (FileStream target = new FileStream(staging, FileMode.CreateNew, FileAccess.Write, FileShare.None))
					{
						new DataContractJsonSerializer(typeof(MaterialCatalog)).WriteObject(target, catalog);
						target.Flush(true);
					}
					if (File.Exists(path))
						File.Replace(staging, path, null);
					else
						File.Move(staging, path);
				}
				finally
				{
					if (File.Exists(staging))
						File.Delete(staging);
				}
			}
		}

		private static IEnumerable<MaterialProfile> CreateCv01Materials()
		{
			const string cv01 = "Creality CV-01";
			yield return new MaterialProfile
			{
				Id = new Guid("b6841257-6b4a-4045-b15f-faee32491726"),
				BuiltIn = true,
				DeviceName = cv01,
				DeviceMaxPower = 255,
				Name = "Dekoratiivpaber (katsetatud näidis)",
				Status = MaterialStatus.Identified,
				Notes = "Katsetatud 1,6 W CV-01-ga. Graveerimisel kasutati Floyd–Steinbergi hajutust sammuga 0,20 mm. Kontrolli oma paberi koostist ja jälgi tööd.",
				Engrave = new MaterialOperation { Power = 153, Feed = 1800, Passes = 1, Confirmed = true },
				Cut = new MaterialOperation { Power = 153, Feed = 540, Passes = 1, Confirmed = true }
			};
			yield return new MaterialProfile
			{
				Id = new Guid("5fc69de3-daad-456e-adaa-6ee4ec5cb469"),
				BuiltIn = true,
				DeviceName = cv01,
				DeviceMaxPower = 255,
				Name = "Jõupaber (katsetatud näidis)",
				Status = MaterialStatus.Identified,
				Notes = "Katsetatud 1,6 W CV-01-ga. Paber võib süttida. Kontrolli oma paberi koostist ja jälgi tööd.",
				Cut = new MaterialOperation { Power = 255, Feed = 180, Passes = 1, Confirmed = true }
			};
		}
	}
}
