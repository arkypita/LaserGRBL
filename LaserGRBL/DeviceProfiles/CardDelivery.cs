using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LaserGRBL.DeviceProfiles
{
	internal static class CardDelivery
	{
		private const string Title = "Save to SD card";

		internal static void ShowDialog(IWin32Window parent, byte[] validatedProgram, string extension)
		{
			string fileName = ManagedCardJobs.JobName(extension);
			List<WindowsCard> cards = WindowsCard.FindRemovableCards();
			if (cards.Count == 0)
			{
				MessageBox.Show(parent, "Insert a writable memory card that Windows reports as removable.", Title,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			using (var dialog = new Form { Text = Title, ClientSize = new Size(485, 171),
				StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog,
				MaximizeBox = false, MinimizeBox = false, ShowInTaskbar = false, AutoScaleMode = AutoScaleMode.Font })
			using (var worker = new BackgroundWorker())
			{
				var label = new Label { Location = new Point(12, 12), Size = new Size(461, 48),
					Text = "Select a card to save " + fileName + ". Only previous jobs identified by the LaserGRBL card manifest can be replaced." };
				var choice = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(12, 65), Width = 461 };
				foreach (WindowsCard card in cards)
					choice.Items.Add(card);
				var status = new Label { Location = new Point(12, 101), Size = new Size(461, 20) };
				var save = new Button { Location = new Point(253, 130), Size = new Size(137, 28), Text = "Replace and eject", Enabled = false };
				var cancel = new Button { Location = new Point(398, 130), Size = new Size(75, 28), Text = "Cancel", DialogResult = DialogResult.Cancel };
				dialog.Controls.AddRange(new Control[] { label, choice, status, save, cancel });
				dialog.CancelButton = cancel;

				choice.SelectedIndexChanged += (sender, args) => save.Enabled = choice.SelectedItem != null;
				dialog.FormClosing += (sender, args) => args.Cancel = worker.IsBusy;
				save.Click += (sender, args) =>
				{
					var selected = (WindowsCard)choice.SelectedItem;
					choice.Enabled = save.Enabled = cancel.Enabled = false;
					status.Text = "Writing, verifying SHA-256 and ejecting…";
					dialog.UseWaitCursor = true;
					worker.RunWorkerAsync(selected);
				};
				worker.DoWork += (sender, args) => args.Result = ((WindowsCard)args.Argument).SaveAndEject(fileName, validatedProgram);
				worker.RunWorkerCompleted += (sender, args) =>
				{
					dialog.UseWaitCursor = false;
					if (args.Error != null)
						MessageBox.Show(dialog, "Card export did not finish. The card was not ejected.\n\n" + args.Error.Message,
							Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
					else if (args.Result is string)
						MessageBox.Show(dialog, fileName + " was saved and verified. Windows could not eject this card. " +
							"Use Windows Safely Remove Hardware before removing it.\n\n" + args.Result,
							Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					else
						MessageBox.Show(dialog, fileName + " was saved, verified and ejected.", Title,
							MessageBoxButtons.OK, MessageBoxIcon.Information);
					dialog.Close();
				};
				dialog.ShowDialog(parent);
			}
		}
	}

	internal sealed class WindowsCard : ICardFiles
	{
		private readonly string root;
		private readonly string volume;
		private readonly uint serial;
		private readonly string label;

		private WindowsCard(string root, string volume, uint serial, string label)
		{
			this.root = root;
			this.volume = volume;
			this.serial = serial;
			this.label = label;
		}

		internal static List<WindowsCard> FindRemovableCards()
		{
			var cards = new List<WindowsCard>();
			foreach (DriveInfo drive in DriveInfo.GetDrives())
			{
				try { cards.Add(Identify(drive.Name)); }
				catch (IOException) { }
			}
			return cards;
		}

		private static WindowsCard Identify(string root)
		{
			if (Native.GetDriveType(root) != (uint)DriveType.Removable)
				throw new IOException("The selected drive is not a removable card.");
			var volume = new StringBuilder(64);
			if (!Native.GetVolumeNameForVolumeMountPoint(root, volume, (uint)volume.Capacity))
				throw Native.Error("Cannot identify the card volume");
			var label = new StringBuilder(261);
			uint serial;
			uint maxComponentLength;
			uint flags;
			if (!Native.GetVolumeInformation(volume.ToString(), label, (uint)label.Capacity, out serial, out maxComponentLength,
				out flags, null, 0))
				throw Native.Error("Cannot read card volume information");
			if ((flags & Native.FileReadOnlyVolume) != 0)
				throw new IOException("The card is write-protected.");
			return new WindowsCard(root, volume.ToString(), serial, label.ToString());
		}

		public override string ToString()
		{
			return root + "  " + (label.Length == 0 ? "Unlabelled" : label) + "  (" + serial.ToString("X8") + ")";
		}

		private void VerifyIdentity()
		{
			WindowsCard current = Identify(root);
			if (!string.Equals(volume, current.volume, StringComparison.OrdinalIgnoreCase) || serial != current.serial)
				throw new IOException("The selected card changed. Select the intended card again.");
		}

		internal string SaveAndEject(string name, byte[] program)
		{
			using (var mutex = new Mutex(false, @"Local\LaserGRBL.Card." + volume.TrimEnd('\\').Substring(4)))
			{
				bool owned;
				try { owned = mutex.WaitOne(0); }
				catch (AbandonedMutexException) { owned = true; }
				if (!owned)
					throw new IOException("LaserGRBL is already writing to this card.");
				try
				{
					VerifyIdentity();
					ManagedCardJobs.Replace(this, name, program);
					try { Eject(); }
					catch (IOException error) { return error.Message; }
					return null;
				}
				finally { mutex.ReleaseMutex(); }
			}
		}

		public byte[] ReadManifest()
		{
			using (FileStream stream = OpenRegular(ManagedCardJobs.ManifestName, Native.GenericRead, Native.OpenExisting, FileAccess.Read, true))
			{
				if (stream == null)
					return null;
				if (stream.Length > ManagedCardJobs.MaxManifestLength)
					throw new IOException("The card manifest is too large.");
				using (var bytes = new MemoryStream())
				{
					stream.CopyTo(bytes);
					return bytes.ToArray();
				}
			}
		}

		public string ReadJobHash(string name)
		{
			return ReadHash(name);
		}

		private string ReadHash(string name)
		{
			using (FileStream stream = OpenRegular(name, Native.GenericRead, Native.OpenExisting, FileAccess.Read, true))
				return stream == null ? null : ManagedCardJobs.Hash(stream);
		}

		public void WriteVerified(string name, byte[] content, string previousHash)
		{
			string staged = "_LGRBL-" + Guid.NewGuid().ToString("N") + ".tmp";
			string expected = ManagedCardJobs.Hash(content);
			bool created = false;
			string backup = null;
			try
			{
				VerifyIdentity();
				using (FileStream writer = OpenRegular(staged, Native.GenericWrite, Native.CreateNew, FileAccess.Write, false))
				{
					created = true;
					writer.Write(content, 0, content.Length);
					writer.Flush(true);
				}
				if (ReadHash(staged) != expected)
					throw new IOException("The staged card file failed SHA-256 verification.");
				VerifyIdentity();
				if (previousHash != null)
					backup = BackUpVerifiedFile(name, previousHash);
				// Omitting replace flags leaves a file that appeared at the destination untouched.
				if (!Native.MoveFileEx(volume + staged, volume + name, Native.MoveWriteThrough))
					throw Native.Error("Cannot install " + name + " on the card");
				created = false;
				if (ReadHash(name) != expected)
					throw new IOException(name + " failed SHA-256 verification after writing.");
				if (backup != null)
					DeleteVerifiedJob(backup, previousHash);
			}
			catch (IOException original)
			{
				if (backup == null)
					throw;
				if (created && RestoreBackup(backup, name, previousHash))
					throw;
				throw new IOException(original.Message + " The previous file was retained as " + backup + ".", original);
			}
			finally
			{
				if (created)
				{
					try { DeleteVerifiedJob(staged, expected); }
					catch (IOException) { } // Leave an unverified staging file for manual recovery.
				}
			}
		}

		private string BackUpVerifiedFile(string name, string expectedHash)
		{
			using (FileStream stream = OpenRegular(name, Native.GenericRead | Native.Delete, Native.OpenExisting, FileAccess.Read, true))
			{
				if (stream == null || ManagedCardJobs.Hash(stream) != expectedHash)
					throw new IOException(name + " changed while the card was being written.");
				string backup = "_LGRBL-" + Guid.NewGuid().ToString("N") + ".bak";
				// A handle-based rename keeps the checked file open against concurrent writes and deletions.
				Native.RenameVerifiedHandle(stream.SafeFileHandle, volume + backup, name);
				return backup;
			}
		}

		private bool RestoreBackup(string backup, string name, string expectedHash)
		{
			try
			{
				VerifyIdentity();
				return ReadHash(backup) == expectedHash && Native.MoveFileEx(volume + backup, volume + name, Native.MoveWriteThrough);
			}
			catch (IOException) { return false; }
		}

		public void DeleteVerifiedJob(string name, string expectedHash)
		{
			VerifyIdentity();
			using (FileStream stream = OpenRegular(name, Native.GenericRead | Native.Delete, Native.OpenExisting, FileAccess.Read, true))
			{
				if (stream == null)
					return;
				if (ManagedCardJobs.Hash(stream) != expectedHash)
					throw new IOException(name + " changed and was kept on the card.");
				// Deleting the hashed handle prevents a path replacement from redirecting the deletion.
				byte deleteOnClose = 1;
				if (!Native.SetFileInformationByHandle(stream.SafeFileHandle, Native.FileDispositionInfo, ref deleteOnClose, 1))
					throw Native.Error("Cannot remove the previous LaserGRBL job " + name);
			}
		}

		private FileStream OpenRegular(string name, uint access, uint disposition, FileAccess streamAccess, bool optional)
		{
			uint flags = Native.FileFlagOpenReparsePoint;
			if (disposition == Native.CreateNew)
				flags |= Native.FileFlagWriteThrough;
			SafeFileHandle handle = Native.CreateFile(volume + name, access, (uint)FileShare.Read, IntPtr.Zero, disposition, flags, IntPtr.Zero);
			if (handle.IsInvalid)
			{
				int error = Marshal.GetLastWin32Error();
				handle.Dispose();
				if (optional && error == Native.ErrorFileNotFound)
					return null;
				throw Native.Error("Cannot open " + name + " on the card", error);
			}
			try
			{
				Native.FileInformation info;
				if (!Native.GetFileInformationByHandle(handle, out info))
					throw Native.Error("Cannot verify card file " + name);
				const FileAttributes protectedAttributes = FileAttributes.Directory | FileAttributes.ReparsePoint |
					FileAttributes.System | FileAttributes.Hidden | FileAttributes.ReadOnly;
				if (info.VolumeSerial != serial || info.NumberOfLinks != 1 || (info.Attributes & protectedAttributes) != 0)
					throw new IOException(name + " is not a regular writable file on the selected card.");
				return new FileStream(handle, streamAccess, 65536);
			}
			catch
			{
				handle.Dispose();
				throw;
			}
		}

		private void Eject()
		{
			VerifyIdentity();
			using (SafeFileHandle handle = Native.CreateFile(volume.TrimEnd('\\'), Native.GenericRead | Native.GenericWrite,
				(uint)(FileShare.Read | FileShare.Write), IntPtr.Zero, Native.OpenExisting, 0, IntPtr.Zero))
			{
				if (handle.IsInvalid)
					throw Native.Error("Cannot open the card for ejection");
				uint transferred;
				if (!Native.DeviceIoControl(handle, Native.LockVolume, null, 0, IntPtr.Zero, 0, out transferred, IntPtr.Zero))
					throw Native.Error("Cannot lock the card volume");
				if (!Native.DeviceIoControl(handle, Native.DismountVolume, null, 0, IntPtr.Zero, 0, out transferred, IntPtr.Zero))
					throw Native.Error("Cannot dismount the card volume");
				Native.DeviceIoControl(handle, Native.AllowMediaRemoval, new byte[] { 0 }, 1, IntPtr.Zero, 0, out transferred, IntPtr.Zero);
				if (!Native.DeviceIoControl(handle, Native.EjectMedia, null, 0, IntPtr.Zero, 0, out transferred, IntPtr.Zero))
					throw Native.Error("The card reader did not accept the eject request");
			}
		}

		private static class Native
		{
			internal const uint FileReadOnlyVolume = 0x00080000;
			internal const uint GenericRead = 0x80000000;
			internal const uint GenericWrite = 0x40000000;
			internal const uint Delete = 0x00010000;
			internal const uint FileFlagOpenReparsePoint = 0x00200000;
			internal const uint FileFlagWriteThrough = 0x80000000;
			internal const uint CreateNew = 1;
			internal const uint OpenExisting = 3;
			internal const uint MoveWriteThrough = 0x8;
			internal const int ErrorFileNotFound = 2;
			private const uint FileRenameInfo = 3;
			internal const uint FileDispositionInfo = 4;
			internal const uint LockVolume = 0x00090018;
			internal const uint DismountVolume = 0x00090020;
			internal const uint AllowMediaRemoval = 0x002D4804;
			internal const uint EjectMedia = 0x002D4808;

			[StructLayout(LayoutKind.Sequential)]
			internal struct FileInformation
			{
				internal FileAttributes Attributes;
				internal System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;
				internal System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
				internal System.Runtime.InteropServices.ComTypes.FILETIME LastWriteTime;
				internal uint VolumeSerial;
				internal uint FileSizeHigh;
				internal uint FileSizeLow;
				internal uint NumberOfLinks;
				internal uint FileIndexHigh;
				internal uint FileIndexLow;
			}

			[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
			internal static extern uint GetDriveType(string root);

			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern bool GetVolumeNameForVolumeMountPoint(string mountPoint, StringBuilder volumeName, uint volumeNameCapacity);

			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern bool GetVolumeInformation(string volumeRoot, StringBuilder label, uint labelCapacity,
				out uint serial, out uint maxComponentLength, out uint flags, StringBuilder fileSystem, uint fileSystemCapacity);

			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern SafeFileHandle CreateFile(string path, uint access, uint share, IntPtr security,
				uint disposition, uint flags, IntPtr template);

			[DllImport("kernel32.dll", SetLastError = true)]
			internal static extern bool GetFileInformationByHandle(SafeFileHandle handle, out FileInformation info);

			[DllImport("kernel32.dll", SetLastError = true)]
			internal static extern bool SetFileInformationByHandle(SafeFileHandle handle, uint fileInfoClass, ref byte buffer, uint bufferSize);
			[DllImport("kernel32.dll", SetLastError = true, EntryPoint = "SetFileInformationByHandle")]
			private static extern bool SetRenameInformation(SafeFileHandle handle, uint fileInfoClass, byte[] buffer, uint bufferSize);

			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern bool MoveFileEx(string source, string destination, uint flags);

			[DllImport("kernel32.dll", SetLastError = true)]
			internal static extern bool DeviceIoControl(SafeFileHandle handle, uint control, byte[] input, uint inputSize,
				IntPtr output, uint outputSize, out uint transferred, IntPtr overlapped);

			internal static void RenameVerifiedHandle(SafeFileHandle handle, string path, string name)
			{
				// FILE_RENAME_INFO contains a DWORD, a pointer-aligned root handle, a byte length and WCHARs.
				// A zeroed ReplaceIfExists and a null RootDirectory select a no-replacement absolute rename.
				int lengthOffset = IntPtr.Size * 2;
				int nameOffset = lengthOffset + sizeof(uint);
				byte[] pathBytes = Encoding.Unicode.GetBytes(path + "\0");
				var info = new byte[nameOffset + pathBytes.Length];
				Buffer.BlockCopy(BitConverter.GetBytes(pathBytes.Length - sizeof(char)), 0, info, lengthOffset, sizeof(uint));
				Buffer.BlockCopy(pathBytes, 0, info, nameOffset, pathBytes.Length);
				if (!SetRenameInformation(handle, FileRenameInfo, info, (uint)info.Length))
					throw Error("Cannot preserve the previous file " + name);
			}

			internal static IOException Error(string operation)
			{
				return Error(operation, Marshal.GetLastWin32Error());
			}

			internal static IOException Error(string operation, int error)
			{
				var detail = new Win32Exception(error);
				return new IOException(operation + ". " + detail.Message, detail);
			}
		}
	}
}
