/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 03/12/2016
 * Time: 22:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
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

			return base.ProcessCmdKey(ref m, keyData);
		}

	}
}
