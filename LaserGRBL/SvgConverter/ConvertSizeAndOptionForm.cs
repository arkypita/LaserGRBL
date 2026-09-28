//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using LaserGRBL.PSHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LaserGRBL.RasterConverter;
using Svg;
using LaserGRBL.Icons;
using LaserGRBL.UserControls;

namespace LaserGRBL.SvgConverter
{
	/// <summary>
	/// Description of ConvertSizeAndOptionForm.
	/// </summary>
	public partial class SvgToGCodeForm : Form
	{
		GrblCore mCore;
		bool supportPWM = Settings.GetObject("Support Hardware PWM", true);

		public ComboboxItem[] LaserOptions = new ComboboxItem[] { new ComboboxItem("M3 - Constant Power", "M3"), new ComboboxItem("M4 - Dynamic Power", "M4") };

		private List<SvgColorLayer> mLayers;
		private VectorImportSource mSource;
		private SvgLayerPreview mPreview;
		private SvgLayerAccordion mAccordion;
		public class ComboboxItem
		{
			public string Text { get; set; }
			public object Value { get; set; }

			public ComboboxItem(string text, object value)
			{ Text = text; Value = value; }

			public override string ToString()
			{
				return Text;
			}
		}

		internal static void CreateAndShowDialog(GrblCore core, string filename, bool append)
		{
			CreateAndShowDialog(core, filename, null, append);
		}

		// source: the file already loaded (svg or dxf), null to read the svg from filename
		internal static void CreateAndShowDialog(GrblCore core, string filename, VectorImportSource source, bool append)
        {
            List<SvgColorLayer> layers;
            try
            {
                if (source == null)
                    source = new SvgImportSource(GCodeFromSVG.ParseSvgFile(filename));
                layers = source.ScanLayers();
            }
            catch (Exception ex)
            {
                Logger.LogException("SvgColorLayer", ex);
                layers = new List<SvgColorLayer>();
            }

            using (SvgToGCodeForm f = new SvgToGCodeForm(core, layers, source))
            {
                f.ShowDialogForm();
                if (f.DialogResult == DialogResult.OK)
                {
                    Settings.SetObject("GrayScaleConversion.VectorizeOptions.BorderSpeed", f.IIBorderTracing.CurrentValue);
                    Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMax", f.IIMaxPower.CurrentValue);
					Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMin", f.IIMinPower.CurrentValue);
					Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOn", (f.CBLaserON.SelectedItem as ComboboxItem).Value);

					foreach (SvgColorLayer layer in layers)
						layer.SaveSettings();

					core.LoadedFile.LoadImportedVector(filename, source, append, core, layers);
                }
            }
        }

        private SvgToGCodeForm(GrblCore core, List<SvgColorLayer> layers, VectorImportSource source)
		{
			InitializeComponent();
			ThemeMgr.SetTheme(this);
            IconsMgr.PrepareButton(BtnCreate, "mdi-checkbox-marked");
            IconsMgr.PrepareButton(BtnCancel, "mdi-close-box");
			IconsMgr.PrepareButton(BtnOnOffInfo, "mdi-information-slab-box", new Size(16, 16));
            IconsMgr.PrepareButton(BtnModulationInfo, "mdi-information-slab-box", new Size(16, 16));
            IconsMgr.PrepareButton(BtnPSHelper, "mdi-information-slab-box", new Size(16, 16));
            IconsMgr.PrepareButton(BtnColorFilter, "mdi-information-slab-box", new Size(16, 16));
            mCore = core;
            mLayers = layers;
            mSource = source;

			BackColor = ColorScheme.FormBackColor;
			GbLaser.ForeColor = GbSpeed.ForeColor = ForeColor = ColorScheme.FormForeColor;
			BtnCancel.BackColor = BtnCreate.BackColor = ColorScheme.FormButtonsColor;

			LblSmin.Visible = LblSmax.Visible = IIMaxPower.Visible = IIMinPower.Visible = BtnModulationInfo.Visible = supportPWM;
			AssignMinMaxLimit();

			CBLaserON.Items.Add(LaserOptions[0]);
			CBLaserON.Items.Add(LaserOptions[1]);

			CreateLayout();
		}

		#region Preview and color layers

