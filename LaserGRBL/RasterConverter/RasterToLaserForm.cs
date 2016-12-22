using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using CsPotrace;

namespace LaserGRBL.RasterConverter
{
	public partial class RasterToLaserForm : Form
	{
		GrblFile mFile;
		Image mOriginal;
		Image mScaled;
		Image mConverted;

		string mFileName;


		double scaleX = 1.0; //square ratio


		private RasterToLaserForm(GrblFile file, string filename)
		{
			InitializeComponent();
			mFile = file;
			mFileName = filename;

			mOriginal = Image.FromFile(filename);
			scaleX = (double)mOriginal.Width / (double)mOriginal.Height;

			Size scaleSize = CalculateResizeToFit(mOriginal.Size, PbConverted.Size);
			mScaled = ImageTransform.ResizeImage(mOriginal, scaleSize.Width, scaleSize.Height);

			PbOriginal.Image = mScaled;

			CbMode.SuspendLayout();
			CbMode.Items.Add(ImageTransform.Formula.SimpleAverage);
			CbMode.Items.Add(ImageTransform.Formula.WeightAverage);
			CbMode.SelectedItem = ImageTransform.Formula.SimpleAverage;
			CbMode.ResumeLayout();

			RefreshPreview();
			RefreshSizes();
		}

		private static Size CalculateResizeToFit(Size imageSize, Size boxSize)
		{
			// TODO: Check for arguments (for null and <=0)
			var widthScale = boxSize.Width / (double)imageSize.Width;
			var heightScale = boxSize.Height / (double)imageSize.Height;
			var scale = Math.Min(widthScale, heightScale);
			return new Size(
				(int)Math.Round((imageSize.Width * scale)),
				(int)Math.Round((imageSize.Height * scale))
				);
		}

		internal static void CreateAndShowDialog(GrblFile file, string filename)
		{
			RasterToLaserForm f = new RasterToLaserForm(file, filename);
			f.ShowDialog();
			f.Dispose();
		}

		private void RefreshPreview()
		{
			TbThreshold.Enabled = CbThreshold.Checked;

			using (Bitmap gs = ImageTransform.GrayScale(mScaled, TBRed.Value / 100.0F, TBGreen.Value / 100.0F, TBBlue.Value / 100.0F, (ImageTransform.Formula)CbMode.SelectedItem))
			{
				using (Bitmap bc = ImageTransform.BrightnessContrast(gs, -((100 - TbBright.Value) / 100.0F), (TbContrast.Value / 100.0F)))
				{
					Bitmap th = ImageTransform.Threshold(bc, TbThreshold.Value / 100.0F, CbThreshold.Checked);

					if (RbLineToLineTracing.Checked)
						PreviewLineByLine(th);
					else if (RbVectorize.Checked)
						PreviewVector(th);

				}
			}
		}



		private void PreviewVector(Bitmap th)
		{
			ArrayList ListOfCurveArray = new ArrayList();
            //Potrace.turdsize = Convert.ToInt32(textBox2.Text);
            //Potrace.alphamax = Convert.ToDouble(textBox5.Text);
            //Potrace.opttolerance = Convert.ToDouble(textBox3.Text);
            //Potrace.curveoptimizing = checkBox4.Checked; //optimize the path p, replacing sequences of Bezier segments by a single segment when possible.
            
            bool[,] Matrix = Potrace.BitMapToBinary(th, 125);
           	Potrace.potrace_trace(Matrix, ListOfCurveArray);

           	using (Graphics g = Graphics.FromImage(th))
				g.Clear(Color.White);
           	
           	drawVector(ListOfCurveArray, th);
           	
			PbConverted.SuspendLayout();
			if (mConverted != null)
				mConverted.Dispose();
			mConverted = th;
			PbConverted.Image = mConverted;
			PbConverted.ResumeLayout();
		}
		
