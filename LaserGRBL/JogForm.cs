//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/
//Copyright (c) 2023 Alexandre Besnier - https://github.com/Varamil/


// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LaserGRBL
{
    public partial class JogForm : System.Windows.Forms.UserControl
    {
        GrblCore Core;
        private System.Collections.Generic.Queue<string> posQueue;

        public JogForm()
        {
            InitializeComponent();
            SettingsForm.SettingsChanged += SettingsForm_SettingsChanged;
            posQueue = new System.Collections.Generic.Queue<string>();            
        }

        public new bool Enabled
        {
            get => base.Enabled;
            set
            {
                //Is job in queue and previous step just end ? 
                if (base.Enabled == false && value == true) ExecuteNextCommand();

                //save the value
                base.Enabled = value;                

                //Enables moves button
                if (Core != null)
                {
                    if (BtnTL != null && value == true)
                    {
                        BtnTL.Enabled = (Core.MachineStatus == GrblCore.MacStatus.Idle && Core.HasProgram);
                        BtnT.Enabled = BtnTL.Enabled;
                        BtnTR.Enabled = BtnTL.Enabled;
                        BtnL.Enabled = BtnTL.Enabled;
                        BtnR.Enabled = BtnTL.Enabled;
                        BtnBL.Enabled = BtnTL.Enabled;
                        BtnB.Enabled = BtnTL.Enabled;
                        BtnBR.Enabled = BtnTL.Enabled;
                        BtnC.Enabled = BtnTL.Enabled;

                        BtnFrame.Enabled = BtnTL.Enabled;
                        BtnFrameInf.Enabled = BtnTL.Enabled;
                        BtnShape.Enabled = BtnTL.Enabled;
                    } //else full control is disabled, so don't care

                    if (Core.InProgram)
                    {
                        BtnFocus.ON = false;
                    }
                }
            }
        }


        public void SetCore(GrblCore core)
		{
            Core = core;

			UpdateFMax.Enabled = true;
			UpdateFMax_Tick(null, null);

			TbSpeed.Value = Math.Max(Math.Min(Settings.GetObject("Jog Speed", 1000), TbSpeed.Maximum), TbSpeed.Minimum);
            
			TbStep.Value = Convert.ToDecimal(Settings.GetObject("Jog Step", 10M));

			TbSpeed_ValueChanged(null, null); //set tooltip
			TbStep_ValueChanged(null, null); //set tooltip

            Core.JogStateChange += Core_JogStateChange;
            SettingsForm_SettingsChanged(this, null);
        }

        private void SettingsForm_SettingsChanged(object sender, EventArgs e)
        {
            TlpStepControl.Visible = !Settings.GetObject("Enable Continuous Jog", false);
            TlpZControl.Visible = Settings.GetObject("Enale Z Jog Control", false);
            BtnClickNJog.ON = Settings.GetObject("Click N Jog", false);
        }

        private void Core_JogStateChange(bool jog)
        {
            BtnHome.Visible = !jog;
        }

        private void OnJogButtonMouseDown(object sender, MouseEventArgs e)
		{
			Core.BeginJog((sender as DirectionButton).JogDirection, e.Button == MouseButtons.Right);
		}

        private void OnJogButtonMouseUp(object sender, MouseEventArgs e)
        {
            Core.EndJogV11();
        }

        private void OnZJogButtonMouseDown(object sender, MouseEventArgs e)
        {
            Core.EnqueueZJog((sender as DirectionStepButton).JogDirection, (sender as DirectionStepButton).JogStep, e.Button == MouseButtons.Right);
        }

        private void TbSpeed_ValueChanged(object sender, EventArgs e)
		{
			TT.SetToolTip(TbSpeed, $"{Strings.SpeedSliderToolTip} {TbSpeed.Value}");
			LblSpeed.Text = String.Format("F{0}", TbSpeed.Value);
			Settings.SetObject("Jog Speed", TbSpeed.Value);
			Core.JogSpeed = TbSpeed.Value;
		}

		private void TbStep_ValueChanged(object sender, EventArgs e)
		{
			TT.SetToolTip(TbStep, $"{Strings.StepSliderToolTip} {TbStep.Value}");
			LblStep.Text = TbStep.Value.ToString();
			Settings.SetObject("Jog Step", TbStep.Value);
			Core.JogStep = TbStep.Value;
		}

        public void ChangeJogStepIndexBy(int value)
        {
            TbStep.ChangeIndexBy(value);
        }

		internal void ChangeJogSpeedIndexBy(int v)
		{
			TbSpeed.Value = Math.Max(Math.Min(TbSpeed.Value + (TbSpeed.LargeChange * v), TbSpeed.Maximum), TbSpeed.Minimum);
		}

		int oldMax;
		private void UpdateFMax_Tick(object sender, EventArgs e)
		{
			int curMax = (int)Math.Max(TbSpeed.Minimum, Math.Max(GrblCore.Configuration.MaxRateX, GrblCore.Configuration.MaxRateY));

			if (oldMax != curMax)
			{
				oldMax = curMax;
				TbSpeed.Maximum = curMax;
				
				TbSpeed.LargeChange = Math.Max(1, curMax / 10);
				TbSpeed.SmallChange = Math.Max(1, curMax / 20);
			}
		}

        private void BtnFocus_Click(object sender, EventArgs e)
        {
            TwoStatesGCodeButton bt = (TwoStatesGCodeButton)sender;
            int percent = Settings.GetObject("Focusing Power", 30);
            Core.ExecuteCustombutton(bt.ON ? string.Format(bt.GCode, percent) : bt.GCode2);
        }

        private void BtnPosition_Click(object sender, EventArgs e)
        {
            string g = "1";
            string speed = "F[jogspeed]";
            if ((e as MouseEventArgs).Button == MouseButtons.Right)
            {
                g = "0";
                speed = "";
            }
            string code = string.Format((sender as GCodeButton).GCode, g, speed);
            Core.ExecuteCustombutton(code);
        }

        private void BtnFrameInf_Click(object sender, EventArgs e)
        {
            string g = "1";
            string speed = "F[jogspeed]";
            if ((e as MouseEventArgs).Button == MouseButtons.Right)
            {
                g = "0";
                speed = "";
            }
            string code = string.Format((sender as GCodeButton).GCode, g, speed);
            if (!string.IsNullOrWhiteSpace(code))
            {
                lock (posQueue)
                {
                    for (int i = 1; i <= (int)TbStep.Value; i++)
                    {
                        posQueue.Enqueue(code);
                    }
                }

                ExecuteNextCommand();
            }   
        }

        private void BtnShape_Click(object sender, EventArgs e)
        {
            bool useProgSpeed = (e as MouseEventArgs).Button == MouseButtons.Right;

            System.Text.StringBuilder tosend = new System.Text.StringBuilder();
            TimeSpan duration = TimeSpan.Zero;
            TimeSpan batchduration = TimeSpan.Zero;
            GrblCommand.StatePositionBuilder spb = new GrblCommand.StatePositionBuilder();
            GrblConfST conf = GrblCore.Configuration;
            string ccmd;

            foreach (GrblCommand cmd in Core.LoadedFile)
            {               
                if (!Regex.Match(cmd.ToString(), "^[MS][0-9]+").Success)
                {
                    ccmd = useProgSpeed ? cmd.ToString() : Core.EvaluateExpression(Regex.Replace(cmd.ToString(), "F[0-9]+", "F[jogspeed]"));
                    duration = spb.AnalyzeCommand(new GrblCommand(ccmd), true, conf);
                    batchduration += duration;
                    if (batchduration.TotalSeconds > 10)
                    {
                        //send batch
                        posQueue.Enqueue(tosend.ToString());
                        //prepare next one
                        tosend.Clear();
                        batchduration = duration;
                    }
                    tosend.AppendLine(ccmd);
                }                    
            }
            if (!string.IsNullOrWhiteSpace(tosend.ToString()))
                posQueue.Enqueue(tosend.ToString());

            ExecuteNextCommand();

        }

        private void ExecuteNextCommand()
        {
            lock (posQueue)
            {
                if (Core != null && Core.LastIssue != GrblCore.DetectedIssue.Unknown)
                    posQueue.Clear();
                else if (posQueue.Count > 0)
                {
                    string tosend = posQueue.Dequeue();
                    Core.ExecuteCustombutton(tosend);
                }
            }            
        }

        private void BtnClickNJog_Click(object sender, EventArgs e)
        {
            Settings.SetObject("Click N Jog", BtnClickNJog.ON);
        }

        public void ExecuteHotKey(HotKeysManager.HotKey.Actions action)
        {
            if (Enabled)
            {
                switch (action)
                {
                    case HotKeysManager.HotKey.Actions.MoveCenter:
                        ExecuteHotKey(BtnC); break;
                    case HotKeysManager.HotKey.Actions.MoveN:
                        ExecuteHotKey(BtnT); break;
                    case HotKeysManager.HotKey.Actions.MoveNE:
                        ExecuteHotKey(BtnTR); break;
                    case HotKeysManager.HotKey.Actions.MoveE:
                        ExecuteHotKey(BtnR); break;
                    case HotKeysManager.HotKey.Actions.MoveSE:
                        ExecuteHotKey(BtnBR); break;
                    case HotKeysManager.HotKey.Actions.MoveS:
                        ExecuteHotKey(BtnB); break;
                    case HotKeysManager.HotKey.Actions.MoveSW:
                        ExecuteHotKey(BtnBL); break;
                    case HotKeysManager.HotKey.Actions.MoveW:
                        ExecuteHotKey(BtnL); break;
                    case HotKeysManager.HotKey.Actions.MoveNW:
                        ExecuteHotKey(BtnTL); break;
                    case HotKeysManager.HotKey.Actions.MoveFrame:
                        ExecuteHotKey(BtnFrame); break;
                    case HotKeysManager.HotKey.Actions.MoveFrameN:
                        ExecuteHotKey(BtnFrameInf); break;
                    case HotKeysManager.HotKey.Actions.MovePath:
                        ExecuteHotKey(BtnShape); break;
                    case HotKeysManager.HotKey.Actions.SwitchFocus:
                        ExecuteHotKey(BtnFocus); break;
                }
            }
        }

        private void ExecuteHotKey(GCodeButton btn)
        {
            if (btn.Enabled)
            {
                btn.PerformLeftClick();
            }
        }
    }

    public class StepBar : System.Windows.Forms.TrackBar
    {
        decimal[] values = { 0.1M, 0.2M, 0.5M, 1, 2, 5, 10, 20, 50, 100, 200 };

        public StepBar()
        {
            Minimum = 0;
            Maximum = values.Length -1;
            SmallChange = LargeChange = 1;
        }

        private int CurIndex { get { return base.Value; } set { base.Value = value; } }

        public new decimal Value
        {
            get
            {
                return values[CurIndex];
            }
            set
            {
                int found = 0;
                for (int index = 0; index < values.Length; index++)
                {
                    if (Math.Abs(value - values[index]) < Math.Abs(value - values[found]))
                        found = index;
                }
                CurIndex = found;
            }
        }

        public void ChangeIndexBy(int value)
        {
            CurIndex = Math.Max(Math.Min(CurIndex + value, Maximum), Minimum);
        }
    }


    public class DirectionButton : UserControls.ImageButton
	{
		private GrblCore.JogDirection mDir = GrblCore.JogDirection.N;

		public GrblCore.JogDirection JogDirection
		{
			get { return mDir; }
			set { mDir = value; }
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			if (Width != Height)
				Width = Height;

			base.OnSizeChanged(e);
		}
	}

    public class GCodeButton : UserControls.ImageButton
    {

        private string gCode;

        [System.ComponentModel.Browsable(true)]
        [System.ComponentModel.EditorAttribute("System.ComponentModel.Design.MultilineStringEditor, System.Design", "System.Drawing.Design.UITypeEditor")]
        public string GCode { get => gCode; set => gCode = value; }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (Width != Height)
                Width = Height;

            base.OnSizeChanged(e);
        }

        public void PerformLeftClick()
        {
            OnClick(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
        }
    }

    public class TwoStatesGCodeButton : GCodeButton
    {
        private bool on;
        public bool ON { get => on;
            set {
                on = value;
                BackColor = on ? System.Drawing.Color.Orange : Parent.BackColor;
                Invalidate();
            } 
        }

        private string gCode2;

        [System.ComponentModel.Browsable(true)]
        [System.ComponentModel.EditorAttribute("System.ComponentModel.Design.MultilineStringEditor, System.Design", "System.Drawing.Design.UITypeEditor")]
        public string GCode2 { get => gCode2; set => gCode2 = value; }

        protected override void OnClick(EventArgs e)
        {
            ON = !ON; 

            base.OnClick(e);
        }

    }

    public class DirectionStepButton : DirectionButton
    {
        private decimal mStep = 1.0M;

        public decimal JogStep
        {
            get { return mStep; }
            set { mStep = value; }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (Width != Height)
                Width = Height;

            base.OnSizeChanged(e);
        }
    }
}
