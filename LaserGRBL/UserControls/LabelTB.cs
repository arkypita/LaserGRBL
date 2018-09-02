using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL.UserControls
{
	public partial class LabelTB : UserControl
	{
		GrblCore Core;
		int fun;

		public LabelTB(GrblCore core, int function)
		{
			InitializeComponent();
			Core = core;
			fun = function;
			
			if (function == 0)
			{
				LabelText = "Rapid";
				TB.Minimum = 0;
				TB.Maximum = 2;
				TB.SmallChange = 1;
				TB.LargeChange = 1;
				TB.TickFrequency = 1;
				if (Core.OverrideG0 == 25)
					TB.Value = 0;
				else if (Core.OverrideG0 == 50)
					TB.Value = 1;
				else
					TB.Value = 2;
			}
			else if (function == 1)
			{
				LabelText = "Linear";
				TB.Minimum = 10;
				TB.Maximum = 200;
				TB.SmallChange = 5;
				TB.LargeChange = 10;
				TB.TickFrequency = 10;
				TB.Value = Core.OverrideG1;
			}
			else
			{
				LabelText = "Power";
				TB.Minimum = 10;
				TB.Maximum = 200;
				TB.SmallChange = 5;
				TB.LargeChange = 10;
				TB.TickFrequency = 10;
				TB.Value = Core.OverrideS;
			}

			RefreshText();
		}

		private void TB_ValueChanged(object sender, EventArgs e)
		{
			if (fun == 0)
				Core.TOverrideG0 = (TB.Value == 0 ? 25 : TB.Value == 1 ? 50 : 100);
			if (fun == 1)
				Core.TOverrideG1 = TB.Value;
			if (fun == 2)
				Core.TOverrideS = TB.Value;
			
			RefreshText();
		}

		private string _basetext;
		public string LabelText
		{
			get { return _basetext; }
			set 
			{ 
				_basetext = value;
				RefreshText();
			}
		}

		private void RefreshText()
		{
			if (fun == 0)
				Lbl.Text = String.Format("{0} [{1:0.00}x]", _basetext, (TB.Value == 0 ? 25 : TB.Value == 1 ? 50 : 100) / 100.0);
			else
				Lbl.Text = String.Format("{0} [{1:0.00}x]", _basetext, TB.Value / 100.0);
		}

		private void TB_MouseUp(object sender, MouseEventArgs e)
		{
			Core.ManageOverrides();
		}

	}
}
