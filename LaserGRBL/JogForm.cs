using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL
{
	public partial class JogForm : UserControls.DockingManager.DockContent
	{
		GrblCom ComPort;

		public JogForm(GrblCom com)
		{
			InitializeComponent();
			ComPort = com;

			TimerUpdate();
		}

		public void TimerUpdate()
		{
			SuspendLayout();
			foreach (UserControls.ImageButton btn in tlp.Controls)
				btn.Enabled = ComPort.IsOpen && ComPort.MachineStatus != GrblCom.MacStatus.Disconnected;
			ResumeLayout();
		}
	}
}