		// the designer layout (settings column + buttons) is kept for the translations: here it becomes the left column
		// of a resizable window, with the preview on the right and the color layers accordion in the former filter box
		private void CreateLayout()
		{
			SuspendLayout();

			int leftWidth = tableLayoutPanel9.PreferredSize.Width;
			float k = leftWidth / 355f; // designer width, to scale the sizes below with the dpi

			Controls.Remove(tableLayoutPanel9);
			tableLayoutPanel9.Controls.Remove(tableLayoutPanel1);
			tableLayoutPanel9.AutoSize = false;
			tableLayoutPanel9.Dock = DockStyle.Fill;
			tableLayoutPanel9.Margin = Padding.Empty;
			tableLayoutPanel9.RowStyles.Clear();
			for (int i = 0; i < tableLayoutPanel9.RowCount; i++)
				tableLayoutPanel9.RowStyles.Add(i == 3 ? new RowStyle(SizeType.Percent, 100) : new RowStyle(SizeType.AutoSize));

			tableLayoutPanel2.Visible = false; // old single color filter
			gbFilter.AutoSize = false;
			gbFilter.Dock = DockStyle.Fill;
			gbFilter.Text = Strings.SvgLayersTitle;
			gbFilter.ForeColor = ColorScheme.FormForeColor;

			mAccordion = new SvgLayerAccordion();
			mAccordion.Dock = DockStyle.Fill;
			mAccordion.LayerClicked += SelectLayer;
			mAccordion.LayerChanged += delegate (SvgColorLayer layer) { mPreview.LayerChanged(layer); };
			gbFilter.Controls.Add(mAccordion);

			mPreview = new SvgLayerPreview();
			mPreview.Dock = DockStyle.Fill;
			mPreview.Margin = new Padding(3, 9, 3, 3);
			mPreview.LayerClicked += SelectLayer;

			TableLayoutPanel main = new TableLayoutPanel();
			main.Dock = DockStyle.Fill;
			main.ColumnCount = 2;
			main.RowCount = 2;
			main.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, leftWidth));
			main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
			main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			main.Controls.Add(tableLayoutPanel9, 0, 0);
			main.SetRowSpan(tableLayoutPanel9, 2);
			main.Controls.Add(mPreview, 1, 0);
			main.Controls.Add(tableLayoutPanel1, 1, 1);
			Controls.Add(main);

			AutoSize = false;
			FormBorderStyle = FormBorderStyle.Sizable;
			MaximizeBox = true;
			MinimizeBox = false;
			MinimumSize = new Size(leftWidth + (int)(320 * k), (int)(480 * k));
			ClientSize = LoadWindowSize(new Size(leftWidth + (int)(640 * k), (int)(600 * k)));
			if (Settings.GetObject("SvgToGCodeForm.Maximized", false))
				WindowState = FormWindowState.Maximized;

			ResumeLayout(true);

