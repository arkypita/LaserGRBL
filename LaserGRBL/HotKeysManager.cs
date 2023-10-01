//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Linq;
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
            // built-in Actions that can be assigned to a hotKey
            public enum Actions
            {
                None = 0,
                ConnectDisconnect = 10, Connect = 11, Disconnect = 12,
                OpenFile = 20, ReopenLastFile = 21, SaveFile = 22, ExecuteFile = 23, AbortFile = 24,
                HelpOnline = 30,
                Reset = 100, Homing = 101, Unlock = 102, PauseJob = 103, ResumeJob = 104, SetNewZero = 105,
                JogHome = 1000, JogN = 1001, JogNE = 1002, JogE = 1003, JogSE = 1004, JogS = 1005, JogSW = 1006, JogW = 1007, JogNW = 1008, JogUp = 1009, JogDown = 1010,
                JogStepIncrease = 1020, JogStepDecrease = 1021,
                JogSpeedIncrease = 1030, JogSpeedDecrease = 1031,
                OverridePowerDefault = 1100, OverridePowerUp = 1101, OverridePowerDown = 1102,
                OverrideLinearDefault = 1110, OverrideLinearUp = 1111, OverrideLinearDown = 1112,
                OverrideRapidDefault = 1120, OverrideRapidUp = 1121, OverrideRapidDown = 1122,
            }

            private Keys mCombination = Keys.None;
            private Actions mAction = Actions.None;
            private string mHotKeyName;

            // Does the hotkey's keyboard Combination match the incoming keys?
            public bool IsMatch(Keys k1)
            {
                bool rv = k1 == Combination;
                //System.Diagnostics.Debug.WriteLine(String.Format("{0} vs {1} = {2}", k1, k2, rv));
                return rv;
            }

            // For built-in actions, the mAction is set
            // For Custom Buttons, the hotkeyName is set
            public string Action => string.IsNullOrEmpty(mHotKeyName) ? mAction.ToString() : mHotKeyName;

            // Get access by a method to the action enum, not showing up in the datagrid
            // For built-in action, the mAction enum is set
            internal Actions GetActionEnum() => mAction;

            // Get access by a method to the hotkeyName string, not showing up in the datagrid
            // For Custom Buttons, the hotkeyName is set
            internal string GetHotKeyName() => mHotKeyName;

            // The chosen keyboard combination
            public Keys Combination => mCombination;

            // Ctor, typically for built-in actions
            public HotKey(Actions action, Keys keys)
            {
                mAction = action;
                mCombination = keys;
            }

            // Ctor, typically for custom button actions
            public HotKey(string hotKeyName, Keys keys)
            {
                mHotKeyName = hotKeyName;
                mCombination = keys;
            }

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

            private static bool IsStandard(Keys key) => IsLetter(key) || IsNumeric(key) || IsFunction(key);

            private static bool IsNumeric(Keys key) => ((key >= Keys.D0 && key <= Keys.D9) || (key >= Keys.NumPad0 && key <= Keys.NumPad9));

            private static bool IsLetter(Keys key) => (key >= Keys.A && key <= Keys.Z);

            private static bool IsFunction(Keys key) => (key >= Keys.F1 && key <= Keys.F24);

            private static bool IsModifier(Keys key) => (key & Keys.Modifiers) == key;

            internal void SetShortcut(Keys shortcut)
            { mCombination = shortcut; }

            public object Clone() => this.MemberwiseClone();

            // handy for debugging
            public override string ToString() => Action.ToString() + ":" + mCombination.ToString();

        }

        private void AddAllFeatures()
        {
            // add the built-in actions
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

            AddNew(new HotKey(HotKey.Actions.Unlock, Keys.L));

            // add the hotkeys for the custom buttons, initially assigned to None
            foreach (CustomButton cb in CustomButtons.Buttons)
                AddNew(new HotKey(cb.HotKeyName, Keys.None));
        }

        private void AddNew(HotKey toadd)
        {
            if (!(from hk in this where hk.Action == toadd.Action select hk).Any())
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
            return string.Compare(y.Action, x.Action);
        }

        internal bool ManageHotKeys(Form parent, Keys keys)
        {
            if (keys == Keys.None)
            {
                if (mJogKeyRequested)
                    mCore.EndJogV11();
                mJogKeyRequested = false;
                EmulateCustomButtonUp();
            }
            else
            {
                foreach (HotKey hk in this)
                    if (hk.IsMatch(keys))
                        return PerformAction(parent, hk);
            }

            return false;
        }

        private bool PerformAction(Form parent, HotKey hotKey)
        {
            // let's see if it's a regular built-in action
            var action = hotKey.GetActionEnum();
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
                    mCore.SaveProgram(parent, false, false, false, 1, false); break;
                case HotKey.Actions.ExecuteFile:
                    mCore.RunProgram(parent); break;
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
                    mCore.HotKeyOverride(action);
                    break;
                default:
                    // let's handle CustomButtons here
                    if (!string.IsNullOrEmpty(hotKey.GetHotKeyName()))
                    {
                        var buttons = mPreviewForm.CustomImageButtons;
                        var len = buttons.Count;
                        for (int t = 0; t < len; ++t)
                        {
                            var customButton = buttons[t].CustomButton;
                            if (customButton.HotKeyName == hotKey.GetHotKeyName() && customButton.EnabledNow(mCore))
                            {
                                EmulateCustomButtonDown(t);
                                return true;
                            }
                        }
                    }
                    break;
            }

            return false;
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
            mCore.BeginJog(dir, false);
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
    }
}
