using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Collections;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;

namespace SharpGL.SceneGraph.Core
{
	/// <summary>
	/// This class represent's a grid of points, just like you'd get on a NURBS
	/// surface, or a patch.
	/// </summary>
	[Serializable()]
	public class VertexGrid
	{
		public virtual void CreateGrid(int x, int y)
		{
			//	Clear the current array.
			vertices.Clear();
			
			//	Add a new set of control points.
			for(int yvals = 0; yvals < y; yvals++)
			{
				for(int xvals = 0; xvals < x; xvals++)
                    vertices.Add( new Vertex(xvals, 1.0f, yvals));
			}

			this.x = x;
			this.y = y;
		}

		/// <summary>
		/// Use this to draw the vertex grid.
		/// </summary>
		/// <param name="gl">OpenGL object.</param>
		/// <param name="points">Draw each individual vertex (with selection names).</param>
		/// <param name="lines">Draw the lines connecting the points.</param>
		public virtual void Draw(OpenGL gl, bool points, bool lines)
		{
			//	Save the attributes.
            gl.PushAttrib(OpenGL.GL_ALL_ATTRIB_BITS);
            gl.Disable(OpenGL.GL_LIGHTING);
			gl.Color(1, 0, 0, 1);

			if(points)
			{
                int name = 0;

				gl.PointSize(5);

				//	Add a new name (the vertex name).
				gl.PushName(0);

				foreach(Vertex v in vertices)
				{
					//	Set the name, draw the vertex.
					gl.LoadName((uint)name++);
					//todo draw vertex
                    //((IInteractable)v).DrawPick(gl);
				}

				//	Pop the name.
				gl.PopName();
			}

			if(lines)
			{
				//	Draw lines along each row, then along each column.
                gl.DepthFunc(OpenGL.GL_ALWAYS);

				gl.LineWidth(1);
                gl.Disable(OpenGL.GL_LINE_SMOOTH);

				for(int col=0; col < y; col++)
				{
					for(int row=0; row < x; row++)
					{
						//	Create vertex indicies.
						int nTopLeft = (col * x) + row;
						int nBottomLeft = ((col + 1) * x) + row;

                        gl.Begin(OpenGL.GL_LINES);
						if(row < (x-1))
						{
							gl.Vertex(vertices[nTopLeft]);
							gl.Vertex(vertices[nTopLeft + 1]);
						}
						if(col < (y-1))
						{
							gl.Vertex(vertices[nTopLeft]);
							gl.Vertex(vertices[nBottomLeft]);
						}
						gl.End();
					}
				}
                gl.DepthFunc(OpenGL.GL_LESS);
			}

			gl.PopAttrib();
		}

		/// <summary>
		/// This function returns all of the control points as a float array, which 
		/// is in the format [0] = vertex 1 X, [1] = vertex 1 Y, [2] = vertex 1 Z, 
		/// [3] = vertex 2 X etc etc... This array is suitable for OpenGL functions
		/// for evaluators and NURBS.
		/// </summary>
		/// <returns>An array of floats.</returns>
		public float[] ToFloatArray()
		{
			float[] floats = new float[Vertices.Count * 3];
			int index = 0;
			foreach(Vertex pt in Vertices)
			{
				floats[index++] = pt.X;
				floats[index++] = pt.Y;
				floats[index++] = pt.Z;
			}

			return floats;
		}

        protected List<Vertex> vertices = new List<Vertex>();
		protected int x = 0;
		protected int y = 0;

        public List<Vertex> Vertices
		{
			get {return vertices;}
			set {vertices = value;}
		}
		public int Width
		{
			get {return x;}
			set {x = value;}
		}
		public int Height
		{
			get {return x;}
			set {y = value;}
		}
	}
}