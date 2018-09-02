using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LaserGRBL;

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

			if (mCom != null)
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

				using (StringFormat esf = new StringFormat(StringFormat.GenericTypographic))
				{
					esf.Trimming = StringTrimming.EllipsisCharacter;

					for (int i = 0; i < mDraw.Count; i++)
					{
						IGrblRow cmd = mDraw[i];
						float respectX = 0;
						if (cmd.GetResult(mCom.SupportCSV, mUseImages) != null)
						{
							respectX = e.Graphics.MeasureString(cmd.GetResult(mCom.SupportCSV, mUseImages), Font).Width;
							using (Brush b = new SolidBrush(cmd.RightColor))
								e.Graphics.DrawString(cmd.GetResult(mCom.SupportCSV, mUseImages), Font, b, Width - ScrollBar.Width - 1, RowHeight * i + 2, new StringFormat(StringFormatFlags.DirectionRightToLeft));
						}
						if (cmd.GetMessage() != null)
						{
							using (Brush b = new SolidBrush(cmd.LeftColor))
								e.Graphics.DrawString(cmd.GetMessage(), Font, b, new RectangleF(mUseImages && cmd.ImageIndex >= 0 ? RowHeight + 1 : 1, RowHeight * i + 1, Width - ScrollBar.Width - (mUseImages ? IL.ImageSize.Width + 2 : 2) - respectX, RowHeight - 2), esf);
							//e.Graphics.DrawString(cmd.GetMessage(), Font, b, mUseImages && cmd.ImageIndex >= 0 ? RowHeight + 1 : 1, RowHeight * i + 1);
						}
						if (mUseImages && cmd.ImageIndex >= 0)
						{
							System.Drawing.Image I = IL.Images[cmd.ImageIndex];
							int iW = RowHeight - 2;
							int iH = RowHeight - 2;
							e.Graphics.DrawImage(I, /*Width - ScrollBar.Width - iW - 2*/ 1, RowHeight * i + (RowHeight - iH) / 2, iW, iH);
						}

						e.Graphics.DrawLine(Pens.LightGray, 0, RowHeight * (i + 1), Width, RowHeight * (i + 1));
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
	}
}
