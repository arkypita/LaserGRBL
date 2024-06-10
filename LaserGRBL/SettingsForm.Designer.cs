namespace LaserGRBL
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
			this.BtnCancel = new LaserGRBL.UserControls.GrblButton();
			this.BtnSave = new LaserGRBL.UserControls.GrblButton();
			this.MainTabPage = new System.Windows.Forms.TabControl();
			this.TpHardware = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.CBCore = new LaserGRBL.UserControls.FlatComboBox();
			this.CbThreadingMode = new LaserGRBL.UserControls.FlatComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.CBStreamingMode = new LaserGRBL.UserControls.FlatComboBox();
			this.BtnStreamingMode = new LaserGRBL.UserControls.ImageButton();
			this.CBProtocol = new LaserGRBL.UserControls.FlatComboBox();
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
			this.BtnFType = new LaserGRBL.UserControls.ImageButton();
			this.CbQueryDI = new System.Windows.Forms.CheckBox();
			this.label46 = new System.Windows.Forms.Label();
			this.TpRasterImport = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.CbUnidirectional = new System.Windows.Forms.CheckBox();
			this.CBSupportPWM = new System.Windows.Forms.CheckBox();
			this.BtnModulationInfo = new LaserGRBL.UserControls.ImageButton();
			this.CbHiRes = new System.Windows.Forms.CheckBox();
			this.label22 = new System.Windows.Forms.Label();
			this.CbDisableSkip = new System.Windows.Forms.CheckBox();
			this.label39 = new System.Windows.Forms.Label();
			this.CbDisableBoundWarn = new System.Windows.Forms.CheckBox();
			this.label40 = new System.Windows.Forms.Label();
			this.TpVectorImport = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel18 = new System.Windows.Forms.TableLayoutPanel();
			this.label43 = new System.Windows.Forms.Label();
			this.CbSmartBezier = new System.Windows.Forms.CheckBox();
			this.imageButton1 = new LaserGRBL.UserControls.ImageButton();
			this.TpJogControl = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.CbEnableZJog = new System.Windows.Forms.CheckBox();
			this.CbContinuosJog = new System.Windows.Forms.CheckBox();
			this.CbClickNJog = new System.Windows.Forms.CheckBox();
			this.label41 = new System.Windows.Forms.Label();
			this.TpAutoCooling = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.label20 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.CbAutoCooling = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
			this.label15 = new System.Windows.Forms.Label();
			this.CbOnMin = new LaserGRBL.UserControls.FlatComboBox();
			this.CbOnSec = new LaserGRBL.UserControls.FlatComboBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
			this.label17 = new System.Windows.Forms.Label();
			this.CbOffMin = new LaserGRBL.UserControls.FlatComboBox();
			this.CbOffSec = new LaserGRBL.UserControls.FlatComboBox();
			this.label18 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label21 = new System.Windows.Forms.Label();
			this.LblWarnOrturAC = new System.Windows.Forms.Label();
			this.TpGCodeSettings = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
			this.LblHeader = new System.Windows.Forms.Label();
			this.groupBox1 = new LaserGRBL.UserControls.GrblGroupBox();
			this.TBHeader = new System.Windows.Forms.TextBox();
			this.groupBox2 = new LaserGRBL.UserControls.GrblGroupBox();
			this.TBFooter = new System.Windows.Forms.TextBox();
			this.groupBox3 = new LaserGRBL.UserControls.GrblGroupBox();
			this.TBPasses = new System.Windows.Forms.TextBox();
			this.LblFooter = new System.Windows.Forms.Label();
			this.LblPasses = new System.Windows.Forms.Label();
			this.TpSoundSettings = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
			this.CbPlaySuccess = new System.Windows.Forms.CheckBox();
			this.CbPlayWarning = new System.Windows.Forms.CheckBox();
			this.CbPlayFatal = new System.Windows.Forms.CheckBox();
			this.CbPlayConnect = new System.Windows.Forms.CheckBox();
			this.CbPlayDisconnect = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
			this.CbTelegramNotification = new System.Windows.Forms.CheckBox();
			this.DisconnectFullLabel = new System.Windows.Forms.Label();
			this.ConnectFullLabel = new System.Windows.Forms.Label();
			this.ErrorFullLabel = new System.Windows.Forms.Label();
			this.WarningFullLabel = new System.Windows.Forms.Label();
			this.SuccesFullLabel = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
			this.label26 = new System.Windows.Forms.Label();
			this.changeWarBtn = new LaserGRBL.UserControls.GrblButton();
			this.label27 = new System.Windows.Forms.Label();
			this.warningSoundLabel = new System.Windows.Forms.Label();
			this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
			this.label29 = new System.Windows.Forms.Label();
			this.changeFatBtn = new LaserGRBL.UserControls.GrblButton();
			this.label30 = new System.Windows.Forms.Label();
			this.fatalSoundLabel = new System.Windows.Forms.Label();
			this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
			this.label34 = new System.Windows.Forms.Label();
			this.changeConBtn = new LaserGRBL.UserControls.GrblButton();
			this.label35 = new System.Windows.Forms.Label();
			this.connectSoundLabel = new System.Windows.Forms.Label();
			this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
			this.label37 = new System.Windows.Forms.Label();
			this.changeDconBtn = new LaserGRBL.UserControls.GrblButton();
			this.label38 = new System.Windows.Forms.Label();
			this.disconnectSoundLabel = new System.Windows.Forms.Label();
			this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
			this.LblSuccessSound = new System.Windows.Forms.Label();
			this.changeSucBtn = new LaserGRBL.UserControls.GrblButton();
			this.label25 = new System.Windows.Forms.Label();
			this.successSoundLabel = new System.Windows.Forms.Label();
			this.label32 = new System.Windows.Forms.Label();
			this.label36 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.tableLayoutPanel17 = new System.Windows.Forms.TableLayoutPanel();
			this.label44 = new System.Windows.Forms.Label();
			this.BtnTelegNoteInfo = new LaserGRBL.UserControls.ImageButton();
			this.label31 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.TxtNotification = new System.Windows.Forms.TextBox();
			this.BtnTestNotification = new LaserGRBL.UserControls.GrblButton();
			this.tableLayoutPanel19 = new System.Windows.Forms.TableLayoutPanel();
			this.UdTelegramNotificationThreshold = new System.Windows.Forms.NumericUpDown();
			this.label45 = new System.Windows.Forms.Label();
			this.label42 = new System.Windows.Forms.Label();
			this.TpOptions = new System.Windows.Forms.TabPage();
			this.Tlp = new System.Windows.Forms.TableLayoutPanel();
			this.CBGraphicMode = new LaserGRBL.UserControls.FlatComboBox();
			this.BtnRenderingMode = new LaserGRBL.UserControls.ImageButton();
			this.CbDisableSafetyCD = new System.Windows.Forms.CheckBox();
			this.label47 = new System.Windows.Forms.Label();
			this.CbQuietSafetyCB = new System.Windows.Forms.CheckBox();
			this.label48 = new System.Windows.Forms.Label();
			this.CbLegacyIcons = new System.Windows.Forms.CheckBox();
			this.label49 = new System.Windows.Forms.Label();
			this.label50 = new System.Windows.Forms.Label();
			this.SoundBrowserDialog = new System.Windows.Forms.OpenFileDialog();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.MainTabPage.SuspendLayout();
			this.TpHardware.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.TpRasterImport.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.TpVectorImport.SuspendLayout();
			this.tableLayoutPanel18.SuspendLayout();
			this.TpJogControl.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.TpAutoCooling.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.tableLayoutPanel7.SuspendLayout();
			this.tableLayoutPanel8.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.TpGCodeSettings.SuspendLayout();
			this.tableLayoutPanel9.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.TpSoundSettings.SuspendLayout();
			this.tableLayoutPanel16.SuspendLayout();
			this.tableLayoutPanel10.SuspendLayout();
			this.tableLayoutPanel11.SuspendLayout();
			this.tableLayoutPanel12.SuspendLayout();
			this.tableLayoutPanel14.SuspendLayout();
			this.tableLayoutPanel15.SuspendLayout();
			this.tableLayoutPanel13.SuspendLayout();
			this.tableLayoutPanel17.SuspendLayout();
			this.tableLayoutPanel19.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.UdTelegramNotificationThreshold)).BeginInit();
			this.TpOptions.SuspendLayout();
			this.Tlp.SuspendLayout();
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
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.BtnCancel, "BtnCancel");
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
			this.MainTabPage.Controls.Add(this.TpHardware);
			this.MainTabPage.Controls.Add(this.TpRasterImport);
			this.MainTabPage.Controls.Add(this.TpVectorImport);
			this.MainTabPage.Controls.Add(this.TpJogControl);
			this.MainTabPage.Controls.Add(this.TpAutoCooling);
			this.MainTabPage.Controls.Add(this.TpGCodeSettings);
			this.MainTabPage.Controls.Add(this.TpSoundSettings);
			this.MainTabPage.Controls.Add(this.TpOptions);
			resources.ApplyResources(this.MainTabPage, "MainTabPage");
			this.MainTabPage.Name = "MainTabPage";
			this.MainTabPage.SelectedIndex = 0;
			// 
			// TpHardware
			// 
			this.TpHardware.Controls.Add(this.tableLayoutPanel3);
			resources.ApplyResources(this.TpHardware, "TpHardware");
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
			this.tableLayoutPanel3.Controls.Add(this.BtnFType, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.CbQueryDI, 1, 7);
			this.tableLayoutPanel3.Controls.Add(this.label46, 2, 7);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			// 
			// CBCore
			// 
			this.CBCore.BackColor = System.Drawing.Color.White;
			resources.ApplyResources(this.CBCore, "CBCore");
			this.CBCore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBCore.ForeColor = System.Drawing.SystemColors.ControlText;
			this.CBCore.FormattingEnabled = true;
			this.CBCore.Name = "CBCore";
			// 
			// CbThreadingMode
			// 
			this.CbThreadingMode.BackColor = System.Drawing.Color.White;
			resources.ApplyResources(this.CbThreadingMode, "CbThreadingMode");
			this.CbThreadingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbThreadingMode.ForeColor = System.Drawing.SystemColors.ControlText;
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
			this.CBStreamingMode.BackColor = System.Drawing.Color.White;
			resources.ApplyResources(this.CBStreamingMode, "CBStreamingMode");
			this.CBStreamingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBStreamingMode.ForeColor = System.Drawing.SystemColors.ControlText;
			this.CBStreamingMode.FormattingEnabled = true;
			this.CBStreamingMode.Name = "CBStreamingMode";
			// 
			// BtnStreamingMode
			// 
			this.BtnStreamingMode.AltImage = null;
			this.BtnStreamingMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnStreamingMode.Caption = null;
			this.BtnStreamingMode.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnStreamingMode.Image = ((System.Drawing.Image)(resources.GetObject("BtnStreamingMode.Image")));
			resources.ApplyResources(this.BtnStreamingMode, "BtnStreamingMode");
			this.BtnStreamingMode.Name = "BtnStreamingMode";
			this.BtnStreamingMode.RoundedBorders = false;
			this.BtnStreamingMode.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnStreamingMode.UseAltImage = false;
			this.BtnStreamingMode.Click += new System.EventHandler(this.BtnStreamingMode_Click);
			// 
			// CBProtocol
			// 
			this.CBProtocol.BackColor = System.Drawing.Color.White;
			resources.ApplyResources(this.CBProtocol, "CBProtocol");
			this.CBProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBProtocol.ForeColor = System.Drawing.SystemColors.ControlText;
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
			this.BtnProtocol.AltImage = null;
			this.BtnProtocol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnProtocol.Caption = null;
			this.BtnProtocol.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnProtocol.Image = ((System.Drawing.Image)(resources.GetObject("BtnProtocol.Image")));
			resources.ApplyResources(this.BtnProtocol, "BtnProtocol");
			this.BtnProtocol.Name = "BtnProtocol";
			this.BtnProtocol.RoundedBorders = false;
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
			this.BtnThreadingModel.AltImage = null;
			this.BtnThreadingModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnThreadingModel.Caption = null;
			this.BtnThreadingModel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnThreadingModel.Image = ((System.Drawing.Image)(resources.GetObject("BtnThreadingModel.Image")));
			resources.ApplyResources(this.BtnThreadingModel, "BtnThreadingModel");
			this.BtnThreadingModel.Name = "BtnThreadingModel";
			this.BtnThreadingModel.RoundedBorders = false;
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
			// BtnFType
			// 
			this.BtnFType.AltImage = null;
			this.BtnFType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnFType.Caption = null;
			this.BtnFType.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnFType.Image = ((System.Drawing.Image)(resources.GetObject("BtnFType.Image")));
			resources.ApplyResources(this.BtnFType, "BtnFType");
			this.BtnFType.Name = "BtnFType";
			this.BtnFType.RoundedBorders = false;
			this.BtnFType.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnFType.UseAltImage = false;
			this.BtnFType.Click += new System.EventHandler(this.BtnFType_Click);
			// 
			// CbQueryDI
			// 
			resources.ApplyResources(this.CbQueryDI, "CbQueryDI");
			this.CbQueryDI.Name = "CbQueryDI";
			this.CbQueryDI.UseVisualStyleBackColor = true;
			// 
			// label46
			// 
			resources.ApplyResources(this.label46, "label46");
			this.label46.Name = "label46";
			// 
			// TpRasterImport
			// 
			this.TpRasterImport.Controls.Add(this.tableLayoutPanel4);
			resources.ApplyResources(this.TpRasterImport, "TpRasterImport");
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
			this.tableLayoutPanel4.Controls.Add(this.CbHiRes, 1, 2);
			this.tableLayoutPanel4.Controls.Add(this.label22, 2, 2);
			this.tableLayoutPanel4.Controls.Add(this.CbDisableSkip, 1, 3);
			this.tableLayoutPanel4.Controls.Add(this.label39, 2, 3);
			this.tableLayoutPanel4.Controls.Add(this.CbDisableBoundWarn, 1, 4);
			this.tableLayoutPanel4.Controls.Add(this.label40, 2, 4);
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
			this.BtnModulationInfo.AltImage = null;
			this.BtnModulationInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnModulationInfo.Caption = null;
			this.BtnModulationInfo.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnModulationInfo.Image = ((System.Drawing.Image)(resources.GetObject("BtnModulationInfo.Image")));
			resources.ApplyResources(this.BtnModulationInfo, "BtnModulationInfo");
			this.BtnModulationInfo.Name = "BtnModulationInfo";
			this.BtnModulationInfo.RoundedBorders = false;
			this.BtnModulationInfo.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnModulationInfo.UseAltImage = false;
			this.BtnModulationInfo.Click += new System.EventHandler(this.BtnModulationInfo_Click);
			// 
			// CbHiRes
			// 
			resources.ApplyResources(this.CbHiRes, "CbHiRes");
			this.CbHiRes.Name = "CbHiRes";
			this.CbHiRes.UseVisualStyleBackColor = true;
			// 
			// label22
			// 
			resources.ApplyResources(this.label22, "label22");
			this.label22.Name = "label22";
			// 
			// CbDisableSkip
			// 
			resources.ApplyResources(this.CbDisableSkip, "CbDisableSkip");
			this.CbDisableSkip.Name = "CbDisableSkip";
			this.CbDisableSkip.UseVisualStyleBackColor = true;
			// 
			// label39
			// 
			resources.ApplyResources(this.label39, "label39");
			this.label39.Name = "label39";
			// 
			// CbDisableBoundWarn
			// 
			resources.ApplyResources(this.CbDisableBoundWarn, "CbDisableBoundWarn");
			this.CbDisableBoundWarn.Name = "CbDisableBoundWarn";
			this.CbDisableBoundWarn.UseVisualStyleBackColor = true;
			// 
			// label40
			// 
			resources.ApplyResources(this.label40, "label40");
			this.label40.Name = "label40";
			// 
			// TpVectorImport
			// 
			this.TpVectorImport.Controls.Add(this.tableLayoutPanel18);
			resources.ApplyResources(this.TpVectorImport, "TpVectorImport");
			this.TpVectorImport.Name = "TpVectorImport";
			this.TpVectorImport.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel18
			// 
			resources.ApplyResources(this.tableLayoutPanel18, "tableLayoutPanel18");
			this.tableLayoutPanel18.Controls.Add(this.label43, 2, 0);
			this.tableLayoutPanel18.Controls.Add(this.CbSmartBezier, 1, 0);
			this.tableLayoutPanel18.Controls.Add(this.imageButton1, 0, 0);
			this.tableLayoutPanel18.Name = "tableLayoutPanel18";
			// 
			// label43
			// 
			resources.ApplyResources(this.label43, "label43");
			this.label43.Name = "label43";
			// 
			// CbSmartBezier
			// 
			resources.ApplyResources(this.CbSmartBezier, "CbSmartBezier");
			this.CbSmartBezier.Name = "CbSmartBezier";
			this.CbSmartBezier.UseVisualStyleBackColor = true;
			// 
			// imageButton1
			// 
			this.imageButton1.AltImage = null;
			this.imageButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.imageButton1.Caption = null;
			this.imageButton1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.imageButton1.Image = ((System.Drawing.Image)(resources.GetObject("imageButton1.Image")));
			resources.ApplyResources(this.imageButton1, "imageButton1");
			this.imageButton1.Name = "imageButton1";
			this.imageButton1.RoundedBorders = false;
			this.imageButton1.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.imageButton1.UseAltImage = false;
			// 
			// TpJogControl
			// 
			this.TpJogControl.Controls.Add(this.tableLayoutPanel5);
			resources.ApplyResources(this.TpJogControl, "TpJogControl");
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
			this.tableLayoutPanel5.Controls.Add(this.CbClickNJog, 1, 2);
			this.tableLayoutPanel5.Controls.Add(this.label41, 2, 2);
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
			// CbClickNJog
			// 
			resources.ApplyResources(this.CbClickNJog, "CbClickNJog");
			this.CbClickNJog.Name = "CbClickNJog";
			this.CbClickNJog.UseVisualStyleBackColor = true;
			// 
			// label41
			// 
			resources.ApplyResources(this.label41, "label41");
			this.label41.Name = "label41";
			// 
			// TpAutoCooling
			// 
			this.TpAutoCooling.Controls.Add(this.tableLayoutPanel6);
			resources.ApplyResources(this.TpAutoCooling, "TpAutoCooling");
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
			this.tableLayoutPanel6.Controls.Add(this.LblWarnOrturAC, 2, 4);
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
			// LblWarnOrturAC
			// 
			resources.ApplyResources(this.LblWarnOrturAC, "LblWarnOrturAC");
			this.LblWarnOrturAC.ForeColor = System.Drawing.Color.Red;
			this.LblWarnOrturAC.Name = "LblWarnOrturAC";
			// 
			// TpGCodeSettings
			// 
			this.TpGCodeSettings.Controls.Add(this.tableLayoutPanel9);
			resources.ApplyResources(this.TpGCodeSettings, "TpGCodeSettings");
			this.TpGCodeSettings.Name = "TpGCodeSettings";
			this.TpGCodeSettings.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel9
			// 
			resources.ApplyResources(this.tableLayoutPanel9, "tableLayoutPanel9");
			this.tableLayoutPanel9.Controls.Add(this.LblHeader, 2, 0);
			this.tableLayoutPanel9.Controls.Add(this.groupBox1, 1, 0);
			this.tableLayoutPanel9.Controls.Add(this.groupBox2, 1, 2);
			this.tableLayoutPanel9.Controls.Add(this.groupBox3, 1, 1);
			this.tableLayoutPanel9.Controls.Add(this.LblFooter, 2, 2);
			this.tableLayoutPanel9.Controls.Add(this.LblPasses, 2, 1);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			// 
			// LblHeader
			// 
			resources.ApplyResources(this.LblHeader, "LblHeader");
			this.LblHeader.Name = "LblHeader";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.TBHeader);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// TBHeader
			// 
			resources.ApplyResources(this.TBHeader, "TBHeader");
			this.TBHeader.Name = "TBHeader";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.TBFooter);
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			// 
			// TBFooter
			// 
			resources.ApplyResources(this.TBFooter, "TBFooter");
			this.TBFooter.Name = "TBFooter";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.TBPasses);
			resources.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			// 
			// TBPasses
			// 
			resources.ApplyResources(this.TBPasses, "TBPasses");
			this.TBPasses.Name = "TBPasses";
			// 
			// LblFooter
			// 
			resources.ApplyResources(this.LblFooter, "LblFooter");
			this.LblFooter.Name = "LblFooter";
			// 
			// LblPasses
			// 
			resources.ApplyResources(this.LblPasses, "LblPasses");
			this.LblPasses.Name = "LblPasses";
			// 
			// TpSoundSettings
			// 
			this.TpSoundSettings.Controls.Add(this.tableLayoutPanel16);
			this.TpSoundSettings.Controls.Add(this.tableLayoutPanel10);
			resources.ApplyResources(this.TpSoundSettings, "TpSoundSettings");
			this.TpSoundSettings.Name = "TpSoundSettings";
			this.TpSoundSettings.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel16
			// 
			resources.ApplyResources(this.tableLayoutPanel16, "tableLayoutPanel16");
			this.tableLayoutPanel16.Controls.Add(this.CbPlaySuccess, 0, 0);
			this.tableLayoutPanel16.Controls.Add(this.CbPlayWarning, 0, 1);
			this.tableLayoutPanel16.Controls.Add(this.CbPlayFatal, 0, 2);
			this.tableLayoutPanel16.Controls.Add(this.CbPlayConnect, 0, 3);
			this.tableLayoutPanel16.Controls.Add(this.CbPlayDisconnect, 0, 4);
			this.tableLayoutPanel16.ForeColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel16.Name = "tableLayoutPanel16";
			// 
			// CbPlaySuccess
			// 
			resources.ApplyResources(this.CbPlaySuccess, "CbPlaySuccess");
			this.CbPlaySuccess.Name = "CbPlaySuccess";
			this.CbPlaySuccess.UseVisualStyleBackColor = true;
			// 
			// CbPlayWarning
			// 
			resources.ApplyResources(this.CbPlayWarning, "CbPlayWarning");
			this.CbPlayWarning.Name = "CbPlayWarning";
			this.CbPlayWarning.UseVisualStyleBackColor = true;
			// 
			// CbPlayFatal
			// 
			resources.ApplyResources(this.CbPlayFatal, "CbPlayFatal");
			this.CbPlayFatal.Name = "CbPlayFatal";
			this.CbPlayFatal.UseVisualStyleBackColor = true;
			// 
			// CbPlayConnect
			// 
			resources.ApplyResources(this.CbPlayConnect, "CbPlayConnect");
			this.CbPlayConnect.Name = "CbPlayConnect";
			this.CbPlayConnect.UseVisualStyleBackColor = true;
			// 
			// CbPlayDisconnect
			// 
			resources.ApplyResources(this.CbPlayDisconnect, "CbPlayDisconnect");
			this.CbPlayDisconnect.Name = "CbPlayDisconnect";
			this.CbPlayDisconnect.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel10
			// 
			resources.ApplyResources(this.tableLayoutPanel10, "tableLayoutPanel10");
			this.tableLayoutPanel10.Controls.Add(this.CbTelegramNotification, 0, 6);
			this.tableLayoutPanel10.Controls.Add(this.DisconnectFullLabel, 0, 4);
			this.tableLayoutPanel10.Controls.Add(this.ConnectFullLabel, 0, 3);
			this.tableLayoutPanel10.Controls.Add(this.ErrorFullLabel, 0, 2);
			this.tableLayoutPanel10.Controls.Add(this.WarningFullLabel, 0, 1);
			this.tableLayoutPanel10.Controls.Add(this.SuccesFullLabel, 0, 0);
			this.tableLayoutPanel10.Controls.Add(this.label23, 2, 0);
			this.tableLayoutPanel10.Controls.Add(this.label24, 2, 1);
			this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel11, 1, 1);
			this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel12, 1, 2);
			this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel14, 1, 3);
			this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel15, 1, 4);
			this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel13, 1, 0);
			this.tableLayoutPanel10.Controls.Add(this.label32, 2, 3);
			this.tableLayoutPanel10.Controls.Add(this.label36, 2, 4);
			this.tableLayoutPanel10.Controls.Add(this.label28, 2, 2);
			this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel17, 1, 6);
			this.tableLayoutPanel10.Controls.Add(this.label42, 2, 6);
			this.tableLayoutPanel10.Name = "tableLayoutPanel10";
			// 
			// CbTelegramNotification
			// 
			resources.ApplyResources(this.CbTelegramNotification, "CbTelegramNotification");
			this.CbTelegramNotification.Name = "CbTelegramNotification";
			this.CbTelegramNotification.UseVisualStyleBackColor = true;
			this.CbTelegramNotification.CheckedChanged += new System.EventHandler(this.CbTelegramNotification_CheckedChanged);
			// 
			// DisconnectFullLabel
			// 
			resources.ApplyResources(this.DisconnectFullLabel, "DisconnectFullLabel");
			this.DisconnectFullLabel.Name = "DisconnectFullLabel";
			// 
			// ConnectFullLabel
			// 
			resources.ApplyResources(this.ConnectFullLabel, "ConnectFullLabel");
			this.ConnectFullLabel.Name = "ConnectFullLabel";
			// 
			// ErrorFullLabel
			// 
			resources.ApplyResources(this.ErrorFullLabel, "ErrorFullLabel");
			this.ErrorFullLabel.Name = "ErrorFullLabel";
			// 
			// WarningFullLabel
			// 
			resources.ApplyResources(this.WarningFullLabel, "WarningFullLabel");
			this.WarningFullLabel.Name = "WarningFullLabel";
			// 
			// SuccesFullLabel
			// 
			resources.ApplyResources(this.SuccesFullLabel, "SuccesFullLabel");
			this.SuccesFullLabel.Name = "SuccesFullLabel";
			// 
			// label23
			// 
			resources.ApplyResources(this.label23, "label23");
			this.label23.Name = "label23";
			// 
			// label24
			// 
			resources.ApplyResources(this.label24, "label24");
			this.label24.Name = "label24";
			// 
			// tableLayoutPanel11
			// 
			resources.ApplyResources(this.tableLayoutPanel11, "tableLayoutPanel11");
			this.tableLayoutPanel11.Controls.Add(this.label26, 0, 0);
			this.tableLayoutPanel11.Controls.Add(this.changeWarBtn, 0, 1);
			this.tableLayoutPanel11.Controls.Add(this.label27, 1, 0);
			this.tableLayoutPanel11.Controls.Add(this.warningSoundLabel, 1, 1);
			this.tableLayoutPanel11.Name = "tableLayoutPanel11";
			// 
			// label26
			// 
			resources.ApplyResources(this.label26, "label26");
			this.label26.Name = "label26";
			// 
			// changeWarBtn
			// 
			resources.ApplyResources(this.changeWarBtn, "changeWarBtn");
			this.changeWarBtn.Name = "changeWarBtn";
			this.changeWarBtn.UseVisualStyleBackColor = true;
			this.changeWarBtn.Click += new System.EventHandler(this.changeWarBtn_Click);
			// 
			// label27
			// 
			resources.ApplyResources(this.label27, "label27");
			this.label27.Name = "label27";
			// 
			// warningSoundLabel
			// 
			resources.ApplyResources(this.warningSoundLabel, "warningSoundLabel");
			this.warningSoundLabel.Name = "warningSoundLabel";
			// 
			// tableLayoutPanel12
			// 
			resources.ApplyResources(this.tableLayoutPanel12, "tableLayoutPanel12");
			this.tableLayoutPanel12.Controls.Add(this.label29, 0, 0);
			this.tableLayoutPanel12.Controls.Add(this.changeFatBtn, 0, 1);
			this.tableLayoutPanel12.Controls.Add(this.label30, 1, 0);
			this.tableLayoutPanel12.Controls.Add(this.fatalSoundLabel, 1, 1);
			this.tableLayoutPanel12.Name = "tableLayoutPanel12";
			// 
			// label29
			// 
			resources.ApplyResources(this.label29, "label29");
			this.label29.Name = "label29";
			// 
			// changeFatBtn
			// 
			resources.ApplyResources(this.changeFatBtn, "changeFatBtn");
			this.changeFatBtn.Name = "changeFatBtn";
			this.changeFatBtn.UseVisualStyleBackColor = true;
			this.changeFatBtn.Click += new System.EventHandler(this.changeFatBtn_Click);
			// 
			// label30
			// 
			resources.ApplyResources(this.label30, "label30");
			this.label30.Name = "label30";
			// 
			// fatalSoundLabel
			// 
			resources.ApplyResources(this.fatalSoundLabel, "fatalSoundLabel");
			this.fatalSoundLabel.Name = "fatalSoundLabel";
			// 
			// tableLayoutPanel14
			// 
			resources.ApplyResources(this.tableLayoutPanel14, "tableLayoutPanel14");
			this.tableLayoutPanel14.Controls.Add(this.label34, 0, 0);
			this.tableLayoutPanel14.Controls.Add(this.changeConBtn, 0, 1);
			this.tableLayoutPanel14.Controls.Add(this.label35, 1, 0);
			this.tableLayoutPanel14.Controls.Add(this.connectSoundLabel, 1, 1);
			this.tableLayoutPanel14.Name = "tableLayoutPanel14";
			// 
			// label34
			// 
			resources.ApplyResources(this.label34, "label34");
			this.label34.Name = "label34";
			// 
			// changeConBtn
			// 
			resources.ApplyResources(this.changeConBtn, "changeConBtn");
			this.changeConBtn.Name = "changeConBtn";
			this.changeConBtn.UseVisualStyleBackColor = true;
			this.changeConBtn.Click += new System.EventHandler(this.changeConBtn_Click);
			// 
			// label35
			// 
			resources.ApplyResources(this.label35, "label35");
			this.label35.Name = "label35";
			// 
			// connectSoundLabel
			// 
			resources.ApplyResources(this.connectSoundLabel, "connectSoundLabel");
			this.connectSoundLabel.Name = "connectSoundLabel";
			// 
			// tableLayoutPanel15
			// 
			resources.ApplyResources(this.tableLayoutPanel15, "tableLayoutPanel15");
			this.tableLayoutPanel15.Controls.Add(this.label37, 0, 0);
			this.tableLayoutPanel15.Controls.Add(this.changeDconBtn, 0, 1);
			this.tableLayoutPanel15.Controls.Add(this.label38, 1, 0);
			this.tableLayoutPanel15.Controls.Add(this.disconnectSoundLabel, 1, 1);
			this.tableLayoutPanel15.Name = "tableLayoutPanel15";
			// 
			// label37
			// 
			resources.ApplyResources(this.label37, "label37");
			this.label37.Name = "label37";
			// 
			// changeDconBtn
			// 
			resources.ApplyResources(this.changeDconBtn, "changeDconBtn");
			this.changeDconBtn.Name = "changeDconBtn";
			this.changeDconBtn.UseVisualStyleBackColor = true;
			this.changeDconBtn.Click += new System.EventHandler(this.changeDconBtn_Click);
			// 
			// label38
			// 
			resources.ApplyResources(this.label38, "label38");
			this.label38.Name = "label38";
			// 
			// disconnectSoundLabel
			// 
			resources.ApplyResources(this.disconnectSoundLabel, "disconnectSoundLabel");
			this.disconnectSoundLabel.Name = "disconnectSoundLabel";
			// 
			// tableLayoutPanel13
			// 
			resources.ApplyResources(this.tableLayoutPanel13, "tableLayoutPanel13");
			this.tableLayoutPanel13.Controls.Add(this.LblSuccessSound, 0, 0);
			this.tableLayoutPanel13.Controls.Add(this.changeSucBtn, 0, 1);
			this.tableLayoutPanel13.Controls.Add(this.label25, 1, 0);
			this.tableLayoutPanel13.Controls.Add(this.successSoundLabel, 1, 1);
			this.tableLayoutPanel13.Name = "tableLayoutPanel13";
			// 
			// LblSuccessSound
			// 
			resources.ApplyResources(this.LblSuccessSound, "LblSuccessSound");
			this.LblSuccessSound.Name = "LblSuccessSound";
			// 
			// changeSucBtn
			// 
			resources.ApplyResources(this.changeSucBtn, "changeSucBtn");
			this.changeSucBtn.Name = "changeSucBtn";
			this.changeSucBtn.UseVisualStyleBackColor = true;
			this.changeSucBtn.Click += new System.EventHandler(this.changeSucBtn_Click);
			// 
			// label25
			// 
			resources.ApplyResources(this.label25, "label25");
			this.label25.Name = "label25";
			// 
			// successSoundLabel
			// 
			resources.ApplyResources(this.successSoundLabel, "successSoundLabel");
			this.successSoundLabel.Name = "successSoundLabel";
			// 
			// label32
			// 
			resources.ApplyResources(this.label32, "label32");
			this.label32.Name = "label32";
			// 
			// label36
			// 
			resources.ApplyResources(this.label36, "label36");
			this.label36.Name = "label36";
			// 
			// label28
			// 
			resources.ApplyResources(this.label28, "label28");
			this.label28.Name = "label28";
			// 
			// tableLayoutPanel17
			// 
			resources.ApplyResources(this.tableLayoutPanel17, "tableLayoutPanel17");
			this.tableLayoutPanel17.Controls.Add(this.label44, 0, 2);
			this.tableLayoutPanel17.Controls.Add(this.BtnTelegNoteInfo, 2, 0);
			this.tableLayoutPanel17.Controls.Add(this.label31, 0, 0);
			this.tableLayoutPanel17.Controls.Add(this.label33, 0, 1);
			this.tableLayoutPanel17.Controls.Add(this.TxtNotification, 1, 1);
			this.tableLayoutPanel17.Controls.Add(this.BtnTestNotification, 2, 1);
			this.tableLayoutPanel17.Controls.Add(this.tableLayoutPanel19, 2, 2);
			this.tableLayoutPanel17.Name = "tableLayoutPanel17";
			// 
			// label44
			// 
			resources.ApplyResources(this.label44, "label44");
			this.tableLayoutPanel17.SetColumnSpan(this.label44, 2);
			this.label44.Name = "label44";
			// 
			// BtnTelegNoteInfo
			// 
			this.BtnTelegNoteInfo.AltImage = null;
			this.BtnTelegNoteInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnTelegNoteInfo.Caption = null;
			this.BtnTelegNoteInfo.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnTelegNoteInfo.Image = ((System.Drawing.Image)(resources.GetObject("BtnTelegNoteInfo.Image")));
			resources.ApplyResources(this.BtnTelegNoteInfo, "BtnTelegNoteInfo");
			this.BtnTelegNoteInfo.Name = "BtnTelegNoteInfo";
			this.BtnTelegNoteInfo.RoundedBorders = false;
			this.BtnTelegNoteInfo.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnTelegNoteInfo.UseAltImage = false;
			this.BtnTelegNoteInfo.Click += new System.EventHandler(this.BtnTelegNoteInfo_Click);
			// 
			// label31
			// 
			resources.ApplyResources(this.label31, "label31");
			this.tableLayoutPanel17.SetColumnSpan(this.label31, 2);
			this.label31.Name = "label31";
			// 
			// label33
			// 
			resources.ApplyResources(this.label33, "label33");
			this.label33.Name = "label33";
			// 
			// TxtNotification
			// 
			resources.ApplyResources(this.TxtNotification, "TxtNotification");
			this.TxtNotification.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.TxtNotification.Name = "TxtNotification";
			this.TxtNotification.TextChanged += new System.EventHandler(this.TbNotification_TextChanged);
			// 
			// BtnTestNotification
			// 
			resources.ApplyResources(this.BtnTestNotification, "BtnTestNotification");
			this.BtnTestNotification.Name = "BtnTestNotification";
			this.BtnTestNotification.UseVisualStyleBackColor = true;
			this.BtnTestNotification.Click += new System.EventHandler(this.BtnTestNotification_Click);
			// 
			// tableLayoutPanel19
			// 
			resources.ApplyResources(this.tableLayoutPanel19, "tableLayoutPanel19");
			this.tableLayoutPanel19.Controls.Add(this.UdTelegramNotificationThreshold, 0, 0);
			this.tableLayoutPanel19.Controls.Add(this.label45, 1, 0);
			this.tableLayoutPanel19.Name = "tableLayoutPanel19";
			// 
			// UdTelegramNotificationThreshold
			// 
			resources.ApplyResources(this.UdTelegramNotificationThreshold, "UdTelegramNotificationThreshold");
			this.UdTelegramNotificationThreshold.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.UdTelegramNotificationThreshold.Name = "UdTelegramNotificationThreshold";
			// 
			// label45
			// 
			resources.ApplyResources(this.label45, "label45");
			this.label45.Name = "label45";
			// 
			// label42
			// 
			resources.ApplyResources(this.label42, "label42");
			this.label42.Name = "label42";
			// 
			// TpOptions
			// 
			this.TpOptions.Controls.Add(this.Tlp);
			resources.ApplyResources(this.TpOptions, "TpOptions");
			this.TpOptions.Name = "TpOptions";
			this.TpOptions.UseVisualStyleBackColor = true;
			// 
			// Tlp
			// 
			resources.ApplyResources(this.Tlp, "Tlp");
			this.Tlp.Controls.Add(this.CBGraphicMode, 1, 3);
			this.Tlp.Controls.Add(this.BtnRenderingMode, 0, 3);
			this.Tlp.Controls.Add(this.CbDisableSafetyCD, 1, 1);
			this.Tlp.Controls.Add(this.label47, 2, 1);
			this.Tlp.Controls.Add(this.CbQuietSafetyCB, 1, 0);
			this.Tlp.Controls.Add(this.label48, 2, 0);
			this.Tlp.Controls.Add(this.CbLegacyIcons, 1, 2);
			this.Tlp.Controls.Add(this.label49, 2, 2);
			this.Tlp.Controls.Add(this.label50, 2, 3);
			this.Tlp.Name = "Tlp";
			// 
			// CBGraphicMode
			// 
			this.CBGraphicMode.BackColor = System.Drawing.Color.White;
			resources.ApplyResources(this.CBGraphicMode, "CBGraphicMode");
			this.CBGraphicMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBGraphicMode.ForeColor = System.Drawing.SystemColors.ControlText;
			this.CBGraphicMode.FormattingEnabled = true;
			this.CBGraphicMode.Name = "CBGraphicMode";
			// 
			// BtnRenderingMode
			// 
			this.BtnRenderingMode.AltImage = null;
			this.BtnRenderingMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnRenderingMode.Caption = null;
			this.BtnRenderingMode.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnRenderingMode.Image = ((System.Drawing.Image)(resources.GetObject("BtnRenderingMode.Image")));
			resources.ApplyResources(this.BtnRenderingMode, "BtnRenderingMode");
			this.BtnRenderingMode.Name = "BtnRenderingMode";
			this.BtnRenderingMode.RoundedBorders = false;
			this.BtnRenderingMode.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnRenderingMode.UseAltImage = false;
			this.BtnRenderingMode.Click += new System.EventHandler(this.BtnRenderingMode_Click);
			// 
			// CbDisableSafetyCD
			// 
			resources.ApplyResources(this.CbDisableSafetyCD, "CbDisableSafetyCD");
			this.CbDisableSafetyCD.Name = "CbDisableSafetyCD";
			this.CbDisableSafetyCD.UseVisualStyleBackColor = true;
			// 
			// label47
			// 
			resources.ApplyResources(this.label47, "label47");
			this.label47.Name = "label47";
			// 
			// CbQuietSafetyCB
			// 
			resources.ApplyResources(this.CbQuietSafetyCB, "CbQuietSafetyCB");
			this.CbQuietSafetyCB.Name = "CbQuietSafetyCB";
			this.CbQuietSafetyCB.UseVisualStyleBackColor = true;
			// 
			// label48
			// 
			resources.ApplyResources(this.label48, "label48");
			this.label48.Name = "label48";
			// 
			// CbLegacyIcons
			// 
			resources.ApplyResources(this.CbLegacyIcons, "CbLegacyIcons");
			this.CbLegacyIcons.Name = "CbLegacyIcons";
			this.CbLegacyIcons.UseVisualStyleBackColor = true;
			// 
			// label49
			// 
			resources.ApplyResources(this.label49, "label49");
			this.label49.Name = "label49";
			// 
			// label50
			// 
			resources.ApplyResources(this.label50, "label50");
			this.label50.Name = "label50";
			// 
			// SoundBrowserDialog
			// 
			resources.ApplyResources(this.SoundBrowserDialog, "SoundBrowserDialog");
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
			this.TpVectorImport.ResumeLayout(false);
			this.TpVectorImport.PerformLayout();
			this.tableLayoutPanel18.ResumeLayout(false);
			this.tableLayoutPanel18.PerformLayout();
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
			this.TpGCodeSettings.ResumeLayout(false);
			this.TpGCodeSettings.PerformLayout();
			this.tableLayoutPanel9.ResumeLayout(false);
			this.tableLayoutPanel9.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.TpSoundSettings.ResumeLayout(false);
			this.TpSoundSettings.PerformLayout();
			this.tableLayoutPanel16.ResumeLayout(false);
			this.tableLayoutPanel16.PerformLayout();
			this.tableLayoutPanel10.ResumeLayout(false);
			this.tableLayoutPanel10.PerformLayout();
			this.tableLayoutPanel11.ResumeLayout(false);
			this.tableLayoutPanel11.PerformLayout();
			this.tableLayoutPanel12.ResumeLayout(false);
			this.tableLayoutPanel12.PerformLayout();
			this.tableLayoutPanel14.ResumeLayout(false);
			this.tableLayoutPanel14.PerformLayout();
			this.tableLayoutPanel15.ResumeLayout(false);
			this.tableLayoutPanel15.PerformLayout();
			this.tableLayoutPanel13.ResumeLayout(false);
			this.tableLayoutPanel13.PerformLayout();
			this.tableLayoutPanel17.ResumeLayout(false);
			this.tableLayoutPanel17.PerformLayout();
			this.tableLayoutPanel19.ResumeLayout(false);
			this.tableLayoutPanel19.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.UdTelegramNotificationThreshold)).EndInit();
			this.TpOptions.ResumeLayout(false);
			this.TpOptions.PerformLayout();
			this.Tlp.ResumeLayout(false);
			this.Tlp.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private LaserGRBL.UserControls.GrblButton BtnCancel;
		private LaserGRBL.UserControls.GrblButton BtnSave;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.CheckBox CBSupportPWM;
		private System.Windows.Forms.Label label1;
		private UserControls.ImageButton BtnModulationInfo;
		private LaserGRBL.UserControls.FlatComboBox CBProtocol;
		private System.Windows.Forms.Label label3;
		private UserControls.ImageButton BtnProtocol;
		private System.Windows.Forms.Label label4;
		private LaserGRBL.UserControls.FlatComboBox CBStreamingMode;
		private UserControls.ImageButton BtnStreamingMode;
		private System.Windows.Forms.CheckBox CbUnidirectional;
		private System.Windows.Forms.Label label5;
		private LaserGRBL.UserControls.FlatComboBox CbThreadingMode;
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
        private LaserGRBL.UserControls.FlatComboBox CBCore;
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
		private LaserGRBL.UserControls.FlatComboBox CbOnMin;
		private LaserGRBL.UserControls.FlatComboBox CbOnSec;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
		private System.Windows.Forms.Label label17;
		private LaserGRBL.UserControls.FlatComboBox CbOffMin;
		private LaserGRBL.UserControls.FlatComboBox CbOffSec;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label21;
		private System.Windows.Forms.TabPage TpGCodeSettings;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
		private System.Windows.Forms.Label LblHeader;
		private LaserGRBL.UserControls.GrblGroupBox groupBox1;
		private System.Windows.Forms.TextBox TBHeader;
		private System.Windows.Forms.Label LblPasses;
		private System.Windows.Forms.Label LblFooter;
		private LaserGRBL.UserControls.GrblGroupBox groupBox2;
		private System.Windows.Forms.TextBox TBFooter;
		private LaserGRBL.UserControls.GrblGroupBox groupBox3;
		private System.Windows.Forms.TextBox TBPasses;
        private UserControls.ImageButton BtnFType;
        private System.Windows.Forms.TabPage TpSoundSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.Label LblSuccessSound;
        private LaserGRBL.UserControls.GrblButton changeSucBtn;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label successSoundLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.Label label26;
        private LaserGRBL.UserControls.GrblButton changeWarBtn;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label warningSoundLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private System.Windows.Forms.Label label29;
        private LaserGRBL.UserControls.GrblButton changeFatBtn;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label fatalSoundLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private System.Windows.Forms.Label label34;
        private LaserGRBL.UserControls.GrblButton changeConBtn;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label connectSoundLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private System.Windows.Forms.Label label37;
        private LaserGRBL.UserControls.GrblButton changeDconBtn;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label disconnectSoundLabel;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.OpenFileDialog SoundBrowserDialog;
        private System.Windows.Forms.Label SuccesFullLabel;
        private System.Windows.Forms.Label DisconnectFullLabel;
        private System.Windows.Forms.Label ConnectFullLabel;
        private System.Windows.Forms.Label ErrorFullLabel;
        private System.Windows.Forms.Label WarningFullLabel;
		private System.Windows.Forms.CheckBox CbHiRes;
		private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
        private System.Windows.Forms.CheckBox CbPlaySuccess;
        private System.Windows.Forms.CheckBox CbPlayWarning;
        private System.Windows.Forms.CheckBox CbPlayFatal;
        private System.Windows.Forms.CheckBox CbPlayConnect;
        private System.Windows.Forms.CheckBox CbPlayDisconnect;
		private System.Windows.Forms.Label LblWarnOrturAC;
		private System.Windows.Forms.CheckBox CbDisableSkip;
		private System.Windows.Forms.Label label39;
		private System.Windows.Forms.CheckBox CbDisableBoundWarn;
		private System.Windows.Forms.Label label40;
		private System.Windows.Forms.CheckBox CbClickNJog;
		private System.Windows.Forms.Label label41;
		private System.Windows.Forms.CheckBox CbTelegramNotification;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel17;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.TextBox TxtNotification;
		private System.Windows.Forms.Label label42;
		private LaserGRBL.UserControls.GrblButton BtnTestNotification;
		private UserControls.ImageButton BtnTelegNoteInfo;
		private System.Windows.Forms.TabPage TpVectorImport;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel18;
		private System.Windows.Forms.Label label43;
		private System.Windows.Forms.CheckBox CbSmartBezier;
		private UserControls.ImageButton imageButton1;
		private System.Windows.Forms.Label label44;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel19;
		private System.Windows.Forms.NumericUpDown UdTelegramNotificationThreshold;
		private System.Windows.Forms.Label label45;
		private System.Windows.Forms.CheckBox CbQueryDI;
		private System.Windows.Forms.Label label46;
		private System.Windows.Forms.TabPage TpOptions;
		private System.Windows.Forms.TableLayoutPanel Tlp;
		private System.Windows.Forms.CheckBox CbDisableSafetyCD;
		private System.Windows.Forms.Label label47;
		private System.Windows.Forms.CheckBox CbQuietSafetyCB;
		private System.Windows.Forms.Label label48;
        private System.Windows.Forms.CheckBox CbLegacyIcons;
		private UserControls.FlatComboBox CBGraphicMode;
		private UserControls.ImageButton BtnRenderingMode;
		private System.Windows.Forms.Label label49;
		private System.Windows.Forms.Label label50;
	}
}