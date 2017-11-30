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
	}


	[Serializable]
	public class CustomButton
	{
		public enum EnableStyles { Always = 0, Connected = 1, Idle = 3, Run = 4, IdleProgram = 10}

		public System.Guid guid = Guid.NewGuid();
		public System.Drawing.Image Image;
		public string GCode;
		public string ToolTip;

		public EnableStyles EnableStyle;

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
