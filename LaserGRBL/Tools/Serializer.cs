//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

using System.IO;
using System.Security.AccessControl;

namespace Tools
{

	public class Serializer
	{
		//private static int counter = 0;
		private const string DataStartString = "<!-- OBJECT DATA BEGIN HERE-->\r\n";
		private static string ThreadLock = "ThreadLock-String";

		private const string SerializerVersion = "v1.0";
		private class TAGS
		{
			public const string SerializerVersionTag = "RemotingBase SerializerVersion";
			public const string EncriptionTag = "Encription";
			public const string CompressionTag = "Compression";
			public const string SerializationModeTag = "Serialization Mode";
			public const string InitializationVectorTag = "Encryption IV";
			public const string PwdHashTag = "Encryption Hash";
		}

		public static bool ObjToFile(object ObjectToSave, string FileWhereSave)
		{ return ObjToFile(ObjectToSave, FileWhereSave, SerializationMode.Auto, null); }

		public static bool ObjToFile(object ObjectToSave, string FileWhereSave, string Password)
		{ return ObjToFile(ObjectToSave, FileWhereSave, SerializationMode.Auto, Password); }

		public static bool ObjToFile(object ObjectToSave, string FileWhereSave, SerializationMode SerializeMode)
		{ return ObjToFile(ObjectToSave, FileWhereSave, SerializeMode, null); }

		public static bool ObjToFile(object ObjectToSave, string FileWhereSave, SerializationMode SerializeMode, string Password)
		{ return ObjToFile(ObjectToSave, FileWhereSave, SerializeMode, Password, false); }


		public static bool ObjToFile(object ObjectToSave, string filename, SerializationMode SerializeMode, string Password, bool Compression)
		{
			lock (ThreadLock)
			{
				Exception err = null;
				System.Security.Cryptography.SymmetricAlgorithm EE = null;
				Stream FinalStream = null;
				System.Runtime.Serialization.IFormatter SR = default(System.Runtime.Serialization.IFormatter);
				byte[] IV = null;
				byte[] CypherKey = null;
				string tmpfile = null;
				string backupfile = null;

				try
				{
					filename = System.IO.Path.Combine(LaserGRBL.GrblCore.DataPath, filename);
					tmpfile = Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar + "tmp_" + System.IO.Path.GetRandomFileName();
					backupfile = filename + ".bak";

					if (SerializeMode == SerializationMode.Auto)
						SerializeMode = ModeFromFname(filename);
					SR = CreateFormatterForMode(SerializeMode);
					//CREATE FORMATTER

					FinalStream = new FileStream(tmpfile, FileMode.CreateNew, FileAccess.Write, FileShare.None);
					//Open a stream on the file for writing and lock the file

					if ((Password != null))
					{
						EE = System.Security.Cryptography.SymmetricAlgorithm.Create();
						IV = EE.IV;
						CypherKey = GenerateKey(Password, Convert.ToInt32(EE.KeySize / 8));
					}

					WriteSerializerTag(FinalStream, SerializerVersion, SerializeMode, CypherKey, IV, Compression);

					if ((Password != null)) FinalStream = new System.Security.Cryptography.CryptoStream(FinalStream, EE.CreateEncryptor(CypherKey, EE.IV), System.Security.Cryptography.CryptoStreamMode.Write);
					if (Compression) FinalStream = new System.IO.Compression.DeflateStream(FinalStream, System.IO.Compression.CompressionMode.Compress);


					SR.Serialize(FinalStream, ObjectToSave); //WRITE DATA
					FinalStream.Flush();
					//If TypeOf (SS) Is System.Security.Cryptography.CryptoStream Then DirectCast(SS, System.Security.Cryptography.CryptoStream).FlushFinalBlock()
					FinalStream.Close();
					//CLOSE STREAM

					if ((System.IO.File.Exists(filename)))
					{
						System.IO.File.Replace(tmpfile, filename, backupfile, true);
						System.IO.File.Delete(backupfile);
					}
					else
					{
						System.IO.File.Move(tmpfile, filename);
					}

					return true;
				}
				catch (Exception ex)
				{
					err = ex;
					try
					{ if ((FinalStream != null))FinalStream.Close(); }
					catch { }
					try { ManageWriteError(ObjectToSave, filename, ex); }
					catch { }
				}
				finally
				{
					//evita di lasciare in giro file temporanei
					if ((tmpfile != null) && System.IO.File.Exists(tmpfile))
					{
						try { System.IO.File.Delete(tmpfile); }
						catch { }
					}
				}
			}
			return false;
		}

