using System;
using System.Collections.Generic;
using System.Data;

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

			internal object[] Models()
			{
				List<string> rv = new List<string>();

				foreach (var record in this)
					if (!rv.Contains(record.Model))
						rv.Add(record.Model);
				rv.Sort();
				return rv.ToArray();
			}

			internal object[] Materials(string model)
			{
				List<string> rv = new List<string>();

				foreach (var record in this)
					if (record.Model == model && !rv.Contains(record.Material))
						rv.Add(record.Material);
				rv.Sort();
				return rv.ToArray();
			}

			internal object[] Thickness(string model, string material)
			{
				List<string> rv = new List<string>();

				foreach (var record in this)
					if (record.Model == model && record.Material == material && !rv.Contains(record.Thickness))
						rv.Add(record.Thickness);

				rv.Sort();
				return rv.ToArray();
			}


			internal object[] Actions(string model, string material, string thickness)
			{
				List<string> rv = new List<string>();

				foreach (var record in this)
					if (record.Model == model && record.Material == material && record.Thickness == thickness && !rv.Contains(record.Action))
						rv.Add(record.Action);

				rv.Sort();
				return rv.ToArray();
			}

			internal MaterialsRow GetResult(string model, string material, string thickness, string action)
			{
				foreach (var record in this)
				{
					if (record.Model == model && record.Material == material && record.Thickness == thickness && record.Action == action)
						return record;
				}
				return null;
			}
		}

		private MaterialsDataTable FromServer = new MaterialsDataTable();

		public static MaterialDB Load()
		{
			MaterialDB rv = new MaterialDB();
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

			rv.Materials.AcceptChanges();
			return rv;
		}

		internal void SaveChanges()
		{
			if (Materials.GetChanges() != null)
			{
				Materials.AcceptChanges();
				Materials.WriteXml(UserFile);
			}
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
			foreach (var row in FromServer)
				if (Materials.FindByid(row.id) == null)
					Materials.ImportRow(row);               //nuove dal server
		}


		private static string UserFile { get => System.IO.Path.Combine(LaserGRBL.GrblCore.DataPath, "UserMaterials.psh"); }
		private static string ServerFile { get => System.IO.Path.Combine(LaserGRBL.GrblCore.ExePath, "StandardMaterials.psh"); }
	}
}
