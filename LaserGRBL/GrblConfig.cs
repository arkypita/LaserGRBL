//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL
{
	public partial class GrblConfig : Form
	{
		private GrblCore Core;
		private List<GrblConf.GrblConfParam> mLocalCopy;

		public GrblConfig(GrblCore core)
		{
			InitializeComponent();
			Core = core;
			BackColor = ColorScheme.FormBackColor;
			GB.ForeColor = ForeColor = ColorScheme.FormForeColor;
            DGV.EnableHeadersVisualStyles = false;
            DGV.BackgroundColor = ColorScheme.FormBackColor; //SystemColors.Control;
            DGV.ForeColor = ColorScheme.FormForeColor; //SystemColors.ControlText;
            DGV.DefaultCellStyle.BackColor = ColorScheme.FormBackColor;
            DGV.ColumnHeadersDefaultCellStyle.BackColor = ColorScheme.FormBackColor;
            DGV.ColumnHeadersDefaultCellStyle.ForeColor = ColorScheme.FormForeColor;
            DGV.RowHeadersDefaultCellStyle.BackColor = ColorScheme.FormBackColor;
            DGV.RowHeadersDefaultCellStyle.ForeColor = ColorScheme.FormForeColor;
            DGV.Columns["Value"].DefaultCellStyle.ForeColor = ColorScheme.FormForeColor;
            DGV.Columns["Value"].DefaultCellStyle.BackColor = ColorScheme.FormBackColor;
            BtnRead.BackColor = BtnWrite.BackColor = BtnImport.BackColor = BtnExport.BackColor = BtnCancel.BackColor = ColorScheme.FormButtonsColor;

			mLocalCopy = Core.Configuration.ToList();
			DGV.DataSource = mLocalCopy;
            Core.MachineStatusChanged += RefreshEnabledButtons;
			
			RefreshEnabledButtons();
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
				Core.MachineStatusChanged -= RefreshEnabledButtons;
			}
			base.Dispose(disposing);
		}

		void RefreshEnabledButtons()
		{
			BtnExport.Enabled = Core.Configuration.Count > 0;
			BtnImport.Enabled = BtnRead.Enabled = BtnWrite.Enabled = Core.CanReadWriteConfig;
			DGV.ReadOnly = LblConnect.Visible = !Core.IsConnected;
		}

		internal static void CreateAndShowDialog(Form parent, GrblCore core)
		{
			using (GrblConfig sf = new GrblConfig(core))
				sf.ShowDialog(parent);
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void BtnRead_Click(object sender, EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				Core.RefreshConfig();
				mLocalCopy = Core.Configuration.ToList();
				DGV.DataSource = mLocalCopy;
				RefreshEnabledButtons();
				Cursor = DefaultCursor;

				ActionResult( String.Format(Strings.BoxReadConfigSuccess, mLocalCopy.Count));
			}
			catch (Exception)
			{
				Cursor = DefaultCursor;
				ActionResult(Strings.BoxReadConfigError);
			}

		}

		private void BtnWrite_Click(object sender, EventArgs e)
		{
			try
			{
				Core.RefreshConfig();
				List<GrblConf.GrblConfParam> changes = GetChanges(); //get changes

				if (changes.Count > 0)
					WriteConf(changes, false);
				else
					ActionResult(Strings.BoxWriteConfigNoChange);
			}
			catch { }
		}

		private bool WriteConf(List<GrblConf.GrblConfParam> conf, bool import)
		{
			bool noerror = false;

			try
			{
				Cursor = Cursors.WaitCursor;
				Core.WriteConfig(conf);
				Cursor = DefaultCursor;
				ActionResult(String.Format(import ? Strings.BoxImportConfigWithoutError : Strings.BoxWriteConfigWithoutError, conf.Count));

				noerror = true;
			}
			catch (GrblCore.WriteConfigException ex)
			{
				Cursor = DefaultCursor;
				System.Windows.Forms.MessageBox.Show(String.Format(import ? Strings.BoxImportConfigWithError : Strings.BoxWriteConfigWithError, conf.Count, ex.Errors.Count) + "\n" + ex.Message, Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
			}
			catch (Exception)
			{
				Cursor = DefaultCursor;
				System.Windows.Forms.MessageBox.Show(String.Format(import ? Strings.BoxImportConfigWithError : Strings.BoxWriteConfigWithError, conf.Count, "unknown"), Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
			}
			finally
			{
				try { Core.RefreshConfig(); } //ensure to have the last conf at least in core
				catch { }
			}


			return noerror;
		}

		private void BtnExport_Click(object sender, EventArgs e)
		{
			string filename = null;
			using (System.Windows.Forms.SaveFileDialog sfd = new SaveFileDialog())
			{
				sfd.Filter = "GCODE Files|*.nc";
				sfd.AddExtension = true;
				sfd.RestoreDirectory = true;

				System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.DialogResult.Cancel;
				try
				{
					dialogResult = sfd.ShowDialog(this);
				}
				catch (System.Runtime.InteropServices.COMException)
				{
					sfd.AutoUpgradeEnabled = false;
					dialogResult = sfd.ShowDialog(this);
				}

				if (dialogResult == System.Windows.Forms.DialogResult.OK)
					filename = sfd.FileName;
			}

			if (filename != null)
			{
				Core.RefreshConfig(); //internally skipped if not possible

				List<GrblConf.GrblConfParam> toexport = Core.Configuration.ToList();
				if (toexport.Count > 0)
				{
					try
					{
						using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filename))
						{
							foreach (GrblConf.GrblConfParam p in toexport)
								sw.WriteLine(string.Format("${0}={1} ({2})", p.Number, p.Value.ToString(System.Globalization.NumberFormatInfo.InvariantInfo), p.Parameter));

							sw.Close();
							ActionResult(String.Format(Strings.BoxExportConfigSuccess, toexport.Count));
						}
					}
					catch (Exception ex)
					{
						Logger.LogException("ExportConfig", ex);
						System.Windows.Forms.MessageBox.Show(Strings.BoxExportConfigError, Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
					}
				}

				RefreshEnabledButtons();
			}
		}

		private void BtnImport_Click(object sender, EventArgs e)
		{
			string filename = null;
			using (System.Windows.Forms.OpenFileDialog ofd = new OpenFileDialog())
			{
				ofd.Filter = "GCODE Files|*.nc;*.gcode";
				ofd.CheckFileExists = true;
				ofd.Multiselect = false;
				ofd.RestoreDirectory = true;

				System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.DialogResult.Cancel;
				try
				{
					dialogResult = ofd.ShowDialog(this);
				}
				catch (System.Runtime.InteropServices.COMException)
				{
					ofd.AutoUpgradeEnabled = false;
					dialogResult = ofd.ShowDialog(this);
				}

				if (dialogResult == System.Windows.Forms.DialogResult.OK)
					filename = ofd.FileName;
			}

			if (filename != null)
			{
				if (!System.IO.File.Exists(filename))
				{
					System.Windows.Forms.MessageBox.Show(Strings.BoxExportConfigFileNotFound, Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				}
				else
				{
					try
					{
						List<GrblConf.GrblConfParam> conf = new List<GrblConf.GrblConfParam>();

						using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
						{
							string rline = null;
							while ((rline = sr.ReadLine()) != null)
							{
								string msg = rline; //"$0=10 (Step pulse time)"
								int num = int.Parse(msg.Split('=')[0].Substring(1));
								decimal val = decimal.Parse(msg.Split('=')[1].Split(' ')[0], System.Globalization.NumberFormatInfo.InvariantInfo);
								conf.Add(new GrblConf.GrblConfParam(num, val));
							}
						}

						if (conf.Count == 0)
							throw new System.IO.InvalidDataException("File does not contain a valid configuration");
						else
							WriteConf(conf, true);

						//refresh actual conf
						Core.RefreshConfig();
						mLocalCopy = Core.Configuration.ToList();
						DGV.DataSource = mLocalCopy;
					}
					catch (Exception ex)
					{
						Logger.LogException("ImportConfig", ex);
						System.Windows.Forms.MessageBox.Show(Strings.BoxImportConfigFileError, Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
					}

					RefreshEnabledButtons();
				}
			}
		}

		private void DGV_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			e.Cancel = true;
			e.ThrowException = false;
			DGV.CancelEdit();
		}

		private void ActionTimer_Tick(object sender, EventArgs e)
		{
			ActionTimer.Stop();
			LblAction.Visible = false;
		}


		private void ActionResult(string result)
		{
			LblAction.Text = result;
			LblAction.Visible = true;
			ActionTimer.Start();
			RefreshEnabledButtons();
		}

		private void GrblConfig_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (HasChanges() && Core.MachineStatus == GrblCore.MacStatus.Idle)
			{
				DialogResult rv = System.Windows.Forms.MessageBox.Show(Strings.BoxConfigDetectedChanges, Strings.BoxConfigDetectedChangesTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

				if (rv == System.Windows.Forms.DialogResult.Yes)
					e.Cancel = !WriteConf(GetChanges(), false);
				else if (rv == System.Windows.Forms.DialogResult.Cancel)
					e.Cancel = true;
			}
		}

		public List<GrblConf.GrblConfParam> GetChanges()
		{
			List<GrblConf.GrblConfParam> rv = new List<GrblConf.GrblConfParam>();
			GrblConf conf = Core.Configuration;
			foreach (GrblConf.GrblConfParam p in mLocalCopy)
				if (conf.HasChanges(p))
					rv.Add(p);
			return rv;
		}

		public bool HasChanges()
		{
			GrblConf conf = Core.Configuration;

			if (conf.Count != mLocalCopy.Count)
				return true;

			foreach(GrblConf.GrblConfParam p in mLocalCopy)
				if (conf.HasChanges(p))
					return true;

			return false;
		}

		private void GrblConfig_Load(object sender, EventArgs e)
		{
			if (BtnRead.Enabled)
				BtnRead.PerformClick();
			else
				ActionResult(Strings.BoxReadConfigPleaseConnect);
		}

		private void DGV_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			if (DGV[e.ColumnIndex, e.RowIndex].IsInEditMode)
			{
				if (e.ColumnIndex == 3)
				{
					object value = DGV[e.ColumnIndex, e.RowIndex].Value;
					int parid = int.Parse(DGV[1, e.RowIndex].Value.ToString().Trim(new char[] { '$' }));

					string error = Core.ValidateConfig(parid, value);

					if (error != null)
					{
						MessageBox.Show(error, Strings.WarnMessageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Warning);

						e.Cancel = true;
						DGV.CancelEdit();
						DGV.EndEdit();
					}
				}
			}
		}

	}
}
