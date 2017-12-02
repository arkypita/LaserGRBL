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
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(329, 136);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.LblCurrentVersion, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.TxtHeader, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.TxtCurrentV, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.LblLatestVersion, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.TxtNewV, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.PB, 0, 3);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 4;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(323, 91);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// LblCurrentVersion
			// 
			this.LblCurrentVersion.AutoSize = true;
			this.LblCurrentVersion.Location = new System.Drawing.Point(90, 35);
			this.LblCurrentVersion.Margin = new System.Windows.Forms.Padding(3);
			this.LblCurrentVersion.Name = "LblCurrentVersion";
			this.LblCurrentVersion.Size = new System.Drawing.Size(31, 13);
			this.LblCurrentVersion.TabIndex = 2;
			this.LblCurrentVersion.Text = "0.0.0";
			// 
			// TxtHeader
			// 
			this.TxtHeader.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.TxtHeader, 2);
			this.TxtHeader.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TxtHeader.Location = new System.Drawing.Point(3, 3);
			this.TxtHeader.Margin = new System.Windows.Forms.Padding(3);
			this.TxtHeader.Name = "TxtHeader";
			this.TxtHeader.Size = new System.Drawing.Size(317, 26);
			this.TxtHeader.TabIndex = 0;
			this.TxtHeader.Text = "A new version has been found. We suggest to update LaserGRBL to take advantage of" +
    " new features and bugfix.";
			// 
			// TxtCurrentV
			// 
			this.TxtCurrentV.AutoSize = true;
			this.TxtCurrentV.Location = new System.Drawing.Point(3, 35);
			this.TxtCurrentV.Margin = new System.Windows.Forms.Padding(3);
			this.TxtCurrentV.Name = "TxtCurrentV";
			this.TxtCurrentV.Size = new System.Drawing.Size(81, 13);
			this.TxtCurrentV.TabIndex = 1;
			this.TxtCurrentV.Text = "Current version:";
			// 
			// LblLatestVersion
			// 
			this.LblLatestVersion.AutoSize = true;
			this.LblLatestVersion.Location = new System.Drawing.Point(90, 54);
			this.LblLatestVersion.Margin = new System.Windows.Forms.Padding(3);
			this.LblLatestVersion.Name = "LblLatestVersion";
			this.LblLatestVersion.Size = new System.Drawing.Size(31, 13);
			this.LblLatestVersion.TabIndex = 4;
			this.LblLatestVersion.Text = "0.0.0";
			// 
			// TxtNewV
			// 
			this.TxtNewV.AutoSize = true;
			this.TxtNewV.Location = new System.Drawing.Point(3, 54);
			this.TxtNewV.Margin = new System.Windows.Forms.Padding(3);
			this.TxtNewV.Name = "TxtNewV";
			this.TxtNewV.Size = new System.Drawing.Size(67, 13);
			this.TxtNewV.TabIndex = 3;
			this.TxtNewV.Text = "Last version:";
			// 
			// PB
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.PB, 2);
			this.PB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PB.Location = new System.Drawing.Point(3, 73);
			this.PB.Name = "PB";
			this.PB.Size = new System.Drawing.Size(317, 15);
			this.PB.TabIndex = 5;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 4;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Controls.Add(this.BtnWebsite, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.BtnCancel, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.BtnUpdate, 3, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 100);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(323, 33);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// BtnWebsite
			// 
			this.BtnWebsite.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnWebsite.Location = new System.Drawing.Point(3, 3);
			this.BtnWebsite.Name = "BtnWebsite";
			this.BtnWebsite.Size = new System.Drawing.Size(74, 27);
			this.BtnWebsite.TabIndex = 8;
			this.BtnWebsite.Text = "Website";
			this.BtnWebsite.UseVisualStyleBackColor = true;
			this.BtnWebsite.Click += new System.EventHandler(this.BtnWebsite_Click);
			// 
			// BtnCancel
			// 
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BtnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnCancel.Location = new System.Drawing.Point(166, 3);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(74, 27);
			this.BtnCancel.TabIndex = 7;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			// 
			// BtnUpdate
			// 
			this.BtnUpdate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnUpdate.Location = new System.Drawing.Point(246, 3);
			this.BtnUpdate.Name = "BtnUpdate";
			this.BtnUpdate.Size = new System.Drawing.Size(74, 27);
			this.BtnUpdate.TabIndex = 6;
			this.BtnUpdate.Text = "Download!";
			this.BtnUpdate.UseVisualStyleBackColor = true;
			this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
			// 
			// NewVersionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(329, 136);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "NewVersionForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New version available";
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