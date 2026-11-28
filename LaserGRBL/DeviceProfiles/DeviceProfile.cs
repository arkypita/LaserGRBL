using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;

namespace LaserGRBL.DeviceProfiles
{
	[DataContract]
	internal sealed class DeviceProfile
	{
		[DataMember(IsRequired = true), Browsable(false)]
		public int Version { get; set; } = 1;

		[DataMember(IsRequired = true), Category("Seade")]
		public string Name { get; set; }
		[DataMember(IsRequired = true), Category("Tööala (mm)")]
		public decimal Width { get; set; }
		[DataMember(IsRequired = true), Category("Tööala (mm)")]
		public decimal Height { get; set; }
		[DataMember, Category("Tööala (mm)")]
		public decimal OriginX { get; set; }
		[DataMember, Category("Tööala (mm)")]
		public decimal OriginY { get; set; }
		[DataMember, Category("Tööala (mm)")]
		public bool ReverseX { get; set; }
		[DataMember, Category("Tööala (mm)")]
		public bool ReverseY { get; set; }
		[DataMember, Category("Tööala (mm)")]
		public decimal Overscan { get; set; }

		[DataMember(IsRequired = true), Category("Kiirus (mm/min)")]
		public decimal WorkFeed { get; set; }
		[DataMember(IsRequired = true), Category("Kiirus (mm/min)")]
		public decimal RapidFeed { get; set; }
		[DataMember(IsRequired = true), Category("Võimsus")]
		public int MaxPower { get; set; }
		[DataMember, Category("Võimsus")]
		public int MinPower { get; set; }

		[DataMember, Category("Käsud"), Description("Eralda käsud märgiga |. Lubatud G21, G90, G17, G94, G28 ja M400.")]
		public string StartCommands { get; set; }
		[DataMember, Category("Käsud"), Description("Eralda käsud märgiga |. Lubatud G21, G90, G17, G94, G28 ja M400.")]
		public string EndCommands { get; set; }
		[DataMember(IsRequired = true), Category("Käsud"), Description("Võimsuse kohatäide on {power}.")]
		public string LaserOn { get; set; }
		[DataMember(IsRequired = true), Category("Käsud")]
		public string LaserOff { get; set; }
		[DataMember, Category("Käsud"), Description("Valikuline M400. Kontrolli seadme püsivara tuge ja ajastust.")]
		public string SynchronizeCommand { get; set; }
		[DataMember, Category("Käsud")]
		public string PauseCommand { get; set; }

		[DataMember, Category("Raam")]
		public int PreviewPower { get; set; }
		[DataMember, Category("Raam")]
		public int PreviewPasses { get; set; }
		[DataMember, Category("Raam")]
		public bool FrameByDefault { get; set; }
		[DataMember, Category("Käivitus"), Description("Lisab faili enne tööd pausi. Vastasel korral algab töö pärast faili käsitsi käivitamist kohe.")]
		public bool PauseByDefault { get; set; }
		[DataMember(IsRequired = true), Category("Fail")]
		public string Extension { get; set; }
		[DataMember(IsRequired = true), Category("Fail")]
		public string LineEndings { get; set; }

		internal static DeviceProfile CreateCv01()
		{
			return new DeviceProfile
			{
				Name = "Creality CV-01",
				Width = 170, Height = 200,
				RapidFeed = 7200, WorkFeed = 1800, Overscan = 4,
				MaxPower = 255, MinPower = 0,
				StartCommands = "G21|G90|G28 X0 Y0",
				EndCommands = "G28 X0 Y0",
				LaserOn = "M106 S{power}", LaserOff = "M107",
				PauseCommand = "M0", PreviewPower = 5, PreviewPasses = 2,
				Extension = ".gcode", LineEndings = "CRLF"
			};
		}

		internal DeviceProfile Copy() { return (DeviceProfile)MemberwiseClone(); }

