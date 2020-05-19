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
			this.DG = new System.Windows.Forms.DataGridView();
			this.ColID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ColModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColMaterial = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.source = new System.Windows.Forms.BindingSource(this.components);
			this.materialDB = new LaserGRBL.PSHelper.MaterialDB();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.TbNewElement = new System.Windows.Forms.ToolStripStatusLabel();
			this.BtnImport = new System.Windows.Forms.ToolStripStatusLabel();
			((System.ComponentModel.ISupportInitialize)(this.DG)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.source)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.materialDB)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// DG
			// 
			this.DG.AutoGenerateColumns = false;
			this.DG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DG.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColID,
            this.ColVisible,
            this.ColModel,
            this.ColMaterial,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9});
			this.DG.DataSource = this.source;
			this.DG.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DG.Location = new System.Drawing.Point(0, 0);
			this.DG.Name = "DG";
			this.DG.Size = new System.Drawing.Size(891, 480);
			this.DG.TabIndex = 1;
			// 
			// ColID
			// 
			this.ColID.DataPropertyName = "id";
			this.ColID.HeaderText = "id";
			this.ColID.Name = "ColID";
			this.ColID.ReadOnly = true;
			// 
			// ColVisible
			// 
			this.ColVisible.DataPropertyName = "Visible";
			this.ColVisible.HeaderText = "Visible";
			this.ColVisible.Name = "ColVisible";
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
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.DataPropertyName = "Thickness";
			this.dataGridViewTextBoxColumn4.HeaderText = "Thickness";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			// 
			// dataGridViewTextBoxColumn5
			// 
			this.dataGridViewTextBoxColumn5.DataPropertyName = "Action";
			this.dataGridViewTextBoxColumn5.HeaderText = "Action";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			// 
			// dataGridViewTextBoxColumn6
			// 
			this.dataGridViewTextBoxColumn6.DataPropertyName = "Power";
			this.dataGridViewTextBoxColumn6.HeaderText = "Power";
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			// 
			// dataGridViewTextBoxColumn7
			// 
			this.dataGridViewTextBoxColumn7.DataPropertyName = "Speed";
			this.dataGridViewTextBoxColumn7.HeaderText = "Speed";
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			// 
			// dataGridViewTextBoxColumn8
			// 
			this.dataGridViewTextBoxColumn8.DataPropertyName = "Cycles";
			this.dataGridViewTextBoxColumn8.HeaderText = "Cycles";
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			// 
			// dataGridViewTextBoxColumn9
			// 
			this.dataGridViewTextBoxColumn9.DataPropertyName = "Remarks";
			this.dataGridViewTextBoxColumn9.HeaderText = "Remarks";
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
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
			// PSEditorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(891, 502);
			this.Controls.Add(this.DG);
			this.Controls.Add(this.statusStrip1);
			this.Name = "PSEditorForm";
			this.Text = "PSEditorForm";
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

		private MaterialDB materialDB;
		private System.Windows.Forms.BindingSource source;
		private System.Windows.Forms.DataGridView DG;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColID;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ColVisible;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColModel;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColMaterial;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripStatusLabel TbNewElement;
		private System.Windows.Forms.ToolStripStatusLabel BtnImport;
	}
}