namespace LaserGRBL
{
	partial class IssueDetectorForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IssueDetectorForm));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnOK = new System.Windows.Forms.Button();
			this.CbDoNotShow = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.LblCause = new System.Windows.Forms.Label();
			this.TxtCause = new System.Windows.Forms.Label();
			this.LblDetected = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.LblBrief = new System.Windows.Forms.Label();
			this.LL = new System.Windows.Forms.LinkLabel();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
			this.tableLayoutPanel2.Controls.Add(this.BtnOK, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.CbDoNotShow, 0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			// 
			// BtnOK
			// 
			this.BtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.BtnOK, "BtnOK");
			this.BtnOK.Name = "BtnOK";
			this.BtnOK.UseVisualStyleBackColor = true;
			this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
			// 
			// CbDoNotShow
			// 
			resources.ApplyResources(this.CbDoNotShow, "CbDoNotShow");
			this.CbDoNotShow.Name = "CbDoNotShow";
			this.CbDoNotShow.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.LblBrief, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.LL, 0, 2);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
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
			// pictureBox1
			// 
			resources.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			// 
			// LblBrief
			// 
			resources.ApplyResources(this.LblBrief, "LblBrief");
			this.tableLayoutPanel3.SetColumnSpan(this.LblBrief, 2);
			this.LblBrief.Name = "LblBrief";
			// 
			// LL
			// 
			resources.ApplyResources(this.LL, "LL");
			this.tableLayoutPanel3.SetColumnSpan(this.LL, 2);
			this.LL.Name = "LL";
			this.LL.TabStop = true;
			this.LL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LL_LinkClicked);
			// 
			// IssueDetectorForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.BtnOK;
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "IssueDetectorForm";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button BtnOK;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label LblBrief;
		private System.Windows.Forms.LinkLabel LL;
		private System.Windows.Forms.CheckBox CbDoNotShow;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Label LblCause;
		private System.Windows.Forms.Label TxtCause;
		private System.Windows.Forms.Label LblDetected;
	}
}