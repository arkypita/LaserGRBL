//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace LaserGRBL.UserControls
{
	/// <summary>
	/// Description of GrblTextBox.
	/// </summary>
	public partial class GrblTextBox : PlaceholderTextBox
	{
		public delegate void CommandEnteredDlg(string command);
		public event CommandEnteredDlg CommandEntered;
		
		List<string> mCommandHistory = new List<string>();
		int mHistoryIndex = 0;
		
		public GrblTextBox()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			if (keyData == Keys.Enter && Text.Trim().Length > 0)
			{
				string cmd = Text.Trim();
				mCommandHistory.Add(cmd);
				Clear();
				mHistoryIndex = mCommandHistory.Count;
				
				if (CommandEntered != null)
					CommandEntered(cmd);

				return true;
			}
			
			if (keyData == Keys.Up)
			{
				if (mHistoryIndex > 0)
					mHistoryIndex--;
				
				if (mHistoryIndex >= 0 && mHistoryIndex < mCommandHistory.Count)
				{
					Text = mCommandHistory[mHistoryIndex];
					SelectionStart = Text.Length;
					SelectionLength = 0;
				}
				
				return true;
			}

			if (keyData == Keys.Down)
			{
				if (mHistoryIndex < mCommandHistory.Count)
					mHistoryIndex++;
				
				if (mHistoryIndex >= 0 && mHistoryIndex < mCommandHistory.Count)
				{
					Text = mCommandHistory[mHistoryIndex];
					SelectionStart = Text.Length;
					SelectionLength = 0;
				}
				
				return true;
			}

			if (keyData == Keys.Escape)
			{
			    Clear();
				
			    return true;
			}

			return base.ProcessCmdKey(ref m, keyData);
		}

	}
}
