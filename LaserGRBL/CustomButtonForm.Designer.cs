namespace LaserGRBL
{
	partial class CustomButtonForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomButtonForm));
			this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.BtnCreate = new System.Windows.Forms.Button();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.BTOpenImage = new LaserGRBL.UserControls.ImageButton();
			this.label2 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.TbToolTip = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.CbEStyles = new System.Windows.Forms.ComboBox();
			this.TBGCode = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tableLayoutPanel9.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel9
			// 
			resources.ApplyResources(this.tableLayoutPanel9, "tableLayoutPanel9");
			this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel1, 0, 3);
			this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel2, 0, 2);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.BtnCancel, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.BtnCreate, 2, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// BtnCancel
			// 
			resources.ApplyResources(this.BtnCancel, "BtnCancel");
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// BtnCreate
			// 
			this.BtnCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.BtnCreate, "BtnCreate");
			this.BtnCreate.Name = "BtnCreate";
			this.BtnCreate.UseVisualStyleBackColor = true;
			this.BtnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
			// 
			// tableLayoutPanel2
			// 
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.BTOpenImage, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.label2, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.label6, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.TbToolTip, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.label7, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.CbEStyles, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.TBGCode, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.label5, 0, 4);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// BTOpenImage
			// 
			this.BTOpenImage.AltImage = null;
			this.BTOpenImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BTOpenImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.BTOpenImage.Coloration = System.Drawing.Color.Empty;
			this.BTOpenImage.Image = ((System.Drawing.Image)(resources.GetObject("BTOpenImage.Image")));
			resources.ApplyResources(this.BTOpenImage, "BTOpenImage");
			this.BTOpenImage.Name = "BTOpenImage";
			this.BTOpenImage.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BTOpenImage.UseAltImage = false;
			this.BTOpenImage.Click += new System.EventHandler(this.BTOpenImage_Click);
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// TbToolTip
			// 
			resources.ApplyResources(this.TbToolTip, "TbToolTip");
			this.tableLayoutPanel2.SetColumnSpan(this.TbToolTip, 2);
			this.TbToolTip.Name = "TbToolTip";
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// CbEStyles
			// 
			resources.ApplyResources(this.CbEStyles, "CbEStyles");
			this.tableLayoutPanel2.SetColumnSpan(this.CbEStyles, 2);
			this.CbEStyles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbEStyles.FormattingEnabled = true;
			this.CbEStyles.Name = "CbEStyles";
			// 
			// TBGCode
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.TBGCode, 2);
			resources.ApplyResources(this.TBGCode, "TBGCode");
			this.TBGCode.Name = "TBGCode";
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.tableLayoutPanel2.SetColumnSpan(this.label5, 3);
			this.label5.Name = "label5";
			// 
			// CustomButtonForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel9);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "CustomButtonForm";
			this.tableLayoutPanel9.ResumeLayout(false);
			this.tableLayoutPanel9.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.Button BtnCreate;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private UserControls.ImageButton BTOpenImage;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox TbToolTip;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox CbEStyles;
		private System.Windows.Forms.TextBox TBGCode;
	}
}