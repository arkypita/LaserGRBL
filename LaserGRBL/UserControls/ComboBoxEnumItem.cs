//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

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