		private void drawVector(ArrayList ListOfCurveArray, Bitmap bmp)
        {
            if (ListOfCurveArray == null) return;
            Graphics g = Graphics.FromImage(bmp);
            GraphicsPath gp = new GraphicsPath();
            for (int i = 0; i < ListOfCurveArray.Count; i++)
            {   
                ArrayList CurveArray = (ArrayList)ListOfCurveArray[i];
                GraphicsPath Contour=null;
                GraphicsPath Hole = null;
                GraphicsPath Current=null;

                for (int j = 0; j < CurveArray.Count; j++)
                {

                    if (j == 0)
                    {
                        Contour = new GraphicsPath();
                        Current = Contour;
                    }
                    else
                    {
                        
                        Hole = new GraphicsPath();
                        Current = Hole;
      
                    }
                    Potrace.Curve[] Curves = (Potrace.Curve[])CurveArray[j];
                    float factor = 1;
                    if (true)
                        factor = 1;
                    for (int k = 0; k < Curves.Length; k++)
                    {
                        if (Curves[k].Kind == Potrace.CurveKind.Bezier)
                            Current.AddBezier((float)Curves[k].A.X * factor, (float)Curves[k].A.Y * factor, (float)Curves[k].ControlPointA.X * factor, (float)Curves[k].ControlPointA.Y * factor,
                                        (float)Curves[k].ControlPointB.X * factor, (float)Curves[k].ControlPointB.Y * factor, (float)Curves[k].B.X * factor, (float)Curves[k].B.Y * factor);
                        else
                            Current.AddLine((float)Curves[k].A.X * factor, (float)Curves[k].A.Y * factor, (float)Curves[k].B.X * factor, (float)Curves[k].B.Y * factor);

                    }
                    if (j > 0) Contour.AddPath(Hole, false);
                }
                gp.AddPath(Contour, false);
            }

            
            if (true)
            g.FillPath(Brushes.Black, gp);
            if (true)
            g.DrawPath(Pens.Red,gp);

       	 	if (true)
       	 	drawPoints(ListOfCurveArray, bmp);


        }
        private void drawPoints(ArrayList ListOfCurveArray, Bitmap bmp)
        {
            if (ListOfCurveArray == null) return;
            Graphics g = Graphics.FromImage(bmp);
            for (int i = 0; i < ListOfCurveArray.Count; i++)
            {
                ArrayList CurveArray = (ArrayList)ListOfCurveArray[i];
                for (int j = 0; j < CurveArray.Count; j++)
                {
                    Potrace.Curve[] Curves = (Potrace.Curve[])CurveArray[j];
                   
                    float factor = 1;
                    if (true)
                        factor = 1;
                    for (int k = 0; k < Curves.Length; k++)
                    {
                        g.FillRectangle(Brushes.Yellow, (float)((Curves[k].A.X) * factor - 1.5), (float)((Curves[k].A.Y) * factor - 1.5), 3, 3);
                    }
                }
            }
        }
		
		
		
		

		private void PreviewLineByLine(Bitmap th)
		{
			if (CbLinePreview.Checked)
			{
				using (Graphics g = Graphics.FromImage(th))
				{
					g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

					int mod = 7 - (int)UDQuality.Value / 3;
					for (int Y = 0; Y < th.Height; Y++)
					{
						using (Pen mark = new Pen(Color.FromArgb(0, 255, 255, 255), 1F))
						{
							using (Pen nomark = new Pen(Color.FromArgb(255, 255, 255, 255), 1F))
							{
								if (Y % mod == 0)
									g.DrawLine(mark, 0, Y, th.Width, Y);
								else
									g.DrawLine(nomark, 0, Y, th.Width, Y);
							}
						}
					}
				}
			}

			PbConverted.SuspendLayout();
			if (mConverted != null)
				mConverted.Dispose();
			mConverted = th;
			PbConverted.Image = mConverted;
			PbConverted.ResumeLayout();
		}

		private void RefreshSizes()
		{
			const double milimetresPerInch = 25.4;

			int H = (int)(mOriginal.Height / mOriginal.VerticalResolution * milimetresPerInch);
			int W = (int)(H * scaleX);

			IISizeW.CurrentValue = W;
			IISizeH.CurrentValue = H;
		}


		private void OnSelectorChange(object sender, EventArgs e)
		{
			RefreshPreview();
		}

		void GoodInput(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				e.Handled = true;
		}

		void BtnCreateClick(object sender, EventArgs e)
		{
			int H = IISizeH.CurrentValue * (int)UDQuality.Value;
			int W = IISizeW.CurrentValue * (int)UDQuality.Value;

			using (Bitmap res = ImageTransform.ResizeImage(mOriginal, W, H))
			{
				using (Bitmap gs = ImageTransform.GrayScale(res, TBRed.Value / 100.0F, TBGreen.Value / 100.0F, TBBlue.Value / 100.0F, (ImageTransform.Formula)CbMode.SelectedItem))
				{
					using (Bitmap bc = ImageTransform.BrightnessContrast(gs, -((100 - TbBright.Value) / 100.0F), (TbContrast.Value / 100.0F)))
					{
						using (Bitmap th = ImageTransform.Threshold(bc, TbThreshold.Value / 100.0F, CbThreshold.Checked))
						{
							using (Bitmap killed = ImageTransform.KillAlfa(th))
							{
								mFile.LoadImage(killed, mFileName, (int)UDQuality.Value, IIOffsetX.CurrentValue, IIOffsetY.CurrentValue, IIMarkSpeed.CurrentValue, IITravelSpeed.CurrentValue);
							}
						}
					}
				}
			}

			Close();
		}

		private void IISizeW_CurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			if (ByUser)
				IISizeH.CurrentValue = (int)(NewValue / scaleX);
		}

		private void IISizeH_CurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			if (ByUser)
				IISizeW.CurrentValue = (int)(NewValue * scaleX);
		}

		private static bool IsOdd(int value)
		{ return value % 2 != 0; }

		private void OnGeneratorChange(object sender, EventArgs e)
		{
			GbLineToLineOptions.Visible = RbLineToLineTracing.Checked;
			GbVectorizeOptions.Visible = RbVectorize.Checked;
			RefreshPreview();
		}

		private void UDQuality_ValueChanged(object sender, EventArgs e)
		{
			RefreshPreview();
		}


	}
}
