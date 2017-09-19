using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace LaserGRBL.UserControls.IntegerInput
{
	[Designer(typeof(System.Windows.Forms.Design.ParentControlDesigner))]
	public class ColoredBorderControl : System.Windows.Forms.UserControl
	{

		//UserControl esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if ((components != null)) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		//Richiesto da Progettazione Windows Form

		private System.ComponentModel.IContainer components;
		//NOTA: la procedura che segue è richiesta da Progettazione Windows Form
		//Può essere modificata in Progettazione Windows Form.  
		//Non modificarla nell'editor del codice.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.SuspendLayout();
			//
			//ColoredBorderControl
			//
			//Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			//Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			this.Name = "ColoredBorderControl";
			this.ResumeLayout(false);
		}


		private Color _bordercolor = Color.Black;
		public Color BorderColor {
			get { return _bordercolor; }
			set {
				if (!value.Equals(_bordercolor)) {
					_bordercolor = value;
					Invalidate();
				}
			}
		}

		private void ResetBorderColor()
		{
			BorderColor = Color.Black;
		}

		private bool ShouldSerializeBorderColor()
		{
			return !BorderColor.Equals(Color.Black);
		}


		private int _bordersize = 1;
		[DefaultValue(1)]
		public int BorderSize {
			get { return _bordersize; }
			set {
				if (!(value == _bordersize)) {
					_bordersize = value;
					Invalidate();
				}
			}
		}

		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			using (Pen p = new Pen(BorderColor, BorderSize)) {
				p.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
				e.Graphics.DrawRectangle(p, 0, 0, this.Width - 1, this.Height - 1);
			}
		}

		public ColoredBorderControl()
		{
			this.InitializeComponent();
			this.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
			this.SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw, true);
		}

		public override Rectangle DisplayRectangle {
			get {
				Size clientSize = base.ClientSize;
				return new Rectangle(_bordersize, _bordersize, Math.Max((clientSize.Width - _bordersize - 1), 0), Math.Max(((clientSize.Height - _bordersize - 1)), 0));
			}
		}


	}

}
