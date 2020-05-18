using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL.PSHelper
{
	public partial class PSHelperControl : UserControl
	{
		PSFile data = new PSFile();

		public PSHelperControl()
		{
			InitializeComponent();
			InitModels();
		}

		private void InitModels()
		{
			string LastModelSelection = null;
			CbModel.BeginUpdate();
			CbModel.Items.Clear();
			CbModel.Items.AddRange(data.Models.ToArray());
			if (LastModelSelection != null && CbModel.Items.Contains(LastModelSelection))
				CbModel.SelectedItem = LastModelSelection;
			else if (CbModel.Items.Count > 0)
				CbModel.SelectedIndex = 0;
			CbModel.Enabled = CbModel.Items.Count > 1;
			CbModel.EndUpdate();
		}

		private class PSFile : CSV.CsvList
		{
			public struct Result
			{
				public int power;
				public int speed;
				public int cycles;
				public string remark;
			}

			public PSFile() : base("lpsm.csv", 8)
			{

			}

			public List<string> Models
			{
				get
				{
					List<string> rv = new List<string>();

					foreach (List<string> row in mD)
						if (!rv.Contains(row[0]))
							rv.Add(row[0]);

					return rv;
				}
			}

			internal List<string> Materials(string model)
			{
				List<string> rv = new List<string>();

				foreach (List<string> row in mD)
					if (row[0] == model && !rv.Contains(row[1]))
						rv.Add(row[1]);

				return rv;
			}

			internal List<string> Thickness(string model, string material)
			{
				List<string> rv = new List<string>();

				foreach (List<string> row in mD)
					if (row[0] == model && row[1] == material && !rv.Contains(row[2]))
						rv.Add(row[2]);

				return rv;
			}

			internal List<string> Actions(string model, string material, string thickness)
			{
				List<string> rv = new List<string>();

				foreach (List<string> row in mD)
					if (row[0] == model && row[1] == material && row[2] == thickness && !rv.Contains(row[3]))
						rv.Add(row[3]);

				return rv;
			}

			internal Result GetResult(string model, string material, string thickness, string action)
			{
				Result rv = new Result();

				foreach (List<string> row in mD)
				{
					if (row[0] == model && row[1] == material && row[2] == thickness && row[3] == action)
					{
						int.TryParse(row[4], out rv.power);
						int.TryParse(row[5], out rv.speed);
						int.TryParse(row[6], out rv.cycles);
						rv.remark = row[7];

						break;
					}
				}
				return rv;
			}
		}

		private void CbModel_SelectedIndexChanged(object sender, EventArgs e)
		{
			string LastMaterialSelection = SelectedMaterial;
			CbMaterial.BeginUpdate();
			CbMaterial.Items.Clear();
			CbMaterial.Items.AddRange(data.Materials(SelectedModel).ToArray());
			if (LastMaterialSelection != null && CbMaterial.Items.Contains(LastMaterialSelection))
				CbMaterial.SelectedItem = LastMaterialSelection;
			else if (CbMaterial.Items.Count > 0)
				CbMaterial.SelectedIndex = 0;
			CbMaterial.Enabled = CbMaterial.Items.Count > 1;
			CbMaterial.EndUpdate();
		}

		private string SelectedModel { get => CbModel.SelectedItem as string; }
		private string SelectedMaterial { get => CbMaterial.SelectedItem as string; }
		private string SelectedThickness { get => CbThickness.SelectedItem as string; }
		private string SelectedAction { get => CbAction.SelectedItem as string; }

		private void CbMaterial_SelectedIndexChanged(object sender, EventArgs e)
		{
			string LastThicknessSelection = SelectedThickness;
			CbThickness.BeginUpdate();
			CbThickness.Items.Clear();
			CbThickness.Items.AddRange(data.Thickness(SelectedModel, SelectedMaterial).ToArray());
			if (LastThicknessSelection != null && CbModel.Items.Contains(LastThicknessSelection))
				CbThickness.SelectedItem = LastThicknessSelection;
			else if (CbThickness.Items.Count > 0)
				CbThickness.SelectedIndex = 0;
			CbThickness.Enabled = CbThickness.Items.Count > 1;
			CbThickness.EndUpdate();
		}

		private void CbThickness_SelectedIndexChanged(object sender, EventArgs e)
		{
			string LastActionSelection = SelectedAction;
			CbAction.BeginUpdate();
			CbAction.Items.Clear();
			CbAction.Items.AddRange(data.Actions(SelectedModel, SelectedMaterial, SelectedThickness).ToArray());
			if (LastActionSelection != null && CbAction.Items.Contains(LastActionSelection))
				CbAction.SelectedItem = LastActionSelection;
			else if (CbAction.Items.Count > 0)
				CbAction.SelectedIndex = 0;
			CbAction.Enabled = CbAction.Items.Count > 1;
			CbAction.EndUpdate();
		}

		private void CbAction_SelectedIndexChanged(object sender, EventArgs e)
		{
			PSFile.Result res = data.GetResult(SelectedModel, SelectedMaterial, SelectedThickness, SelectedAction);
			TbPower.Text = $"{res.power} %";
			TbSpeed.Text = $"{res.speed} mm/min";
			TbPasses.Text = $"{res.cycles} pass";
		}
	}

	
}
