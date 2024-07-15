using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Tools
{
	public static class Protector
	{
		/// <summary>
		/// Specifies the data protection scope of the DPAPI.
		/// </summary>
		private const DataProtectionScope Scope = DataProtectionScope.CurrentUser;

		public static string Encrypt(string text)
		{
			if (text == null)
				return null;
			if (text == "")
				return "";

			//encrypt data
			var data = Encoding.Unicode.GetBytes(text);
			byte[] encrypted = ProtectedData.Protect(data, null, Scope);

			//return as base64 string
			return Convert.ToBase64String(encrypted);
		}

		public static string Decrypt(string cipher, string defval)
		{
			try
			{
				if (cipher == null)
					return null;

				if (cipher == "")
					return "";

				//parse base64 string
				byte[] data = Convert.FromBase64String(cipher);

				//decrypt data
				byte[] decrypted = ProtectedData.Unprotect(data, null, Scope);
				return Encoding.Unicode.GetString(decrypted);
			}
			catch { return defval; }
		}

	}
}
