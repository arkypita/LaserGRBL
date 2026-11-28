using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LaserGRBL.DeviceProfiles;

internal static class CardRegression
{
	internal static void Run()
	{
		string gcode = ManagedCardJobs.JobName(".gcode");
		string nc = ManagedCardJobs.JobName("NC");
		byte[] first = Encoding.ASCII.GetBytes("M107\nG0 X1\nM107\n");
		byte[] second = Encoding.ASCII.GetBytes("M107\nG0 X2\nM107\n");
		var card = new CardFiles();
		card.Files["manual.gcode"] = first;
		card.Files["firmware.bin"] = first;
		ManagedCardJobs.Replace(card, gcode, first);
		Assert(card.ReadJobHash(gcode) == ManagedCardJobs.Hash(first), "first export");
		ManagedCardJobs.Replace(card, gcode, second);
		Assert(card.ReadJobHash(gcode) == ManagedCardJobs.Hash(second), "same name replacement");
		ManagedCardJobs.Replace(card, nc, first);
		Assert(!card.Files.ContainsKey(gcode) && card.Files.ContainsKey(nc), "replace managed job with another extension");
		Assert(card.Files["manual.gcode"] == first && card.Files["firmware.bin"] == first, "preserve unrelated files");

		card.Files[gcode] = first;
		AssertFails(() => ManagedCardJobs.Replace(card, gcode, second), "reject unknown file with selected name");
		Assert(card.Files[gcode] == first && card.Files[nc] == first, "keep unknown file and previous job");
		card.Files.Remove(gcode);
		card.Files[nc] = second;
		AssertFails(() => ManagedCardJobs.Replace(card, gcode, first), "reject modified former job");
		Assert(card.Files[nc] == second, "keep modified former job");
		card.Files[nc] = first;

		byte[] ownManifest = card.ReadManifest();
		card.Files[ManagedCardJobs.ManifestName] = second;
		AssertFails(() => ManagedCardJobs.Replace(card, gcode, first), "reject foreign manifest");
		Assert(card.Files[ManagedCardJobs.ManifestName] == second && card.Files[nc] == first, "keep foreign manifest and previous job");
		card.Files[ManagedCardJobs.ManifestName] = ownManifest;

		card.FailWriteTo = nc;
		AssertFails(() => ManagedCardJobs.Replace(card, nc, second), "report interrupted write");
		Assert(card.Files[nc] == first, "keep previous job when incoming write fails");
		card.FailWriteTo = null;
		ManagedCardJobs.Replace(card, nc, second);
		Assert(card.Files[nc] == second, "recover interrupted same-name replacement");

		card.FailDelete = true;
		AssertFails(() => ManagedCardJobs.Replace(card, gcode, first), "report interrupted retirement");
		Assert(card.Files[nc] == second && card.Files[gcode] == first, "new verified job exists before retiring old jobs");
		card.FailDelete = false;
		ManagedCardJobs.Replace(card, gcode, first);
		Assert(!card.Files.ContainsKey(nc) && card.Files[gcode] == first, "recover interrupted retirement");
	}

	private static void Assert(bool condition, string message)
	{
		if (!condition)
			throw new InvalidOperationException("Card regression failure: " + message);
	}

	private static void AssertFails(Action action, string message)
	{
		try { action(); }
		catch (IOException) { return; }
		throw new InvalidOperationException("Card regression failure: " + message);
	}

	private sealed class CardFiles : ICardFiles
	{
		internal readonly Dictionary<string, byte[]> Files = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);
		internal string FailWriteTo;
		internal bool FailDelete;

		public byte[] ReadManifest()
		{
			byte[] manifest;
			return Files.TryGetValue(ManagedCardJobs.ManifestName, out manifest) ? manifest : null;
		}

		public string ReadJobHash(string name)
		{
			byte[] contents;
			return Files.TryGetValue(name, out contents) ? ManagedCardJobs.Hash(contents) : null;
		}

		public void WriteVerified(string name, byte[] content, string previousHash)
		{
			if (name == FailWriteTo || ReadJobHash(name) != previousHash)
				throw new IOException("Simulated card write failure");
			Files[name] = content;
		}

		public void DeleteVerifiedJob(string name, string expectedHash)
		{
			if (FailDelete || ReadJobHash(name) != expectedHash)
				throw new IOException("Simulated card retirement failure");
			Files.Remove(name);
		}
	}
}
