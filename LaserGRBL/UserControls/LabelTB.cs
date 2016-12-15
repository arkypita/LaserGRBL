using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace VirtualSimulator.UserControls
{
	public partial class LabelTB : UserControl
	{

		public event System.EventHandler ValueChanged;

		public LabelTB()
		{
			InitializeComponent();
		}

		private void TB_ValueChanged(object sender, EventArgs e)
		{
			if (ValueChanged != null)
				ValueChanged(this, null);

			RefreshText();
		}

		public int Minimum
		{
			get { return TB.Minimum ; }
			set { TB.Minimum = value; }
		}

		public int Maximum
		{
			get { return TB.Maximum; }
			set { TB.Maximum = value; }
		}

		public int Value
		{
			get { return TB.Value; }
			set { TB.Value = value; }
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
			label1.Text = String.Format("{0} [{1:0.00}x]", _basetext, ConvertSpeedFromInt(Value));
		}


		static double K1 = Math.Pow(2.0, 0.1);

		public int ConvertSpeedToInt(double speed)
		{
			//return Math.Min(10, Math.Max(-10,(int)Math.Log(speed, K1)));
			return (int)speed;
		}

		public double ConvertSpeedFromInt(int val)
		{
			//return Math.Round(Math.Pow(K1, val), 2);
			return val;
		}


	}
}
