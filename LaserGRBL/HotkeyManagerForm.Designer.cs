namespace LaserGRBL
{
	partial class HotkeyManagerForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HotkeyManagerForm));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.LblConnect = new System.Windows.Forms.Label();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.BtnSave = new System.Windows.Forms.Button();
			this.GB = new System.Windows.Forms.GroupBox();
			this.DGV = new LaserGRBL.MyDatagridView();
			this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Shortcut = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.GB.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.GB, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(560, 519);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.LblConnect, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnCancel, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnSave, 2, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 468);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(554, 48);
			this.tableLayoutPanel2.TabIndex = 3;
			// 
			// LblConnect
			// 
			this.LblConnect.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.LblConnect.AutoSize = true;
			this.LblConnect.ForeColor = System.Drawing.Color.Red;
			this.LblConnect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblConnect.Location = new System.Drawing.Point(48, 11);
			this.LblConnect.Name = "LblConnect";
			this.LblConnect.Size = new System.Drawing.Size(232, 26);
			this.LblConnect.TabIndex = 19;
			this.LblConnect.Text = "Type any keys combination to set shortcut.\r\nPress \"Canc\" keyboard key to remove s" +
    "hortcut.";
			this.LblConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// BtnCancel
			// 
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("BtnCancel.Image")));
			this.BtnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnCancel.Location = new System.Drawing.Point(331, 3);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(107, 42);
			this.BtnCancel.TabIndex = 13;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.BtnCancel.UseVisualStyleBackColor = true;
			// 
			// BtnSave
			// 
			this.BtnSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BtnSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnSave.Image")));
			this.BtnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnSave.Location = new System.Drawing.Point(444, 3);
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.Size = new System.Drawing.Size(107, 42);
			this.BtnSave.TabIndex = 14;
			this.BtnSave.Text = "Save";
			this.BtnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.BtnSave.UseVisualStyleBackColor = true;
			this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// GB
			// 
			this.GB.Controls.Add(this.DGV);
			this.GB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GB.Font = new System.Drawing.Font("Courier New", 8.25F);
			this.GB.Location = new System.Drawing.Point(3, 3);
			this.GB.Name = "GB";
			this.GB.Size = new System.Drawing.Size(554, 459);
			this.GB.TabIndex = 4;
			this.GB.TabStop = false;
			this.GB.Text = "Available shortcut";
			// 
			// DGV
			// 
			this.DGV.AllowUserToAddRows = false;
			this.DGV.AllowUserToDeleteRows = false;
			this.DGV.AllowUserToResizeRows = false;
			this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Action,
            this.Shortcut});
			this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DGV.Location = new System.Drawing.Point(3, 16);
			this.DGV.Name = "DGV";
			this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.DGV.Size = new System.Drawing.Size(548, 440);
			this.DGV.TabIndex = 0;
			// 
			// Action
			// 
			this.Action.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.Action.DataPropertyName = "Action";
			this.Action.HeaderText = "Action";
			this.Action.MinimumWidth = 140;
			this.Action.Name = "Action";
			this.Action.ReadOnly = true;
			this.Action.Width = 140;
			// 
			// Shortcut
			// 
			this.Shortcut.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Shortcut.DataPropertyName = "Combination";
			this.Shortcut.HeaderText = "Shortcut";
			this.Shortcut.Name = "Shortcut";
			this.Shortcut.ReadOnly = true;
			// 
			// HotkeyManagerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(560, 519);
			this.Controls.Add(this.tableLayoutPanel1);
			this.KeyPreview = true;
			this.Name = "HotkeyManagerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Hotkey Manager";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.GB.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.GroupBox GB;
		private System.Windows.Forms.Button BtnCancel;
		private MyDatagridView DGV;
		private System.Windows.Forms.DataGridViewTextBoxColumn Action;
		private System.Windows.Forms.DataGridViewTextBoxColumn Shortcut;
		private System.Windows.Forms.Button BtnSave;
		private System.Windows.Forms.Label LblConnect;
	}
}