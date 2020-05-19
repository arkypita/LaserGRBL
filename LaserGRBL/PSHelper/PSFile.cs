using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LaserGRBL.PSHelper
{
	[Serializable]
	public class PSFile : BindingList<PSFile.Record>
	{
		[NonSerialized] PSFile nr = null;

		[Serializable]
		public class Record
		{
			public Guid id;

			private bool visible;

			private string model;
			private string material;
			private string thickness;
			private string action;

			private int power;
			private int speed;
			private int cycles;
			private string remark;

			public bool Visible { get => visible; set => visible = value; }
			public string Model { get => model; set => model = value; }
			public string Material { get => material; set => material = value; }
			public string Thickness { get => thickness; set => thickness = value; }
			public string Action { get => action; set => action = value; }
			public int Power { get => power; set => power = value; }
			public int Speed { get => speed; set => speed = value; }
			public int Cycles { get => cycles; set => cycles = value; }
			public string Remark { get => remark; set => remark = value; }
			

			public Record()
			{
				id = new Guid();
				visible = true;
				model = "";
				material = "";
				thickness = "";
				action = "";

				power = 100;
				speed = 1000;
				cycles = 1;
				remark = "";
			}

			public override bool Equals(object obj)
			{
				Record o = obj as Record;
				return o != null && o.id == id;
			}

			public override int GetHashCode()
			{
				return id.GetHashCode();
			}
		}

		public static PSFile Load()
		{
			PSFile rv = null;

			if (System.IO.File.Exists("UserDB.psh"))
				rv = Tools.Serializer.ObjFromFile("UserDB.psh") as PSFile;
			if (rv == null)
				rv = new PSFile();

			rv.LoadUpdatedDB();

			return rv;
		}

		private void LoadUpdatedDB()
		{
			if (System.IO.File.Exists("MaterialDB.psh"))
				nr = Tools.Serializer.ObjFromFile("MaterialDB.psh") as PSFile;
			if (nr == null)
				nr = new PSFile();
		}

		public List<string> Models
		{
			get
			{
				List<string> rv = new List<string>();

				foreach (Record record in this)
					if (!rv.Contains(record.Model))
						rv.Add(record.Model);

				return rv;
			}
		}

		internal List<string> Materials(string model)
		{
			List<string> rv = new List<string>();

			foreach (Record record in this)
				if (record.Model == model && !rv.Contains(record.Material))
					rv.Add(record.Material);

			return rv;
		}

		internal List<string> Thickness(string model, string material)
		{
			List<string> rv = new List<string>();

			foreach (Record record in this)
				if (record.Model == model && record.Material == material && !rv.Contains(record.Thickness))
					rv.Add(record.Thickness);

			return rv;
		}

		internal List<string> Actions(string model, string material, string thickness)
		{
			List<string> rv = new List<string>();

			foreach (Record record in this)
				if (record.Model == model && record.Material == material && record.Thickness == thickness && !rv.Contains(record.Action))
					rv.Add(record.Action);

			return rv;
		}

		internal Record GetResult(string model, string material, string thickness, string action)
		{
			foreach (Record record in this)
			{
				if (record.Model == model && record.Material == material && record.Thickness == thickness && record.Action == action)
					return record;
			}
			return null;
		}
	}
}
