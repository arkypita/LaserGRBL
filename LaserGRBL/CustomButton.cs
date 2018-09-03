using System;
using System.Collections.Generic;
using System.Text;

namespace LaserGRBL
{
	[Serializable()]
	public class CustomButtons
	{
		private static List<CustomButton> buttons;
		private static string filename = System.IO.Path.Combine(GrblCore.DataPath, "CustomButtons.bin");

		public static void LoadFile() //in ingresso
		{
			buttons = (List<CustomButton>)Tools.Serializer.ObjFromFile(filename);
			if (buttons == null)
			{
				if (buttons == null) buttons = (List<CustomButton>)Settings.GetAndDeleteObject("Custom Buttons", null);
				if (buttons == null) buttons = new List<CustomButton>();
				Settings.Save();
				SaveFile();
			}
		}

		public static void SaveFile()
		{
			Tools.Serializer.ObjToFile(buttons, filename); //salva
		}

		internal static void Add(CustomButton cb)
		{buttons.Add(cb);}

		public static IEnumerable<CustomButton> Buttons
		{ get { return buttons; } }

		internal static void Remove(CustomButton cb)
		{buttons.Remove(cb);}

		internal static CustomButton GetButton(int index)
		{
			if (buttons != null && buttons.Count > index)
				return buttons[index];
			else
				return null;
		}

		public static void Export()
		{
			using (System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog())
			{
				sfd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				sfd.Filter = "ZippedButton|*.gz";
				sfd.AddExtension = true;
				sfd.FileName = "CustomButtons.gz";
				if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK && sfd.FileName != null)
					Tools.Serializer.ObjToFile(buttons, sfd.FileName, Tools.Serializer.SerializationMode.Binary, null, true);
			}
		}

		public static bool Import()
		{
			using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
			{
				ofd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				ofd.Filter = "ZippedButton|*.gz";
				ofd.AddExtension = true;
				ofd.FileName = "CustomButtons.gz";
				if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK && ofd.FileName != null && System.IO.File.Exists(ofd.FileName))
				{
					List<CustomButton> list = Tools.Serializer.ObjFromFile(ofd.FileName) as List<CustomButton>;
					if (list.Count > 0)
					{
						System.Windows.Forms.DialogResult rv = buttons.Count == 0 ? System.Windows.Forms.DialogResult.No : System.Windows.Forms.MessageBox.Show(Strings.BoxImportCustomButtonClearText, Strings.BoxImportCustomButtonCaption, System.Windows.Forms.MessageBoxButtons.YesNoCancel);

						if (rv == System.Windows.Forms.DialogResult.Yes || rv == System.Windows.Forms.DialogResult.No)
						{
							if (rv == System.Windows.Forms.DialogResult.Yes)
								buttons.Clear();

							foreach (CustomButton cb in list)
							{
								if (System.Windows.Forms.MessageBox.Show(string.Format("Import \"{0}\"?" ,  cb.ToolTip.Trim().Length > 0 ? cb.ToolTip : "[no name]"), Strings.BoxImportCustomButtonCaption, System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
									buttons.Add(cb);
							}

							return true;
						}
					}
				}
			}

			return false;
		}

		public static int Count { get { return buttons.Count; } }
	}


	[Serializable]
	public class CustomButton
	{
		public enum EnableStyles { Always = 0, Connected = 1, Idle = 3, Run = 4, IdleProgram = 10}
		public enum ButtonTypes { Button = 0, TwoStateButton = 1, PushButton =2 }

		public System.Guid guid = Guid.NewGuid();
		public System.Drawing.Image Image;
		public string GCode;
		public string GCode2;
		public string ToolTip;

		public EnableStyles EnableStyle;
		public ButtonTypes ButtonType;

		public bool EnabledNow(GrblCore core)
		{
			if (EnableStyle == EnableStyles.Always)
				return true;
			else if (EnableStyle == EnableStyles.Connected)
				return core.IsOpen;
			else if (EnableStyle == EnableStyles.Idle)
				return core.MachineStatus == GrblCore.MacStatus.Idle;
			else if (EnableStyle == EnableStyles.Run)
				return core.MachineStatus == GrblCore.MacStatus.Run;
			else if (EnableStyle == EnableStyles.IdleProgram)
				return core.MachineStatus == GrblCore.MacStatus.Idle && core.HasProgram;

			return false;
		}
	}
}
