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
