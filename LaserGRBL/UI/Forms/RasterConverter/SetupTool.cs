using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL.UI.Forms.RasterConverter
{
	public partial class SetupTool : UserControl
	{
		public event EventHandler ValueChanged; 

		private GrblCore mCore;
		private LaserGRBL.Core.RasterToGcode.Configuration Configuration;
		private ToolConfig Instance;

		public SetupTool()
		{
			InitializeComponent();
		}

		public void Start(GrblCore core, LaserGRBL.Core.RasterToGcode.Configuration setup)
		{
			Configuration = setup;
			mCore = core;

			CbTools.SuspendLayout();
			foreach (Core.RasterToGcode.ConversionTool.AvailableTools tools in Enum.GetValues(typeof(Core.RasterToGcode.ConversionTool.AvailableTools)))
				CbTools.AddItem(tools);
			CbTools.SelectedIndex = 0;
			CbTools.ResumeLayout();
		}

		private void CbTools_SelectedIndexChanged(object sender, EventArgs e)
		{
			 ChangeTool((Core.RasterToGcode.ConversionTool.AvailableTools)CbTools.SelectedItem);
			 ChangeUI((Core.RasterToGcode.ConversionTool.AvailableTools)CbTools.SelectedItem);

			 Instance_ValueChanged(this, new EventArgs());
		}

		private void ChangeTool(Core.RasterToGcode.ConversionTool.AvailableTools selected)
		{
			if (Configuration != null)
			{
				if (selected == Core.RasterToGcode.ConversionTool.AvailableTools.Dithering)
					Configuration.SelectedTool = new Core.RasterToGcode.Dithering();
				if (selected == Core.RasterToGcode.ConversionTool.AvailableTools.Line2Line)
					Configuration.SelectedTool = new Core.RasterToGcode.LineToLine();
				if (selected == Core.RasterToGcode.ConversionTool.AvailableTools.Vectorize)
					Configuration.SelectedTool = new Core.RasterToGcode.Vectorization();
			}
		}

		private void ChangeUI(Core.RasterToGcode.ConversionTool.AvailableTools selected)
		{
			if (Configuration != null)
			{
				SuspendLayout();
				if (Instance != null)
				{
					Instance.ValueChanged -= Instance_ValueChanged;
					PNL.Controls.Remove(Instance);
					Instance.Dispose();
					Instance = null;
				}

				if (selected == Core.RasterToGcode.ConversionTool.AvailableTools.Dithering)
					Instance = new SetupDithering(mCore, (Core.RasterToGcode.Dithering)Configuration.SelectedTool);
				if (selected == Core.RasterToGcode.ConversionTool.AvailableTools.Line2Line)
					Instance = new SetupLineToLine(mCore, (Core.RasterToGcode.LineToLine)Configuration.SelectedTool);
				if (selected == Core.RasterToGcode.ConversionTool.AvailableTools.Vectorize)
					Instance = new SetupVectorization(mCore, (Core.RasterToGcode.Vectorization)Configuration.SelectedTool);

				if (Instance != null)
				{
					PNL.Controls.Add(Instance);
					Instance.Dock = DockStyle.Fill;
					Instance.BringToFront();
					Instance.ValueChanged += Instance_ValueChanged;
				}

				ResumeLayout();
			}
		}

		private void Instance_ValueChanged(object sender, EventArgs e)
		{
			if (ValueChanged != null)
				ValueChanged(sender, e);
		}

		private void CbLinePreview_CheckedChanged(object sender, EventArgs e)
		{Instance_ValueChanged(sender, e);}

		public bool LinePreview
		{ get { return CbLinePreview.Checked; } }
	}

	public class ToolConfig : UserControl
	{
		public event EventHandler ValueChanged;

		protected void Change()
		{
			if (ValueChanged != null)
				ValueChanged(this, new EventArgs());
		}
	}
}
