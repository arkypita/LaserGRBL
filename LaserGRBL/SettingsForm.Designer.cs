﻿namespace LaserGRBL
{
	partial class SettingsForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.MainTabPage = new System.Windows.Forms.TabControl();
            this.TpHardware = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.CBCore = new System.Windows.Forms.ComboBox();
            this.CbThreadingMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CBStreamingMode = new System.Windows.Forms.ComboBox();
            this.BtnStreamingMode = new LaserGRBL.UserControls.ImageButton();
            this.CBProtocol = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnProtocol = new LaserGRBL.UserControls.ImageButton();
            this.label6 = new System.Windows.Forms.Label();
            this.BtnThreadingModel = new LaserGRBL.UserControls.ImageButton();
            this.CbIssueDetector = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.CbSoftReset = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CbHardReset = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.TpRasterImport = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.CbUnidirectional = new System.Windows.Forms.CheckBox();
            this.CBSupportPWM = new System.Windows.Forms.CheckBox();
            this.BtnModulationInfo = new LaserGRBL.UserControls.ImageButton();
            this.TpJogControl = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.CbEnableZJog = new System.Windows.Forms.CheckBox();
            this.CbContinuosJog = new System.Windows.Forms.CheckBox();
            this.TpAutoCooling = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.label20 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.CbAutoCooling = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.label15 = new System.Windows.Forms.Label();
            this.CbOnMin = new System.Windows.Forms.ComboBox();
            this.CbOnSec = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.label17 = new System.Windows.Forms.Label();
            this.CbOffMin = new System.Windows.Forms.ComboBox();
            this.CbOffSec = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label21 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.MainTabPage.SuspendLayout();
            this.TpHardware.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.TpRasterImport.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.TpJogControl.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.TpAutoCooling.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.MainTabPage, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.BtnCancel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.BtnSave, 2, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // BtnCancel
            // 
            resources.ApplyResources(this.BtnCancel, "BtnCancel");
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnSave
            // 
            resources.ApplyResources(this.BtnSave, "BtnSave");
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // MainTabPage
            // 
            resources.ApplyResources(this.MainTabPage, "MainTabPage");
            this.MainTabPage.Controls.Add(this.TpHardware);
            this.MainTabPage.Controls.Add(this.TpRasterImport);
            this.MainTabPage.Controls.Add(this.TpJogControl);
            this.MainTabPage.Controls.Add(this.TpAutoCooling);
            this.MainTabPage.Name = "MainTabPage";
            this.MainTabPage.SelectedIndex = 0;
            // 
            // TpHardware
            // 
            resources.ApplyResources(this.TpHardware, "TpHardware");
            this.TpHardware.Controls.Add(this.tableLayoutPanel3);
            this.TpHardware.Name = "TpHardware";
            this.TpHardware.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.CBCore, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.CbThreadingMode, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.label4, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.CBStreamingMode, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.BtnStreamingMode, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.CBProtocol, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label3, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.BtnProtocol, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label6, 2, 3);
            this.tableLayoutPanel3.Controls.Add(this.BtnThreadingModel, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.CbIssueDetector, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.label7, 2, 4);
            this.tableLayoutPanel3.Controls.Add(this.CbSoftReset, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.label2, 2, 5);
            this.tableLayoutPanel3.Controls.Add(this.CbHardReset, 1, 6);
            this.tableLayoutPanel3.Controls.Add(this.label8, 2, 6);
            this.tableLayoutPanel3.Controls.Add(this.label9, 2, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // CBCore
            // 
            resources.ApplyResources(this.CBCore, "CBCore");
            this.CBCore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBCore.FormattingEnabled = true;
            this.CBCore.Name = "CBCore";
            // 
            // CbThreadingMode
            // 
            resources.ApplyResources(this.CbThreadingMode, "CbThreadingMode");
            this.CbThreadingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbThreadingMode.FormattingEnabled = true;
            this.CbThreadingMode.Name = "CbThreadingMode";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // CBStreamingMode
            // 
            resources.ApplyResources(this.CBStreamingMode, "CBStreamingMode");
            this.CBStreamingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBStreamingMode.FormattingEnabled = true;
            this.CBStreamingMode.Name = "CBStreamingMode";
            // 
            // BtnStreamingMode
            // 
            resources.ApplyResources(this.BtnStreamingMode, "BtnStreamingMode");
            this.BtnStreamingMode.AltImage = null;
            this.BtnStreamingMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnStreamingMode.Coloration = System.Drawing.Color.Empty;
            this.BtnStreamingMode.Image = ((System.Drawing.Image)(resources.GetObject("BtnStreamingMode.Image")));
            this.BtnStreamingMode.Name = "BtnStreamingMode";
            this.BtnStreamingMode.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnStreamingMode.UseAltImage = false;
            this.BtnStreamingMode.Click += new System.EventHandler(this.BtnStreamingMode_Click);
            // 
            // CBProtocol
            // 
            resources.ApplyResources(this.CBProtocol, "CBProtocol");
            this.CBProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBProtocol.FormattingEnabled = true;
            this.CBProtocol.Name = "CBProtocol";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // BtnProtocol
            // 
            resources.ApplyResources(this.BtnProtocol, "BtnProtocol");
            this.BtnProtocol.AltImage = null;
            this.BtnProtocol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnProtocol.Coloration = System.Drawing.Color.Empty;
            this.BtnProtocol.Image = ((System.Drawing.Image)(resources.GetObject("BtnProtocol.Image")));
            this.BtnProtocol.Name = "BtnProtocol";
            this.BtnProtocol.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnProtocol.UseAltImage = false;
            this.BtnProtocol.Click += new System.EventHandler(this.BtnProtocol_Click);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // BtnThreadingModel
            // 
            resources.ApplyResources(this.BtnThreadingModel, "BtnThreadingModel");
            this.BtnThreadingModel.AltImage = null;
            this.BtnThreadingModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnThreadingModel.Coloration = System.Drawing.Color.Empty;
            this.BtnThreadingModel.Image = ((System.Drawing.Image)(resources.GetObject("BtnThreadingModel.Image")));
            this.BtnThreadingModel.Name = "BtnThreadingModel";
            this.BtnThreadingModel.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnThreadingModel.UseAltImage = false;
            this.BtnThreadingModel.Click += new System.EventHandler(this.BtnThreadingModel_Click);
            // 
            // CbIssueDetector
            // 
            resources.ApplyResources(this.CbIssueDetector, "CbIssueDetector");
            this.CbIssueDetector.Name = "CbIssueDetector";
            this.CbIssueDetector.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // CbSoftReset
            // 
            resources.ApplyResources(this.CbSoftReset, "CbSoftReset");
            this.CbSoftReset.Name = "CbSoftReset";
            this.CbSoftReset.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // CbHardReset
            // 
            resources.ApplyResources(this.CbHardReset, "CbHardReset");
            this.CbHardReset.Name = "CbHardReset";
            this.CbHardReset.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // TpRasterImport
            // 
            resources.ApplyResources(this.TpRasterImport, "TpRasterImport");
            this.TpRasterImport.Controls.Add(this.tableLayoutPanel4);
            this.TpRasterImport.Name = "TpRasterImport";
            this.TpRasterImport.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.label1, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.label5, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.CbUnidirectional, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.CBSupportPWM, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.BtnModulationInfo, 0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // CbUnidirectional
            // 
            resources.ApplyResources(this.CbUnidirectional, "CbUnidirectional");
            this.CbUnidirectional.Name = "CbUnidirectional";
            this.CbUnidirectional.UseVisualStyleBackColor = true;
            // 
            // CBSupportPWM
            // 
            resources.ApplyResources(this.CBSupportPWM, "CBSupportPWM");
            this.CBSupportPWM.Name = "CBSupportPWM";
            this.CBSupportPWM.UseVisualStyleBackColor = true;
            // 
            // BtnModulationInfo
            // 
            resources.ApplyResources(this.BtnModulationInfo, "BtnModulationInfo");
            this.BtnModulationInfo.AltImage = null;
            this.BtnModulationInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnModulationInfo.Coloration = System.Drawing.Color.Empty;
            this.BtnModulationInfo.Image = ((System.Drawing.Image)(resources.GetObject("BtnModulationInfo.Image")));
            this.BtnModulationInfo.Name = "BtnModulationInfo";
            this.BtnModulationInfo.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnModulationInfo.UseAltImage = false;
            this.BtnModulationInfo.Click += new System.EventHandler(this.BtnModulationInfo_Click);
            // 
            // TpJogControl
            // 
            resources.ApplyResources(this.TpJogControl, "TpJogControl");
            this.TpJogControl.Controls.Add(this.tableLayoutPanel5);
            this.TpJogControl.Name = "TpJogControl";
            this.TpJogControl.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
            this.tableLayoutPanel5.Controls.Add(this.label10, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.label11, 2, 1);
            this.tableLayoutPanel5.Controls.Add(this.CbEnableZJog, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.CbContinuosJog, 1, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // CbEnableZJog
            // 
            resources.ApplyResources(this.CbEnableZJog, "CbEnableZJog");
            this.CbEnableZJog.Name = "CbEnableZJog";
            this.CbEnableZJog.UseVisualStyleBackColor = true;
            // 
            // CbContinuosJog
            // 
            resources.ApplyResources(this.CbContinuosJog, "CbContinuosJog");
            this.CbContinuosJog.Name = "CbContinuosJog";
            this.CbContinuosJog.UseVisualStyleBackColor = true;
            // 
            // TpAutoCooling
            // 
            resources.ApplyResources(this.TpAutoCooling, "TpAutoCooling");
            this.TpAutoCooling.Controls.Add(this.tableLayoutPanel6);
            this.TpAutoCooling.Name = "TpAutoCooling";
            this.TpAutoCooling.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel6
            // 
            resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
            this.tableLayoutPanel6.Controls.Add(this.label20, 2, 2);
            this.tableLayoutPanel6.Controls.Add(this.label12, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.label13, 2, 1);
            this.tableLayoutPanel6.Controls.Add(this.CbAutoCooling, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel7, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel8, 1, 2);
            this.tableLayoutPanel6.Controls.Add(this.pictureBox1, 1, 3);
            this.tableLayoutPanel6.Controls.Add(this.label21, 2, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // CbAutoCooling
            // 
            resources.ApplyResources(this.CbAutoCooling, "CbAutoCooling");
            this.CbAutoCooling.Name = "CbAutoCooling";
            this.CbAutoCooling.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel7
            // 
            resources.ApplyResources(this.tableLayoutPanel7, "tableLayoutPanel7");
            this.tableLayoutPanel7.Controls.Add(this.label15, 2, 0);
            this.tableLayoutPanel7.Controls.Add(this.CbOnMin, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.CbOnSec, 3, 0);
            this.tableLayoutPanel7.Controls.Add(this.label14, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.label16, 4, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // CbOnMin
            // 
            resources.ApplyResources(this.CbOnMin, "CbOnMin");
            this.CbOnMin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbOnMin.FormattingEnabled = true;
            this.CbOnMin.Name = "CbOnMin";
            // 
            // CbOnSec
            // 
            resources.ApplyResources(this.CbOnSec, "CbOnSec");
            this.CbOnSec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbOnSec.FormattingEnabled = true;
            this.CbOnSec.Name = "CbOnSec";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // tableLayoutPanel8
            // 
            resources.ApplyResources(this.tableLayoutPanel8, "tableLayoutPanel8");
            this.tableLayoutPanel8.Controls.Add(this.label17, 2, 0);
            this.tableLayoutPanel8.Controls.Add(this.CbOffMin, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.CbOffSec, 3, 0);
            this.tableLayoutPanel8.Controls.Add(this.label18, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.label19, 4, 0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // CbOffMin
            // 
            resources.ApplyResources(this.CbOffMin, "CbOffMin");
            this.CbOffMin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbOffMin.FormattingEnabled = true;
            this.CbOffMin.Name = "CbOffMin";
            // 
            // CbOffSec
            // 
            resources.ApplyResources(this.CbOffSec, "CbOffSec");
            this.CbOffSec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbOffSec.FormattingEnabled = true;
            this.CbOffSec.Name = "CbOffSec";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.ForeColor = System.Drawing.Color.Red;
            this.label21.Name = "label21";
            // 
            // SettingsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.MainTabPage.ResumeLayout(false);
            this.TpHardware.ResumeLayout(false);
            this.TpHardware.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.TpRasterImport.ResumeLayout(false);
            this.TpRasterImport.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.TpJogControl.ResumeLayout(false);
            this.TpJogControl.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.TpAutoCooling.ResumeLayout(false);
            this.TpAutoCooling.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.Button BtnSave;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.CheckBox CBSupportPWM;
		private System.Windows.Forms.Label label1;
		private UserControls.ImageButton BtnModulationInfo;
		private System.Windows.Forms.ComboBox CBProtocol;
		private System.Windows.Forms.Label label3;
		private UserControls.ImageButton BtnProtocol;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox CBStreamingMode;
		private UserControls.ImageButton BtnStreamingMode;
		private System.Windows.Forms.CheckBox CbUnidirectional;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox CbThreadingMode;
		private System.Windows.Forms.Label label6;
		private UserControls.ImageButton BtnThreadingModel;
		private System.Windows.Forms.CheckBox CbIssueDetector;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.CheckBox CbSoftReset;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox CbHardReset;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TabControl MainTabPage;
		private System.Windows.Forms.TabPage TpHardware;
		private System.Windows.Forms.TabPage TpRasterImport;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ComboBox CBCore;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabPage TpJogControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox CbEnableZJog;
        private System.Windows.Forms.CheckBox CbContinuosJog;
		private System.Windows.Forms.TabPage TpAutoCooling;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.CheckBox CbAutoCooling;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
		private System.Windows.Forms.ComboBox CbOnMin;
		private System.Windows.Forms.ComboBox CbOnSec;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.ComboBox CbOffMin;
		private System.Windows.Forms.ComboBox CbOffSec;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label21;
    }
}