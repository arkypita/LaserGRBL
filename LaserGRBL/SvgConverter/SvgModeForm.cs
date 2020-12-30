using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL.SvgConverter
{
	public partial class SvgModeForm : Form
	{
		public Mode Result { get; private set; } = Mode.None;

		public enum Mode { Vector, Raster, None }


		public SvgModeForm()
		{
			InitializeComponent();
		}

		internal static Mode CreateAndShow(Form parent, string filename)
		{
			SvgModeForm frm = new SvgModeForm();
			frm.CreatePreview(filename);
			frm.ShowDialog(parent);
			frm.PbVector.Image.Dispose();
			frm.PbImage.Image.Dispose();
			frm.Dispose();

			return frm.Result;
		}

		private void CreatePreview(string filename)
		{

			string fcontent = System.IO.File.ReadAllText(filename);
			Svg.SvgDocument svg = Svg.SvgDocument.FromSvg<Svg.SvgDocument>(fcontent);
			svg.Ppi = 600;

			PbImage.Image = svg.Draw();
			PbVector.Image = svg.Draw(true);
		}

		private void PbVector_Click(object sender, EventArgs e)
		{
			Result = Mode.Vector;
			Close();
		}

		private void PbImage_Click(object sender, EventArgs e)
		{
			Result = Mode.Raster;
			Close();
		}
	}

}
