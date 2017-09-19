
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

	[Description("Color Progress Bar")]
	public partial class DoubleProgressBar : ColorProgressBar
	{

		#region " Codice generato da Progettazione Windows Form "

		public DoubleProgressBar() : base()
		{
			//Chiamata richiesta da Progettazione Windows Form.
			InitializeComponent();

			//Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()
			base.FillStyle = FillStyles.Solid;
		}

		//Richiesto da Progettazione Windows Form

		#endregion

		public class Bar
		{
			private double m_value;

			private Color m_color;
			public Bar(System.Drawing.Color BarColor)
			{
				m_color = BarColor;
			}

			public double Value
			{
				get { return m_value; }
				set { m_value = value; }
			}

			public Color BarColor
			{
				get { return m_color; }
				set { m_color = value; }
			}

		}

		public class Reference
		{
			private double m_value;
			private Color m_color;
			private Color m_bcolor;

			private bool m_bottom;
			public Reference(Color ReferenceColor, Color BorderColor, bool bottom)
			{
				m_color = ReferenceColor;
				m_bcolor = BorderColor;
				m_bottom = bottom;
			}

			public double Value
			{
				get { return m_value; }
				set { m_value = value; }
			}

			public Color ReferenceColor
			{
				get { return m_color; }
				set { m_color = value; }
			}

			public Color BorderColor
			{
				get { return m_bcolor; }
				set { m_bcolor = value; }
			}

			public bool Bottom
			{
				get { return m_bottom; }
				set { m_bottom = value; }
			}


		}

		private System.Collections.Generic.List<Bar> m_Bars = new System.Collections.Generic.List<Bar>();

		private System.Collections.Generic.List<Reference> m_References = new System.Collections.Generic.List<Reference>();
		public System.Collections.Generic.IList<Bar> Bars
		{
			get { return m_Bars; }
		}

		public System.Collections.Generic.IList<Reference> References
		{
			get { return m_References; }
		}

		private string m_PercString;
		public string PercString
		{
			get { return m_PercString; }
			set { m_PercString = value; }
		}



		protected override void DrawProgres(System.Drawing.Graphics g)
		{
			//SALVA HEIGHT E WIDTH PER CALCOLI PIU VELOCI
			int W = this.ClientRectangle.Width;
			int H = this.ClientRectangle.Height;

			foreach (Bar B in Bars)
			{
				DrawBar(g, W, H, B.Value, B.BarColor);
			}

			foreach (Reference R in References)
			{
				DrawRef(g, W, H, R.Value, R.ReferenceColor, R.BorderColor, R.Bottom);
			}

		}


		private void DrawRef(Graphics G, int W, int H, double V, Color C, Color BC, bool Bottom)
		{
			int RefPos = 0;

			if (!double.IsNaN(V) && !double.IsInfinity(V) && !((Maximum - Minimum) == 0)) {
				RefPos = Convert.ToInt32(Math.Floor(((W - 3) * (Math.Min(V, Maximum) - Minimum)) / (Maximum - Minimum)));
				if (RefPos == 0)
					RefPos = 1;
				//se con i conti mi viene 0 lo sposto un pò più in la per riuscire a disegnare

				SmoothingMode smoothingModeTmp = G.SmoothingMode;
				G.SmoothingMode = SmoothingMode.AntiAlias;

				using (SolidBrush B = new SolidBrush(C)) {
					using (Pen P = new Pen(BC)) {
						Point[] points = {
							new Point(RefPos - 3, 0),
							new Point(RefPos + 3, 0),
							new Point(RefPos, 6)
						};
						if (Bottom)
							points = new Point[] {
								new Point(RefPos - 3, Height),
								new Point(RefPos + 3, Height),
								new Point(RefPos, Height - 6)
							};

						G.FillPolygon(B, points);
						G.DrawPolygon(P, points);
					}
				}

				G.SmoothingMode = smoothingModeTmp;

			}
		}


		protected override void DrawString(Graphics g)
		{
			string perc = m_PercString;
			//String.Format("{0:p" & ProgressStringDecimals.ToString & "}", (Value - Minimum) / (Maximum - Minimum))
			using (SolidBrush B = new SolidBrush(ForeColor))
			{
				g.DrawString(perc, Font, B, Convert.ToInt32((Width - g.MeasureString(perc, Font).Width) / 2), Convert.ToInt32((Height - g.MeasureString(perc, Font).Height) / 2));
			}
		}

	}

}

