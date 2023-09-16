using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LaserGRBL.PSHelper
{
	public partial class PSEditorForm : Form
	{
		public PSEditorForm()
		{
			InitializeComponent();
			source.DataSource = GrblCore.MaterialDB.Materials;
			//TbNewElement.Text = GrblCore.MaterialDB.GetNewCount().ToString();
		}

		public static void CreateAndShowDialog(Form parent)
		{
			using (PSEditorForm f = new PSEditorForm())
				f.ShowDialog(parent);
		}

		private void PSEditorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			GrblCore.MaterialDB.SaveChanges();
		}

		private void BtnImport_Click(object sender, EventArgs e)
		{
			GrblCore.MaterialDB.ImportServer();
			//TbNewElement.Text = GrblCore.MaterialDB.GetNewCount().ToString();
		}

		private void DG_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			if (e.Exception != null)
			{
				e.Cancel = MessageBox.Show(e.Exception.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) != DialogResult.Cancel;
				e.ThrowException = false;
			}
		}

		private void DG_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
		{
			if (e.ColumnIndex == ColPower.Index || e.ColumnIndex == ColSpeed.Index)
			{  
				try
				{
					e.Value = Int32.Parse(StripNonNumber(e.Value.ToString()));
					e.ParsingApplied = true;
				}
				catch (FormatException)
				{
					e.ParsingApplied = false;
				}
			}
			else
			{   // parsing any other column, let the system parse it
				e.ParsingApplied = false;
			}
		}

		private string StripNonNumber(string input)
		{
			return new string(input.Where(c => char.IsDigit(c)).ToArray());
		}

		private void Paste()
		{
			try
			{
				DataObject o = (DataObject)Clipboard.GetDataObject();
				if (o.GetDataPresent(DataFormats.Text))
				{
					string[] rows = Regex.Split(o.GetData(DataFormats.Text).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");

					foreach (string row in rows)
					{
						try
						{
							string[] cells = row.Split(new char[] { '\t' });
							GrblCore.MaterialDB.Materials.AddMaterialsRow(Guid.NewGuid(), true, cells[0], cells[1], cells[3], cells[2], GetInt(cells[4]), GetInt(cells[5]), GetInt(cells[6]), cells[7]);
						}
						catch (Exception ex) { MessageBox.Show($"Error importing line: [{row}]\r\n{ex.Message}"); }
					}
				}
			}
			catch (Exception ex) { }
		}

		private Int32 GetInt(string v)
		{
			Int32 rv = 0;
			Int32.TryParse(StripNonNumber(v), out rv);
			return rv;
		}

		private void DG_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == System.Windows.Forms.Keys.V && e.Control)
				Paste();
		}
	}
}
