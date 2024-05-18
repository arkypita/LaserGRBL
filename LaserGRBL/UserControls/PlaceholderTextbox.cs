//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace LaserGRBL.UserControls
{
	public class PlaceholderTextBox : TextBox
	{
		#region Fields

		#region Protected Fields

		protected string _waterMarkText = ""; //The watermark text
		protected Color _waterMarkColor; //Color of the watermark when the control does not have focus
		protected Color _waterMarkActiveColor; //Color of the watermark when the control has focus

		public Color WaterMarkColor
		{
            get => _waterMarkColor;
            set => _waterMarkColor = value;
        }
        public Color WaterMarkActiveColor
        {
            get => _waterMarkActiveColor;
            set => _waterMarkActiveColor = value;
        }

        #endregion

        #region Private Fields

        private Panel waterMarkContainer; //Container to hold the watermark
		private Font waterMarkFont; //Font of the watermark
		private SolidBrush waterMarkBrush; //Brush for the watermark

		#endregion

		#endregion

		#region Constructors

		public PlaceholderTextBox()
		{
			Initialize();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Initializes watermark properties and adds CtextBox events
		/// </summary>
		private void Initialize()
		{
			//Sets some default values to the watermark properties
			_waterMarkColor = Color.LightGray;
			_waterMarkActiveColor = Color.Gray;
			waterMarkFont = this.Font;
			waterMarkBrush = new SolidBrush(_waterMarkActiveColor);
			waterMarkContainer = null;
			BorderStyle = BorderStyle.None;
			//Draw the watermark, so we can see it in design time
			DrawWaterMark();

			//Eventhandlers which contains function calls. 
			//Either to draw or to remove the watermark
			this.Enter += new EventHandler(ThisHasFocus);
			this.Leave += new EventHandler(ThisWasLeaved);
			this.TextChanged += new EventHandler(ThisTextChanged);
		}

		/// <summary>
		/// Removes the watermark if it should
		/// </summary>
		private void RemoveWaterMark()
		{
			if (waterMarkContainer != null)
			{
				this.Controls.Remove(waterMarkContainer);
				waterMarkContainer = null;
			}
		}

		/// <summary>
		/// Draws the watermark if the text length is 0
		/// </summary>
		private void DrawWaterMark()
		{
			if (this.waterMarkContainer == null && this.TextLength <= 0)
			{
				waterMarkContainer = new Panel(); // Creates the new panel instance
				waterMarkContainer.Paint += new PaintEventHandler(waterMarkContainer_Paint);
				waterMarkContainer.Invalidate();
				waterMarkContainer.Click += new EventHandler(waterMarkContainer_Click);
				this.Controls.Add(waterMarkContainer); // adds the control
			}
		}

		#endregion

		#region Eventhandlers

		#region WaterMark Events

		private void waterMarkContainer_Click(object sender, EventArgs e)
		{
			this.Focus(); //Makes sure you can click wherever you want on the control to gain focus
		}

		private void waterMarkContainer_Paint(object sender, PaintEventArgs e)
		{
			BackColor = ColorScheme.LogBackColor;
			ForeColor = ColorScheme.FormForeColor;
			//Setting the watermark container up
			waterMarkContainer.Location = new Point(2, 0); // sets the location
			waterMarkContainer.Height = this.Height; // Height should be the same as its parent
			waterMarkContainer.Width = this.Width; // same goes for width and the parent
			waterMarkContainer.Anchor = AnchorStyles.Left | AnchorStyles.Right; // makes sure that it resizes with the parent control



			if (this.ContainsFocus)
			{
				//if focused use normal color
				waterMarkBrush = new SolidBrush(this._waterMarkActiveColor);
			}

			else
			{
				//if not focused use not active color
				waterMarkBrush = new SolidBrush(this._waterMarkColor);
			}

			//Drawing the string into the panel 
			Graphics g = e.Graphics;
			g.DrawString(this._waterMarkText, waterMarkFont, waterMarkBrush, new PointF(-2f, 1f));//Take a look at that point
			//The reason I'm using the panel at all, is because of this feature, that it has no limits
			//I started out with a label but that looked very very bad because of its paddings 

		}

		#endregion

		#region CTextBox Events

		private void ThisHasFocus(object sender, EventArgs e)
		{
			//if focused use focus color
			waterMarkBrush = new SolidBrush(this._waterMarkActiveColor);

			//The watermark should not be drawn if the user has already written some text
			if (this.TextLength <= 0)
			{
				RemoveWaterMark();
				DrawWaterMark();
			}
		}

		private void ThisWasLeaved(object sender, EventArgs e)
		{
			//if the user has written something and left the control
			if (this.TextLength > 0)
			{
				//Remove the watermark
				RemoveWaterMark();
			}
			else
			{
				//But if the user didn't write anything, Then redraw the control.
				this.Invalidate();
			}
		}

		private void ThisTextChanged(object sender, EventArgs e)
		{
			//If the text of the textbox is not empty
			if (this.TextLength > 0)
			{
				//Remove the watermark
				RemoveWaterMark();
			}
			else
			{
				//But if the text is empty, draw the watermark again.
				DrawWaterMark();
			}
		}

		#region Overrided Events

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			//Draw the watermark even in design time
			DrawWaterMark();
		}

		protected override void OnInvalidated(InvalidateEventArgs e)
		{
			base.OnInvalidated(e);
			//Check if there is a watermark
			if (waterMarkContainer != null)
				//if there is a watermark it should also be invalidated();
				waterMarkContainer.Invalidate();
		}

		#endregion

		#endregion

		#endregion

		#region Properties
		[Localizable(true)]
		[Category("Watermark attribtues")]
		[Description("Sets the text of the watermark")]
		public string WaterMark
		{
			get { return this._waterMarkText; }
			set
			{
				this._waterMarkText = value;
				this.Invalidate();
			}
		}

		[Category("Watermark attribtues")]
		[Description("When the control gaines focus, this color will be used as the watermark's forecolor")]
		public Color WaterMarkActiveForeColor
		{
			get { return this._waterMarkActiveColor; }

			set
			{
				this._waterMarkActiveColor = value;
				this.Invalidate();
			}
		}

		[Category("Watermark attribtues")]
		[Description("When the control looses focus, this color will be used as the watermark's forecolor")]
		public Color WaterMarkForeColor
		{
			get { return this._waterMarkColor; }

			set
			{
				this._waterMarkColor = value;
				this.Invalidate();
			}
		}

		[Category("Watermark attribtues")]
		[Description("The font used on the watermark. Default is the same as the control")]
		public Font WaterMarkFont
		{
			get
			{
				return this.waterMarkFont;
			}

			set
			{
				this.waterMarkFont = value;
				this.Invalidate();
			}
		}

		#endregion
	}
}