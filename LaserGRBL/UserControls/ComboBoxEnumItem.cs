using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL.UserControls
{
	public class ComboBox<T> : System.Windows.Forms.ComboBox
	{
		public void AddItem(T item)
		{ Items.Add(new ComboBoxEnumItem<T>(item)); }

		public new T SelectedItem
		{
			get {return base.SelectedItem != null ? ((ComboBoxEnumItem<T>)base.SelectedItem).Value : default(T);}
			set { base.SelectedItem = new ComboBoxEnumItem<T>(value); }
		}

		public void Clear()
		{ Items.Clear(); }

		private class ComboBoxEnumItem<E>
		{
			private E mValue;

			public ComboBoxEnumItem(E value)
			{ mValue = value; }

			public E Value
			{ get { return mValue; } }

			public override string ToString()
			{ return GrblCore.TranslateEnum(mValue as Enum); }

			public override bool Equals(object obj)
			{
				if (obj is ComboBoxEnumItem<E>)
					return object.Equals((obj as ComboBoxEnumItem<E>).mValue, mValue);
				else
					return false;
			}

			public override int GetHashCode()
			{return mValue.GetHashCode();}
	
		}
	}


}
