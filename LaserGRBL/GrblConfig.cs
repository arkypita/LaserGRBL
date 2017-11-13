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
		private GrblConf mLocalCopy;

		public GrblConfig(GrblCore core)
		{
			InitializeComponent();
			Core = core;
			BackColor = ColorScheme.FormBackColor;
			GB.ForeColor = ForeColor = ColorScheme.FormForeColor;
			DGV.BackgroundColor = SystemColors.Control;
			DGV.ForeColor = SystemColors.ControlText;
			BtnRead.BackColor = BtnWrite.BackColor = BtnImport.BackColor = BtnExport.BackColor = BtnCancel.BackColor = ColorScheme.FormButtonsColor;

			mLocalCopy = (GrblConf)Core.GrblConfiguration.Clone();
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
			BtnRead.Enabled = BtnWrite.Enabled = Core.MachineStatus == GrblCore.MacStatus.Idle;
			BtnExport.Enabled = mLocalCopy.Count > 0;
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
				mLocalCopy = Core.ReadConfig();
				DGV.DataSource = mLocalCopy;
				Core.GrblConfiguration = mLocalCopy.Clone() as GrblConf;
				RefreshEnabledButtons();
				Cursor = DefaultCursor;

				System.Windows.Forms.MessageBox.Show(String.Format(Strings.BoxReadConfigSuccess, mLocalCopy.Count), Strings.BoxExportConfigSuccessTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				Cursor = DefaultCursor;
				System.Windows.Forms.MessageBox.Show(Strings.BoxReadConfigError, Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
			}
			
		}

		private void BtnWrite_Click(object sender, EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				Core.WriteConfig(mLocalCopy);
				Cursor = DefaultCursor;

				System.Windows.Forms.MessageBox.Show(String.Format(Strings.BoxWriteConfigWithoutError, mLocalCopy.Count), Strings.BoxExportConfigSuccessTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
			}
			catch (GrblCore.WriteConfigException ex)
			{
				Cursor = DefaultCursor;
				System.Windows.Forms.MessageBox.Show(String.Format(Strings.BoxWriteConfigWithError, mLocalCopy.Count, ex.ErrorCount), Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
			}
			catch (Exception ex)
			{
				Cursor = DefaultCursor;
				System.Windows.Forms.MessageBox.Show(String.Format(Strings.BoxWriteConfigWithError, mLocalCopy.Count, "unknown"), Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
			{ ExportConfig(filename); }
		}

		public void ExportConfig(string filename)
		{
			if (mLocalCopy.Count > 0)
			{
				try
				{
					using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filename))
					{
						foreach (GrblConf.GrblConfParam p in mLocalCopy)
							sw.WriteLine(string.Format("${0}={1} ({2})", p.Number, p.Value.ToString(System.Globalization.NumberFormatInfo.InvariantInfo), p.Parameter));

						sw.Close();
						System.Windows.Forms.MessageBox.Show(String.Format(Strings.BoxExportConfigSuccess, mLocalCopy.Count), Strings.BoxExportConfigSuccessTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
					}
				}
				catch (Exception ex)
				{
					Logger.LogException("ExportConfig", ex);
					System.Windows.Forms.MessageBox.Show(Strings.BoxExportConfigError, Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				}
			}
		}
	}


}


/*
void MnImportConfigClick(object sender, EventArgs e)
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
	
	Core.ImportConfig(filename);
}
*/


/*

	

		public void ImportConfig(string filename)
		{

			if (!System.IO.File.Exists(filename))
			{
				System.Windows.Forms.MessageBox.Show(Strings.BoxExportConfigFileNotFound, Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return;
			}

			if (mMachineStatus == MacStatus.Idle)
			{
				try
				{
					using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
					{
						lock (this)
						{
							mSentPtr = new System.Collections.Generic.List<IGrblRow>(); //assign sent queue
							mQueuePtr = new System.Collections.Generic.Queue<GrblCommand>();

							string rline = null;
							while ((rline = sr.ReadLine()) != null)
								if (rline.StartsWith("$"))
									mQueuePtr.Enqueue(new GrblCommand(rline));
						}

						try
						{
							sr.Close();
							while (com.IsOpen && (mQueuePtr.Count > 0 || mPending.Count > 0)) //resta in attesa del completamento della comunicazione
								;

							int errors = 0;
							foreach (IGrblRow row in mSentPtr)
							{
								if (row is GrblCommand)
									if (((GrblCommand)row).Status != GrblCommand.CommandStatus.ResponseGood)
										errors++;
							}

							if (errors > 0)
								System.Windows.Forms.MessageBox.Show(String.Format(Strings.BoxImportConfigWithError, mSentPtr.Count, errors), Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
							else
								System.Windows.Forms.MessageBox.Show(String.Format(Strings.BoxImportConfigWithoutError, mSentPtr.Count), Strings.BoxExportConfigSuccessTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
						}
						catch (Exception ex)
						{
							Logger.LogException("ImportConfig", ex);
							System.Windows.Forms.MessageBox.Show(Strings.BoxImportConfigFileError, Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
						}

						lock (this)
						{
							mQueuePtr = mQueue;
							mSentPtr = mSent; //restore queue
						}
					}
				}
				catch (Exception ex)
				{
					Logger.LogException("ImportConfig", ex);
					System.Windows.Forms.MessageBox.Show(Strings.BoxImportConfigFileError, Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				}
			}
		}

*/