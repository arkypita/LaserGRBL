using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Core;
using Index = SharpGL.SceneGraph.Index;

namespace SharpGL.Serialization.Caligari
{
    internal class PolygonChunk : CaligariChunk
    {
        /// <summary>
        /// This function reads a polygon.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override object ReadData(BinaryReader reader)
        {
            //	Create a polygon.
            Polygon poly = new Polygon();

            //	Read a name chunk.
            CaligariName name = new CaligariName();
            name.Read(reader);
            poly.Name = name.name;

            //	Read the local axies.
            CaligariAxies axies = new CaligariAxies();
            axies.Read(reader);
            poly.Transformation.TranslateX = axies.centre.X;
            poly.Transformation.TranslateY = axies.centre.Y;
            poly.Transformation.TranslateZ = axies.centre.Z;
            //	poly.Rotate = axies.rotate;

            //	Read the position matrix.
            CaligariPosition pos = new CaligariPosition();
            pos.Read(reader);

            //	Read number of verticies.
            int verticesCount = reader.ReadInt32();

            //	Get them all 
            for (int i = 0; i < verticesCount; i++)
            {
                //	Read a vertex.
                Vertex vertex = new Vertex(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                //	Multiply it by the position matrix.
                vertex = vertex * pos.matrix;

                //	Add it.
                poly.Vertices.Add(vertex);
            }

            //	Read UV count.
            int uvsCount = reader.ReadInt32();

            //	Read all of the UVs
            for (int i = 0; i < uvsCount; i++)
                poly.UVs.Add(new UV(reader.ReadInt32(), reader.ReadInt32()));

            //	Read faces count.
            int faces = reader.ReadInt32();

            //	Read each face.
            for (int f = 0; f < faces; f++)
            {
                Face face = new Face();
                poly.Faces.Add(face);

                //	Read face type flags.
                byte flags = reader.ReadByte();

                //	Read vertex count
                short verticesInFace = reader.ReadInt16();

                //	Do we read a material number?
                if ((flags & 0x08) == 0)	//	is it a 'hole' face?
                    reader.ReadInt16();

                //	Now read the indices, a vertex index and a uv index.
                for (short j = 0; j < verticesInFace; j++)
                    face.Indices.Add(new Index(reader.ReadInt32(), reader.ReadInt32()));
            }

            //	Any extra stuff?
            if (header.minorVersion > 4)
            {
                //	read flags.
                reader.ReadChars(4);

                if ((header.minorVersion > 5) && (header.minorVersion < 8))
                    reader.ReadChars(2);
            }

            //	Now that we've loaded the polygon, we triangulate it.
            poly.Triangulate();

            //	Finally, we update normals.
            poly.Validate(true);

            return poly;
        }
    }
}
