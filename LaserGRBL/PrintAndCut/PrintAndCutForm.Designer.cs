namespace LaserGRBL.PrintAndCut
{
	partial class PrintAndCutForm
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
			this.TlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.LblInfo = new System.Windows.Forms.Label();
			this.GbFirst = new System.Windows.Forms.GroupBox();
			this.TlpFirst = new System.Windows.Forms.TableLayoutPanel();
			this.CbFirst = new System.Windows.Forms.ComboBox();
			this.BtnJogFirst = new System.Windows.Forms.Button();
			this.BtnSetFirst = new System.Windows.Forms.Button();
			this.LblFirst = new System.Windows.Forms.Label();
			this.GbSecond = new System.Windows.Forms.GroupBox();
			this.TlpSecond = new System.Windows.Forms.TableLayoutPanel();
			this.CbSecond = new System.Windows.Forms.ComboBox();
			this.BtnJogSecond = new System.Windows.Forms.Button();
			this.BtnSetSecond = new System.Windows.Forms.Button();
			this.LblSecond = new System.Windows.Forms.Label();
			this.BtnAim = new System.Windows.Forms.Button();
			this.FlpAlign = new System.Windows.Forms.FlowLayoutPanel();
			this.BtnScaled = new System.Windows.Forms.Button();
			this.BtnNoScaling = new System.Windows.Forms.Button();
			this.BtnReset = new System.Windows.Forms.Button();
			this.LblStatus = new System.Windows.Forms.Label();
			this.TT = new System.Windows.Forms.ToolTip(this.components);
			this.RefreshTimer = new System.Windows.Forms.Timer(this.components);
			this.AimTimeout = new System.Windows.Forms.Timer(this.components);
			this.TlpMain.SuspendLayout();
			this.GbFirst.SuspendLayout();
			this.TlpFirst.SuspendLayout();
			this.GbSecond.SuspendLayout();
			this.TlpSecond.SuspendLayout();
			this.FlpAlign.SuspendLayout();
			this.SuspendLayout();
			//
			// TlpMain
			//
			this.TlpMain.AutoSize = true;
			this.TlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TlpMain.ColumnCount = 1;
			this.TlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TlpMain.Controls.Add(this.LblInfo, 0, 0);
			this.TlpMain.Controls.Add(this.GbFirst, 0, 1);
			this.TlpMain.Controls.Add(this.GbSecond, 0, 2);
			this.TlpMain.Controls.Add(this.BtnAim, 0, 3);
			this.TlpMain.Controls.Add(this.FlpAlign, 0, 4);
			this.TlpMain.Controls.Add(this.LblStatus, 0, 5);
			this.TlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TlpMain.Location = new System.Drawing.Point(0, 0);
			this.TlpMain.Name = "TlpMain";
			this.TlpMain.Padding = new System.Windows.Forms.Padding(8);
			this.TlpMain.RowCount = 6;
			this.TlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TlpMain.Size = new System.Drawing.Size(440, 330);
			this.TlpMain.TabIndex = 0;
			//
			// LblInfo
			//
			this.LblInfo.AutoSize = true;
			this.LblInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LblInfo.Location = new System.Drawing.Point(11, 8);
			this.LblInfo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 6);
			this.LblInfo.MaximumSize = new System.Drawing.Size(420, 0);
			this.LblInfo.Name = "LblInfo";
			this.LblInfo.Size = new System.Drawing.Size(418, 13);
			this.LblInfo.TabIndex = 0;
			//
			// GbFirst
			//
			this.GbFirst.AutoSize = true;
			this.GbFirst.Controls.Add(this.TlpFirst);
			this.GbFirst.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GbFirst.Name = "GbFirst";
			this.GbFirst.TabIndex = 1;
			this.GbFirst.TabStop = false;
			//
			// TlpFirst
			//
			this.TlpFirst.AutoSize = true;
			this.TlpFirst.ColumnCount = 3;
			this.TlpFirst.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TlpFirst.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TlpFirst.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TlpFirst.Controls.Add(this.CbFirst, 0, 0);
			this.TlpFirst.Controls.Add(this.BtnJogFirst, 1, 0);
			this.TlpFirst.Controls.Add(this.BtnSetFirst, 2, 0);
			this.TlpFirst.Controls.Add(this.LblFirst, 0, 1);
			this.TlpFirst.SetColumnSpan(this.LblFirst, 3);
			this.TlpFirst.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TlpFirst.Name = "TlpFirst";
			this.TlpFirst.RowCount = 2;
			this.TlpFirst.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TlpFirst.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TlpFirst.TabIndex = 0;
			//
			// CbFirst
			//
			this.CbFirst.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CbFirst.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbFirst.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.CbFirst.MinimumSize = new System.Drawing.Size(220, 0);
			this.CbFirst.Name = "CbFirst";
			this.CbFirst.TabIndex = 0;
			this.CbFirst.SelectedIndexChanged += new System.EventHandler(this.CbFirst_SelectedIndexChanged);
			//
			// BtnJogFirst
			//
			this.BtnJogFirst.AutoSize = true;
			this.BtnJogFirst.MinimumSize = new System.Drawing.Size(75, 27);
			this.BtnJogFirst.Name = "BtnJogFirst";
			this.BtnJogFirst.TabIndex = 1;
			this.BtnJogFirst.UseVisualStyleBackColor = true;
			this.BtnJogFirst.Click += new System.EventHandler(this.BtnJogFirst_Click);
			//
			// BtnSetFirst
			//
			this.BtnSetFirst.AutoSize = true;
			this.BtnSetFirst.MinimumSize = new System.Drawing.Size(75, 27);
			this.BtnSetFirst.Name = "BtnSetFirst";
			this.BtnSetFirst.TabIndex = 2;
			this.BtnSetFirst.UseVisualStyleBackColor = true;
			this.BtnSetFirst.Click += new System.EventHandler(this.BtnSetFirst_Click);
			//
			// LblFirst
			//
			this.LblFirst.AutoSize = true;
			this.LblFirst.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
			this.LblFirst.Name = "LblFirst";
			this.LblFirst.TabIndex = 3;
			//
			// GbSecond
			//
			this.GbSecond.AutoSize = true;
			this.GbSecond.Controls.Add(this.TlpSecond);
			this.GbSecond.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GbSecond.Name = "GbSecond";
			this.GbSecond.TabIndex = 2;
			this.GbSecond.TabStop = false;
			//
			// TlpSecond
			//
			this.TlpSecond.AutoSize = true;
			this.TlpSecond.ColumnCount = 3;
			this.TlpSecond.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TlpSecond.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TlpSecond.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TlpSecond.Controls.Add(this.CbSecond, 0, 0);
			this.TlpSecond.Controls.Add(this.BtnJogSecond, 1, 0);
			this.TlpSecond.Controls.Add(this.BtnSetSecond, 2, 0);
			this.TlpSecond.Controls.Add(this.LblSecond, 0, 1);
			this.TlpSecond.SetColumnSpan(this.LblSecond, 3);
			this.TlpSecond.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TlpSecond.Name = "TlpSecond";
			this.TlpSecond.RowCount = 2;
			this.TlpSecond.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TlpSecond.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TlpSecond.TabIndex = 0;
			//
			// CbSecond
			//
			this.CbSecond.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CbSecond.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbSecond.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.CbSecond.MinimumSize = new System.Drawing.Size(220, 0);
			this.CbSecond.Name = "CbSecond";
			this.CbSecond.TabIndex = 0;
			this.CbSecond.SelectedIndexChanged += new System.EventHandler(this.CbSecond_SelectedIndexChanged);
			//
			// BtnJogSecond
			//
			this.BtnJogSecond.AutoSize = true;
			this.BtnJogSecond.MinimumSize = new System.Drawing.Size(75, 27);
			this.BtnJogSecond.Name = "BtnJogSecond";
			this.BtnJogSecond.TabIndex = 1;
			this.BtnJogSecond.UseVisualStyleBackColor = true;
			this.BtnJogSecond.Click += new System.EventHandler(this.BtnJogSecond_Click);
			//
			// BtnSetSecond
			//
			this.BtnSetSecond.AutoSize = true;
			this.BtnSetSecond.MinimumSize = new System.Drawing.Size(75, 27);
			this.BtnSetSecond.Name = "BtnSetSecond";
			this.BtnSetSecond.TabIndex = 2;
			this.BtnSetSecond.UseVisualStyleBackColor = true;
			this.BtnSetSecond.Click += new System.EventHandler(this.BtnSetSecond_Click);
			//
			// LblSecond
			//
			this.LblSecond.AutoSize = true;
			this.LblSecond.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
			this.LblSecond.Name = "LblSecond";
			this.LblSecond.TabIndex = 3;
			//
			// BtnAim
			//
			this.BtnAim.AutoSize = true;
			this.BtnAim.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BtnAim.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.BtnAim.MinimumSize = new System.Drawing.Size(0, 32);
			this.BtnAim.Name = "BtnAim";
			this.BtnAim.TabIndex = 3;
			this.BtnAim.UseVisualStyleBackColor = true;
			this.BtnAim.MouseCaptureChanged += new System.EventHandler(this.BtnAim_MouseCaptureChanged);
			this.BtnAim.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnAim_MouseDown);
			this.BtnAim.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnAim_MouseUp);
			this.BtnAim.Leave += new System.EventHandler(this.BtnAim_Leave);
			//
			// FlpAlign
			//
			this.FlpAlign.AutoSize = true;
			this.FlpAlign.Controls.Add(this.BtnScaled);
			this.FlpAlign.Controls.Add(this.BtnNoScaling);
			this.FlpAlign.Controls.Add(this.BtnReset);
			this.FlpAlign.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FlpAlign.Name = "FlpAlign";
			this.FlpAlign.TabIndex = 4;
			this.FlpAlign.WrapContents = false;
			//
			// BtnScaled
			//
			this.BtnScaled.AutoSize = true;
			this.BtnScaled.MinimumSize = new System.Drawing.Size(100, 27);
			this.BtnScaled.Name = "BtnScaled";
			this.BtnScaled.TabIndex = 0;
			this.BtnScaled.UseVisualStyleBackColor = true;
			this.BtnScaled.Click += new System.EventHandler(this.BtnScaled_Click);
			//
			// BtnNoScaling
			//
			this.BtnNoScaling.AutoSize = true;
			this.BtnNoScaling.MinimumSize = new System.Drawing.Size(100, 27);
			this.BtnNoScaling.Name = "BtnNoScaling";
			this.BtnNoScaling.TabIndex = 1;
			this.BtnNoScaling.UseVisualStyleBackColor = true;
			this.BtnNoScaling.Click += new System.EventHandler(this.BtnNoScaling_Click);
			//
			// BtnReset
			//
			this.BtnReset.AutoSize = true;
			this.BtnReset.MinimumSize = new System.Drawing.Size(100, 27);
			this.BtnReset.Name = "BtnReset";
			this.BtnReset.TabIndex = 2;
			this.BtnReset.UseVisualStyleBackColor = true;
			this.BtnReset.Click += new System.EventHandler(this.BtnReset_Click);
			//
			// LblStatus
			//
			this.LblStatus.AutoSize = true;
			this.LblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LblStatus.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
			this.LblStatus.MaximumSize = new System.Drawing.Size(420, 0);
			this.LblStatus.Name = "LblStatus";
			this.LblStatus.TabIndex = 5;
			//
			// TT
			//
			this.TT.AutoPopDelay = 15000;
			this.TT.InitialDelay = 400;
			this.TT.ReshowDelay = 100;
			//
			// RefreshTimer
			//
			this.RefreshTimer.Enabled = true;
			this.RefreshTimer.Interval = 250;
			this.RefreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
			//
			// AimTimeout
			//
			this.AimTimeout.Interval = 10000;
			this.AimTimeout.Tick += new System.EventHandler(this.AimTimeout_Tick);
			//
			// PrintAndCutForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(440, 330);
			this.Controls.Add(this.TlpMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PrintAndCutForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Deactivate += new System.EventHandler(this.PrintAndCutForm_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PrintAndCutForm_FormClosing);
			this.Load += new System.EventHandler(this.PrintAndCutForm_Load);
			this.TlpMain.ResumeLayout(false);
			this.TlpMain.PerformLayout();
			this.GbFirst.ResumeLayout(false);
			this.GbFirst.PerformLayout();
			this.TlpFirst.ResumeLayout(false);
			this.TlpFirst.PerformLayout();
			this.GbSecond.ResumeLayout(false);
			this.GbSecond.PerformLayout();
			this.TlpSecond.ResumeLayout(false);
			this.TlpSecond.PerformLayout();
			this.FlpAlign.ResumeLayout(false);
			this.FlpAlign.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel TlpMain;
		private System.Windows.Forms.Label LblInfo;
		private System.Windows.Forms.GroupBox GbFirst;
		private System.Windows.Forms.TableLayoutPanel TlpFirst;
		private System.Windows.Forms.ComboBox CbFirst;
		private System.Windows.Forms.Button BtnJogFirst;
		private System.Windows.Forms.Button BtnSetFirst;
		private System.Windows.Forms.Label LblFirst;
		private System.Windows.Forms.GroupBox GbSecond;
		private System.Windows.Forms.TableLayoutPanel TlpSecond;
		private System.Windows.Forms.ComboBox CbSecond;
		private System.Windows.Forms.Button BtnJogSecond;
		private System.Windows.Forms.Button BtnSetSecond;
		private System.Windows.Forms.Label LblSecond;
		private System.Windows.Forms.Button BtnAim;
		private System.Windows.Forms.FlowLayoutPanel FlpAlign;
		private System.Windows.Forms.Button BtnScaled;
		private System.Windows.Forms.Button BtnNoScaling;
		private System.Windows.Forms.Button BtnReset;
		private System.Windows.Forms.Label LblStatus;
		private System.Windows.Forms.ToolTip TT;
		private System.Windows.Forms.Timer RefreshTimer;
		private System.Windows.Forms.Timer AimTimeout;
	}
}