			Shown += delegate { mPreview.Load(mSource, mLayers, mCore); };
			FormClosing += delegate { SaveWindowSize(); };
			FormClosed += delegate { Cursor = Cursors.WaitCursor; mPreview.StopWorker(); }; // the conversion starts right after
		}

		private Size LoadWindowSize(Size def)
		{
			string[] parts = Settings.GetObject("SvgToGCodeForm.Size", "").Split(',');
			int w, h;
			if (parts.Length == 2 && int.TryParse(parts[0], out w) && int.TryParse(parts[1], out h))
				return new Size(Math.Max(w, MinimumSize.Width), Math.Max(h, MinimumSize.Height));
			return def;
		}

		private void SaveWindowSize()
		{
			Settings.SetObject("SvgToGCodeForm.Maximized", WindowState == FormWindowState.Maximized);
			if (WindowState == FormWindowState.Normal)
				Settings.SetObject("SvgToGCodeForm.Size", ClientSize.Width + "," + ClientSize.Height);
		}

		// new colors get the global speed and power, the others their last settings
		private void InitLayers()
		{
			foreach (SvgColorLayer layer in mLayers)
			{
				if (!layer.LoadSettings())
				{
					layer.Speed = IIBorderTracing.CurrentValue;
					layer.Power = IIMaxPower.CurrentValue;
				}
			}
			mAccordion.SetLayers(mLayers, (int)GrblCore.Configuration.MaxRateX, (int)GrblCore.Configuration.MaxPWM);
		}

		// the same selection from the preview and from the accordion
		private void SelectLayer(SvgColorLayer layer)
		{
			mAccordion.SelectedLayer = layer;
			mPreview.SelectedLayer = layer;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			// esc closes the selected color first, then the dialog
			if (keyData == Keys.Escape && mPreview != null && mPreview.SelectedLayer != null && !(ActiveControl is ComboBox && ((ComboBox)ActiveControl).DroppedDown))
			{
				SelectLayer(null);
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		// global speed/power act as default: propagate the change to the layers that still use the previous value
		private void PropagateToLayers(bool speed, int oldValue, int newValue)
		{
			if (mAccordion == null)
				return;

			foreach (SvgColorLayer layer in mLayers)
			{
				if (speed && layer.Speed == oldValue)
					layer.Speed = newValue;
				else if (!speed && layer.Power == oldValue)
					layer.Power = newValue;
			}
			mAccordion.RefreshValues();
		}

		#endregion

		private void AssignMinMaxLimit()
        { 
			IIBorderTracing.MaxValue = (int)GrblCore.Configuration.MaxRateX;
			IIMaxPower.MaxValue = (int)GrblCore.Configuration.MaxPWM;
		}

		public void ShowDialogForm()
        {
			IIBorderTracing.CurrentValue = Settings.GetObject("GrayScaleConversion.VectorizeOptions.BorderSpeed", 1000);

			string LaserOn = Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOn", "M3");

			if (LaserOn == "M3" || !GrblCore.Configuration.LaserMode)
				CBLaserON.SelectedItem = LaserOptions[0];
			else
				CBLaserON.SelectedItem = LaserOptions[1];

			string LaserOff = "M5"; //Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOff", "M5");

			IIMinPower.CurrentValue = Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMin", 0);
			IIMaxPower.CurrentValue = Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMax", (int)GrblCore.Configuration.MaxPWM);

			IIBorderTracing.Visible = LblBorderTracing.Visible = LblBorderTracingmm.Visible = true;

			RefreshPerc();

			InitLayers(); //after speed/power initialization: they are the default for new colors

			ShowDialog(FormsHelper.MainForm);
		}


		void IIBorderTracingCurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			PropagateToLayers(true, OldValue, NewValue);
		}

	
		void IIMinPowerCurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			if (ByUser && IIMaxPower.CurrentValue <= NewValue)
				IIMaxPower.CurrentValue = NewValue + 1;

			RefreshPerc();
		}
		void IIMaxPowerCurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			if (ByUser && IIMinPower.CurrentValue >= NewValue)
				IIMinPower.CurrentValue = NewValue - 1;

			PropagateToLayers(false, OldValue, NewValue);
			RefreshPerc();
		}

		private void RefreshPerc()
		{
			decimal maxpwm = GrblCore.Configuration != null ? GrblCore.Configuration.MaxPWM : -1;

			if (maxpwm > 0)
			{
				LblMaxPerc.Text = (IIMaxPower.CurrentValue / GrblCore.Configuration.MaxPWM).ToString("P1");
				LblMinPerc.Text = (IIMinPower.CurrentValue / GrblCore.Configuration.MaxPWM).ToString("P1");
			}
			else
			{
				LblMaxPerc.Text = "";
				LblMinPerc.Text = "";
			}
		}

		private void BtnOnOffInfo_Click(object sender, EventArgs e)
		{Tools.Utils.OpenLink(@"https://lasergrbl.com/usage/raster-image-import/target-image-size-and-laser-options/#laser-modes");}

		private void BtnModulationInfo_Click(object sender, EventArgs e)
		{Tools.Utils.OpenLink(@"https://lasergrbl.com/usage/raster-image-import/target-image-size-and-laser-options/#power-modulation");}

		private void CBLaserON_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboboxItem mode = CBLaserON.SelectedItem as ComboboxItem;

			if (mode != null)
			{
				if (!GrblCore.Configuration.LaserMode && (mode.Value as string) == "M4")
					MessageBox.Show(Strings.WarnWrongLaserMode, Strings.WarnWrongLaserModeTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);//warning!!
			}

		}



		private void BtnPSHelper_Click(object sender, EventArgs e)
		{
			MaterialDB.MaterialsRow row = PSHelperForm.CreateAndShowDialog(this);
			if (row != null)
			{
				if (IIBorderTracing.Visible)
					IIBorderTracing.CurrentValue = row.Speed;
				//if (IILinearFilling.Visible)
				//	IILinearFilling.CurrentValue = row.Speed;

				IIMaxPower.CurrentValue = IIMaxPower.MaxValue * row.Power / 100;
			}
		}
		private void BtnColorFilter_Click(object sender, EventArgs e)
		{Tools.Utils.OpenLink(@"https://lasergrbl.com/usage/raster-image-import/target-image-size-and-laser-options/#color-filter");}
		//private void IISizeW_OnTheFlyValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		//{
		//	if (ByUser)
		//		IISizeH.CurrentValue = IP.WidthToHeight(NewValue);
		//}

		//private void IISizeH_OnTheFlyValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		//{
		//	if (ByUser) IISizeW.CurrentValue = IP.HeightToWidht(NewValue);
		//}
	}
}
