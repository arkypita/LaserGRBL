using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SharpGL.SceneGraph
{
    /// <summary>
    /// An index into a set of arrays.
    /// </summary>
    [Serializable()]
    public class Index
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Index"/> class.
        /// </summary>
        public Index()
        {

        }
        //	Constructors.
        public Index(Index index) { vertex = index.vertex; uv = index.uv; }
        public Index(int vertex) { this.vertex = vertex; }
        public Index(int vertex, int uv) { this.vertex = vertex; this.uv = uv; }
        public Index(int vertex, int uv, int normal) { this.vertex = vertex; this.uv = uv; this.normal = normal; }

        /// <summary>
        /// This is the vertex in the polygon vertex array that the index refers to.
        /// </summary>
        private int vertex = -1;

        /// <summary>
        /// This is the material coord in the polygon UV array that the index refers to.
        /// </summary>
        private int uv = -1;

        /// <summary>
        /// This is the index into the normal array for this vertex. A value of -1 will
        /// generate a normal on the fly.
        /// </summary>
        private int normal = -1;

        /// <summary>
        /// Gets or sets the vertex.
        /// </summary>
        /// <value>
        /// The vertex.
        /// </value>
        [Description("The vertex index from the Polygons Vertex Array."), Category("Index")]
        [XmlAttribute]
        public int Vertex
        {
            get { return vertex; }
            set { vertex = value; }
        }

        /// <summary>
        /// Gets or sets the UV.
        /// </summary>
        /// <value>
        /// The UV.
        /// </value>
        [Description("The UV index from the polygons UV Array."), Category("Index")]
        [XmlAttribute]
        public int UV
        {
            get { return uv; }
            set { uv = value; }
        }

        /// <summary>
        /// Gets or sets the normal.
        /// </summary>
        /// <value>
        /// The normal.
        /// </value>
        [Description("The normal index, -1 means a normal will be generated on the fly."), Category("Index")]
        [XmlAttribute]
        public int Normal
        {
            get { return normal; }
            set { normal = value; }
        }
    }
}
