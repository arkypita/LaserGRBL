//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using LaserGRBL.Icons;
using LaserGRBL.UserControls;
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
		private List<GrblConfST.GrblConfParam> mLocalCopy;

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
			ThemeMgr.SetTheme(this, true);
			IconsMgr.PrepareButton(BtnCancel, "mdi-close-box");
            IconsMgr.PrepareButton(BtnWrite, "mdi-text-box-edit");
            IconsMgr.PrepareButton(BtnRead, "mdi-text-box-search");
            IconsMgr.PrepareButton(BtnImport, "mdi-download-box");
            IconsMgr.PrepareButton(BtnExport, "mdi-upload-box");

            mLocalCopy = GrblCore.Configuration.ToList();
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
			BtnExport.Enabled = GrblCore.Configuration.Count > 0;
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
			DoRead(GrblCore.RefreshCause.OnRead);

		}

		private void DoRead(GrblCore.RefreshCause cause)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				Core.RefreshConfig(cause);
				mLocalCopy = GrblCore.Configuration.ToList();
				DGV.DataSource = mLocalCopy;
				RefreshEnabledButtons();
				Cursor = DefaultCursor;

				ActionResult(String.Format(Strings.BoxReadConfigSuccess, mLocalCopy.Count));
			}
			catch (Exception ex)
			{
				Cursor = DefaultCursor;
				ActionResult($"{Strings.BoxReadConfigError} {ex.Message}");
			}
		}

		private void BtnWrite_Click(object sender, EventArgs e)
		{
			try
			{
				Core.RefreshConfig(GrblCore.RefreshCause.OnWriteBegin);
				List<GrblConfST.GrblConfParam> changes = GetChanges(); //get changes

				if (changes.Count > 0)
					WriteConf(changes, false);
				else
					ActionResult(Strings.BoxWriteConfigNoChange);
			}
			catch { }
		}

		private bool WriteConf(List<GrblConfST.GrblConfParam> conf, bool import)
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
				MessageBox.Show(String.Format(import ? Strings.BoxImportConfigWithError : Strings.BoxWriteConfigWithError, conf.Count, ex.Errors.Count) + "\n" + ex.Message, Strings.BoxExportConfigErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception)
			{
				Cursor = DefaultCursor;
				MessageBox.Show(String.Format(import ? Strings.BoxImportConfigWithError : Strings.BoxWriteConfigWithError, conf.Count, "unknown"), Strings.BoxExportConfigErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				try { Core.RefreshConfig(GrblCore.RefreshCause.OnWriteEnd); } //ensure to have the last conf at least in core
				catch { }
			}


			return noerror;
		}

		private void BtnExport_Click(object sender, EventArgs e)
		{
			string filename = null;
			using (SaveFileDialog sfd = new SaveFileDialog())
			{
				sfd.Filter = "GCODE Files|*.nc";
				sfd.AddExtension = true;
				sfd.RestoreDirectory = true;

				DialogResult dialogResult = DialogResult.Cancel;
				try
				{
					dialogResult = sfd.ShowDialog(this);
				}
				catch (System.Runtime.InteropServices.COMException)
				{
					sfd.AutoUpgradeEnabled = false;
					dialogResult = sfd.ShowDialog(this);
				}

				if (dialogResult == DialogResult.OK)
					filename = sfd.FileName;
			}

			if (filename != null)
			{
				try { Core.RefreshConfig(GrblCore.RefreshCause.OnExport); } //internally skipped if not possible
				catch { }

				try
				{
					List<GrblConfST.GrblConfParam> toexport = GrblCore.Configuration.ToList();

					if (toexport.Count == 0)
						throw new Exception("Nothing to export!");

					using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filename))
					{
						foreach (GrblConfST.GrblConfParam p in toexport)
							sw.WriteLine(string.Format("${0}={1}", p.Number, p.Value, p.Parameter));

						sw.Close();
						ActionResult(String.Format(Strings.BoxExportConfigSuccess, toexport.Count));
					}
				}
				catch (Exception ex)
				{
					Logger.LogException("ExportConfig", ex);
					MessageBox.Show(Strings.BoxExportConfigError, Strings.BoxExportConfigErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				RefreshEnabledButtons();
			}
		}

		private void BtnImport_Click(object sender, EventArgs e)
		{
			string filename = null;
			using (OpenFileDialog ofd = new OpenFileDialog())
			{
				ofd.Filter = "GCODE Files|*.nc;*.gcode";
				ofd.CheckFileExists = true;
				ofd.Multiselect = false;
				ofd.RestoreDirectory = true;

				DialogResult dialogResult = DialogResult.Cancel;
				try
				{
					dialogResult = ofd.ShowDialog(this);
				}
				catch (System.Runtime.InteropServices.COMException)
				{
					ofd.AutoUpgradeEnabled = false;
					dialogResult = ofd.ShowDialog(this);
				}

				if (dialogResult == DialogResult.OK)
					filename = ofd.FileName;
			}

			if (filename != null)
			{
				if (!System.IO.File.Exists(filename))
				{
					MessageBox.Show(Strings.BoxExportConfigFileNotFound, Strings.BoxExportConfigErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
				{
					try
					{
						List<GrblConfST.GrblConfParam> conf = new List<GrblConfST.GrblConfParam>();

						using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
						{
							string rline = null;
							while ((rline = sr.ReadLine()) != null)
							{
								string msg = rline; //"$0=10 (Step pulse time)"
								int num = int.Parse(msg.Split('=')[0].Substring(1));
								string val = msg.Split('=')[1].Trim();
								conf.Add(new GrblConfST.GrblConfParam(num, val));
							}
						}

						if (conf.Count == 0)
							throw new System.IO.InvalidDataException("File does not contain a valid configuration");
						else
							WriteConf(conf, true);

						//refresh actual conf
						Core.RefreshConfig(GrblCore.RefreshCause.OnImport);
						mLocalCopy = GrblCore.Configuration.ToList();
						DGV.DataSource = mLocalCopy;
					}
					catch (Exception ex)
					{
						Logger.LogException("ImportConfig", ex);
						MessageBox.Show(Strings.BoxImportConfigFileError, Strings.BoxExportConfigErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				DialogResult rv = MessageBox.Show(Strings.BoxConfigDetectedChanges, Strings.BoxConfigDetectedChangesTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

				if (rv == DialogResult.Yes)
					e.Cancel = !WriteConf(GetChanges(), false);
				else if (rv == DialogResult.Cancel)
					e.Cancel = true;
			}
		}

		public List<GrblConfST.GrblConfParam> GetChanges()
		{
			List<GrblConfST.GrblConfParam> rv = new List<GrblConfST.GrblConfParam>();
			GrblConfST conf = GrblCore.Configuration;
			foreach (GrblConfST.GrblConfParam p in mLocalCopy)
				if (conf.HasChanges(p))
					rv.Add(p);
			return rv;
		}

		public bool HasChanges()
		{
			GrblConfST conf = GrblCore.Configuration;

			if (conf.Count != mLocalCopy.Count)
				return true;

			foreach(GrblConfST.GrblConfParam p in mLocalCopy)
				if (conf.HasChanges(p))
					return true;

			return false;
		}

		private void GrblConfig_Load(object sender, EventArgs e)
		{
			if (BtnRead.Enabled)
				DoRead(GrblCore.RefreshCause.OnDialog);
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
