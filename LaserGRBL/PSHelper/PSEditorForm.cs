using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL.PSHelper
{
	public partial class PSEditorForm : Form
	{
		public PSEditorForm()
		{
			InitializeComponent();
			source.DataSource = GrblCore.MaterialDB.Materials;
			TbNewElement.Text = GrblCore.MaterialDB.GetNewCount().ToString();
		}

		public static void CreateAndShowDialog()
		{
			using (PSEditorForm f = new PSEditorForm())
				f.ShowDialog();
		}

		private void PSEditorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			GrblCore.MaterialDB.SaveChanges();
		}

		private void BtnImport_Click(object sender, EventArgs e)
		{
			GrblCore.MaterialDB.ImportServer();
			TbNewElement.Text = GrblCore.MaterialDB.GetNewCount().ToString();
		}

		private void DG_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			if (e.Exception != null)
			{
				e.Cancel = MessageBox.Show(e.Exception.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) != DialogResult.Cancel;
				e.ThrowException = false;
			}
		}

		private void DG_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
		{
			//DataGridViewRow dgRow = DG.Rows[e.RowIndex];

			//if ((dgRow.Cells["yourColumnName"].Value == null) ||
			//	(dgRow.Cells["yourColumnName"].Value.ToString().Length == 0))
			//{
			//	// Set both the row and cell error text at the same time.
			//	dgRow.ErrorText = dgRow.Cells["dgTxtColTest List"].ErrorText =
			//		"You must enter a value in the " + yourColumnName + " column."
		

			//	e.Cancel = true;
			//}
		}

		private void DG_RowValidated(object sender, DataGridViewCellEventArgs e)
		{
			//// Clear errors from row header and cells in the row
			//DataGridViewRow row = DG.Rows[e.RowIndex];
			//row.ErrorText = ""; // Clear the row header error text

			//// Clear all error texts from the row
			//foreach (DataGridViewCell cell in row.Cells)
			//	cell.ErrorText = ""; // Clear each cell in the row as now row is valid
		}

		private void DG_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			if (DG.Columns[e.ColumnIndex] == ColCycles) //tra 1 e 100
			{
				int val = Convert.ToInt32(e.FormattedValue);
				if (val < 1) DG[e.ColumnIndex, e.RowIndex].ErrorText = "Please enter positive value";
			}
		}

	}
}
