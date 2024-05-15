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
			this.BtnCancel = new LaserGRBL.UserControls.GrblButton();
			this.BtnCreate = new LaserGRBL.UserControls.GrblButton();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.LblImage = new System.Windows.Forms.Label();
			this.BTOpenImage = new LaserGRBL.UserControls.ImageButton();
			this.LblToolTip = new System.Windows.Forms.Label();
			this.TbToolTip = new LaserGRBL.UserControls.TextInput();
			this.LblEnabled = new System.Windows.Forms.Label();
			this.CbEStyles = new LaserGRBL.UserControls.FlatComboBox();
			this.LblDescription = new System.Windows.Forms.LinkLabel();
			this.LblType = new System.Windows.Forms.Label();
			this.CbByttonType = new LaserGRBL.UserControls.FlatComboBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.LblGCode2 = new System.Windows.Forms.Label();
			this.TBGCode = new LaserGRBL.UserControls.TextInput();
			this.LblGCode = new System.Windows.Forms.Label();
			this.TBGCode2 = new LaserGRBL.UserControls.TextInput();
			this.TbCaption = new LaserGRBL.UserControls.TextInput();
			this.LblCaption = new System.Windows.Forms.Label();
			this.tableLayoutPanel9.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
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
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.BtnCancel, "BtnCancel");
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// BtnCreate
			// 
			resources.ApplyResources(this.BtnCreate, "BtnCreate");
			this.BtnCreate.Name = "BtnCreate";
			this.BtnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
			// 
			// tableLayoutPanel2
			// 
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.LblImage, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.BTOpenImage, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.LblToolTip, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.TbToolTip, 1, 5);
			this.tableLayoutPanel2.Controls.Add(this.LblEnabled, 0, 6);
			this.tableLayoutPanel2.Controls.Add(this.CbEStyles, 1, 6);
			this.tableLayoutPanel2.Controls.Add(this.LblDescription, 0, 7);
			this.tableLayoutPanel2.Controls.Add(this.LblType, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.CbByttonType, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.TbCaption, 1, 4);
			this.tableLayoutPanel2.Controls.Add(this.LblCaption, 0, 4);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			// 
			// LblImage
			// 
			resources.ApplyResources(this.LblImage, "LblImage");
			this.LblImage.Name = "LblImage";
			// 
			// BTOpenImage
			// 
			this.BTOpenImage.AltImage = null;
			this.BTOpenImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BTOpenImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.BTOpenImage.Caption = "";
			this.BTOpenImage.Image = ((System.Drawing.Image)(resources.GetObject("BTOpenImage.Image")));
			resources.ApplyResources(this.BTOpenImage, "BTOpenImage");
			this.BTOpenImage.Name = "BTOpenImage";
			this.BTOpenImage.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BTOpenImage.TabStop = false;
			this.BTOpenImage.UseAltImage = false;
			this.BTOpenImage.Click += new System.EventHandler(this.BTOpenImage_Click);
			// 
			// LblToolTip
			// 
			resources.ApplyResources(this.LblToolTip, "LblToolTip");
			this.LblToolTip.Name = "LblToolTip";
			// 
			// TbToolTip
			// 
			resources.ApplyResources(this.TbToolTip, "TbToolTip");
			this.TbToolTip.Name = "TbToolTip";
			// 
			// LblEnabled
			// 
			resources.ApplyResources(this.LblEnabled, "LblEnabled");
			this.LblEnabled.Name = "LblEnabled";
			// 
			// CbEStyles
			// 
			resources.ApplyResources(this.CbEStyles, "CbEStyles");
			this.CbEStyles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbEStyles.FormattingEnabled = true;
			this.CbEStyles.Name = "CbEStyles";
			// 
			// LblDescription
			// 
			resources.ApplyResources(this.LblDescription, "LblDescription");
			this.tableLayoutPanel2.SetColumnSpan(this.LblDescription, 2);
			this.LblDescription.Name = "LblDescription";
			this.LblDescription.TabStop = true;
			this.LblDescription.Tag = "https://lasergrbl.com/usage/custom-buttons/";
			this.LblDescription.UseCompatibleTextRendering = true;
			this.LblDescription.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LblDescription_LinkClicked);
			// 
			// LblType
			// 
			resources.ApplyResources(this.LblType, "LblType");
			this.LblType.Name = "LblType";
			// 
			// CbByttonType
			// 
			resources.ApplyResources(this.CbByttonType, "CbByttonType");
			this.CbByttonType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbByttonType.FormattingEnabled = true;
			this.CbByttonType.Name = "CbByttonType";
			this.CbByttonType.SelectedIndexChanged += new System.EventHandler(this.CbByttonType_SelectedIndexChanged);
			// 
			// tableLayoutPanel3
			// 
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
			this.tableLayoutPanel3.Controls.Add(this.LblGCode2, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.TBGCode, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.LblGCode, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.TBGCode2, 1, 1);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel2.SetRowSpan(this.tableLayoutPanel3, 2);
			// 
			// LblGCode2
			// 
			resources.ApplyResources(this.LblGCode2, "LblGCode2");
			this.LblGCode2.Name = "LblGCode2";
			// 
			// TBGCode
			// 
			resources.ApplyResources(this.TBGCode, "TBGCode");
			this.TBGCode.Name = "TBGCode";
			// 
			// LblGCode
			// 
			resources.ApplyResources(this.LblGCode, "LblGCode");
			this.LblGCode.Name = "LblGCode";
			// 
			// TBGCode2
			// 
			resources.ApplyResources(this.TBGCode2, "TBGCode2");
			this.TBGCode2.Name = "TBGCode2";
			// 
			// TbCaption
			// 
			resources.ApplyResources(this.TbCaption, "TbCaption");
			this.TbCaption.Name = "TbCaption";
			// 
			// LblCaption
			// 
			resources.ApplyResources(this.LblCaption, "LblCaption");
			this.LblCaption.Name = "LblCaption";
			// 
			// CustomButtonForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.BtnCancel;
			this.Controls.Add(this.tableLayoutPanel9);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "CustomButtonForm";
			this.tableLayoutPanel9.ResumeLayout(false);
			this.tableLayoutPanel9.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private LaserGRBL.UserControls.GrblButton BtnCancel;
		private LaserGRBL.UserControls.GrblButton BtnCreate;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private UserControls.ImageButton BTOpenImage;
		private System.Windows.Forms.Label LblImage;
		private System.Windows.Forms.Label LblGCode;
		private LaserGRBL.UserControls.TextInput TbToolTip;
		private System.Windows.Forms.Label LblToolTip;
		private System.Windows.Forms.Label LblEnabled;
		private LaserGRBL.UserControls.FlatComboBox CbEStyles;
		private LaserGRBL.UserControls.TextInput TBGCode;
		private System.Windows.Forms.Label LblType;
		private LaserGRBL.UserControls.FlatComboBox CbByttonType;
		private LaserGRBL.UserControls.TextInput TBGCode2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label LblGCode2;
		private LaserGRBL.UserControls.TextInput TbCaption;
		private System.Windows.Forms.Label LblCaption;
        private System.Windows.Forms.LinkLabel LblDescription;
    }
}