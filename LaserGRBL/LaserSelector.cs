using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL
{
	public partial class LaserSelector : Form
	{
		public LaserSelector()
		{
			InitializeComponent();

			CbLaser.DisplayMember = "Name";
			List<LaserLifeHandler.LaserLifeCounter> orig = LaserLifeHandler.GetListClone();
			string lastguid = Settings.GetObject<string>("Last laser used", null);
			LaserLifeHandler.LaserLifeCounter last = lastguid != null ? orig.FirstOrDefault(o => o.Guid == lastguid) : null;
			List<LaserLifeHandler.LaserLifeCounter> alive = orig.Where(o => o != last && o.DeathDate == null).OrderByDescending(o => o.MonitoringDate).ToList();
			List<LaserLifeHandler.LaserLifeCounter> death = orig.Where(o => o != last && o.DeathDate != null).OrderByDescending(o => o.MonitoringDate).ToList();
			if (last != null) CbLaser.Items.Add(last);
			CbLaser.Items.AddRange(alive.ToArray());
			CbLaser.Items.AddRange(death.ToArray());
			CbLaser.SelectedIndex = 0;
		}

		internal static string CreateAndShowDialog(Form parent)
		{
			using (LaserSelector sf = new LaserSelector())
			{
				if (sf.ShowDialog(parent) == DialogResult.OK)
					return (sf.CbLaser.SelectedItem as LaserLifeHandler.LaserLifeCounter)?.Guid;
				else
					return null;
			}
		}
	}
}
