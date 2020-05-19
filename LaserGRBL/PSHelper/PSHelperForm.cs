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
	public partial class PSHelperForm : Form
	{
		MaterialDB.MaterialsDataTable data = GrblCore.MaterialDB.Materials;

		private PSHelperForm()
		{
			InitializeComponent();
			InitModels();
		}

		public static void CreateAndShowDialog()
		{
			using (PSHelperForm f = new PSHelperForm())
				f.ShowDialog();
		}


		private void InitModels()
		{
			string LastModelSelection = null;
			CbModel.BeginUpdate();
			CbModel.Items.Clear();
			CbModel.Items.AddRange(data.Models());
			if (LastModelSelection != null && CbModel.Items.Contains(LastModelSelection))
				CbModel.SelectedItem = LastModelSelection;
			else if (CbModel.Items.Count > 0)
				CbModel.SelectedIndex = 0;
			CbModel.Enabled = CbModel.Items.Count > 1;
			CbModel.EndUpdate();
		}



		private void CbModel_SelectedIndexChanged(object sender, EventArgs e)
		{
			string LastMaterialSelection = SelectedMaterial;
			CbMaterial.BeginUpdate();
			CbMaterial.Items.Clear();
			CbMaterial.Items.AddRange(data.Materials(SelectedModel));
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
			CbThickness.Items.AddRange(data.Thickness(SelectedModel, SelectedMaterial));
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
			CbAction.Items.AddRange(data.Actions(SelectedModel, SelectedMaterial, SelectedThickness));
			if (LastActionSelection != null && CbAction.Items.Contains(LastActionSelection))
				CbAction.SelectedItem = LastActionSelection;
			else if (CbAction.Items.Count > 0)
				CbAction.SelectedIndex = 0;
			CbAction.Enabled = CbAction.Items.Count > 1;
			CbAction.EndUpdate();
		}

		private void CbAction_SelectedIndexChanged(object sender, EventArgs e)
		{
			var res = data.GetResult(SelectedModel, SelectedMaterial, SelectedThickness, SelectedAction);
			TbPower.Text = $"{res.Power} %";
			TbSpeed.Text = $"{res.Speed} mm/min";
			TbPasses.Text = $"{res.Cycles} pass";
		}


	}
}