		public static object ObjFromFile(string FileWhereRead)
		{ return ObjFromFile(FileWhereRead, null, false); }

		public static object ObjFromFile(string FileWhereRead, string Password)
		{ return ObjFromFile(FileWhereRead, Password, false); }

		public static object ObjFromFile(string filename, string Password, bool AskForMissingPassword)
		{
			object rv = null;

			lock (ThreadLock)
			{
				Exception err = null;
				filename = System.IO.Path.Combine(LaserGRBL.GrblCore.DataPath, filename);

				if ((File.Exists(filename + ".bak") & !File.Exists(filename)))
					ManageOrphanTmp(filename);

				if (File.Exists(filename))
				{
					System.Security.Cryptography.SymmetricAlgorithm EE = null;
					Stream FinalStream = null;
					System.Runtime.Serialization.IFormatter SR = default(System.Runtime.Serialization.IFormatter);

					bool REncrypted = false;
					bool RCompressed = false;
					SerializationMode Rmode = default(SerializationMode);
					string RVersion = null;
					byte[] Rhash = null;

					byte[] IV = null;
					byte[] CypherKey = null;


					try
					{
						//Open a stream on the file for reading (overwrite if exist) and lock the file
						FinalStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
						GetSerializerTag(FinalStream, ref RVersion, ref Rmode, ref REncrypted, ref IV, ref Rhash, ref RCompressed);
						//Get serializer tag end move stream position

						//GESTISCI LA VERSIONE CORRENTE
						if (RVersion == SerializerVersion)
						{
							SR = CreateFormatterForMode(Rmode);
							if (RCompressed) FinalStream = new System.IO.Compression.DeflateStream(FinalStream, System.IO.Compression.CompressionMode.Decompress);

							if (REncrypted && Password == null)
							{
								if (AskForMissingPassword)
								{
									string NewKey = InputBox.Show(null, "Insert password:", "Protected file", "", null).Text;
									if ((NewKey != null))
									{
										FinalStream.Close();
										return ObjFromFile(filename, NewKey, AskForMissingPassword);
									}
									else
									{
										throw new MissingPasswordException(filename);
									}
								}
								else
								{
									throw new MissingPasswordException(filename);
								}
							}

							//GENERATE KEY AND CRYPTO SERVICE
							if (REncrypted)
							{
								EE = System.Security.Cryptography.SymmetricAlgorithm.Create();
								EE.IV = IV;
								CypherKey = GenerateKey(Password, Convert.ToInt32(EE.KeySize / 8));
							}


							//TEST KEY VALIDITY WITH HASH COMPARE
							if (REncrypted)
							{
								byte[] CurHash = GenerateHash(CypherKey);
								if (Rhash == null || CurHash == null)
									throw new WrongPasswordException(filename);
								if (!(Rhash.Length == CurHash.Length))
									throw new WrongPasswordException(filename);
								for (int I = 0; I <= Rhash.Length - 1; I++)
								{
									if (!(Rhash[I] == CurHash[I]))
										throw new WrongPasswordException(filename);
								}
							}

							if (REncrypted)
								FinalStream = new System.Security.Cryptography.CryptoStream(FinalStream, EE.CreateDecryptor(CypherKey, EE.IV), System.Security.Cryptography.CryptoStreamMode.Read);


							rv = SR.Deserialize(FinalStream); 							//READ DATA
							FinalStream.Close();
						}
						else
						{
							if ((FinalStream != null))
								FinalStream.Close();
							rv = ManageOldVersion(RVersion, filename, Password);
						}

					}
					catch (Exception ex)
					{
						err = ex;
						System.Diagnostics.Debug.WriteLine(string.Format("Serialization exception in {0} Position {1}", filename, FinalStream.Position));

						try
						{
							if ((FinalStream != null))
								FinalStream.Close();
						}
						catch
						{
						}
						try
						{
							ManageReadError(filename, ex);
						}
						catch
						{
						}
					}
				}
				else
				{
					;
				}
			}
			return rv;
		}



