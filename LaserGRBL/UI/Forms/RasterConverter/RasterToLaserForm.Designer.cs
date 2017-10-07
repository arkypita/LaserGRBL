namespace LaserGRBL.UI.Forms.RasterConverter
{
	partial class RasterToLaserForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RasterToLaserForm));
			this.RightGrid = new System.Windows.Forms.TableLayoutPanel();
			this.TCOriginalPreview = new System.Windows.Forms.TabControl();
			this.TpPreview = new System.Windows.Forms.TabPage();
			this.PbConverted = new System.Windows.Forms.PictureBox();
			this.TpOriginal = new System.Windows.Forms.TabPage();
			this.PbOriginal = new System.Windows.Forms.PictureBox();
			this.FlipControl = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.BtnCreate = new System.Windows.Forms.Button();
			this.WT = new System.Windows.Forms.Timer(this.components);
			this.TT = new System.Windows.Forms.ToolTip(this.components);
			this.WB = new LaserGRBL.UserControls.WaitingProgressBar();
			this.BtFlipV = new LaserGRBL.UserControls.ImageButton();
			this.BtFlipH = new LaserGRBL.UserControls.ImageButton();
			this.BtRotateCW = new LaserGRBL.UserControls.ImageButton();
			this.BtRotateCCW = new LaserGRBL.UserControls.ImageButton();
			this.BtnCrop = new LaserGRBL.UserControls.ImageButton();
			this.BtnRevert = new LaserGRBL.UserControls.ImageButton();
			this.GS = new LaserGRBL.UI.Forms.RasterConverter.SetupGrayscale();
			this.ST = new LaserGRBL.UI.Forms.RasterConverter.SetupTool();
			this.RightGrid.SuspendLayout();
			this.TCOriginalPreview.SuspendLayout();
			this.TpPreview.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PbConverted)).BeginInit();
			this.TpOriginal.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PbOriginal)).BeginInit();
			this.FlipControl.SuspendLayout();
			this.tableLayoutPanel8.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// RightGrid
			// 
			resources.ApplyResources(this.RightGrid, "RightGrid");
			this.RightGrid.Controls.Add(this.TCOriginalPreview, 1, 0);
			this.RightGrid.Controls.Add(this.FlipControl, 1, 1);
			this.RightGrid.Controls.Add(this.tableLayoutPanel8, 0, 0);
			this.RightGrid.Controls.Add(this.tableLayoutPanel1, 3, 1);
			this.RightGrid.Name = "RightGrid";
			// 
			// TCOriginalPreview
			// 
			this.RightGrid.SetColumnSpan(this.TCOriginalPreview, 3);
			this.TCOriginalPreview.Controls.Add(this.TpPreview);
			this.TCOriginalPreview.Controls.Add(this.TpOriginal);
			resources.ApplyResources(this.TCOriginalPreview, "TCOriginalPreview");
			this.TCOriginalPreview.Name = "TCOriginalPreview";
			this.TCOriginalPreview.SelectedIndex = 0;
			// 
			// TpPreview
			// 
			this.TpPreview.Controls.Add(this.WB);
			this.TpPreview.Controls.Add(this.PbConverted);
			resources.ApplyResources(this.TpPreview, "TpPreview");
			this.TpPreview.Name = "TpPreview";
			this.TpPreview.UseVisualStyleBackColor = true;
			// 
			// PbConverted
			// 
			this.PbConverted.BackColor = System.Drawing.Color.White;
			resources.ApplyResources(this.PbConverted, "PbConverted");
			this.PbConverted.Name = "PbConverted";
			this.PbConverted.TabStop = false;
			this.PbConverted.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbConvertedMouseDown);
			this.PbConverted.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PbConvertedMouseMove);
			this.PbConverted.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PbConvertedMouseUp);
			this.PbConverted.Resize += new System.EventHandler(this.PbConverted_Resize);
			// 
			// TpOriginal
			// 
			this.TpOriginal.Controls.Add(this.PbOriginal);
			resources.ApplyResources(this.TpOriginal, "TpOriginal");
			this.TpOriginal.Name = "TpOriginal";
			this.TpOriginal.UseVisualStyleBackColor = true;
			// 
			// PbOriginal
			// 
			this.PbOriginal.BackColor = System.Drawing.Color.White;
			resources.ApplyResources(this.PbOriginal, "PbOriginal");
			this.PbOriginal.Name = "PbOriginal";
			this.PbOriginal.TabStop = false;
			// 
			// FlipControl
			// 
			resources.ApplyResources(this.FlipControl, "FlipControl");
			this.FlipControl.Controls.Add(this.BtFlipV, 5, 0);
			this.FlipControl.Controls.Add(this.BtFlipH, 4, 0);
			this.FlipControl.Controls.Add(this.BtRotateCW, 2, 0);
			this.FlipControl.Controls.Add(this.BtRotateCCW, 3, 0);
			this.FlipControl.Controls.Add(this.BtnCrop, 6, 0);
			this.FlipControl.Controls.Add(this.BtnRevert, 0, 0);
			this.FlipControl.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
			this.FlipControl.Name = "FlipControl";
			// 
			// tableLayoutPanel8
			// 
			resources.ApplyResources(this.tableLayoutPanel8, "tableLayoutPanel8");
			this.tableLayoutPanel8.Controls.Add(this.GS, 0, 0);
			this.tableLayoutPanel8.Controls.Add(this.ST, 0, 1);
			this.tableLayoutPanel8.Name = "tableLayoutPanel8";
			this.RightGrid.SetRowSpan(this.tableLayoutPanel8, 2);
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.BtnCancel, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.BtnCreate, 1, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// BtnCancel
			// 
			resources.ApplyResources(this.BtnCancel, "BtnCancel");
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancelClick);
			// 
			// BtnCreate
			// 
			resources.ApplyResources(this.BtnCreate, "BtnCreate");
			this.BtnCreate.Name = "BtnCreate";
			this.BtnCreate.UseVisualStyleBackColor = true;
			this.BtnCreate.Click += new System.EventHandler(this.BtnCreateClick);
			// 
			// WT
			// 
			this.WT.Interval = 50;
			this.WT.Tick += new System.EventHandler(this.WTTick);
			// 
			// WB
			// 
			resources.ApplyResources(this.WB, "WB");
			this.WB.BarColor = System.Drawing.Color.SteelBlue;
			this.WB.BorderColor = System.Drawing.Color.Black;
			this.WB.BouncingMode = LaserGRBL.UserControls.WaitingProgressBar.BouncingModeEnum.PingPong;
			this.WB.DrawProgressString = false;
			this.WB.FillColor = System.Drawing.Color.White;
			this.WB.FillStyle = LaserGRBL.UserControls.FillStyles.Solid;
			this.WB.Interval = 25D;
			this.WB.Maximum = 20D;
			this.WB.Minimum = 0D;
			this.WB.Name = "WB";
			this.WB.ProgressStringDecimals = 0;
			this.WB.Reverse = true;
			this.WB.Running = false;
			this.WB.Step = 1D;
			this.WB.ThrowExceprion = false;
			this.WB.Value = 0D;
			// 
			// BtFlipV
			// 
			this.BtFlipV.AltImage = null;
			this.BtFlipV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtFlipV.Coloration = System.Drawing.Color.Empty;
			this.BtFlipV.Image = ((System.Drawing.Image)(resources.GetObject("BtFlipV.Image")));
			resources.ApplyResources(this.BtFlipV, "BtFlipV");
			this.BtFlipV.Name = "BtFlipV";
			this.BtFlipV.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.TT.SetToolTip(this.BtFlipV, resources.GetString("BtFlipV.ToolTip"));
			this.BtFlipV.UseAltImage = false;
			this.BtFlipV.Click += new System.EventHandler(this.BtFlipVClick);
			// 
			// BtFlipH
			// 
			this.BtFlipH.AltImage = null;
			this.BtFlipH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtFlipH.Coloration = System.Drawing.Color.Empty;
			this.BtFlipH.Image = ((System.Drawing.Image)(resources.GetObject("BtFlipH.Image")));
			resources.ApplyResources(this.BtFlipH, "BtFlipH");
			this.BtFlipH.Name = "BtFlipH";
			this.BtFlipH.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.TT.SetToolTip(this.BtFlipH, resources.GetString("BtFlipH.ToolTip"));
			this.BtFlipH.UseAltImage = false;
			this.BtFlipH.Click += new System.EventHandler(this.BtFlipHClick);
			// 
			// BtRotateCW
			// 
			this.BtRotateCW.AltImage = null;
			this.BtRotateCW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtRotateCW.Coloration = System.Drawing.Color.Empty;
			this.BtRotateCW.Image = ((System.Drawing.Image)(resources.GetObject("BtRotateCW.Image")));
			resources.ApplyResources(this.BtRotateCW, "BtRotateCW");
			this.BtRotateCW.Name = "BtRotateCW";
			this.BtRotateCW.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.TT.SetToolTip(this.BtRotateCW, resources.GetString("BtRotateCW.ToolTip"));
			this.BtRotateCW.UseAltImage = false;
			this.BtRotateCW.Click += new System.EventHandler(this.BtRotateCWClick);
			// 
			// BtRotateCCW
			// 
			this.BtRotateCCW.AltImage = null;
			this.BtRotateCCW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtRotateCCW.Coloration = System.Drawing.Color.Empty;
			this.BtRotateCCW.Image = ((System.Drawing.Image)(resources.GetObject("BtRotateCCW.Image")));
			resources.ApplyResources(this.BtRotateCCW, "BtRotateCCW");
			this.BtRotateCCW.Name = "BtRotateCCW";
			this.BtRotateCCW.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.TT.SetToolTip(this.BtRotateCCW, resources.GetString("BtRotateCCW.ToolTip"));
			this.BtRotateCCW.UseAltImage = false;
			this.BtRotateCCW.Click += new System.EventHandler(this.BtRotateCCWClick);
			// 
			// BtnCrop
			// 
			this.BtnCrop.AltImage = null;
			this.BtnCrop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnCrop.Coloration = System.Drawing.Color.Empty;
			this.BtnCrop.Image = ((System.Drawing.Image)(resources.GetObject("BtnCrop.Image")));
			resources.ApplyResources(this.BtnCrop, "BtnCrop");
			this.BtnCrop.Name = "BtnCrop";
			this.BtnCrop.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.TT.SetToolTip(this.BtnCrop, resources.GetString("BtnCrop.ToolTip"));
			this.BtnCrop.UseAltImage = false;
			this.BtnCrop.Click += new System.EventHandler(this.BtnCropClick);
			// 
			// BtnRevert
			// 
			this.BtnRevert.AltImage = null;
			this.BtnRevert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnRevert.Coloration = System.Drawing.Color.Empty;
			this.BtnRevert.Image = ((System.Drawing.Image)(resources.GetObject("BtnRevert.Image")));
			resources.ApplyResources(this.BtnRevert, "BtnRevert");
			this.BtnRevert.Name = "BtnRevert";
			this.BtnRevert.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.TT.SetToolTip(this.BtnRevert, resources.GetString("BtnRevert.ToolTip"));
			this.BtnRevert.UseAltImage = false;
			this.BtnRevert.Click += new System.EventHandler(this.BtnRevertClick);
			// 
			// GS
			// 
			resources.ApplyResources(this.GS, "GS");
			this.GS.ClipPreview = false;
			this.GS.Name = "GS";
			this.GS.ValueChanged += new System.EventHandler(this.GS_ValueChanged);
			// 
			// ST
			// 
			resources.ApplyResources(this.ST, "ST");
			this.ST.Name = "ST";
			this.ST.ValueChanged += new System.EventHandler(this.ST_ValueChanged);
			// 
			// RasterToLaserForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.RightGrid);
			this.MinimizeBox = false;
			this.Name = "RasterToLaserForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RasterToLaserFormFormClosing);
			this.Load += new System.EventHandler(this.RasterToLaserForm_Load);
			this.RightGrid.ResumeLayout(false);
			this.RightGrid.PerformLayout();
			this.TCOriginalPreview.ResumeLayout(false);
			this.TpPreview.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PbConverted)).EndInit();
			this.TpOriginal.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PbOriginal)).EndInit();
			this.FlipControl.ResumeLayout(false);
			this.tableLayoutPanel8.ResumeLayout(false);
			this.tableLayoutPanel8.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel RightGrid;
		private System.Windows.Forms.TabControl TCOriginalPreview;
		private System.Windows.Forms.TabPage TpPreview;
		private System.Windows.Forms.PictureBox PbConverted;
		private System.Windows.Forms.TabPage TpOriginal;
		private System.Windows.Forms.PictureBox PbOriginal;
		private System.Windows.Forms.Button BtnCreate;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
		private LaserGRBL.UserControls.WaitingProgressBar WB;
		private System.Windows.Forms.Timer WT;
		private System.Windows.Forms.TableLayoutPanel FlipControl;
		private LaserGRBL.UserControls.ImageButton BtFlipV;
		private LaserGRBL.UserControls.ImageButton BtFlipH;
		private LaserGRBL.UserControls.ImageButton BtRotateCW;
		private LaserGRBL.UserControls.ImageButton BtRotateCCW;
		private LaserGRBL.UserControls.ImageButton BtnCrop;
		private LaserGRBL.UserControls.ImageButton BtnRevert;
		private System.Windows.Forms.ToolTip TT;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button BtnCancel;
		private SetupGrayscale GS;
		private SetupTool ST;
	}
}