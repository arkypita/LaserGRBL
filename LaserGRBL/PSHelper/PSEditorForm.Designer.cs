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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PSEditorForm));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.DG = new System.Windows.Forms.DataGridView();
			this.ColID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ColModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColMaterial = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColThick = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColPower = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColCycles = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.ColPower,
            this.ColSpeed,
            this.ColCycles,
            this.ColRemarks});
			this.DG.DataSource = this.source;
			resources.ApplyResources(this.DG, "DG");
			this.DG.Name = "DG";
			this.DG.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DG_CellParsing);
			this.DG.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DG_DataError);
			this.DG.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DG_KeyDown);
			// 
			// ColID
			// 
			this.ColID.DataPropertyName = "id";
			resources.ApplyResources(this.ColID, "ColID");
			this.ColID.Name = "ColID";
			this.ColID.ReadOnly = true;
			// 
			// ColVisible
			// 
			this.ColVisible.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.ColVisible.DataPropertyName = "Visible";
			resources.ApplyResources(this.ColVisible, "ColVisible");
			this.ColVisible.Name = "ColVisible";
			// 
			// ColModel
			// 
			this.ColModel.DataPropertyName = "Model";
			resources.ApplyResources(this.ColModel, "ColModel");
			this.ColModel.Name = "ColModel";
			// 
			// ColMaterial
			// 
			this.ColMaterial.DataPropertyName = "Material";
			resources.ApplyResources(this.ColMaterial, "ColMaterial");
			this.ColMaterial.Name = "ColMaterial";
			// 
			// ColAction
			// 
			this.ColAction.DataPropertyName = "Action";
			resources.ApplyResources(this.ColAction, "ColAction");
			this.ColAction.Name = "ColAction";
			// 
			// ColThick
			// 
			this.ColThick.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ColThick.DataPropertyName = "Thickness";
			resources.ApplyResources(this.ColThick, "ColThick");
			this.ColThick.Name = "ColThick";
			// 
			// ColPower
			// 
			this.ColPower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ColPower.DataPropertyName = "Power";
			dataGridViewCellStyle1.Format = "0\\%";
			dataGridViewCellStyle1.NullValue = null;
			this.ColPower.DefaultCellStyle = dataGridViewCellStyle1;
			resources.ApplyResources(this.ColPower, "ColPower");
			this.ColPower.Name = "ColPower";
			// 
			// ColSpeed
			// 
			this.ColSpeed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ColSpeed.DataPropertyName = "Speed";
			dataGridViewCellStyle2.Format = "0 mm/min";
			this.ColSpeed.DefaultCellStyle = dataGridViewCellStyle2;
			resources.ApplyResources(this.ColSpeed, "ColSpeed");
			this.ColSpeed.Name = "ColSpeed";
			// 
			// ColCycles
			// 
			this.ColCycles.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ColCycles.DataPropertyName = "Cycles";
			resources.ApplyResources(this.ColCycles, "ColCycles");
			this.ColCycles.Name = "ColCycles";
			// 
			// ColRemarks
			// 
			this.ColRemarks.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ColRemarks.DataPropertyName = "Remarks";
			resources.ApplyResources(this.ColRemarks, "ColRemarks");
			this.ColRemarks.Name = "ColRemarks";
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
			resources.ApplyResources(this.statusStrip1, "statusStrip1");
			this.statusStrip1.Name = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
			// 
			// TbNewElement
			// 
			this.TbNewElement.Name = "TbNewElement";
			resources.ApplyResources(this.TbNewElement, "TbNewElement");
			// 
			// BtnImport
			// 
			this.BtnImport.IsLink = true;
			this.BtnImport.Name = "BtnImport";
			resources.ApplyResources(this.BtnImport, "BtnImport");
			this.BtnImport.Click += new System.EventHandler(this.BtnImport_Click);
			// 
			// PSEditorForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.DG);
			this.Controls.Add(this.statusStrip1);
			this.MinimizeBox = false;
			this.Name = "PSEditorForm";
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
		private MaterialDB materialDB;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColID;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ColVisible;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColModel;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColMaterial;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColAction;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColThick;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColSpeed;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColCycles;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColRemarks;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColPower;
	}
}