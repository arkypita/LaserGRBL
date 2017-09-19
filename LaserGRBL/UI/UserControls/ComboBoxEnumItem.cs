using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL.UserControls
{
	public class EnumComboBox : System.Windows.Forms.ComboBox
	{

		public void AddItem(Enum item)
		{ Items.Add(new ComboBoxEnumItem(item)); }

		public new Enum SelectedItem
		{
			get {return base.SelectedItem != null ? ((ComboBoxEnumItem)base.SelectedItem).Value : default(Enum) ;}
			set { base.SelectedItem = new ComboBoxEnumItem(value); }
		}

		public void Clear()
		{Items.Clear(); }

		private class ComboBoxEnumItem
		{
			private Enum mValue;

			public ComboBoxEnumItem(Enum value)
			{ mValue = value; }

			public Enum Value
			{ get { return mValue; } }

			public override string ToString()
			{ return GrblCore.TranslateEnum(mValue); }

			public override bool Equals(object obj)
			{
				if (obj is ComboBoxEnumItem)
					return object.Equals((obj as ComboBoxEnumItem).mValue, mValue);
				else
					return false;
			}

			public override int GetHashCode()
			{return mValue.GetHashCode();}
	
		}
	}


}
