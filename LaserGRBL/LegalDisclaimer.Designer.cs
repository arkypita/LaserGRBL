namespace LaserGRBL
{
	partial class LegalDisclaimer
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LegalDisclaimer));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.LblFree = new System.Windows.Forms.Label();
			this.LblNotToy = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.LblHaveFun = new System.Windows.Forms.Label();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnAccept = new System.Windows.Forms.Button();
			this.CbIHaveRead = new System.Windows.Forms.CheckBox();
			this.LlTranslate = new System.Windows.Forms.LinkLabel();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.LblFree, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.LblNotToy, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.LblHaveFun, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.LlTranslate, 0, 2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// LblFree
			// 
			resources.ApplyResources(this.LblFree, "LblFree");
			this.tableLayoutPanel1.SetColumnSpan(this.LblFree, 2);
			this.LblFree.Name = "LblFree";
			// 
			// LblNotToy
			// 
			resources.ApplyResources(this.LblNotToy, "LblNotToy");
			this.LblNotToy.Name = "LblNotToy";
			// 
			// pictureBox1
			// 
			resources.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			// 
			// LblHaveFun
			// 
			resources.ApplyResources(this.LblHaveFun, "LblHaveFun");
			this.LblHaveFun.Name = "LblHaveFun";
			// 
			// tableLayoutPanel2
			// 
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
			this.tableLayoutPanel2.Controls.Add(this.BtnAccept, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.CbIHaveRead, 0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			// 
			// BtnAccept
			// 
			resources.ApplyResources(this.BtnAccept, "BtnAccept");
			this.BtnAccept.Name = "BtnAccept";
			this.BtnAccept.UseVisualStyleBackColor = true;
			this.BtnAccept.Click += new System.EventHandler(this.BtnAccept_Click);
			// 
			// CbIHaveRead
			// 
			resources.ApplyResources(this.CbIHaveRead, "CbIHaveRead");
			this.CbIHaveRead.Name = "CbIHaveRead";
			this.CbIHaveRead.UseVisualStyleBackColor = true;
			this.CbIHaveRead.CheckedChanged += new System.EventHandler(this.CbIHaveRead_CheckedChanged);
			// 
			// LlTranslate
			// 
			resources.ApplyResources(this.LlTranslate, "LlTranslate");
			this.LlTranslate.Name = "LlTranslate";
			this.LlTranslate.TabStop = true;
			this.LlTranslate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LlTranslate_LinkClicked);
			// 
			// LegalDisclaimer
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LegalDisclaimer";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label LblNotToy;
		private System.Windows.Forms.Label LblFree;
		private System.Windows.Forms.Label LblHaveFun;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button BtnAccept;
		private System.Windows.Forms.LinkLabel LlTranslate;
		private System.Windows.Forms.CheckBox CbIHaveRead;
	}
}