		public enum SerializationMode
		{
			Binary,
			Xml,
			Auto
		}

		private static System.Runtime.Serialization.IFormatter CreateFormatterForMode(SerializationMode mode)
		{
			System.Runtime.Serialization.IFormatter RET = default(System.Runtime.Serialization.IFormatter);

			//if (mode == SerializationMode.Xml)
			//{
			//	RET = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
			//	((System.Runtime.Serialization.Formatters.Soap.SoapFormatter)RET).AssemblyFormat = Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
			//}
			//else if (mode == SerializationMode.Binary)
			//{
			RET = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			((System.Runtime.Serialization.Formatters.Binary.BinaryFormatter)RET).AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
			//}
			//else
			//{
			//	throw new Exception("Unknown Serialization Mode");
			//}

			//RET.Binder = Binder;

			return RET;
		}

		private static void WriteSerializerTag(Stream SS, string Version, SerializationMode mode, byte[] CypherKey, byte[] IV, bool Compression)
		{
			WriteTag(SS, TAGS.SerializerVersionTag, Version);
			WriteTag(SS, TAGS.SerializationModeTag, mode.ToString());
			WriteTag(SS, TAGS.EncriptionTag, Convert.ToString(((CypherKey != null) ? "Enabled" : "Disabled")));
			WriteTag(SS, TAGS.CompressionTag, Convert.ToString((Compression ? "Enabled" : "Disabled")));
			if ((IV != null))
				WriteTag(SS, TAGS.InitializationVectorTag, Convert.ToBase64String(IV));
			if ((CypherKey != null))
				WriteTag(SS, TAGS.PwdHashTag, Convert.ToBase64String(GenerateHash(CypherKey)));
			WriteEndTag(SS);
		}

		private static void WriteTag(Stream SS, string Tag, string Value)
		{
			byte[] T = ConvertStringToASCIIByte(string.Format("<!-- {0}: {1} -->\r\n", Tag, Value));
			SS.Write(T, 0, T.Length);
		}

		private static void WriteEndTag(Stream SS)
		{
			byte[] T = ConvertStringToASCIIByte(DataStartString);
			SS.Write(T, 0, T.Length);
		}



		private static void GetSerializerTag(Stream SS, ref string Version, ref SerializationMode mode, ref bool encrypted, ref byte[] IV, ref byte[] PwdHash, ref bool compressed)
		{
			System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
			byte[] DATABEGIN = encoding.GetBytes(DataStartString);
			byte[] INTESTAZIONE = new byte[1024];
			//0-1023

			int MATCH_PTR = 0;
			int HDR_PTR = 0;

			while (MATCH_PTR < DATABEGIN.Length & HDR_PTR < INTESTAZIONE.Length & SS.CanRead)
			{
				int IB = SS.ReadByte();
				if (IB < 0)
					break; // TODO: might not be correct. Was : Exit While
				//eof

				byte B = Convert.ToByte(IB);
				if (B == DATABEGIN[MATCH_PTR])
					MATCH_PTR += 1;
				else
					MATCH_PTR = 0;

				INTESTAZIONE[HDR_PTR] = B;
				HDR_PTR += 1;
			}

			if (MATCH_PTR == DATABEGIN.Length)
			{
				string S = encoding.GetString(INTESTAZIONE, 0, HDR_PTR);
				Version = GetTagValue(S, TAGS.SerializerVersionTag, "v0.0");
				mode = (SerializationMode)Enum.Parse(typeof(SerializationMode), GetTagValue(S, TAGS.SerializationModeTag, "Auto"));
				encrypted = (GetTagValue(S, TAGS.EncriptionTag, "Disabled") == "Enabled");
				compressed = (GetTagValue(S, TAGS.CompressionTag, "Disabled") == "Enabled");
				string IVstring = GetTagValue(S, TAGS.InitializationVectorTag, null);
				if ((IVstring != null))
					IV = Convert.FromBase64String(IVstring);
				string PWDstring = GetTagValue(S, TAGS.PwdHashTag, null);
				if ((PWDstring != null))
					PwdHash = Convert.FromBase64String(PWDstring);
				//				SS.Position = DataStringIndex + DataStartString.Length				 'MOVE STREAM POINTER BEFORE DATASTRING
			}
			else
			{
				Version = "v0.0";
				mode = SerializationMode.Auto;
				encrypted = false;
				compressed = false;
				IV = null;
				SS.Position = 0;
			}



		}

