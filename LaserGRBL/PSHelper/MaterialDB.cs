using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LaserGRBL.PSHelper
{


	partial class MaterialDB
	{
		partial class MaterialsDataTable
		{
			public override void EndInit()
			{
				base.EndInit();
				TableNewRow += Materials_TableNewRow;
			}

			private void Materials_TableNewRow(object sender, DataTableNewRowEventArgs e)
			{
				MaterialsRow target = e.Row as MaterialsRow;
				MaterialsRow last = Rows.Count > 0 ? Rows[Rows.Count - 1] as MaterialsRow : null;
				if (target != null)
				{
					target.id = Guid.NewGuid();

					if (last != null)
					{
						target.Model = last.Model;
						target.Material = last.Material;
					}
				}
			}

			private IEnumerable<MaterialsRow> EnabledRows { get => this.Where(x => x.Visible); }


			internal object[] Models()
			{
				return EnabledRows.Select(x => x.Model).Distinct().OrderBy(s => s).ToArray();
			}

			internal object[] Materials(string model)
			{
				return EnabledRows.Where(x => x.Model == model).Select(x => x.Material).Distinct().OrderBy(s => s).ToArray();
			}

			internal object[] Thickness(string model, string material)
			{
				return EnabledRows.Where(x => x.Model == model && x.Material == material).Select(x => x.Thickness).Distinct().OrderBy(s => s).ToArray();
			}


			internal object[] Actions(string model, string material, string thickness)
			{
				return EnabledRows.Where(x => x.Model == model && x.Material == material && x.Thickness == thickness).Select(x => x.Action).Distinct().OrderBy(s => s).ToArray();
			}

			internal MaterialsRow GetResult(string model, string material, string thickness, string action)
			{
				return EnabledRows.Where(x => x.Model == model && x.Material == material && x.Thickness == thickness && x.Action == action).First();
			}
		}

		private MaterialsDataTable FromServer = new MaterialsDataTable();

		public static MaterialDB Load()
		{
			MaterialDB rv = new MaterialDB();
			try
			{
				MaterialsDataTable user = new MaterialsDataTable() { Namespace = rv.Namespace };
				MaterialsDataTable server = new MaterialsDataTable() { Namespace = rv.Namespace };

				if (System.IO.File.Exists(UserFile))
					user.ReadXml(UserFile);

				if (System.IO.File.Exists(ServerFile))
					server.ReadXml(ServerFile);

				foreach (var row in user)
					rv.Materials.ImportRow(row);

				if (rv.Materials.Rows.Count == 0)
				{
					foreach (var row in server)
						rv.Materials.ImportRow(row);
				}
				else
				{
					foreach (var row in server)
						if (rv.Materials.FindByid(row.id) == null)
							rv.FromServer.ImportRow(row);               //nuove dal server
				}
			}
			catch { }

			rv.Materials.AcceptChanges();
			return rv;
		}

		internal void SaveChanges()
		{
			try
			{
				if (Materials.GetChanges() != null)
				{
					Materials.AcceptChanges();
					Materials.WriteXml(UserFile);
				}
			}
			catch { }
		}

		internal int GetNewCount()
		{
			int count = 0;
			foreach (var row in FromServer)
				if (Materials.FindByid(row.id) == null)
					count++;
			return count;
		}

		internal void ImportServer()
		{
			try
			{
				foreach (var row in FromServer)
					if (Materials.FindByid(row.id) == null)
						Materials.ImportRow(row);               //nuove dal server
			}
			catch { }
		}


		private static string UserFile { get => System.IO.Path.Combine(LaserGRBL.GrblCore.DataPath, "UserMaterials.psh"); }
		private static string ServerFile { get => System.IO.Path.Combine(LaserGRBL.GrblCore.ExePath, "StandardMaterials.psh"); }
	}
}
