using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LaserGRBL.DeviceProfiles
{
	internal interface ICardFiles
	{
		byte[] ReadManifest();
		string ReadJobHash(string name);
		void WriteVerified(string name, byte[] content, string previousHash);
		void DeleteVerifiedJob(string name, string expectedHash);
	}

	internal static class ManagedCardJobs
	{
		internal const string ManifestName = "LGRBL.SHA";
		internal const int MaxManifestLength = 4096;
		private const string ManifestHeader = "LaserGRBL managed card files v1";
		private static readonly Regex Extension = new Regex(@"\A[A-Za-z0-9]{1,12}\z", RegexOptions.CultureInvariant);
		private static readonly Regex Record = new Regex(@"\A(LASERGRB\.[A-Za-z0-9]{1,12}) ([0-9A-F]{64})\z", RegexOptions.CultureInvariant);

		internal static string JobName(string extension)
		{
			if (extension != null && extension.StartsWith(".", StringComparison.Ordinal))
				extension = extension.Substring(1);
			if (extension == null || !Extension.IsMatch(extension))
				throw new ArgumentException("Kaardifaili laiendis peab olema 1–12 ASCII tähte või numbrit.", "extension");
			return "LASERGRB." + extension.ToLowerInvariant();
		}

		internal static void Replace(ICardFiles files, string name, byte[] program)
		{
			if (program == null || program.Length == 0)
				throw new ArgumentException("Kontrollitud tööfail on tühi.", "program");
			if (!Record.IsMatch(name + " " + new string('0', 64)))
				throw new ArgumentException("Kaardi tööfaili nimi on vigane.", "name");

			byte[] previousManifest = files.ReadManifest();
			var owned = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			foreach (var group in ReadRecords(previousManifest).GroupBy(entry => entry.Key, StringComparer.OrdinalIgnoreCase))
			{
				string onCard = files.ReadJobHash(group.Key);
				if (onCard == null)
					continue;
				if (!group.Any(entry => onCard == entry.Value))
					throw new IOException("Faili " + group.Key + " muudeti väljaspool LaserGRBL-i. Tõsta see enne uue töö salvestamist kaardilt mujale.");
				owned.Add(group.Key, onCard);
			}

			string previousJobHash;
			if (!owned.TryGetValue(name, out previousJobHash) && files.ReadJobHash(name) != null)
				throw new IOException("Kaardil on samanimeline fail " + name + ". Tõsta see enne uue töö salvestamist kaardilt mujale.");

			// The journal accepts both the old and new hashes until the new job has been read back.
			// An interrupted replacement can therefore be retried without treating either file as foreign.
			string newJobHash = Hash(program);
			var next = new KeyValuePair<string, string>(name, newJobHash);
			byte[] journal = WriteRecords(owned.Concat(new[] { next }));
			files.WriteVerified(ManifestName, journal, previousManifest == null ? null : Hash(previousManifest));
			files.WriteVerified(name, program, previousJobHash);

			foreach (var entry in owned)
				if (!string.Equals(entry.Key, name, StringComparison.OrdinalIgnoreCase))
					files.DeleteVerifiedJob(entry.Key, entry.Value);

			files.WriteVerified(ManifestName, WriteRecords(new[] { next }), Hash(journal));
		}

		private static IEnumerable<KeyValuePair<string, string>> ReadRecords(byte[] content)
		{
			if (content == null)
				yield break;
			string[] lines = Encoding.ASCII.GetString(content).Split('\n');
			if (content.Length > MaxManifestLength || lines.Length < 3 || lines[0] != ManifestHeader || lines[lines.Length - 1] != "")
				throw new IOException("LaserGRBL ei tuvasta kaardi tööde nimekirja " + ManifestName + ".");
			for (int i = 1; i < lines.Length - 1; i++)
			{
				Match match = Record.Match(lines[i]);
				if (!match.Success)
					throw new IOException("Kaardi tööde nimekirjas " + ManifestName + " on vigane kirje.");
				yield return new KeyValuePair<string, string>(match.Groups[1].Value, match.Groups[2].Value);
			}
		}

		private static byte[] WriteRecords(IEnumerable<KeyValuePair<string, string>> entries)
		{
			var text = new StringBuilder(ManifestHeader).Append('\n');
			foreach (var entry in entries.Distinct())
				text.Append(entry.Key).Append(' ').Append(entry.Value).Append('\n');
			if (text.Length > MaxManifestLength)
				throw new IOException("Kaardi LaserGRBL-i tööde nimekirjas on liiga palju kirjeid.");
			return Encoding.ASCII.GetBytes(text.ToString());
		}

		internal static string Hash(byte[] content)
		{
			using (SHA256 sha = SHA256.Create())
				return BitConverter.ToString(sha.ComputeHash(content)).Replace("-", "");
		}

		internal static string Hash(Stream stream)
		{
			using (SHA256 sha = SHA256.Create())
				return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-", "");
		}
	}
}
