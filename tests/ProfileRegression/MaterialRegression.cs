using System;
using System.IO;
using System.Linq;
using LaserGRBL.DeviceProfiles;

internal static class MaterialRegression
{
	internal static void Run()
	{
		string folder = Path.Combine(Path.GetTempPath(), "lasergrbl-material-test-" + Guid.NewGuid().ToString("N"));
		try
		{
			VerifyCv01Presets(folder);
			VerifyUserCatalog(folder);
		}
		finally
		{
			if (Directory.Exists(folder))
				Directory.Delete(folder, true);
		}
	}

	private static void VerifyCv01Presets(string folder)
	{
		var cv01 = MaterialProfiles.Load(folder, "Creality CV-01", 255);
		MaterialProfile decorative = cv01.Single(profile => profile.Name.StartsWith("Dekoratiiv", StringComparison.Ordinal));
		MaterialProfile kraft = cv01.Single(profile => profile.Name.StartsWith("Jõu", StringComparison.Ordinal));
		Check(cv01.Count == 2 && cv01.All(profile => profile.BuiltIn && profile.Confirmed), "CV-01 preset scope");
		Check(OperationMatches(decorative.Engrave, 153, 1800, 1), "decorative paper engrave");
		Check(OperationMatches(decorative.Cut, 153, 540, 1), "decorative paper cut");
		Check(OperationMatches(kraft.Cut, 255, 180, 1), "kraft paper cut");
		Check(kraft.Engrave == null && kraft.Score == null && decorative.Score == null, "missing options stay unset");
		Check(MaterialProfiles.GetWarning(decorative, MaterialOperationKind.Engrave) == null, "tested operation status");
		Check(MaterialProfiles.GetWarning(decorative, MaterialOperationKind.Score) != null, "missing operation warning");
		Check(MaterialProfiles.GetWarning(null) != null, "unknown material warning");
		Check(MaterialProfiles.Load(folder, "Creality CV-01 Pro", 255).Count == 0 &&
			MaterialProfiles.Load(folder, "Creality CV-01", 1000).Count == 0, "presets stay limited to the 255-PWM CV-01");
	}

	private static void VerifyUserCatalog(string folder)
	{
		MaterialProfile paper = CreateMaterial("Workshop paper", "Custom device", 1000);
		paper.Engrave = new MaterialOperation { Power = 300, Feed = 900.125m, Passes = 2, Confirmed = true };
		paper.Score = new MaterialOperation { Power = 100, Feed = 800, Passes = 1, Confirmed = false };
		MaterialProfiles.Save(folder, paper, false);
		VerifyDuplicateRejected(folder, CreateMaterial("workshop paper", "CUSTOM DEVICE", 1000));

		MaterialProfile unsuitable = CreateMaterial("PVC sheet", "Other device", 255);
		MaterialProfiles.Save(folder, unsuitable, false);
		Check(unsuitable.IsUnsuitable && MaterialProfiles.GetWarning(unsuitable) != null, "PVC is blocked");

		MaterialProfile reloaded = MaterialProfiles.Load(folder, "CUSTOM DEVICE", 1000).Single();
		Check(reloaded.Id == paper.Id && OperationMatches(reloaded.Engrave, 300, 900.125m, 2), "saved operation round trip");
		Check(reloaded.Score != null && !reloaded.Score.Confirmed && MaterialProfiles.GetWarning(reloaded, MaterialOperationKind.Score) != null,
			"verification is specific to an operation");
		Check(MaterialProfiles.Load(folder, "Custom device", 255).Count == 0, "PWM scale scopes saved materials");

		reloaded.Name = "New paper name";
		MaterialProfiles.Save(folder, reloaded, true);
		Check(MaterialProfiles.Load(folder, "Custom device", 1000).Single().Name == "New paper name", "saved edit");
		MaterialProfiles.Remove(folder, reloaded);
		Check(MaterialProfiles.Load(folder, "Custom device", 1000).Count == 0 &&
			MaterialProfiles.Load(folder, "Other device", 255).Single().Id == unsuitable.Id, "removal preserves another device's material");
	}

	private static MaterialProfile CreateMaterial(string name, string deviceName, int maxPower)
	{
		return new MaterialProfile
		{
			Id = Guid.NewGuid(), Name = name, DeviceName = deviceName, DeviceMaxPower = maxPower, Status = MaterialStatus.Identified
		};
	}

	private static void VerifyDuplicateRejected(string folder, MaterialProfile duplicate)
	{
		try
		{
			MaterialProfiles.Save(folder, duplicate, false);
		}
		catch (InvalidDataException)
		{
			return;
		}
		throw new InvalidOperationException("Duplicate material name was accepted for the same device.");
	}

	private static bool OperationMatches(MaterialOperation operation, int power, decimal feed, int passes)
	{
		return operation != null && operation.Confirmed && operation.Power == power && operation.Feed == feed && operation.Passes == passes;
	}

	private static void Check(bool condition, string scenario)
	{
		if (!condition)
			throw new InvalidOperationException("Material regression failed for " + scenario + ".");
	}
}
