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

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace LaserGRBL.UserControls
{

	public enum FillStyles
	{
		Solid,
		Dashed
	}
	//FillStyles


	[Description("Color Progress Bar"), ToolboxBitmap(typeof(ProgressBar)), Designer(typeof(ColorProgressBarDesigner))]
	public partial class ColorProgressBar : System.Windows.Forms.UserControl
	{

		//
		// set default values
		//
		private double _Value = 0;
		private double _Minimum = 0;
		private double _Maximum = 100;
		private double _Step = 10;
		private bool _Reverse = false;
		private bool _DrawProgressString = false;
		private int _ProgressStringDecimals = 0;

		private bool _ThrowExceprion = false;

		private FillStyles _FillStyle = FillStyles.Dashed;
		private Color _FillColor = Color.White;
		private Color _BarColor = Color.FromArgb(255, 128, 128);

		private Color _BorderColor = Color.Black;

		public ColorProgressBar()
		{
			base.Size = new Size(150, 10);
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true);
		}



		[Description("ColorProgressBar color"), Category("ColorProgressBar")]
		public Color BarColor
		{
			get { return _BarColor; }
			set
			{
				if (value != _BarColor)
				{
					_BarColor = value;
					this.Invalidate();
				}
			}
		}

		[Description("ColorProgressBar fill color"), Category("ColorProgressBar")]
		public Color FillColor
		{
			get { return _FillColor; }
			set
			{
				_FillColor = value;
				this.Invalidate();
			}
		}

		[Description("Reverse Direction"), Category("ColorProgressBar")]
		public bool Reverse
		{
			get { return _Reverse; }
			set
			{
				_Reverse = value;
				this.Invalidate();
			}
		}

		[Description("Throw exception on inconsistent value"), Category("ColorProgressBar")]
		public bool ThrowExceprion
		{
			get { return _ThrowExceprion; }
			set
			{
				_ThrowExceprion = value;
				this.Invalidate();
			}
		}

		[Description("ColorProgressBar fill style"), Category("ColorProgressBar"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public FillStyles FillStyle
		{
			get { return _FillStyle; }
			set
			{
				if (value != FillStyle)
				{
					_FillStyle = value;
					this.Invalidate();
				}
			}
		}

		[Description("The current value for the ColorProgressBar, " + "in the range specified by the Minimum and Maximum properties."), Category("ColorProgressBar"), RefreshProperties(RefreshProperties.All)]
		public double Value
		{
			// the rest of the Properties windows must be updated when this peroperty is changed.
			get { return _Value; }
			set
			{

				if ((value != _Value))
				{
					if (value < _Minimum & ThrowExceprion)
					{
						throw new ArgumentException("'" + value + "' is not a valid value for 'Value'.\r\n'Value' must be between 'Minimum' and 'Maximum'.");
					}

					if (value > _Maximum & ThrowExceprion)
					{
						throw new ArgumentException("'" + value + "' is not a valid value for 'Value'.\r\n'Value' must be between 'Minimum' and 'Maximum'.");
					}

					_Value = value;
					this.Invalidate();
				}
			}
		}

		[Description("The lower bound of the range this ColorProgressbar is working with."), Category("ColorProgressBar"), RefreshProperties(RefreshProperties.All)]
		public double Minimum
		{
			get { return _Minimum; }
			set
			{
				if (value != _Minimum)
				{
					if (value >= Maximum)
					{
						if (ThrowExceprion)
							throw new Exception("Maximum must be smaller then minimum");
					}
					_Value = Math.Max(_Value, value);
					_Minimum = value;
					this.Invalidate();
				}
			}
		}

		[Description("The uppper bound of the range this ColorProgressbar is working with."), Category("ColorProgressBar"), RefreshProperties(RefreshProperties.All)]
		public double Maximum
		{
			get { return _Maximum; }
			set
			{
				if (value != _Maximum)
				{
					if (value <= Minimum)
					{
						if (ThrowExceprion)
							throw new Exception("Maximum must be greather then minimum");
					}
					_Value = Math.Min(_Value, value);
					_Maximum = value;
					this.Invalidate();
				}
			}
		}

		[Description("The amount to jump the current value of the control by when the Step() method is called."), Category("ColorProgressBar")]
		public double Step
		{
			get { return _Step; }
			set
			{
				_Step = value;
				this.Invalidate();
			}
		}

		[Description("The border color of ColorProgressBar"), Category("ColorProgressBar")]
		public Color BorderColor
		{
			get { return _BorderColor; }
			set
			{
				_BorderColor = value;
				this.Invalidate();
			}
		}

		[Description("Draw progress string"), Category("ColorProgressBar")]
		public bool DrawProgressString
		{
			get { return _DrawProgressString; }
			set
			{
				_DrawProgressString = value;
				this.Invalidate();
			}
		}

		[Description("Draw progress string decimal point"), Category("ColorProgressBar")]
		public int ProgressStringDecimals
		{
			get { return _ProgressStringDecimals; }
			set
			{
				_ProgressStringDecimals = value;
				this.Invalidate();
			}
		}

		//
		// Call the PerformStep() method to increase the value displayed by the amount set in the Step property
		//
		public void PerformStep()
		{
			if (_Value < _Maximum)
			{
				_Value += _Step;
			}
			else
			{
				_Value = _Maximum;
			}
			this.Invalidate();
		}
		//PerformStep


		//
		// Call the PerformStepBack() method to decrease the value displayed by the amount set in the Step property
		//
		public void PerformStepBack()
		{
			if (_Value > _Minimum)
			{
				_Value -= _Step;
			}
			else
			{
				_Value = _Minimum;
			}
			this.Invalidate();
		}
		//PerformStepBack


		//
		// Call the Increment() method to increase the value displayed by an integer you specify
		// 
		public void Increment(double value)
		{
			if (_Value < _Maximum)
			{
				_Value += value;
			}
			else
			{
				_Value = _Maximum;
			}
			this.Invalidate();
		}
		//Increment


		//
		// Call the Decrement() method to decrease the value displayed by an integer you specify
		// 
		public void Decrement(double value)
		{
			if (_Value > _Minimum)
			{
				_Value -= value;
			}
			else
			{
				_Value = _Minimum;
			}
			this.Invalidate();
		}
		//Decrement

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			DrawBackground(e);
			DrawProgres(e.Graphics);
			if (FillStyle == FillStyles.Dashed)
				DrawTick(e.Graphics);
			if (DrawProgressString)
				DrawString(e.Graphics);
			DrawBorder(e);
		}


		protected virtual void DrawString(Graphics g)
		{
			double pval = (Value - Minimum) / (Maximum - Minimum);
			string pstr = null;
			if ((double.IsNaN(pval)))
			{
				pstr = "";
			}
			else
			{
				pstr = string.Format("{0:p" + ProgressStringDecimals.ToString() + "}", pval);
			}
			using (SolidBrush B = new SolidBrush(ForeColor))
			{
				g.DrawString(pstr, Font, B, Convert.ToInt32((Width - g.MeasureString(pstr, Font).Width) / 2), Convert.ToInt32((Height - g.MeasureString(pstr, Font).Height) / 2));
			}
		}


		protected virtual void DrawBorder(PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, ColorScheme.ControlsBorder, ButtonBorderStyle.Solid);
		}

		protected virtual void DrawBackground(PaintEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(FillColor), e.ClipRectangle);
		}

		//L = Larghezza di una tacca
		protected int L = 6;
		//S = Spazio tra una tacca e l'altra
		protected int S = 2;

		protected virtual void DrawProgres(System.Drawing.Graphics g)
		{
			if (this.Enabled)
			{
				DrawBar(g, ClientRectangle.Width, ClientRectangle.Height, Value, BarColor);
			}
			else
			{
				DrawBar(g, ClientRectangle.Width, ClientRectangle.Height, Value, Color.LightGray);
			}
		}

		protected void DrawBar(Graphics G, int W, int H, double V, Color C)
		{
			int BarWidth = 0;

			if (!double.IsNaN(V) && !double.IsInfinity(V) && !((Maximum - Minimum) == 0))
			{
				BarWidth = Convert.ToInt32(Math.Floor(((W - 3) * (Math.Min(V, Maximum) - Minimum)) / (Maximum - Minimum)));
			}


			int BarHeight = H - 4;

			//If FillStyle = FillStyles.Dashed Then BarWidth = ((CInt(BarWidth / SL)) * SL) - S

			//BarWidth = Math.Min(W - 3, BarWidth)


			if (!(BarWidth <= 0) & !(BarHeight <= 0))
			{
				Rectangle ColoredBar = default(Rectangle);
				if (Reverse)
				{
					ColoredBar = new Rectangle(W - BarWidth - 1, 2, BarWidth, BarHeight);
				}
				else
				{
					ColoredBar = new Rectangle(2, 2, BarWidth, BarHeight);
				}
				/*
				using (LinearGradientBrush brush = new LinearGradientBrush(ColoredBar, this.FillColor, C, 90f))
				{
					float[] relativeIntensities = {
						0.1f,
						1f,
						1f,
						1f,
						1f,
						0.85f,
						0.1f
					};
					float[] relativePositions = {
						0f,
						0.2f,
						0.5f,
						0.5f,
						0.5f,
						0.8f,
						1f
					};

					// create a Blend object and assign it to brush
					Blend blend = new Blend();
					blend.Factors = relativeIntensities;
					blend.Positions = relativePositions;
					brush.Blend = blend;

					G.FillRectangle(brush, ColoredBar);
				}
				*/
				using (SolidBrush brush = new SolidBrush(C))
				{
					G.FillRectangle(brush, ColoredBar);
				}
				
			}
		}

		protected virtual void DrawTick(Graphics g)
		{
			//SALVA HEIGHT E WIDTH PER CALCOLI PIU VELOCI
			int W = this.ClientRectangle.Width;
			int H = this.ClientRectangle.Height;

			int CurPos = L + 3;
			while (CurPos < W)
			{
				using (Pen P = new Pen(this.FillColor, S))
				{
					g.DrawLine(P, (CurPos), 0, (CurPos), this.Height);
				}
				CurPos += S+L;
			}
		}


		protected override void SetBoundsCore(int x, int y, int width, int height, System.Windows.Forms.BoundsSpecified specified)
		{
			width = Math.Max(width, 20);
			height = Math.Max(height, 6);

			int calcwidth = width;
			if (FillStyle == FillStyles.Dashed)
				calcwidth = (Convert.ToInt32(calcwidth / 8) * 8) + 1;
			base.SetBoundsCore(x, y, calcwidth, height, specified);
		}


	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