		private static System.Text.RegularExpressions.Regex RegX = new System.Text.RegularExpressions.Regex("<!-- (?<Tag>.+): (?<Val>.+) -->", System.Text.RegularExpressions.RegexOptions.Compiled);
		private static string GetTagValue(string S, string TagName, string DefVal)
		{
			foreach (System.Text.RegularExpressions.Match M in RegX.Matches(S))
			{
				if (M.Result("${Tag}") == TagName)
					return M.Result("${Val}");
			}

			return DefVal;
		}


		private static byte[] GenerateKey(string strPassword, int lenght)
		{
			byte[] bytSalt = System.Text.Encoding.ASCII.GetBytes("salt");
			System.Security.Cryptography.PasswordDeriveBytes pdb = new System.Security.Cryptography.PasswordDeriveBytes(strPassword, bytSalt);
			// OBSOLETO CON FRAMEWORK 2.0 
			//Dim pdb As New System.Security.Cryptography.Rfc2898DeriveBytes(strPassword, bytSalt)           ' DA USARE CON 2.0 RICHIEDE UN SALT PIU' LUNGO E INCOMPATIBILE
			return pdb.GetBytes(lenght);
		}

		private static byte[] GenerateHash(byte[] CypherKey)
		{
			System.Security.Cryptography.SHA1Managed sha = new System.Security.Cryptography.SHA1Managed();
			return sha.ComputeHash(CypherKey);
		}

		public static byte[] ConvertStringToASCIIByte(string stringToConvert)
		{
			return (new System.Text.ASCIIEncoding()).GetBytes(stringToConvert);
		}

		private static SerializationMode ModeFromFname(string File)
		{
			//if (File.ToLower().EndsWith(".xml"))
			//	return SerializationMode.Xml;
			//else if (File.ToLower().EndsWith(".txt"))
			//	return SerializationMode.Xml;
			//else if (File.ToLower().EndsWith(".dat"))
			//	return SerializationMode.Binary;
			//else if (File.ToLower().EndsWith(".bin"))
			//	return SerializationMode.Binary;
			//else
				return SerializationMode.Binary;
		}

		private static void ManageOrphanTmp(string FileName)
		{
			LogDatedWriter Log = null;
			try
			{
				string LogFname = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(FileName)), "SerializationError.log");
				Log = new LogDatedWriter(LogFname);

				Log.WriteEmptyLine();
				Log.WriteLogLines(string.Format("Trovato file .bak orfano \"{0}\"", FileName + ".bak"));

				try
				{
					string OrigFile = Path.GetFullPath(FileName + ".bak");
					string NewFileName = string.Format("{0:dd-MM-yyyy HH.mm.ss} {1}.cpy", DateTime.Now, Path.GetFileName(OrigFile));
					string CopyFile = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(FileName + ".bak")), NewFileName);
					File.Copy(OrigFile, CopyFile);

