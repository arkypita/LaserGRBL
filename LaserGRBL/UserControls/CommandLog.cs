using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LaserGRBL;

namespace LaserGRBL.UserControls
{
	public partial class CommandLog : UserControl
	{
		GrblCom mCom;
		int RowHeight = 16;
		System.Collections.Generic.List<IGrblRow> mDraw;
		int mPosition = -1;

		public CommandLog()
		{
			InitializeComponent();
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
		}

		public void SetCom(GrblCom com)
		{mCom = com;}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (mCom != null)
			{
				//e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				//e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				//e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
				e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
				
				RowHeight = Font.Height +2;

				int queueCount = mCom.Executed;
				int controlCapacity = Height / RowHeight;

				if (queueCount > controlCapacity)
				{
					bool keepScrolling = (ScrollBar.Value == ScrollBar.Maximum);

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

				for (int i = 0; i < mDraw.Count; i++)
				{
					IGrblRow cmd = mDraw[i];

					if (cmd.LeftString != null)
					{
						using (Brush b = new SolidBrush(cmd.LeftColor))
							e.Graphics.DrawString(cmd.LeftString, Font, b, 1, RowHeight * i + 1);
					}

					if (cmd.RightString != null)
					{
						using (Brush b = new SolidBrush(cmd.RightColor))
							e.Graphics.DrawString(cmd.RightString, Font, b, Width - ScrollBar.Width - 1, RowHeight * i + 2, new StringFormat(StringFormatFlags.DirectionRightToLeft));
					}

					e.Graphics.DrawLine(Pens.LightGray, 0, RowHeight * (i + 1), Width, RowHeight * (i + 1));
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
					TT.SetToolTip(this, Tools.WordWrap.WrapString(mDraw[position].ToolTip, 60));
				else
					TT.SetToolTip(this, null);
			
				mPosition = position;
			}
		}
	}
}
