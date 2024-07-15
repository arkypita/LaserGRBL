namespace LaserGRBL
{
	partial class FlashGrbl
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlashGrbl));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.BtnOK = new System.Windows.Forms.Button();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.LblNotForOrtur = new System.Windows.Forms.Label();
			this.LblWarning = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.CbTarget = new System.Windows.Forms.ComboBox();
			this.LblTarget = new System.Windows.Forms.Label();
			this.CbFirmware = new System.Windows.Forms.ComboBox();
			this.LblFirmware = new System.Windows.Forms.Label();
			this.LblPort = new System.Windows.Forms.Label();
			this.CbPort = new System.Windows.Forms.ComboBox();
			this.LblSpeed = new System.Windows.Forms.Label();
			this.CbBaudRate = new System.Windows.Forms.ComboBox();
			this.BtnTarget = new LaserGRBL.UserControls.ImageButton();
			this.BtnFirmware = new LaserGRBL.UserControls.ImageButton();
			this.TT = new System.Windows.Forms.ToolTip(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.tableLayoutPanel5.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// tableLayoutPanel2
			// 
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.BtnCancel, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnOK, 2, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			// 
			// BtnCancel
			// 
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.BtnCancel, "BtnCancel");
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			// 
			// BtnOK
			// 
			this.BtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.BtnOK, "BtnOK");
			this.BtnOK.Name = "BtnOK";
			this.BtnOK.UseVisualStyleBackColor = true;
			this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
			// 
			// tableLayoutPanel3
			// 
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 0, 2);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			// 
			// tableLayoutPanel4
			// 
			resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
			this.tableLayoutPanel4.Controls.Add(this.LblNotForOrtur, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.LblWarning, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			// 
			// LblNotForOrtur
			// 
			resources.ApplyResources(this.LblNotForOrtur, "LblNotForOrtur");
			this.LblNotForOrtur.ForeColor = System.Drawing.Color.Red;
			this.LblNotForOrtur.Name = "LblNotForOrtur";
			// 
			// LblWarning
			// 
			resources.ApplyResources(this.LblWarning, "LblWarning");
			this.LblWarning.ForeColor = System.Drawing.Color.Red;
			this.LblWarning.Name = "LblWarning";
			// 
			// pictureBox1
			// 
			resources.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			// 
			// tableLayoutPanel5
			// 
			resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
			this.tableLayoutPanel5.Controls.Add(this.CbTarget, 1, 0);
			this.tableLayoutPanel5.Controls.Add(this.LblTarget, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.CbFirmware, 1, 1);
			this.tableLayoutPanel5.Controls.Add(this.LblFirmware, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.LblPort, 0, 2);
			this.tableLayoutPanel5.Controls.Add(this.CbPort, 1, 2);
			this.tableLayoutPanel5.Controls.Add(this.LblSpeed, 0, 3);
			this.tableLayoutPanel5.Controls.Add(this.CbBaudRate, 1, 3);
			this.tableLayoutPanel5.Controls.Add(this.BtnTarget, 2, 0);
			this.tableLayoutPanel5.Controls.Add(this.BtnFirmware, 2, 1);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			// 
			// CbTarget
			// 
			resources.ApplyResources(this.CbTarget, "CbTarget");
			this.CbTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbTarget.FormattingEnabled = true;
			this.CbTarget.Items.AddRange(new object[] {
            resources.GetString("CbTarget.Items"),
            resources.GetString("CbTarget.Items1")});
			this.CbTarget.Name = "CbTarget";
			this.CbTarget.SelectedIndexChanged += new System.EventHandler(this.CbTarget_SelectedIndexChanged);
			// 
			// LblTarget
			// 
			resources.ApplyResources(this.LblTarget, "LblTarget");
			this.LblTarget.Name = "LblTarget";
			// 
			// CbFirmware
			// 
			resources.ApplyResources(this.CbFirmware, "CbFirmware");
			this.CbFirmware.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbFirmware.FormattingEnabled = true;
			this.CbFirmware.Name = "CbFirmware";
			// 
			// LblFirmware
			// 
			resources.ApplyResources(this.LblFirmware, "LblFirmware");
			this.LblFirmware.Name = "LblFirmware";
			// 
			// LblPort
			// 
			resources.ApplyResources(this.LblPort, "LblPort");
			this.LblPort.Name = "LblPort";
			// 
			// CbPort
			// 
			this.CbPort.FormattingEnabled = true;
			resources.ApplyResources(this.CbPort, "CbPort");
			this.CbPort.Name = "CbPort";
			this.CbPort.TextChanged += new System.EventHandler(this.CbPort_TextChanged);
			// 
			// LblSpeed
			// 
			resources.ApplyResources(this.LblSpeed, "LblSpeed");
			this.LblSpeed.Name = "LblSpeed";
			// 
			// CbBaudRate
			// 
			this.CbBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbBaudRate.FormattingEnabled = true;
			resources.ApplyResources(this.CbBaudRate, "CbBaudRate");
			this.CbBaudRate.Name = "CbBaudRate";
			// 
			// BtnTarget
			// 
			this.BtnTarget.AltImage = null;
			resources.ApplyResources(this.BtnTarget, "BtnTarget");
			this.BtnTarget.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnTarget.Caption = null;
			this.BtnTarget.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnTarget.Image = ((System.Drawing.Image)(resources.GetObject("BtnTarget.Image")));
			this.BtnTarget.Name = "BtnTarget";
			this.BtnTarget.RoundedBorders = false;
			this.BtnTarget.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.TT.SetToolTip(this.BtnTarget, resources.GetString("BtnTarget.ToolTip"));
			this.BtnTarget.UseAltImage = false;
			this.BtnTarget.Click += new System.EventHandler(this.BtnTarget_Click);
			// 
			// BtnFirmware
			// 
			this.BtnFirmware.AltImage = null;
			resources.ApplyResources(this.BtnFirmware, "BtnFirmware");
			this.BtnFirmware.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnFirmware.Caption = null;
			this.BtnFirmware.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnFirmware.Image = ((System.Drawing.Image)(resources.GetObject("BtnFirmware.Image")));
			this.BtnFirmware.Name = "BtnFirmware";
			this.BtnFirmware.RoundedBorders = false;
			this.BtnFirmware.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.TT.SetToolTip(this.BtnFirmware, resources.GetString("BtnFirmware.ToolTip"));
			this.BtnFirmware.UseAltImage = false;
			this.BtnFirmware.Click += new System.EventHandler(this.BtnFirmware_Click);
			// 
			// TT
			// 
			this.TT.AutoPopDelay = 10000;
			this.TT.InitialDelay = 500;
			this.TT.ReshowDelay = 100;
			// 
			// FlashGrbl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.BtnCancel;
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FlashGrbl";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button BtnOK;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label LblWarning;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Label LblFirmware;
		private System.Windows.Forms.Label LblPort;
		private System.Windows.Forms.Label LblSpeed;
		private System.Windows.Forms.ComboBox CbFirmware;
		private System.Windows.Forms.ComboBox CbBaudRate;
		private System.Windows.Forms.ComboBox CbPort;
		private System.Windows.Forms.Label LblTarget;
		private System.Windows.Forms.ComboBox CbTarget;
		private System.Windows.Forms.Button BtnCancel;
		private UserControls.ImageButton BtnTarget;
		private UserControls.ImageButton BtnFirmware;
		private System.Windows.Forms.ToolTip TT;
		private System.Windows.Forms.Label LblNotForOrtur;
	}
}