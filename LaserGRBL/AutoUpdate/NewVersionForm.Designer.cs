namespace LaserGRBL
{
	partial class NewVersionForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewVersionForm));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.LblCurrentVersion = new System.Windows.Forms.Label();
			this.TxtHeader = new System.Windows.Forms.Label();
			this.TxtCurrentV = new System.Windows.Forms.Label();
			this.LblLatestVersion = new System.Windows.Forms.Label();
			this.TxtNewV = new System.Windows.Forms.Label();
			this.PB = new System.Windows.Forms.ProgressBar();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnWebsite = new System.Windows.Forms.Button();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.BtnUpdate = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// tableLayoutPanel2
			// 
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.LblCurrentVersion, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.TxtHeader, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.TxtCurrentV, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.LblLatestVersion, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.TxtNewV, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.PB, 0, 3);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			// 
			// LblCurrentVersion
			// 
			resources.ApplyResources(this.LblCurrentVersion, "LblCurrentVersion");
			this.LblCurrentVersion.Name = "LblCurrentVersion";
			// 
			// TxtHeader
			// 
			resources.ApplyResources(this.TxtHeader, "TxtHeader");
			this.tableLayoutPanel2.SetColumnSpan(this.TxtHeader, 2);
			this.TxtHeader.Name = "TxtHeader";
			// 
			// TxtCurrentV
			// 
			resources.ApplyResources(this.TxtCurrentV, "TxtCurrentV");
			this.TxtCurrentV.Name = "TxtCurrentV";
			// 
			// LblLatestVersion
			// 
			resources.ApplyResources(this.LblLatestVersion, "LblLatestVersion");
			this.LblLatestVersion.Name = "LblLatestVersion";
			// 
			// TxtNewV
			// 
			resources.ApplyResources(this.TxtNewV, "TxtNewV");
			this.TxtNewV.Name = "TxtNewV";
			// 
			// PB
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.PB, 2);
			resources.ApplyResources(this.PB, "PB");
			this.PB.Name = "PB";
			// 
			// tableLayoutPanel3
			// 
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
			this.tableLayoutPanel3.Controls.Add(this.BtnWebsite, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.BtnCancel, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.BtnUpdate, 3, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			// 
			// BtnWebsite
			// 
			resources.ApplyResources(this.BtnWebsite, "BtnWebsite");
			this.BtnWebsite.Name = "BtnWebsite";
			this.BtnWebsite.UseVisualStyleBackColor = true;
			this.BtnWebsite.Click += new System.EventHandler(this.BtnWebsite_Click);
			// 
			// BtnCancel
			// 
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.BtnCancel, "BtnCancel");
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			// 
			// BtnUpdate
			// 
			resources.ApplyResources(this.BtnUpdate, "BtnUpdate");
			this.BtnUpdate.Name = "BtnUpdate";
			this.BtnUpdate.UseVisualStyleBackColor = true;
			this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
			// 
			// NewVersionForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "NewVersionForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewVersionForm_FormClosing);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label TxtHeader;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label LblCurrentVersion;
		private System.Windows.Forms.Label TxtCurrentV;
		private System.Windows.Forms.Label TxtNewV;
		private System.Windows.Forms.Label LblLatestVersion;
		private System.Windows.Forms.Button BtnUpdate;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.ProgressBar PB;
		private System.Windows.Forms.Button BtnWebsite;
	}
}