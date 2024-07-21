namespace LaserGRBL
{
	partial class LaserUsage
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LaserUsage));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnUnmark = new System.Windows.Forms.Button();
			this.BtnMark = new System.Windows.Forms.Button();
			this.BtnEdit = new System.Windows.Forms.Button();
			this.BtnRemove = new System.Windows.Forms.Button();
			this.BtnAddNew = new System.Windows.Forms.Button();
			this.Preview = new LaserGRBL.UserControls.UsageClassPreview();
			this.LVLasers = new System.Windows.Forms.ListView();
			this.ChName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ChBrand = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ChModel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ChRunTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ChPowerTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ChStressTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ChAvgPow = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ChDateBuy = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ChDateStartM = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ChDateEndL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.BtnClose = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// tableLayoutPanel2
			// 
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.label1, 1, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			// 
			// pictureBox1
			// 
			resources.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// tableLayoutPanel3
			// 
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.LVLasers, 0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			// 
			// tableLayoutPanel4
			// 
			resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
			this.tableLayoutPanel4.Controls.Add(this.BtnUnmark, 0, 4);
			this.tableLayoutPanel4.Controls.Add(this.BtnMark, 0, 3);
			this.tableLayoutPanel4.Controls.Add(this.BtnEdit, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.BtnRemove, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.BtnAddNew, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.Preview, 0, 6);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			// 
			// BtnUnmark
			// 
			resources.ApplyResources(this.BtnUnmark, "BtnUnmark");
			this.BtnUnmark.Name = "BtnUnmark";
			this.BtnUnmark.UseVisualStyleBackColor = true;
			this.BtnUnmark.Click += new System.EventHandler(this.BtnUnmark_Click);
			// 
			// BtnMark
			// 
			resources.ApplyResources(this.BtnMark, "BtnMark");
			this.BtnMark.Name = "BtnMark";
			this.BtnMark.UseVisualStyleBackColor = true;
			this.BtnMark.Click += new System.EventHandler(this.BtnMark_Click);
			// 
			// BtnEdit
			// 
			resources.ApplyResources(this.BtnEdit, "BtnEdit");
			this.BtnEdit.Name = "BtnEdit";
			this.BtnEdit.UseVisualStyleBackColor = true;
			this.BtnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
			// 
			// BtnRemove
			// 
			resources.ApplyResources(this.BtnRemove, "BtnRemove");
			this.BtnRemove.Name = "BtnRemove";
			this.BtnRemove.UseVisualStyleBackColor = true;
			this.BtnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
			// 
			// BtnAddNew
			// 
			resources.ApplyResources(this.BtnAddNew, "BtnAddNew");
			this.BtnAddNew.Name = "BtnAddNew";
			this.BtnAddNew.UseVisualStyleBackColor = true;
			this.BtnAddNew.Click += new System.EventHandler(this.BtnAddNew_Click);
			// 
			// Preview
			// 
			resources.ApplyResources(this.Preview, "Preview");
			this.Preview.LifeCounter = null;
			this.Preview.Name = "Preview";
			// 
			// LVLasers
			// 
			this.LVLasers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ChName,
            this.ChBrand,
            this.ChModel,
            this.ChRunTime,
            this.ChPowerTime,
            this.ChStressTime,
            this.ChAvgPow,
            this.ChDateBuy,
            this.ChDateStartM,
            this.ChDateEndL});
			resources.ApplyResources(this.LVLasers, "LVLasers");
			this.LVLasers.FullRowSelect = true;
			this.LVLasers.GridLines = true;
			this.LVLasers.HideSelection = false;
			this.LVLasers.Name = "LVLasers";
			this.LVLasers.UseCompatibleStateImageBehavior = false;
			this.LVLasers.View = System.Windows.Forms.View.Details;
			this.LVLasers.SelectedIndexChanged += new System.EventHandler(this.LVLasers_SelectedIndexChanged);
			this.LVLasers.DoubleClick += new System.EventHandler(this.LVLasers_DoubleClick);
			// 
			// ChName
			// 
			resources.ApplyResources(this.ChName, "ChName");
			// 
			// ChBrand
			// 
			resources.ApplyResources(this.ChBrand, "ChBrand");
			// 
			// ChModel
			// 
			resources.ApplyResources(this.ChModel, "ChModel");
			// 
			// ChRunTime
			// 
			resources.ApplyResources(this.ChRunTime, "ChRunTime");
			// 
			// ChPowerTime
			// 
			resources.ApplyResources(this.ChPowerTime, "ChPowerTime");
			// 
			// ChStressTime
			// 
			resources.ApplyResources(this.ChStressTime, "ChStressTime");
			// 
			// ChAvgPow
			// 
			resources.ApplyResources(this.ChAvgPow, "ChAvgPow");
			// 
			// ChDateBuy
			// 
			resources.ApplyResources(this.ChDateBuy, "ChDateBuy");
			// 
			// ChDateStartM
			// 
			resources.ApplyResources(this.ChDateStartM, "ChDateStartM");
			// 
			// ChDateEndL
			// 
			resources.ApplyResources(this.ChDateEndL, "ChDateEndL");
			// 
			// tableLayoutPanel5
			// 
			resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
			this.tableLayoutPanel5.Controls.Add(this.label2, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.BtnClose, 1, 0);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// BtnClose
			// 
			resources.ApplyResources(this.BtnClose, "BtnClose");
			this.BtnClose.Name = "BtnClose";
			this.BtnClose.UseVisualStyleBackColor = true;
			this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
			// 
			// LaserUsage
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LaserUsage";
			this.ShowInTaskbar = false;
			this.Load += new System.EventHandler(this.LaserUsage_Load);
			this.Shown += new System.EventHandler(this.LaserUsage_Shown);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Button BtnEdit;
		private System.Windows.Forms.Button BtnRemove;
		private System.Windows.Forms.Button BtnAddNew;
		private System.Windows.Forms.ListView LVLasers;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Button BtnClose;
		private System.Windows.Forms.Button BtnMark;
		private System.Windows.Forms.ColumnHeader ChName;
		private System.Windows.Forms.ColumnHeader ChDateBuy;
		private System.Windows.Forms.ColumnHeader ChDateStartM;
		private System.Windows.Forms.ColumnHeader ChDateEndL;
		private System.Windows.Forms.ColumnHeader ChRunTime;
		private System.Windows.Forms.ColumnHeader ChPowerTime;
		private System.Windows.Forms.ColumnHeader ChAvgPow;
		private System.Windows.Forms.ColumnHeader ChBrand;
		private System.Windows.Forms.ColumnHeader ChModel;
		private System.Windows.Forms.Button BtnUnmark;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ColumnHeader ChStressTime;
		private UserControls.UsageClassPreview Preview;
	}
}