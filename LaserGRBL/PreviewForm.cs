/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 05/12/2016
 * Time: 23:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LaserGRBL
{
	/// <summary>
	/// Description of PreviewForm.
	/// </summary>
	public partial class PreviewForm : UserControls.DockingManager.DockContent
	{
		GrblCom ComPort;
		GrblFile LoadedFile;
		
		public PreviewForm(GrblCom com, GrblFile file)
		{
			InitializeComponent();

			ComPort = com;
			LoadedFile = file;
			Preview.SetComProgram(com, file);
		}
		
		public void TimerUpdate()
		{
			Preview.TimerUpdate();
		}
	}
}
