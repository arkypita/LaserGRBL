/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 05/12/2016
 * Time: 23:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace LaserGRBL
{
	partial class PreviewForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreviewForm));
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.BtnReset = new LaserGRBL.UserControls.ImageButton();
            this.CustomButtonArea = new LaserGRBL.PreviewForm.MyFlowPanel();
            this.MNAddCB = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addCustomButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MnLockButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportCustomButtonsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importCustomButtonsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnUnlock = new LaserGRBL.UserControls.ImageButton();
            this.BtnHoming = new LaserGRBL.UserControls.ImageButton();
            this.BtnZeroing = new LaserGRBL.UserControls.ImageButton();
            this.BtnResume = new LaserGRBL.UserControls.ImageButton();
            this.BtnStop = new LaserGRBL.UserControls.ImageButton();
            this.TT = new System.Windows.Forms.ToolTip(this.components);
            this.MNRemEditCB = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RemoveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.editButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel8.SuspendLayout();
            this.MNAddCB.SuspendLayout();
            this.MNRemEditCB.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel8
            // 
            resources.ApplyResources(this.tableLayoutPanel8, "tableLayoutPanel8");
            this.tableLayoutPanel8.Controls.Add(this.BtnReset, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.CustomButtonArea, 5, 0);
            this.tableLayoutPanel8.Controls.Add(this.BtnUnlock, 2, 0);
            this.tableLayoutPanel8.Controls.Add(this.BtnHoming, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.BtnZeroing, 4, 0);
            this.tableLayoutPanel8.Controls.Add(this.BtnResume, 7, 0);
            this.tableLayoutPanel8.Controls.Add(this.BtnStop, 8, 0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            // 
            // BtnReset
            // 
            this.BtnReset.AltImage = null;
            resources.ApplyResources(this.BtnReset, "BtnReset");
            this.BtnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnReset.Caption = null;
            this.BtnReset.Image = ((System.Drawing.Image)(resources.GetObject("BtnReset.Image")));
            this.BtnReset.Name = "BtnReset";
            this.BtnReset.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.TT.SetToolTip(this.BtnReset, resources.GetString("BtnReset.ToolTip"));
            this.BtnReset.UseAltImage = false;
            this.BtnReset.Click += new System.EventHandler(this.BtnResetClick);
            // 
            // CustomButtonArea
            // 
            this.CustomButtonArea.AllowDrop = true;
            this.CustomButtonArea.ContextMenuStrip = this.MNAddCB;
            resources.ApplyResources(this.CustomButtonArea, "CustomButtonArea");
            this.CustomButtonArea.Name = "CustomButtonArea";
            // 
            // MNAddCB
            // 
            this.MNAddCB.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCustomButtonToolStripMenuItem,
            this.MnLockButton,
            this.toolStripMenuItem1,
            this.exportCustomButtonsToolStripMenuItem,
            this.importCustomButtonsToolStripMenuItem});
            this.MNAddCB.Name = "CMM";
            resources.ApplyResources(this.MNAddCB, "MNAddCB");
            this.MNAddCB.Opening += new System.ComponentModel.CancelEventHandler(this.MNAddCB_Opening);
            // 
            // addCustomButtonToolStripMenuItem
            // 
            this.addCustomButtonToolStripMenuItem.Name = "addCustomButtonToolStripMenuItem";
            resources.ApplyResources(this.addCustomButtonToolStripMenuItem, "addCustomButtonToolStripMenuItem");
            this.addCustomButtonToolStripMenuItem.Click += new System.EventHandler(this.addCustomButtonToolStripMenuItem_Click);
            // 
            // MnLockButton
            // 
            this.MnLockButton.Checked = true;
            this.MnLockButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MnLockButton.Name = "MnLockButton";
            resources.ApplyResources(this.MnLockButton, "MnLockButton");
            this.MnLockButton.Click += new System.EventHandler(this.MnUnlockButtonClick);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // exportCustomButtonsToolStripMenuItem
            // 
            this.exportCustomButtonsToolStripMenuItem.Name = "exportCustomButtonsToolStripMenuItem";
            resources.ApplyResources(this.exportCustomButtonsToolStripMenuItem, "exportCustomButtonsToolStripMenuItem");
            this.exportCustomButtonsToolStripMenuItem.Click += new System.EventHandler(this.exportCustomButtonsToolStripMenuItem_Click);
            // 
            // importCustomButtonsToolStripMenuItem
            // 
            this.importCustomButtonsToolStripMenuItem.Name = "importCustomButtonsToolStripMenuItem";
            resources.ApplyResources(this.importCustomButtonsToolStripMenuItem, "importCustomButtonsToolStripMenuItem");
            this.importCustomButtonsToolStripMenuItem.Click += new System.EventHandler(this.importCustomButtonsToolStripMenuItem_Click);
            // 
            // BtnUnlock
            // 
            this.BtnUnlock.AltImage = null;
            resources.ApplyResources(this.BtnUnlock, "BtnUnlock");
            this.BtnUnlock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnUnlock.Caption = null;
            this.BtnUnlock.Image = ((System.Drawing.Image)(resources.GetObject("BtnUnlock.Image")));
            this.BtnUnlock.Name = "BtnUnlock";
            this.BtnUnlock.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.TT.SetToolTip(this.BtnUnlock, resources.GetString("BtnUnlock.ToolTip"));
            this.BtnUnlock.UseAltImage = false;
            this.BtnUnlock.Click += new System.EventHandler(this.BtnUnlockClick);
            // 
            // BtnHoming
            // 
            this.BtnHoming.AltImage = null;
            resources.ApplyResources(this.BtnHoming, "BtnHoming");
            this.BtnHoming.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnHoming.Caption = null;
            this.BtnHoming.Image = ((System.Drawing.Image)(resources.GetObject("BtnHoming.Image")));
            this.BtnHoming.Name = "BtnHoming";
            this.BtnHoming.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.TT.SetToolTip(this.BtnHoming, resources.GetString("BtnHoming.ToolTip"));
            this.BtnHoming.UseAltImage = false;
            this.BtnHoming.Click += new System.EventHandler(this.BtnGoHomeClick);
            // 
            // BtnZeroing
            // 
            this.BtnZeroing.AltImage = null;
            resources.ApplyResources(this.BtnZeroing, "BtnZeroing");
            this.BtnZeroing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnZeroing.Caption = null;
            this.BtnZeroing.Image = ((System.Drawing.Image)(resources.GetObject("BtnZeroing.Image")));
            this.BtnZeroing.Name = "BtnZeroing";
            this.BtnZeroing.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.TT.SetToolTip(this.BtnZeroing, resources.GetString("BtnZeroing.ToolTip"));
            this.BtnZeroing.UseAltImage = false;
            this.BtnZeroing.Click += new System.EventHandler(this.BtnZeroing_Click);
            // 
            // BtnResume
            // 
            this.BtnResume.AltImage = null;
            resources.ApplyResources(this.BtnResume, "BtnResume");
            this.BtnResume.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnResume.Caption = null;
            this.BtnResume.Image = ((System.Drawing.Image)(resources.GetObject("BtnResume.Image")));
            this.BtnResume.Name = "BtnResume";
            this.BtnResume.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.TT.SetToolTip(this.BtnResume, resources.GetString("BtnResume.ToolTip"));
            this.BtnResume.UseAltImage = false;
            this.BtnResume.Click += new System.EventHandler(this.BtnResumeClick);
            // 
            // BtnStop
            // 
            this.BtnStop.AltImage = ((System.Drawing.Image)(resources.GetObject("BtnStop.AltImage")));
            resources.ApplyResources(this.BtnStop, "BtnStop");
            this.BtnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnStop.Caption = null;
            this.BtnStop.Image = ((System.Drawing.Image)(resources.GetObject("BtnStop.Image")));
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.TT.SetToolTip(this.BtnStop, resources.GetString("BtnStop.ToolTip"));
            this.BtnStop.UseAltImage = false;
            this.BtnStop.Click += new System.EventHandler(this.BtnStopClick);
            // 
            // MNRemEditCB
            // 
            this.MNRemEditCB.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RemoveButton,
            this.editButtonToolStripMenuItem});
            this.MNRemEditCB.Name = "CMM";
            resources.ApplyResources(this.MNRemEditCB, "MNRemEditCB");
            // 
            // RemoveButton
            // 
            this.RemoveButton.Name = "RemoveButton";
            resources.ApplyResources(this.RemoveButton, "RemoveButton");
            // 
            // editButtonToolStripMenuItem
            // 
            this.editButtonToolStripMenuItem.Name = "editButtonToolStripMenuItem";
            resources.ApplyResources(this.editButtonToolStripMenuItem, "editButtonToolStripMenuItem");
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel8, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // PreviewForm
            // 
            this.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this, "$this");
            this.Name = "PreviewForm";
            this.tableLayoutPanel8.ResumeLayout(false);
            this.MNAddCB.ResumeLayout(false);
            this.MNRemEditCB.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

		}

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
		private UserControls.ImageButton BtnReset;
		private UserControls.ImageButton BtnHoming;
		private UserControls.ImageButton BtnStop;
		private UserControls.ImageButton BtnResume;
		private UserControls.ImageButton BtnUnlock;
		private System.Windows.Forms.ToolTip TT;
		private System.Windows.Forms.ContextMenuStrip MNAddCB;
		private System.Windows.Forms.ToolStripMenuItem addCustomButtonToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip MNRemEditCB;
		private System.Windows.Forms.ToolStripMenuItem RemoveButton;
		private System.Windows.Forms.ToolStripMenuItem editButtonToolStripMenuItem;
		private UserControls.ImageButton BtnZeroing;
		private PreviewForm.MyFlowPanel CustomButtonArea;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem exportCustomButtonsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importCustomButtonsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MnLockButton;
	}
}
