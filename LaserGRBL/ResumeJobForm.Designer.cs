namespace LaserGRBL
{
	partial class ResumeJobForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResumeJobForm));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.CbRestoreWCO = new System.Windows.Forms.CheckBox();
			this.BtnOK = new System.Windows.Forms.Button();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.CbRedoHoming = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.LblSomeLines = new System.Windows.Forms.Label();
			this.RbSomeLines = new System.Windows.Forms.RadioButton();
			this.LblLineNumber = new System.Windows.Forms.Label();
			this.RbFromSpecific = new System.Windows.Forms.RadioButton();
			this.RbFromBeginning = new System.Windows.Forms.RadioButton();
			this.LblOptions = new System.Windows.Forms.Label();
			this.LblBegin = new System.Windows.Forms.Label();
			this.UdSpecific = new System.Windows.Forms.NumericUpDown();
			this.RbFromSent = new System.Windows.Forms.RadioButton();
			this.LblSent = new System.Windows.Forms.Label();
			this.LblBrief = new System.Windows.Forms.Label();
			this.PBWarn = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.LblCause = new System.Windows.Forms.Label();
			this.TxtCause = new System.Windows.Forms.Label();
			this.LblDetected = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.UdSpecific)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PBWarn)).BeginInit();
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
			this.tableLayoutPanel2.Controls.Add(this.CbRestoreWCO, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnOK, 3, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnCancel, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.CbRedoHoming, 0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			// 
			// CbRestoreWCO
			// 
			resources.ApplyResources(this.CbRestoreWCO, "CbRestoreWCO");
			this.CbRestoreWCO.Name = "CbRestoreWCO";
			this.CbRestoreWCO.UseVisualStyleBackColor = true;
			// 
			// BtnOK
			// 
			resources.ApplyResources(this.BtnOK, "BtnOK");
			this.BtnOK.Name = "BtnOK";
			this.BtnOK.UseVisualStyleBackColor = true;
			this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
			// 
			// BtnCancel
			// 
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.BtnCancel, "BtnCancel");
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// CbRedoHoming
			// 
			resources.ApplyResources(this.CbRedoHoming, "CbRedoHoming");
			this.CbRedoHoming.Name = "CbRedoHoming";
			this.CbRedoHoming.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.LblBrief, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.PBWarn, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 1, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			// 
			// tableLayoutPanel4
			// 
			resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
			this.tableLayoutPanel3.SetColumnSpan(this.tableLayoutPanel4, 2);
			this.tableLayoutPanel4.Controls.Add(this.LblSomeLines, 1, 2);
			this.tableLayoutPanel4.Controls.Add(this.RbSomeLines, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.LblLineNumber, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.RbFromSpecific, 0, 4);
			this.tableLayoutPanel4.Controls.Add(this.RbFromBeginning, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.LblOptions, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.LblBegin, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.UdSpecific, 1, 4);
			this.tableLayoutPanel4.Controls.Add(this.RbFromSent, 0, 3);
			this.tableLayoutPanel4.Controls.Add(this.LblSent, 1, 3);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			// 
			// LblSomeLines
			// 
			resources.ApplyResources(this.LblSomeLines, "LblSomeLines");
			this.LblSomeLines.Name = "LblSomeLines";
			// 
			// RbSomeLines
			// 
			resources.ApplyResources(this.RbSomeLines, "RbSomeLines");
			this.RbSomeLines.Name = "RbSomeLines";
			this.RbSomeLines.UseVisualStyleBackColor = true;
			this.RbSomeLines.CheckedChanged += new System.EventHandler(this.RbCheckedChanged);
			// 
			// LblLineNumber
			// 
			resources.ApplyResources(this.LblLineNumber, "LblLineNumber");
			this.LblLineNumber.BackColor = System.Drawing.Color.SteelBlue;
			this.LblLineNumber.ForeColor = System.Drawing.Color.White;
			this.LblLineNumber.Name = "LblLineNumber";
			// 
			// RbFromSpecific
			// 
			resources.ApplyResources(this.RbFromSpecific, "RbFromSpecific");
			this.RbFromSpecific.Name = "RbFromSpecific";
			this.RbFromSpecific.UseVisualStyleBackColor = true;
			this.RbFromSpecific.CheckedChanged += new System.EventHandler(this.RbCheckedChanged);
			// 
			// RbFromBeginning
			// 
			resources.ApplyResources(this.RbFromBeginning, "RbFromBeginning");
			this.RbFromBeginning.Name = "RbFromBeginning";
			this.RbFromBeginning.UseVisualStyleBackColor = true;
			this.RbFromBeginning.CheckedChanged += new System.EventHandler(this.RbCheckedChanged);
			// 
			// LblOptions
			// 
			resources.ApplyResources(this.LblOptions, "LblOptions");
			this.LblOptions.BackColor = System.Drawing.Color.SteelBlue;
			this.LblOptions.ForeColor = System.Drawing.Color.White;
			this.LblOptions.Name = "LblOptions";
			// 
			// LblBegin
			// 
			resources.ApplyResources(this.LblBegin, "LblBegin");
			this.LblBegin.Name = "LblBegin";
			// 
			// UdSpecific
			// 
			resources.ApplyResources(this.UdSpecific, "UdSpecific");
			this.UdSpecific.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.UdSpecific.Name = "UdSpecific";
			this.UdSpecific.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// RbFromSent
			// 
			resources.ApplyResources(this.RbFromSent, "RbFromSent");
			this.RbFromSent.Name = "RbFromSent";
			this.RbFromSent.UseVisualStyleBackColor = true;
			this.RbFromSent.CheckedChanged += new System.EventHandler(this.RbCheckedChanged);
			// 
			// LblSent
			// 
			resources.ApplyResources(this.LblSent, "LblSent");
			this.LblSent.Name = "LblSent";
			// 
			// LblBrief
			// 
			resources.ApplyResources(this.LblBrief, "LblBrief");
			this.tableLayoutPanel3.SetColumnSpan(this.LblBrief, 2);
			this.LblBrief.Name = "LblBrief";
			// 
			// PBWarn
			// 
			resources.ApplyResources(this.PBWarn, "PBWarn");
			this.PBWarn.Name = "PBWarn";
			this.PBWarn.TabStop = false;
			// 
			// tableLayoutPanel5
			// 
			resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
			this.tableLayoutPanel5.Controls.Add(this.LblCause, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.TxtCause, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.LblDetected, 0, 0);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			// 
			// LblCause
			// 
			resources.ApplyResources(this.LblCause, "LblCause");
			this.LblCause.Name = "LblCause";
			// 
			// TxtCause
			// 
			resources.ApplyResources(this.TxtCause, "TxtCause");
			this.TxtCause.ForeColor = System.Drawing.Color.Red;
			this.TxtCause.Name = "TxtCause";
			// 
			// LblDetected
			// 
			resources.ApplyResources(this.LblDetected, "LblDetected");
			this.tableLayoutPanel5.SetColumnSpan(this.LblDetected, 2);
			this.LblDetected.Name = "LblDetected";
			// 
			// ResumeJobForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.BtnCancel;
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ResumeJobForm";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.UdSpecific)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PBWarn)).EndInit();
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
		private System.Windows.Forms.PictureBox PBWarn;
		private System.Windows.Forms.Label LblBrief;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.RadioButton RbFromBeginning;
		private System.Windows.Forms.RadioButton RbFromSent;
		private System.Windows.Forms.RadioButton RbFromSpecific;
		private System.Windows.Forms.Label LblLineNumber;
		private System.Windows.Forms.Label LblOptions;
		private System.Windows.Forms.Label LblSent;
		private System.Windows.Forms.Label LblBegin;
		private System.Windows.Forms.NumericUpDown UdSpecific;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.RadioButton RbSomeLines;
		private System.Windows.Forms.Label LblSomeLines;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Label LblDetected;
		private System.Windows.Forms.Label TxtCause;
		private System.Windows.Forms.Label LblCause;
		private System.Windows.Forms.CheckBox CbRedoHoming;
		private System.Windows.Forms.CheckBox CbRestoreWCO;
	}
}