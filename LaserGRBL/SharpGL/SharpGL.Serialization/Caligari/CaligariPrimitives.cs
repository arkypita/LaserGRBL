using System;
using System.IO;
using System.Collections.Generic;

using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Collections;
using SharpGL.SceneGraph.Core;

namespace SharpGL.Serialization.Caligari
{
	internal class CaligariString
	{
		public virtual void Read(BinaryReader stream)
		{
			short length = stream.ReadInt16();
			name = new string(stream.ReadChars(length));
		}

		public string name;
	}

	internal class CaligariName
	{
		public virtual void Read(BinaryReader stream)
		{
			dupecount = stream.ReadInt16();
			CaligariString str = new CaligariString();
			str.Read(stream);
			name = str.name;
		
		}
				
		public short dupecount;
		public string name;
	}

	internal class CaligariAxies
	{
		public virtual void Read(BinaryReader stream)
		{
			centre = new Vertex(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
			directionX = new Vertex(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
			directionY = new Vertex(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
			directionZ = new Vertex(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());

			//	The X axis in SharpGL is always (1, 0, 0).

			xAxis = new Vertex(1, 0, 0);
			float angleX = xAxis.ScalarProduct(directionX);
			yAxis = new Vertex(0, 1, 0);
			float angleY = yAxis.ScalarProduct(directionY);
			zAxis = new Vertex(0, 0, 1);
			float angleZ = zAxis.ScalarProduct(directionZ);
			angleX = (float)System.Math.Asin(angleX);
			angleY = (float)System.Math.Asin(angleY);
			angleZ = (float)System.Math.Asin(angleZ);

			angleX = (180 * angleX) / (float)Math.PI;
			angleY = (180 * angleY) / (float)Math.PI;
			angleZ = (180 * angleZ) / (float)Math.PI;

			rotate = new Vertex(-angleX, -angleY, -angleZ);

			xAxis = xAxisGL = directionX;
			yAxis = zAxisGL = directionY;
			zAxis = yAxisGL = directionZ;
			xAxisGL.X = -xAxisGL.X;

		}
		public Vertex centre;
		public Vertex rotate;

		public Vertex directionX;
		public Vertex directionY;
		public Vertex directionZ;

		public Vertex xAxis;
		public Vertex yAxis;
		public Vertex zAxis;
		public Vertex xAxisGL;
		public Vertex yAxisGL;
		public Vertex zAxisGL;
	}

	internal class CaligariPosition
	{
		public virtual void Read(BinaryReader stream)
		{
			row1 = new float[] {stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle()};
			row2 = new float[] {stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle()};
			row3 = new float[] {stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle()};
		
			//	Convert it to our own matrix.
            matrix[0,0] = row1[0]; matrix[0,1] = row1[1]; matrix[0,2] = row1[2];
            matrix[1,0] = row2[0]; matrix[1,1] = row2[1]; matrix[1,2] = row2[2];
            matrix[2,0] = row3[0]; matrix[2,1] = row3[1]; matrix[2,2] = row3[2];    

			matrix.Transpose();
		}

		public float[] row1;
		public float[] row2;
		public float[] row3;

		
		public Matrix matrix = new Matrix(3, 3);
	}
			
	
}