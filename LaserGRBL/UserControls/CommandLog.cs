//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LaserGRBL.UserControls
{
    public partial class CommandLog : UserControl
	{
		GrblCore mCom;
		int RowHeight = 16;
		System.Collections.Generic.List<IGrblRow> mDraw;
		int mPosition = -1;
		bool mUseImages = true;

		public CommandLog()
		{
			InitializeComponent();
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
        }

		public void SetCom(GrblCore core)
		{ mCom = core; }

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(ColorScheme.LogBackColor);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (mCom != null && e != null && e.Graphics != null)
			{
				e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
				
				RowHeight = Font.Height +2;

				int queueCount = mCom.Executed;
				int controlCapacity = Height / RowHeight;

				if (queueCount > controlCapacity)
				{
					bool keepScrolling = (ScrollBar.Value >= ScrollBar.Maximum - 10);

					ScrollBar.Minimum = 0;
					ScrollBar.Maximum = (queueCount - controlCapacity);

					if (keepScrolling)
						ScrollBar.Value = ScrollBar.Maximum;

				}
				else
				{
					ScrollBar.Minimum = 0;
					ScrollBar.Value = 0;
					ScrollBar.Maximum = 0; 
				}




				int howmany = Math.Min(Height / RowHeight, queueCount);
				int index = ScrollBar.Value;
				mDraw = mCom.SentCommand(index, howmany);
                if (mDraw != null)
				{
					using (StringFormat esf = new StringFormat(StringFormat.GenericTypographic))
					{
						esf.Trimming = StringTrimming.EllipsisCharacter;

						for (int i = 0; i < mDraw.Count; i++)
						{
							IGrblRow cmd = mDraw[i];
							if (cmd != null)
							{
								float respectX = 0;
								string result = cmd.GetResult(mCom.SupportCSV, mUseImages);
								string message = cmd.GetDecodedMessage();
								if (result != null)
								{
									respectX = e.Graphics.MeasureString(result, Font).Width;
									using (Brush b = new SolidBrush(cmd.RightColor))
										e.Graphics.DrawString(result, Font, b, Width - ScrollBar.Width - 1, RowHeight * i + 2, new StringFormat(StringFormatFlags.DirectionRightToLeft));
								}
								if (message != null)
								{
									using (Brush b = new SolidBrush(cmd.LeftColor))
										e.Graphics.DrawString(message, Font, b, new RectangleF(mUseImages && cmd.ImageIndex >= 0 ? RowHeight + 1 : 1, RowHeight * i + 1, Width - ScrollBar.Width - (mUseImages ? IL.ImageSize.Width + 2 : 2) - respectX, RowHeight - 2), esf);
									//e.Graphics.DrawString(cmd.GetMessage(), Font, b, mUseImages && cmd.ImageIndex >= 0 ? RowHeight + 1 : 1, RowHeight * i + 1);
								}
								if (mUseImages && cmd.ImageIndex >= 0 && cmd.ImageIndex < IL.Images.Count)
								{
									Image I = IL.Images[cmd.ImageIndex];
									if (I != null)
									{
										int iW = RowHeight - 2;
										int iH = RowHeight - 2;
										e.Graphics.DrawImage(I, 1, RowHeight * i + (RowHeight - iH) / 2, iW, iH);
									}
								}
							}
						}
					}
				}
			}
		}

		public void TimerUpdate()
		{
			Invalidate();
		}
		
		void CommandLogMouseMove(object sender, MouseEventArgs e)
		{
			int idx = e.Y / RowHeight;
			if (mDraw != null && idx < mDraw.Count)
				SetPosition(idx);
			else
				SetPosition(-1);
		}
		
		void SetPosition(int position)
		{
			if (mPosition != position)
			{
				if (mDraw != null && position >= 0 && position < mDraw.Count)
					TT.SetToolTip(this, Tools.WordWrap.WrapString(mDraw[position].GetToolTip(mCom.SupportCSV), 60));
				else
					TT.SetToolTip(this, null);
			
				mPosition = position;
			}
		}

		public void OnColorChange()
		{
            ThemeMgr.SetTheme(ScrollBar);
        }

	}
}
