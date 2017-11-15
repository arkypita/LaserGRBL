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
			this.BtnCancel = new System.Windows.Forms.Button();
			this.BtnSave = new System.Windows.Forms.Button();
			this.GB = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.CbThreadingMode = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.CBStreamingMode = new System.Windows.Forms.ComboBox();
			this.BtnStreamingMode = new LaserGRBL.UserControls.ImageButton();
			this.CBProtocol = new System.Windows.Forms.ComboBox();
			this.CBSupportPWM = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.BtnModulationInfo = new LaserGRBL.UserControls.ImageButton();
			this.label3 = new System.Windows.Forms.Label();
			this.BtnProtocol = new LaserGRBL.UserControls.ImageButton();
			this.CbUnidirectional = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.BtnThreadingModel = new LaserGRBL.UserControls.ImageButton();
			this.CbIssueDetector = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.GB.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.GB, 0, 0);
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
			// GB
			// 
			this.GB.Controls.Add(this.tableLayoutPanel3);
			resources.ApplyResources(this.GB, "GB");
			this.GB.Name = "GB";
			this.GB.TabStop = false;
			// 
			// tableLayoutPanel3
			// 
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
			this.tableLayoutPanel3.Controls.Add(this.CbThreadingMode, 1, 3);
			this.tableLayoutPanel3.Controls.Add(this.label4, 2, 2);
			this.tableLayoutPanel3.Controls.Add(this.CBStreamingMode, 1, 2);
			this.tableLayoutPanel3.Controls.Add(this.BtnStreamingMode, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.CBProtocol, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.CBSupportPWM, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.label1, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.BtnModulationInfo, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.label3, 2, 1);
			this.tableLayoutPanel3.Controls.Add(this.BtnProtocol, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.CbUnidirectional, 1, 4);
			this.tableLayoutPanel3.Controls.Add(this.label5, 2, 4);
			this.tableLayoutPanel3.Controls.Add(this.label6, 2, 3);
			this.tableLayoutPanel3.Controls.Add(this.BtnThreadingModel, 0, 3);
			this.tableLayoutPanel3.Controls.Add(this.CbIssueDetector, 1, 5);
			this.tableLayoutPanel3.Controls.Add(this.label7, 2, 5);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
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
			this.BtnStreamingMode.AltImage = null;
			this.BtnStreamingMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnStreamingMode.Coloration = System.Drawing.Color.Empty;
			this.BtnStreamingMode.Image = ((System.Drawing.Image)(resources.GetObject("BtnStreamingMode.Image")));
			resources.ApplyResources(this.BtnStreamingMode, "BtnStreamingMode");
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
			// CBSupportPWM
			// 
			resources.ApplyResources(this.CBSupportPWM, "CBSupportPWM");
			this.CBSupportPWM.Name = "CBSupportPWM";
			this.CBSupportPWM.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// BtnModulationInfo
			// 
			this.BtnModulationInfo.AltImage = null;
			this.BtnModulationInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnModulationInfo.Coloration = System.Drawing.Color.Empty;
			this.BtnModulationInfo.Image = ((System.Drawing.Image)(resources.GetObject("BtnModulationInfo.Image")));
			resources.ApplyResources(this.BtnModulationInfo, "BtnModulationInfo");
			this.BtnModulationInfo.Name = "BtnModulationInfo";
			this.BtnModulationInfo.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnModulationInfo.UseAltImage = false;
			this.BtnModulationInfo.Click += new System.EventHandler(this.BtnModulationInfo_Click);
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
			this.BtnProtocol.Coloration = System.Drawing.Color.Empty;
			this.BtnProtocol.Image = ((System.Drawing.Image)(resources.GetObject("BtnProtocol.Image")));
			resources.ApplyResources(this.BtnProtocol, "BtnProtocol");
			this.BtnProtocol.Name = "BtnProtocol";
			this.BtnProtocol.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnProtocol.UseAltImage = false;
			this.BtnProtocol.Click += new System.EventHandler(this.BtnProtocol_Click);
			// 
			// CbUnidirectional
			// 
			resources.ApplyResources(this.CbUnidirectional, "CbUnidirectional");
			this.CbUnidirectional.Name = "CbUnidirectional";
			this.CbUnidirectional.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
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
			this.BtnThreadingModel.Coloration = System.Drawing.Color.Empty;
			this.BtnThreadingModel.Image = ((System.Drawing.Image)(resources.GetObject("BtnThreadingModel.Image")));
			resources.ApplyResources(this.BtnThreadingModel, "BtnThreadingModel");
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
			// SettingsForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.BtnCancel;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "SettingsForm";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.GB.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.Button BtnSave;
		private System.Windows.Forms.GroupBox GB;
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
    }
}