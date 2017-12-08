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
			DGV.BackgroundColor = SystemColors.Control;
			DGV.ForeColor = SystemColors.ControlText;
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
			BtnExport.Enabled = BtnImport.Enabled = BtnRead.Enabled = BtnWrite.Enabled = Core.MachineStatus == GrblCore.MacStatus.Idle;
			DGV.ReadOnly = LblConnect.Visible = !BtnRead.Enabled;
		}

		internal static void CreateAndShowDialog(GrblCore core)
		{
			using (GrblConfig sf = new GrblConfig(core))
				sf.ShowDialog();
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
			catch (Exception ex)
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
			catch (Exception ex)
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
			using (System.Windows.Forms.SaveFileDialog ofd = new SaveFileDialog())
			{
				ofd.Filter = "GCODE Files|*.nc";
				ofd.AddExtension = true;
				ofd.RestoreDirectory = true;
				if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					filename = ofd.FileName;
			}

			if (filename != null)
			{
				Core.RefreshConfig();
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
				if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
	}


}
