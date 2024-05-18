using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SharpGL.SceneGraph.Core;

namespace SharpGL.SceneGraph.Primitives
{
	/// <summary>
	/// A simple cube polygon.
	/// </summary>
	[Serializable]
	public class Cube : Polygon
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Polygon"/> class.
		/// </summary>
		public Cube() 
		{
            //  Set the name.
            Name = "Cube";

            //  Create the cube geometry.
            CreateCubeGeometry();
		}

		/// <summary>
		/// This function makes a simple cube shape.
		/// </summary>
		private void CreateCubeGeometry()
		{
			UVs.Add(new UV(0, 0));
			UVs.Add(new UV(0, 1));
			UVs.Add(new UV(1, 1));
			UVs.Add(new UV(1, 0));
	
			//	Add the vertices.
			Vertices.Add(new Vertex(-1, -1, -1));
			Vertices.Add(new Vertex( 1, -1, -1));
			Vertices.Add(new Vertex( 1, -1,  1));
			Vertices.Add(new Vertex(-1, -1,  1));
			Vertices.Add(new Vertex(-1,  1, -1));
			Vertices.Add(new Vertex( 1,  1, -1));
			Vertices.Add(new Vertex( 1,  1,  1));
			Vertices.Add(new Vertex(-1,  1,  1));

			//	Add the faces.
			Face face = new Face();	//	bottom
			face.Indices.Add(new Index(1, 0));
			face.Indices.Add(new Index(2, 1));
			face.Indices.Add(new Index(3, 2));
			face.Indices.Add(new Index(0, 3));
			Faces.Add(face);

			face = new Face();		//	top
			face.Indices.Add(new Index(7, 0));
			face.Indices.Add(new Index(6, 1));
			face.Indices.Add(new Index(5, 2));
			face.Indices.Add(new Index(4, 3));
            Faces.Add(face);

			face = new Face();		//	right
			face.Indices.Add(new Index(5, 0));
			face.Indices.Add(new Index(6, 1));
			face.Indices.Add(new Index(2, 2));
			face.Indices.Add(new Index(1, 3));
            Faces.Add(face);
	
			face = new Face();		//	left
			face.Indices.Add(new Index(7, 0));
			face.Indices.Add(new Index(4, 1));
			face.Indices.Add(new Index(0, 2));
			face.Indices.Add(new Index(3, 3));
            Faces.Add(face);

			face = new Face();		// front
			face.Indices.Add(new Index(4, 0));
			face.Indices.Add(new Index(5, 1));
			face.Indices.Add(new Index(1, 2));
			face.Indices.Add(new Index(0, 3));
            Faces.Add(face);

			face = new Face();		//	back
			face.Indices.Add(new Index(6, 0));
			face.Indices.Add(new Index(7, 1));
			face.Indices.Add(new Index(3, 2));
			face.Indices.Add(new Index(2, 3));
            Faces.Add(face);
		}
    }
}
