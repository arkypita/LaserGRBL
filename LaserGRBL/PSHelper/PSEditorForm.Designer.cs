namespace LaserGRBL.PSHelper
{
	partial class PSEditorForm
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PSEditorForm));
			this.DG = new System.Windows.Forms.DataGridView();
			this.source = new System.Windows.Forms.BindingSource(this.components);
			this.materialDB = new LaserGRBL.PSHelper.MaterialDB();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.TbNewElement = new System.Windows.Forms.ToolStripStatusLabel();
			this.BtnImport = new System.Windows.Forms.ToolStripStatusLabel();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.visibleDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.modelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.materialDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.thicknessDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.actionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.powerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.speedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cyclesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ColModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColMaterial = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColThick = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Power = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColCycles = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.DG)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.source)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.materialDB)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// DG
			// 
			this.DG.AllowUserToResizeRows = false;
			this.DG.AutoGenerateColumns = false;
			this.DG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DG.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColID,
            this.ColVisible,
            this.ColModel,
            this.ColMaterial,
            this.ColAction,
            this.ColThick,
            this.Power,
            this.ColSpeed,
            this.ColCycles,
            this.ColRemarks});
			this.DG.DataSource = this.source;
			this.DG.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DG.Location = new System.Drawing.Point(0, 0);
			this.DG.Name = "DG";
			this.DG.Size = new System.Drawing.Size(891, 480);
			this.DG.TabIndex = 1;
			this.DG.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DG_DataError);
			// 
			// source
			// 
			this.source.DataMember = "Materials";
			this.source.DataSource = this.materialDB;
			// 
			// materialDB
			// 
			this.materialDB.DataSetName = "MaterialDB";
			this.materialDB.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.TbNewElement,
            this.BtnImport});
			this.statusStrip1.Location = new System.Drawing.Point(0, 480);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(891, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(85, 17);
			this.toolStripStatusLabel1.Text = "New elements:";
			// 
			// TbNewElement
			// 
			this.TbNewElement.Name = "TbNewElement";
			this.TbNewElement.Size = new System.Drawing.Size(13, 17);
			this.TbNewElement.Text = "0";
			// 
			// BtnImport
			// 
			this.BtnImport.IsLink = true;
			this.BtnImport.Name = "BtnImport";
			this.BtnImport.Size = new System.Drawing.Size(43, 17);
			this.BtnImport.Text = "import";
			this.BtnImport.Click += new System.EventHandler(this.BtnImport_Click);
			// 
			// idDataGridViewTextBoxColumn
			// 
			this.idDataGridViewTextBoxColumn.DataPropertyName = "id";
			this.idDataGridViewTextBoxColumn.HeaderText = "id";
			this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
			this.idDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// visibleDataGridViewCheckBoxColumn
			// 
			this.visibleDataGridViewCheckBoxColumn.DataPropertyName = "Visible";
			this.visibleDataGridViewCheckBoxColumn.HeaderText = "Visible";
			this.visibleDataGridViewCheckBoxColumn.Name = "visibleDataGridViewCheckBoxColumn";
			// 
			// modelDataGridViewTextBoxColumn
			// 
			this.modelDataGridViewTextBoxColumn.DataPropertyName = "Model";
			this.modelDataGridViewTextBoxColumn.HeaderText = "Model";
			this.modelDataGridViewTextBoxColumn.Name = "modelDataGridViewTextBoxColumn";
			// 
			// materialDataGridViewTextBoxColumn
			// 
			this.materialDataGridViewTextBoxColumn.DataPropertyName = "Material";
			this.materialDataGridViewTextBoxColumn.HeaderText = "Material";
			this.materialDataGridViewTextBoxColumn.Name = "materialDataGridViewTextBoxColumn";
			// 
			// thicknessDataGridViewTextBoxColumn
			// 
			this.thicknessDataGridViewTextBoxColumn.DataPropertyName = "Thickness";
			this.thicknessDataGridViewTextBoxColumn.HeaderText = "Thickness";
			this.thicknessDataGridViewTextBoxColumn.Name = "thicknessDataGridViewTextBoxColumn";
			// 
			// actionDataGridViewTextBoxColumn
			// 
			this.actionDataGridViewTextBoxColumn.DataPropertyName = "Action";
			this.actionDataGridViewTextBoxColumn.HeaderText = "Action";
			this.actionDataGridViewTextBoxColumn.Name = "actionDataGridViewTextBoxColumn";
			// 
			// powerDataGridViewTextBoxColumn
			// 
			this.powerDataGridViewTextBoxColumn.DataPropertyName = "Power";
			this.powerDataGridViewTextBoxColumn.HeaderText = "Power";
			this.powerDataGridViewTextBoxColumn.Name = "powerDataGridViewTextBoxColumn";
			// 
			// speedDataGridViewTextBoxColumn
			// 
			this.speedDataGridViewTextBoxColumn.DataPropertyName = "Speed";
			this.speedDataGridViewTextBoxColumn.HeaderText = "Speed";
			this.speedDataGridViewTextBoxColumn.Name = "speedDataGridViewTextBoxColumn";
			// 
			// cyclesDataGridViewTextBoxColumn
			// 
			this.cyclesDataGridViewTextBoxColumn.DataPropertyName = "Cycles";
			this.cyclesDataGridViewTextBoxColumn.HeaderText = "Cycles";
			this.cyclesDataGridViewTextBoxColumn.Name = "cyclesDataGridViewTextBoxColumn";
			// 
			// remarksDataGridViewTextBoxColumn
			// 
			this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
			this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
			this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
			// 
			// ColID
			// 
			this.ColID.DataPropertyName = "id";
			this.ColID.HeaderText = "id";
			this.ColID.Name = "ColID";
			this.ColID.ReadOnly = true;
			this.ColID.Visible = false;
			// 
			// ColVisible
			// 
			this.ColVisible.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.ColVisible.DataPropertyName = "Visible";
			this.ColVisible.HeaderText = "Visible";
			this.ColVisible.Name = "ColVisible";
			this.ColVisible.Width = 43;
			// 
			// ColModel
			// 
			this.ColModel.DataPropertyName = "Model";
			this.ColModel.HeaderText = "Model";
			this.ColModel.Name = "ColModel";
			// 
			// ColMaterial
			// 
			this.ColMaterial.DataPropertyName = "Material";
			this.ColMaterial.HeaderText = "Material";
			this.ColMaterial.Name = "ColMaterial";
			// 
			// ColAction
			// 
			this.ColAction.DataPropertyName = "Action";
			this.ColAction.HeaderText = "Action";
			this.ColAction.Name = "ColAction";
			// 
			// ColThick
			// 
			this.ColThick.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ColThick.DataPropertyName = "Thickness";
			this.ColThick.HeaderText = "Thickness";
			this.ColThick.Name = "ColThick";
			this.ColThick.Width = 81;
			// 
			// Power
			// 
			this.Power.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.Power.DataPropertyName = "Power";
			dataGridViewCellStyle1.Format = "0\\%";
			dataGridViewCellStyle1.NullValue = null;
			this.Power.DefaultCellStyle = dataGridViewCellStyle1;
			this.Power.HeaderText = "Power";
			this.Power.Name = "Power";
			this.Power.Width = 62;
			// 
			// ColSpeed
			// 
			this.ColSpeed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ColSpeed.DataPropertyName = "Speed";
			dataGridViewCellStyle2.Format = "0 mm/min";
			this.ColSpeed.DefaultCellStyle = dataGridViewCellStyle2;
			this.ColSpeed.HeaderText = "Speed";
			this.ColSpeed.Name = "ColSpeed";
			this.ColSpeed.Width = 63;
			// 
			// ColCycles
			// 
			this.ColCycles.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ColCycles.DataPropertyName = "Cycles";
			this.ColCycles.HeaderText = "Cycles";
			this.ColCycles.Name = "ColCycles";
			this.ColCycles.Width = 63;
			// 
			// ColRemarks
			// 
			this.ColRemarks.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ColRemarks.DataPropertyName = "Remarks";
			this.ColRemarks.HeaderText = "Remarks";
			this.ColRemarks.Name = "ColRemarks";
			// 
			// PSEditorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(891, 502);
			this.Controls.Add(this.DG);
			this.Controls.Add(this.statusStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "PSEditorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Laser & Material Database";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PSEditorForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.DG)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.source)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.materialDB)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.BindingSource source;
		private System.Windows.Forms.DataGridView DG;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripStatusLabel TbNewElement;
		private System.Windows.Forms.ToolStripStatusLabel BtnImport;
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn visibleDataGridViewCheckBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn modelDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn materialDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn thicknessDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn actionDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn powerDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn speedDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn cyclesDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
		private MaterialDB materialDB;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColID;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ColVisible;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColModel;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColMaterial;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColAction;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColThick;
		private System.Windows.Forms.DataGridViewTextBoxColumn Power;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColSpeed;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColCycles;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColRemarks;
	}
}