using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL
{
	[Serializable]
	public class HotKeysManager : List<HotKeysManager.HotKey>
	{
		[NonSerialized] private GrblCore mCore;

		[Serializable]
		public class HotKey : ICloneable
		{
			public enum Actions
			{
				ConnectDisconnect = 10, Connect = 11, Disconnect = 12,
				OpenFile = 20, ReopenLastFile = 21, SaveFile = 22, ExecuteFile = 23, AbortFile = 24,
				HelpOnline = 30,
				Reset = 100, Homing = 101, Unlock = 102,  PauseJob = 103, ResumeJob = 104, SetNewZero = 105,
				JogHome = 1000, JogN = 1001, JogNE = 1002, JogE = 1003, JogSE = 1004, JogS = 1005, JogSW = 1006, JogW = 1007, JogNW = 1008,
				OverridePowerDefault = 1100, OverridePowerUp = 1101, OverridePowerDown = 1102,
				OverrideLinearDefault = 1110, OverrideLinearUp = 1111, OverrideLinearDown = 1112,
				OverrideRapidDefault = 1120, OverrideRapidUp = 1121, OverrideRapidDown = 1122,
				CustomButton1 = 2000, CustomButton2 = 2001, CustomButton3 = 2002, CustomButton4 = 2003, CustomButton5 = 2004, CustomButton6 = 2005, CustomButton7 = 2006, CustomButton8 = 2007, CustomButton9 = 2008, CustomButton10 = 2009
			}

			private Keys mCombination;
			private Actions mAction;

			public Actions Action
			{ get { return mAction; } }

			public Keys Combination
			{ get { return mCombination; } }

			public HotKey(Actions action, Keys keys)
			{ mAction = action; mCombination = keys; }

			public static bool ValidShortcut(Keys Combination)
			{
				Keys control = Keys.None;
				Keys standard = Keys.None;

				if (Combination != Keys.None)
				{
					foreach (Keys x in Enum.GetValues(typeof(Keys)))
						if ((x & Combination) == x)
							if (IsStandard(x))
								standard |= x;
							else
								control |= x;
				}

				return standard != Keys.None;
			}

			private static bool IsStandard(Keys key)
			{ return IsLetter(key) || IsNumeric(key) || IsFunction(key); }

			private static bool IsNumeric(Keys key)
			{return ((key >= Keys.D0 && key <= Keys.D9) || (key >= Keys.NumPad0 && key <= Keys.NumPad9));}

			private static bool IsLetter(Keys key)
			{return (key >= Keys.A && key <= Keys.Z);}

			private static bool IsFunction(Keys key)
			{{ return (key >= Keys.F1 && key <= Keys.F24); }}

			private static bool IsModifier(Keys key)
			{ { return (key & Keys.Modifiers) == key; } }

			internal void SetShortcut(Keys shortcut)
			{mCombination = shortcut;}

			public object Clone()
			{
				return this.MemberwiseClone();
			}
		}

		public HotKeysManager()
		{
			
		}

		private void AddAllFeatures()
		{
			AddNew(new HotKey(HotKey.Actions.ConnectDisconnect, Keys.F12));
			AddNew(new HotKey(HotKey.Actions.Connect, Keys.None));
			AddNew(new HotKey(HotKey.Actions.Disconnect, Keys.None));

			AddNew(new HotKey(HotKey.Actions.OpenFile, Keys.Control | Keys.O));
			AddNew(new HotKey(HotKey.Actions.ReopenLastFile, Keys.Control | Keys.R));
			AddNew(new HotKey(HotKey.Actions.SaveFile, Keys.Control | Keys.S));
			AddNew(new HotKey(HotKey.Actions.ExecuteFile, Keys.F5));
			AddNew(new HotKey(HotKey.Actions.AbortFile, Keys.Control | Keys.F5));

			AddNew(new HotKey(HotKey.Actions.HelpOnline, Keys.Control | Keys.F1));

			AddNew(new HotKey(HotKey.Actions.Reset, Keys.Control | Keys.X));
			AddNew(new HotKey(HotKey.Actions.Homing, Keys.Control | Keys.H));
			AddNew(new HotKey(HotKey.Actions.Unlock, Keys.Control | Keys.U));
			AddNew(new HotKey(HotKey.Actions.PauseJob, Keys.F6));
			AddNew(new HotKey(HotKey.Actions.ResumeJob, Keys.F7));
			AddNew(new HotKey(HotKey.Actions.SetNewZero, Keys.Control | Keys.Z));

			AddNew(new HotKey(HotKey.Actions.JogHome, Keys.NumPad5));
			AddNew(new HotKey(HotKey.Actions.JogN, Keys.NumPad8));
			AddNew(new HotKey(HotKey.Actions.JogNE, Keys.NumPad9));
			AddNew(new HotKey(HotKey.Actions.JogE, Keys.NumPad6));
			AddNew(new HotKey(HotKey.Actions.JogSE, Keys.NumPad3));
			AddNew(new HotKey(HotKey.Actions.JogS, Keys.NumPad2));
			AddNew(new HotKey(HotKey.Actions.JogSW, Keys.NumPad1));
			AddNew(new HotKey(HotKey.Actions.JogW, Keys.NumPad4));
			AddNew(new HotKey(HotKey.Actions.JogNW, Keys.NumPad7));

			AddNew(new HotKey(HotKey.Actions.OverridePowerDefault, Keys.None));
			AddNew(new HotKey(HotKey.Actions.OverridePowerUp, Keys.None));
			AddNew(new HotKey(HotKey.Actions.OverridePowerDown, Keys.None));

			AddNew(new HotKey(HotKey.Actions.OverrideLinearDefault, Keys.None));
			AddNew(new HotKey(HotKey.Actions.OverrideLinearUp, Keys.None));
			AddNew(new HotKey(HotKey.Actions.OverrideLinearDown, Keys.None));

			AddNew(new HotKey(HotKey.Actions.OverrideRapidDefault, Keys.None));
			AddNew(new HotKey(HotKey.Actions.OverrideRapidUp, Keys.None));
			AddNew(new HotKey(HotKey.Actions.OverrideRapidDown, Keys.None));

			AddNew(new HotKey(HotKey.Actions.CustomButton1, Keys.Control | Keys.D1));
			AddNew(new HotKey(HotKey.Actions.CustomButton2, Keys.Control | Keys.D2));
			AddNew(new HotKey(HotKey.Actions.CustomButton3, Keys.Control | Keys.D3));
			AddNew(new HotKey(HotKey.Actions.CustomButton4, Keys.Control | Keys.D4));
			AddNew(new HotKey(HotKey.Actions.CustomButton5, Keys.Control | Keys.D5));
			AddNew(new HotKey(HotKey.Actions.CustomButton6, Keys.Control | Keys.D6));
			AddNew(new HotKey(HotKey.Actions.CustomButton7, Keys.Control | Keys.D7));
			AddNew(new HotKey(HotKey.Actions.CustomButton8, Keys.Control | Keys.D8));
			AddNew(new HotKey(HotKey.Actions.CustomButton9, Keys.Control | Keys.D9));
			AddNew(new HotKey(HotKey.Actions.CustomButton10, Keys.Control | Keys.D0));
		}

		private void AddNew(HotKey toadd)
		{
			foreach (HotKey hk in this)
				if (hk.Action == toadd.Action)
					return;
			
			Add(toadd);
		}

		public void Init(GrblCore core)
		{
			mCore = core;
			AddAllFeatures();
			Sort(CompareKey);
		}

		private int CompareKey(HotKey x, HotKey y)
		{
			return x.Action - y.Action;
		}

		internal bool ManageHotKeys(Keys keys)
		{
			foreach (HotKey hk in this)
				if (Match(keys, hk.Combination))
					return PerformAction(hk.Action);

			return false;
		}

		private bool Match(Keys k1, Keys k2)
		{
			bool rv = k1 == k2;
			System.Diagnostics.Debug.WriteLine(String.Format("{0} vs {1} = {2}", k1, k2, rv));
			return rv;	
		}

		private bool PerformAction(HotKey.Actions action)
		{
			switch (action)
			{
				case HotKey.Actions.ConnectDisconnect:
					mCore.HKConnectDisconnect(); break;
				case HotKey.Actions.Connect:
					mCore.HKConnect(); break;
				case HotKey.Actions.Disconnect:
					mCore.HKDisconnect(); break;
				case HotKey.Actions.OpenFile:
					mCore.OpenFile(Application.OpenForms[0]); break;
				case HotKey.Actions.ReopenLastFile:
					mCore.ReOpenFile(Application.OpenForms[0]); break;
				case HotKey.Actions.SaveFile:
					mCore.SaveProgram(); break;
				case HotKey.Actions.ExecuteFile:
					mCore.RunProgram(); break;
				case HotKey.Actions.AbortFile:
					mCore.AbortProgram(); break;
				case HotKey.Actions.HelpOnline:
					mCore.HelpOnLine(); break;
				case HotKey.Actions.Reset:
					mCore.GrblReset(); break;
				case HotKey.Actions.Homing:
					mCore.GrblHoming(); break;
				case HotKey.Actions.Unlock:
					mCore.GrblUnlock(); break;
				case HotKey.Actions.PauseJob:
					mCore.FeedHold(); break;
				case HotKey.Actions.ResumeJob:
					mCore.CycleStartResume(); break;
				case HotKey.Actions.SetNewZero:
					mCore.SetNewZero(); break;
				case HotKey.Actions.JogHome:
					mCore.JogHome(); break;
				case HotKey.Actions.JogN:
					mCore.Jog(GrblCore.JogDirection.N); break;
				case HotKey.Actions.JogNE:
					mCore.Jog(GrblCore.JogDirection.NE); break;
				case HotKey.Actions.JogE:
					mCore.Jog(GrblCore.JogDirection.E); break;
				case HotKey.Actions.JogSE:
					mCore.Jog(GrblCore.JogDirection.SE); break;
				case HotKey.Actions.JogS:
					mCore.Jog(GrblCore.JogDirection.S); break;
				case HotKey.Actions.JogSW:
					mCore.Jog(GrblCore.JogDirection.SW); break;
				case HotKey.Actions.JogW:
					mCore.Jog(GrblCore.JogDirection.W); break;
				case HotKey.Actions.JogNW:
					mCore.Jog(GrblCore.JogDirection.NW); break;
				case HotKey.Actions.OverridePowerDefault:
				case HotKey.Actions.OverridePowerUp:
				case HotKey.Actions.OverridePowerDown:
				case HotKey.Actions.OverrideLinearDefault:
				case HotKey.Actions.OverrideLinearUp:
				case HotKey.Actions.OverrideLinearDown:
				case HotKey.Actions.OverrideRapidDefault:
				case HotKey.Actions.OverrideRapidUp:
				case HotKey.Actions.OverrideRapidDown:
					mCore.HotKeyOverride(action); break;
				case HotKey.Actions.CustomButton1:
					mCore.HKCustomButton(0); break;
				case HotKey.Actions.CustomButton2:
					mCore.HKCustomButton(1); break;
				case HotKey.Actions.CustomButton3:
					mCore.HKCustomButton(2); break;
				case HotKey.Actions.CustomButton4:
					mCore.HKCustomButton(3); break;
				case HotKey.Actions.CustomButton5:
					mCore.HKCustomButton(4); break;
				case HotKey.Actions.CustomButton6:
					mCore.HKCustomButton(5); break;
				case HotKey.Actions.CustomButton7:
					mCore.HKCustomButton(6); break;
				case HotKey.Actions.CustomButton8:
					mCore.HKCustomButton(7); break;
				case HotKey.Actions.CustomButton9:
					mCore.HKCustomButton(8); break;
				case HotKey.Actions.CustomButton10:
					mCore.HKCustomButton(9); break;
				default:
					break;
			}

				//ConnectDisconnect = 10, Connect = 11, Disconnect = 12,
				//OpenFile = 20, ReopenLastFile = 21, SaveFile = 22, ExecuteFile = 23,
				//HelpOnline = 30,
				//Reset = 100, Homing = 101, Unlock = 102,  PauseJob = 103, ResumeJob = 104, SetNewZero = 105,
				//JogHome = 1000, JogN = 1001, JogNE = 1002, JogE = 1003, JogSE = 1004, JogS = 1005, JogSO = 1006, JogO = 1007, JogNO = 1008,
				//CustomButton1 = 2000, CustomButton2 = 2001, CustomButton3 = 2002, CustomButton4 = 2003, CustomButton5 = 2004, CustomButton6 = 2005, CustomButton7 = 2006, CustomButton8 = 2007, CustomButton9 = 2008, CustomButton10 = 2009


			return true;
		}

		public List<HotKeysManager.HotKey> GetEditList()
		{
			List<HotKeysManager.HotKey> rv = new List<HotKeysManager.HotKey>();
			foreach (HotKeysManager.HotKey hk in this)
				rv.Add(hk.Clone() as HotKeysManager.HotKey);
			return rv;
		}
	}
}
