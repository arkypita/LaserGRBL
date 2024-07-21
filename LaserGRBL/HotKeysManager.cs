//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

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
        [NonSerialized] private PreviewForm mPreviewForm;
		[NonSerialized] private JogForm mJogForm;
		[NonSerialized] List<int> mCustomButtonPressed;
		[NonSerialized] private bool mJogKeyRequested = false;

		[Serializable]
		public class HotKey : ICloneable
		{
			public enum Actions
			{
				ConnectDisconnect = 10, Connect = 11, Disconnect = 12,
				OpenFile = 20, ReopenLastFile = 21, SaveFile = 22, ExecuteFile = 23, AbortFile = 24,
				HelpOnline = 30,
				AutoSizeDrawing = 40, ZoomInDrawing = 41, ZoomOutDrawing = 42,
				Reset = 100, Homing = 101, Unlock = 102, PauseJob = 103, ResumeJob = 104, SetNewZero = 105,
				JogHome = 1000, JogN = 1001, JogNE = 1002, JogE = 1003, JogSE = 1004, JogS = 1005, JogSW = 1006, JogW = 1007, JogNW = 1008, JogUp = 1009, JogDown = 1010,
				JogStepIncrease = 1020, JogStepDecrease = 1021,
				JogSpeedIncrease = 1030, JogSpeedDecrease = 1031,
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

			public string ActionString
			{
				get 
				{
					if (Enum.IsDefined(typeof(Actions), Action))
						return Action.ToString();
					else
						return "-";
				}
			}


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

        public void SetCustomButtonToolbar()
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
            AddNew(new HotKey(HotKey.Actions.JogUp, (Keys)107));
            AddNew(new HotKey(HotKey.Actions.JogDown, (Keys)109));

			AddNew(new HotKey(HotKey.Actions.JogStepIncrease, Keys.Multiply));
			AddNew(new HotKey(HotKey.Actions.JogStepDecrease, Keys.Divide));

			AddNew(new HotKey(HotKey.Actions.JogSpeedIncrease, Keys.None));
			AddNew(new HotKey(HotKey.Actions.JogSpeedDecrease, Keys.None));

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

			AddNew(new HotKey(HotKey.Actions.AutoSizeDrawing, Keys.Control | Keys.A));
			AddNew(new HotKey(HotKey.Actions.ZoomInDrawing, Keys.Control | Keys.Add));
			AddNew(new HotKey(HotKey.Actions.ZoomOutDrawing, Keys.Control | Keys.Subtract));
		}

		private void AddNew(HotKey toadd)
		{
			foreach (HotKey hk in this)
			{
				if (hk.Action == toadd.Action)
					return;
				if (toadd.Combination != Keys.None && hk.Combination == toadd.Combination)
					toadd.SetShortcut(Keys.None);
			}

			
			Add(toadd);
		}

		public void Init(GrblCore core, PreviewForm cbform, JogForm jogform)
		{
			mCore = core;
            mPreviewForm = cbform;
			mJogForm = jogform;
            mCustomButtonPressed = new List<int>();
            AddAllFeatures();
			Sort(CompareKey);
		}

		private int CompareKey(HotKey x, HotKey y)
		{
			return x.Action - y.Action;
		}

		internal bool ManageHotKeys(Form parent, Keys keys)
		{
            if (keys == Keys.None)
            {
				if (mJogKeyRequested)
					mCore.JogAbort();
				mJogKeyRequested = false;

				EmulateCustomButtonUp();
                return false;
            }
            else
            { 
                foreach (HotKey hk in this)
                    if (Match(keys, hk.Combination))
                        return PerformAction(parent, hk.Action);

                return false;
            }
		}

		private bool Match(Keys k1, Keys k2)
		{
			bool rv = k1 == k2;
			//System.Diagnostics.Debug.WriteLine(String.Format("{0} vs {1} = {2}", k1, k2, rv));
			return rv;	
		}

		private bool PerformAction(Form parent, HotKey.Actions action)
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
					mCore.OpenFile(); break;
				case HotKey.Actions.ReopenLastFile:
					mCore.ReOpenFile(); break;
				case HotKey.Actions.SaveFile:
					mCore.SaveProgram(parent, false, false, false, 1, false); break;
				case HotKey.Actions.ExecuteFile:
					mCore.RunProgram(parent); break;
				case HotKey.Actions.AbortFile:
					mCore.AbortProgram(); break;
				case HotKey.Actions.HelpOnline:
					mCore.HelpOnLine(); break;
                case HotKey.Actions.AutoSizeDrawing:
                    mCore.AutoSizeDrawing(); break;
				case HotKey.Actions.ZoomInDrawing:
					mCore.ZoomInDrawing(); break;
				case HotKey.Actions.ZoomOutDrawing:
					mCore.ZoomOutDrawing(); break;
				case HotKey.Actions.Reset:
					mCore.GrblReset(); break;
				case HotKey.Actions.Homing:
					mCore.GrblHoming(); break;
				case HotKey.Actions.Unlock:
					mCore.GrblUnlock(); break;
				case HotKey.Actions.PauseJob:
					mCore.FeedHold(false); break;
				case HotKey.Actions.ResumeJob:
					mCore.CycleStartResume(false); break;
				case HotKey.Actions.SetNewZero:
					mCore.SetNewZero(); break;
				case HotKey.Actions.JogHome:
					RequestJog(GrblCore.JogDirection.Home); break;
				case HotKey.Actions.JogN:
					RequestJog(GrblCore.JogDirection.N); break;
				case HotKey.Actions.JogNE:
					RequestJog(GrblCore.JogDirection.NE); break;
				case HotKey.Actions.JogE:
					RequestJog(GrblCore.JogDirection.E); break;
				case HotKey.Actions.JogSE:
					RequestJog(GrblCore.JogDirection.SE); break;
				case HotKey.Actions.JogS:
					RequestJog(GrblCore.JogDirection.S); break;
				case HotKey.Actions.JogSW:
					RequestJog(GrblCore.JogDirection.SW); break;
				case HotKey.Actions.JogW:
					RequestJog(GrblCore.JogDirection.W); break;
				case HotKey.Actions.JogNW:
					RequestJog(GrblCore.JogDirection.NW); break;
				case HotKey.Actions.JogUp:
					RequestJog(GrblCore.JogDirection.Zup); break;
				case HotKey.Actions.JogDown:
					RequestJog(GrblCore.JogDirection.Zdown); break;
				case HotKey.Actions.JogStepIncrease:
					ChangeJogStep(true); break;
				case HotKey.Actions.JogStepDecrease:
					ChangeJogStep(false); break;
				case HotKey.Actions.JogSpeedIncrease:
					ChangeJogSpeed(true); break;
				case HotKey.Actions.JogSpeedDecrease:
					ChangeJogSpeed(false); break;
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
					EmulateCustomButtonDown(0); break;
				case HotKey.Actions.CustomButton2:
					EmulateCustomButtonDown(1); break;
				case HotKey.Actions.CustomButton3:
					EmulateCustomButtonDown(2); break;
				case HotKey.Actions.CustomButton4:
					EmulateCustomButtonDown(3); break;
				case HotKey.Actions.CustomButton5:
					EmulateCustomButtonDown(4); break;
				case HotKey.Actions.CustomButton6:
					EmulateCustomButtonDown(5); break;
				case HotKey.Actions.CustomButton7:
					EmulateCustomButtonDown(6); break;
				case HotKey.Actions.CustomButton8:
					EmulateCustomButtonDown(7); break;
				case HotKey.Actions.CustomButton9:
					EmulateCustomButtonDown(8); break;
				case HotKey.Actions.CustomButton10:
					EmulateCustomButtonDown(9); break;
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

		private void ChangeJogStep(bool increase)
		{
			if (mCore.JogEnabled)
				mJogForm.ChangeJogStepIndexBy(increase ? 1 : -1);
		}

		private void ChangeJogSpeed(bool increase)
		{
			if (mCore.JogEnabled)
				mJogForm.ChangeJogSpeedIndexBy(increase ? 1 : -1);
		}

		private void RequestJog(GrblCore.JogDirection dir)
		{
			mJogKeyRequested = true;
			mCore.JogToDirection(dir, false);
		}
        private void EmulateCustomButtonDown(int index)
        {
            List<PreviewForm.CustomButtonIB> buttons = mPreviewForm.CustomImageButtons;
            if (index < buttons.Count)
            {
                if (!mCustomButtonPressed.Contains(index))
                { 
                    mCustomButtonPressed.Add(index);
                    buttons[index].EmulateMouseInside = true;
                    buttons[index].PerformMouseDown(new MouseEventArgs(MouseButtons.Left, 1, 1, 1, 0));
                }
            }
        }

        private void EmulateCustomButtonUp()
        {
            List<PreviewForm.CustomButtonIB> buttons = mPreviewForm.CustomImageButtons;

            foreach (int index in mCustomButtonPressed)
            {
                if (index < buttons.Count)
                {
                    buttons[index].PerformMouseUp(new MouseEventArgs(MouseButtons.Left, 1, 1, 1, 0));
                    buttons[index].PerformClick(new MouseEventArgs(MouseButtons.Left, 1, 1, 1, 0));
                    buttons[index].EmulateMouseInside = false;
                }
            }

            mCustomButtonPressed.Clear();
        }


        public List<HotKeysManager.HotKey> GetEditList()
		{
			List<HotKeysManager.HotKey> rv = new List<HotKeysManager.HotKey>();
			foreach (HotKeysManager.HotKey hk in this)
				rv.Add(hk.Clone() as HotKeysManager.HotKey);
			return rv;
		}

		private static KeysConverter cnv = new KeysConverter();
		internal string GetHotKeyString(HotKey.Actions action)
		{
			foreach (HotKey hk in this)
				if (hk.Action == action && hk.Combination != Keys.None)
					return cnv.ConvertToString(hk.Combination);

			return null;
		}
	}
}