					Log.WriteLogLines("E' stata creata una copia del file in oggetto");
					Log.WriteLogLines(string.Format("La copia si chiama \"{0}\"", NewFileName));
					Log.WriteEmptyLine();

				}
				catch (Exception)
				{
					Log.WriteLogLines("Non è stato possibile creare una copia del file in oggetto");
					Log.WriteEmptyLine();
				}

				try
				{
					File.Move(FileName + ".bak", FileName);
					Log.WriteLogLines(string.Format("Il file è stato rinominato in: {0} per permettere la rilettura", FileName));
				}
				catch (Exception)
				{
					Log.WriteLogLines(string.Format("Impossibile rinominare il file {0} in {0}", FileName + ".bak", FileName));
				}

				Log.WriteEmptyLine();
				Log.WriteSeparator();

			}
			catch (Exception le)
			{
				string Message = string.Format("Si è verificato un errore nella scrittura sul file \"{0}\" e non è stato possibile creare un file di log.{1}La directory di lavoro corrente era: \"{2}\"{1}Dettagli errore serializzazione: {1}{3}{1}Dettagli errore Logger: {1}{4}", FileName + ".bak", "\r\n", System.Environment.CurrentDirectory, "", le.ToString());
				//System.Diagnostics.EventLog.WriteEntry(Base.CommonFunction.Varie.EventSource, Message, EventLogEntryType.Error);
			}
			finally
			{
				try
				{
					if ((Log != null))
						Log.Close();
				}
				catch
				{
				}
			}
		}

		private static void ManageWriteError(object obj, string FileName, System.Exception ex)
		{
			LogDatedWriter Log = null;
			try
			{
				string LogFname = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(FileName)), "SerializationError.log");
				Log = new LogDatedWriter(LogFname);



				Log.WriteEmptyLine();
				Log.WriteLogLines(string.Format("Si è verificato un errore durante la scrittura dell' oggetto \"{0}\" sul file \"{1}\"", obj, FileName));
				if (ex is System.IO.IOException)
					LogFileUsage(Log, FileName);
				Log.WriteEmptyLine();

				Log.WriteLogLines("Descrizione dell' errore:");
				Log.WriteLogLines(ex.Message);
				Log.WriteEmptyLine();

				Log.WriteLogLines("Dettagli errore:");
				Log.WriteLogLines(ex.ToString());

				Log.WriteSeparator();

			}
			catch (Exception le)
			{
				string Message = string.Format("Si è verificato un errore nella scrittura sul file \"{0}\" e non è stato possibile creare un file di log.{1}La directory di lavoro corrente era: \"{2}\"{1}Dettagli errore serializzazione: {1}{3}{1}Dettagli errore Logger: {1}{4}", FileName, "\r\n", System.Environment.CurrentDirectory, ex.ToString(), le.ToString());
				//System.Diagnostics.EventLog.WriteEntry(Base.CommonFunction.Varie.EventSource, Message, EventLogEntryType.Error);
			}
			finally
			{
				try
				{
					if ((Log != null))
						Log.Close();
				}
				catch
				{
				}
			}
		}

		private static void ManageReadError(string FileName, System.Exception ex)
		{
			LogDatedWriter Log = null;
			try
			{
				string LogFname = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(FileName)), "SerializationError.log");
				Log = new LogDatedWriter(LogFname);



				Log.WriteEmptyLine();
				Log.WriteLogLines(string.Format("Si è verificato un errore durante la lettura del file \"{0}\"", FileName));
				if (ex is System.IO.IOException)
					LogFileUsage(Log, FileName);
				Log.WriteEmptyLine();

				if (!((ex) is MissingPasswordException || (ex) is WrongPasswordException))
				{
					//Create a copy of problematic file
					try
					{
						string OrigFile = Path.GetFullPath(FileName);
						string NewFileName = string.Format("{0:dd-MM-yyyy HH.mm.ss} {1}.dam", DateTime.Now, Path.GetFileName(OrigFile));
						string CopyFile = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(FileName)), NewFileName);
						File.Copy(OrigFile, CopyFile);

						Log.WriteLogLines("E' stata creata una copia del file in oggetto");
						Log.WriteLogLines(string.Format("La copia si chiama \"{0}\"", NewFileName));
						Log.WriteEmptyLine();

					}
					catch (Exception)
					{
						Log.WriteLogLines("Non è stato possibile creare una copia del file in oggetto");
						Log.WriteEmptyLine();
					}
				}

				Log.WriteLogLines("Descrizione dell' errore:");
				Log.WriteLogLines(ex.Message);
				Log.WriteEmptyLine();

				Log.WriteLogLines("Dettagli errore:");
				Log.WriteLogLines(ex.ToString());

				Log.WriteSeparator();

			}
			catch (Exception le)
			{
				string Message = string.Format("Si è verificato un errore nella lettura del file \"{0}\" e non è stato possibile creare un file di log.{1}La directory di lavoro corrente era: \"{2}\"{1}Dettagli errore serializzazione: {1}{3}{1}Dettagli errore Logger: {1}{4}", FileName, "\r\n", System.Environment.CurrentDirectory, ex.ToString(), le.ToString());
				//System.Diagnostics.EventLog.WriteEntry(Base.CommonFunction.Varie.EventSource, Message, EventLogEntryType.Error);
			}
			finally
			{
				try
				{
					if ((Log != null))
						Log.Close();
				}
				catch
				{
				}
			}
		}

		private static void LogFileUsage(LogDatedWriter Log, string FileName)
		{
			//try
			//{
			//	System.Collections.Generic.List<string> files = new System.Collections.Generic.List<string>();
			//	files.Add(FileName);

			//	System.Collections.Generic.IList<System.Diagnostics.Process> PA = LockDetector.GetProcessesUsingFiles(files);
			//	if (PA.Count > 0)
			//	{
			//		string Compose = "File locked by: ";

			//		foreach (System.Diagnostics.Process P in PA)
			//		{
			//			Compose += string.Format("{0} [{1}] ", P.ProcessName, P.MainModule.FileName);
			//		}
			//		Log.WriteLogLines(Compose);
			//	}


			//}
			//catch (Exception ex)
			//{
			//}
		}



		private class LogDatedWriter
		{
			public LogDatedWriter(string Fname)
			{

			}

			public void WriteLogLines(string Text)
			{
				LaserGRBL.Logger.LogMessage("Serialization", Text);
			}

			public void WriteEmptyLine()
			{
				WriteLogLines("");
			}

			public void WriteSeparator()
			{
				WriteLogLines("");
				LaserGRBL.Logger.LogMessage("Serialization", "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
			}


			public void Close()
			{
				
			}

		}


		public class MissingPasswordException : Exception
		{

			public MissingPasswordException(string Fname)
				: base(string.Format("File \"{0}\" is encripted but password was not specified when decrypting.", Fname))
			{
			}

		}

		public class WrongPasswordException : Exception
		{

			public WrongPasswordException(string Fname)
				: base(string.Format("File \"{0}\" is encripted but specified password was not valid.", Fname))
			{
			}

		}

		private static object ManageOldVersion(string Version, string FileWhereRead, string Password)
		{
			try
			{
				switch (Version)
				{
					case "v0.0":

						if (!System.IO.File.Exists(FileWhereRead))
							return null;

						SerializationMode mode = ModeFromFname(FileWhereRead);

						System.Runtime.Serialization.IFormatter FR = default(System.Runtime.Serialization.IFormatter);
						if (mode == SerializationMode.Xml)
						{
							FR = CreateFormatterForMode(SerializationMode.Xml);
						}
						else if (mode == SerializationMode.Binary)
						{
							FR = CreateFormatterForMode(SerializationMode.Binary);
						}
						else
						{
							throw new Exception("Unknown DeSerialization Mode");
						}

						System.IO.FileStream FS = default(System.IO.FileStream);
						//File Stream 
						System.Security.Cryptography.SymmetricAlgorithm DE = default(System.Security.Cryptography.SymmetricAlgorithm);
						//Decryption Engine
						System.Security.Cryptography.CryptoStream DS = null;
						//Decrypted Stream

						FS = new System.IO.FileStream(FileWhereRead, FileMode.Open, FileAccess.Read, FileShare.Read);

						if ((Password != null))
						{
							DE = System.Security.Cryptography.SymmetricAlgorithm.Create();
							DS = new System.Security.Cryptography.CryptoStream(FS, DE.CreateDecryptor(GenerateKey(Password, Convert.ToInt32(DE.KeySize / 8)), GenerateKey("Vettore di inizializzazione", 16)), System.Security.Cryptography.CryptoStreamMode.Read);
						}

						object Result = null;

						if ((DS != null))
						{
							Result = FR.Deserialize(DS);
							FS.Close();
						}
						else
						{
							Result = FR.Deserialize(FS);
							FS.Close();
						}

						//Save in newer version 
						ObjToFile(Result, FileWhereRead, Password);


						return (Result);

				}

			}
			catch (Exception ex)
			{
				ManageReadError(FileWhereRead, ex);
			}

			return null;
		}



	}







}