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

			mLocalCopy = Core.GrblConfiguration.ToList();
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
				mLocalCopy = Core.GrblConfiguration.ToList();
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
			WriteConf(mLocalCopy, false);
		}

		private void WriteConf(List<GrblConf.GrblConfParam> conf, bool import)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				Core.WriteConfig(conf);
				Cursor = DefaultCursor;

				ActionResult(String.Format(import ? Strings.BoxImportConfigWithoutError :  Strings.BoxWriteConfigWithoutError, conf.Count));
			}
			catch (GrblCore.WriteConfigException ex)
			{
				Cursor = DefaultCursor;

				string errLines = "\n";
				foreach (IGrblRow r in ex.Errors)
					errLines += string.Format("{0} {1}\n", r.GetMessage(), r.GetResult(true, false));

				System.Windows.Forms.MessageBox.Show(String.Format(import ? Strings.BoxImportConfigWithError : Strings.BoxWriteConfigWithError, conf.Count, ex.Errors.Count) + errLines, Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
			}
			catch (Exception ex)
			{
				Cursor = DefaultCursor;
				System.Windows.Forms.MessageBox.Show(String.Format(import ? Strings.BoxImportConfigWithError : Strings.BoxWriteConfigWithError, conf.Count, "unknown"), Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
			}
			finally
			{
				try{Core.RefreshConfig();}
				catch{}
			}
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
				List<GrblConf.GrblConfParam> toexport = Core.GrblConfiguration.ToList();
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
	}


}
