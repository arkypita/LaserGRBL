using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;
using System.Xml.Serialization;
using SharpGL.SceneGraph.Assets;

namespace SharpGL.SceneGraph
{
    /// <summary>
    /// A Face is a set of indices to vertices.
    /// </summary>
    public class Face : IHasMaterial
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Face"/> class.
        /// </summary>
        public Face() 
        {
        }

        /// <summary>
        /// Returns the plane equation (ax + by + cz + d = 0) of the face.
        /// </summary>
        /// <param name="parent">The parent polygon.</param>
        /// <returns>An array of four coefficients a,b,c,d.</returns>
        public float[] GetPlaneEquation(Polygon parent)
        {
            //	Get refs to vertices.
            Vertex v1 = parent.Vertices[indices[0].Vertex];
            Vertex v2 = parent.Vertices[indices[1].Vertex];
            Vertex v3 = parent.Vertices[indices[2].Vertex];

            float a = v1.Y * (v2.Z - v3.Z) + v2.Y * (v3.Z - v1.Z) + v3.Y * (v1.Z - v2.Z);
            float b = v1.Z * (v2.X - v3.X) + v2.Z * (v3.X - v1.X) + v3.Z * (v1.X - v2.X);
            float c = v1.X * (v2.Y - v3.Y) + v2.X * (v3.Y - v1.Y) + v3.X * (v1.Y - v2.Y);
            float d = -(v1.X * (v2.Y * v3.Z - v3.Y * v2.Z) +
                v2.X * (v3.Y * v1.Z - v1.Y * v3.Z) +
                v3.X * (v1.Y * v2.Z - v2.Y * v1.Z));

            return new float[] { a, b, c, d };
        }

        /// <summary>
        /// Gets the surface normal.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public Vertex GetSurfaceNormal(Polygon parent)
        {
            //	Do we have enough vertices for a normal?
            if (indices.Count < 3)
                return new Vertex(0, 0, 0);

            Vertex v1 = parent.Vertices[indices[0].Vertex];
            Vertex v2 = parent.Vertices[indices[1].Vertex];
            Vertex v3 = parent.Vertices[indices[2].Vertex];
            Vertex va = v1 - v2;
            Vertex vb = v2 - v3;
            return va.VectorProduct(vb);
        }

        /// <summary>
        /// This function reverses the order of the indices, i.e changes which direction
        /// this face faces in.
        /// </summary>
        /// <param name="parent">The parent polygon.</param>
        public void Reorder(Polygon parent)
        {
            //	Create a new index collection.
            List<Index> newIndices = new List<Index>();

            //	Go through every old index and add it.
            for (int i = 0; i < indices.Count; i++)
                newIndices.Add(indices[indices.Count - (i + 1)]);

            //	Set the new index array.
            indices = newIndices;

            //	Recreate each normal.
            GenerateNormals(parent);
        }

        /// <summary>
        /// This function generates normals for every vertex.
        /// </summary>
        /// <param name="parent">The parent polygon.</param>
        public void GenerateNormals(Polygon parent)
        {
            if (Indices.Count >= 3)
            {
                foreach (Index index in Indices)
                {
                    //	Do we have enough vertices for a normal?
                    if (Indices.Count >= 3)
                    {
                        //	Create a normal.
                        Vertex vNormal = GetSurfaceNormal(parent);
                        vNormal.UnitLength();

                        //	Add it to the normals, setting the index for next time.
                        if (index.Normal != -1)
                            parent.Normals.RemoveAt(index.Normal);
                        index.Normal = parent.Normals.Count;
                        parent.Normals.Add(vNormal);
                    }
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Face, " + indices.Count + " indices";
        }


        /// <summary>
        /// The indices.
        /// </summary>
        private List<Index> indices = new List<Index>();

        /// <summary>
        /// The neighbor indices.
        /// </summary>
        private int[] neighbourIndices;

        /// <summary>
        /// Gets the count.
        /// </summary>
        [XmlIgnore]
        public int Count
        {
            get { return indices.Count; }
        }

        /// <summary>
        /// Gets or sets the indices.
        /// </summary>
        /// <value>
        /// The indices.
        /// </value>
        public List<Index> Indices
        {
            get { return indices; }
            set { indices = value; }
        }

        /// <summary>
        /// Gets or sets the neighbour indicies.
        /// </summary>
        /// <value>
        /// The neighbour indicies.
        /// </value>
        public int[] NeighbourIndices
        {
            get { return neighbourIndices; }
            set { neighbourIndices = value; }
        }

        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        /// <value>
        /// The material.
        /// </value>
        public Material Material
        {
            get;
            set;
        }
    }
}