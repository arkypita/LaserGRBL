using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Core;

namespace SharpGL.Controls
{
	/// <summary>
	/// Summary description for VertexControl.
	/// </summary>
	public class VertexControl : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public VertexControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			UpdateVertex();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBoxX = new System.Windows.Forms.TextBox();
			this.textBoxY = new System.Windows.Forms.TextBox();
			this.textBoxZ = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// textBoxX
			// 
			this.textBoxX.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(192)), ((System.Byte)(192)));
			this.textBoxX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxX.ForeColor = System.Drawing.Color.Red;
			this.textBoxX.Location = new System.Drawing.Point(0, 8);
			this.textBoxX.Name = "textBoxX";
			this.textBoxX.Size = new System.Drawing.Size(40, 20);
			this.textBoxX.TabIndex = 3;
			this.textBoxX.Text = "";
			this.textBoxX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.textBoxX.TextChanged += new System.EventHandler(this.textBoxX_TextChanged);
			// 
			// textBoxY
			// 
			this.textBoxY.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.textBoxY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxY.ForeColor = System.Drawing.Color.Green;
			this.textBoxY.Location = new System.Drawing.Point(48, 8);
			this.textBoxY.Name = "textBoxY";
			this.textBoxY.Size = new System.Drawing.Size(40, 20);
			this.textBoxY.TabIndex = 4;
			this.textBoxY.Text = "";
			this.textBoxY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.textBoxY.TextChanged += new System.EventHandler(this.textBoxY_TextChanged);
			// 
			// textBoxZ
			// 
			this.textBoxZ.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.textBoxZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxZ.ForeColor = System.Drawing.Color.Blue;
			this.textBoxZ.Location = new System.Drawing.Point(96, 8);
			this.textBoxZ.Name = "textBoxZ";
			this.textBoxZ.Size = new System.Drawing.Size(40, 20);
			this.textBoxZ.TabIndex = 5;
			this.textBoxZ.Text = "";
			this.textBoxZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.textBoxZ.TextChanged += new System.EventHandler(this.textBoxZ_TextChanged);
			// 
			// VertexControl
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.textBoxZ,
																		  this.textBoxY,
																		  this.textBoxX});
			this.Name = "VertexControl";
			this.Size = new System.Drawing.Size(136, 32);
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.TextBox textBoxX;
		private System.Windows.Forms.TextBox textBoxY;
		private System.Windows.Forms.TextBox textBoxZ;

		protected Vertex vertex;

		protected void DoVertexChanged()
		{
			if(VertexChanged != null)
				VertexChanged(this, new System.EventArgs());
		}

		public void UpdateVertex()
		{
            textBoxX.Enabled = true;
            textBoxY.Enabled = true;
            textBoxZ.Enabled = true;
            textBoxX.Text = vertex.X.ToString();
            textBoxY.Text = vertex.Y.ToString();
            textBoxZ.Text = vertex.Z.ToString();
		}

		private void textBoxX_TextChanged(object sender, System.EventArgs e)
		{
			try
			{				
				vertex.X = float.Parse(textBoxX.Text); // Culture-dependant parsing
			}
			catch(System.FormatException)
			{
				MessageBox.Show(this, "Please enter a number.");
			}
			DoVertexChanged();
		}

		private void textBoxY_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				vertex.Y = float.Parse(textBoxY.Text);// Culture-dependant parsing
			}
			catch(System.FormatException)
			{
				MessageBox.Show(this, "Please enter a number.");
			}
			DoVertexChanged();
		}

		private void textBoxZ_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				vertex.Z = float.Parse(textBoxZ.Text);// Culture-dependant parsing
			}
			catch(System.FormatException)
			{
				MessageBox.Show(this, "Please enter a number.");
			}
			DoVertexChanged();
		}

		[Description("This is the vertex associated with the control."), Category("Vertex Control")]
		public Vertex Vertex
		{
			get {return vertex;}
			set 
			{
				vertex = value;
				UpdateVertex();
			}
		}

		[Description("Called whenever the vertex has changed."), Category("Behaviour")]
		public event EventHandler VertexChanged;
	}
}
