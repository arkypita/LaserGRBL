﻿using System;
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

		internal static Mode CreateAndShow(string filename)
		{
			SvgModeForm frm = new SvgModeForm();
			frm.CreatePreview(filename);
			frm.ShowDialog();
			frm.PbVector.Image.Dispose();
			frm.PbImage.Image.Dispose();
			frm.Dispose();

			return frm.Result;
		}

		private void CreatePreview(string filename)
		{
			throw new NotImplementedException();
			//string fcontent = System.IO.File.ReadAllText(filename);
			//Svg.SvgDocument svg = Svg.SvgDocument.FromSvg<Svg.SvgDocument>(fcontent);
			//svg.Ppi = 600;
			//
			//PbImage.Image = svg.Draw();
			//PbVector.Image = svg.Draw(true);
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