		internal void Validate()
		{
			if (Version != 1 || string.IsNullOrWhiteSpace(Name) || Name.Length > 100 || Name.Contains("\n") || Name.Contains("\r"))
				throw new InvalidDataException("Profiili versioon või nimi on vigane.");
			if (Width <= 0 || Height <= 0 || Width > 10000 || Height > 10000 ||
				OriginX < 0 || OriginX > Width || OriginY < 0 || OriginY > Height ||
				Overscan < 0 || Overscan >= Width / 2)
				throw new InvalidDataException("Kontrolli tööala, nullpunkti ja kiirendusvaru.");
			if (WorkFeed <= 0 || RapidFeed < WorkFeed || RapidFeed > 100000)
				throw new InvalidDataException("Töökiirus peab jääma vahemikku 0 kuni kiirliikumise kiirus.");
			if (MinPower < 0 || MaxPower <= 0 || MinPower > MaxPower || MaxPower > 100000 ||
				PreviewPower < 0 || PreviewPower > MaxPower || PreviewPasses < 0 || PreviewPasses > 10 ||
				(PreviewPasses > 0 && PreviewPower == 0))
				throw new InvalidDataException("Kontrolli töö- ja raamimisvõimsuse piirväärtusi.");
			if (!Regex.IsMatch(Extension ?? "", @"^\.[A-Za-z0-9]{1,10}\z") ||
				(LineEndings != "CRLF" && LineEndings != "LF"))
				throw new InvalidDataException("Määra faililaiend ja reavahetus LF või CRLF.");

			bool grbl = Regex.IsMatch(LaserOn ?? "", @"^M[34] S\{power\}\z") && LaserOff == "M5";
			Match fanOn = Regex.Match(LaserOn ?? "", @"^M106(?: P([0-9]))? S\{power\}\z");
			Match fanOff = Regex.Match(LaserOff ?? "", @"^(?:M107(?: P([0-9]))?|M106(?: P([0-9]))? S0)\z");
			string fanOffIndex = fanOff.Groups[1].Success ? fanOff.Groups[1].Value : fanOff.Groups[2].Value;
			bool marlin = fanOn.Success && fanOff.Success &&
				(fanOn.Groups[1].Success ? fanOn.Groups[1].Value : "0") ==
				(fanOffIndex.Length > 0 ? fanOffIndex : "0");
			if (!grbl && !marlin)
				throw new InvalidDataException("Kasuta vastavat laserikäskude paari M3/M4 ja M5 või M106 ja M107/M106 S0.");
			if (marlin && MaxPower > 255)
				throw new InvalidDataException("M106 võimsusvahemiku ülempiir on 255.");
			if ((!string.IsNullOrEmpty(SynchronizeCommand) && SynchronizeCommand != "M400") ||
				(!string.IsNullOrEmpty(PauseCommand) && PauseCommand != "M0" && PauseCommand != "M1"))
				throw new InvalidDataException("Lubatud sünkroonimine on M400 ja paus M0 või M1.");
			if (grbl && (!string.IsNullOrEmpty(SynchronizeCommand) || !string.IsNullOrEmpty(PauseCommand)))
				throw new InvalidDataException("GRBL-profiilis jäta Marlini sünkroonimine ja paus tühjaks.");
			if ((FrameByDefault && PreviewPasses == 0) || (PauseByDefault && string.IsNullOrEmpty(PauseCommand)))
				throw new InvalidDataException("Vaikimisi raamimiseks või pausiks määra vastavad käsud ja väärtused.");

			foreach (string command in Commands(StartCommands))
				ValidateSetup(command, grbl);
			foreach (string command in Commands(EndCommands))
				ValidateSetup(command, grbl);
		}

		private static void ValidateSetup(string command, bool grbl)
		{
			if (command == "G21" || command == "G90" || command == "G17" || command == "G94")
				return;
			if (!grbl && (command == "M400" || Regex.IsMatch(command, @"^G28(?: X0(?: Y0)?| Y0)\z")))
				return;
			if (grbl && command == "$H")
				return;
			throw new InvalidDataException("Algus- või lõpukäsk pole selle profiili jaoks lubatud: " + command);
		}

		internal static string[] Commands(string commands)
		{
			if (string.IsNullOrWhiteSpace(commands))
				return new string[0];
			if (commands.Length > 4000)
				throw new InvalidDataException("Algus- või lõpukäskude loend on liiga pikk.");
			return Array.ConvertAll(commands.Split(new[] { '|', '\r', '\n' },
				StringSplitOptions.RemoveEmptyEntries), command => command.Trim());
		}

		internal static DeviceProfile Read(string path)
		{
			using (var stream = File.OpenRead(path))
			{
				if (stream.Length > 65536)
					throw new InvalidDataException("Profiili JSON-fail on liiga suur.");
				var profile = (DeviceProfile)new DataContractJsonSerializer(typeof(DeviceProfile)).ReadObject(stream);
				if (profile == null)
					throw new InvalidDataException("Profiili JSON-fail on tühi.");
				profile.Validate();
				return profile;
			}
		}

		internal void Write(string path)
		{
			Validate();
			using (var stream = new MemoryStream())
			{
				new DataContractJsonSerializer(typeof(DeviceProfile)).WriteObject(stream, this);
				ProfileFile.Write(path, stream.ToArray());
			}
		}
	}

	internal static class ProfileFile
	{
		internal static void Write(string path, byte[] bytes)
		{
			string destination = Path.GetFullPath(path);
			string staging = Path.Combine(Path.GetDirectoryName(destination), Guid.NewGuid().ToString("N") + ".tmp");
			try
			{
				using (var stream = new FileStream(staging, FileMode.CreateNew, FileAccess.Write, FileShare.None))
				{
					stream.Write(bytes, 0, bytes.Length);
					stream.Flush(true);
				}
				if (File.Exists(destination))
					File.Replace(staging, destination, null);
				else
					File.Move(staging, destination);
			}
			finally
			{
				File.Delete(staging);
			}
		}
	}
}
