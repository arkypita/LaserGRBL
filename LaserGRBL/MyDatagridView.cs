//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL
{
	class MyDatagridView : System.Windows.Forms.DataGridView
	{

		protected override void OnHandleCreated(EventArgs e)
		{
			// Touching the TopLeftHeaderCell here prevents
			// System.InvalidOperationException:
			// This operation cannot be performed while
			// an auto-filled column is being resized.

			// https://github.com/arkypita/LaserGRBL/issues/171
			// https://stackoverflow.com/questions/34344499/invalidoperationexception-this-operation-cannot-be-performed-while-an-auto-fill
			// https://connect.microsoft.com/VisualStudio/feedback/details/481029/this-operation-cannot-be-performed-while-an-auto-filled-column-is-being-resized

			System.Windows.Forms.DataGridViewHeaderCell cell = TopLeftHeaderCell;
			base.OnHandleCreated(e);
		}
	}
}
