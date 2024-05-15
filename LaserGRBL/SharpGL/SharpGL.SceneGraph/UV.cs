using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SharpGL.SceneGraph
{

    /// <summary>
    /// A texture coordinate.
    /// </summary>
    public struct UV
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UV"/> struct.
        /// </summary>
        /// <param name="u">The u.</param>
        /// <param name="v">The v.</param>
        public UV(float u, float v) 
        { 
            this.u = u; 
            this.v = v; 
        }

        /// <summary>
        /// The u value.
        /// </summary>
        private float u;

        /// <summary>
        /// The v value.
        /// </summary>
        private float v;

        public override string ToString()
        {
            return "(" + u + ", " + v + ")";
        }

        public static implicit operator float[](UV rhs)
        {
            return new float[] { rhs.u, rhs.v };
        }

        /// <summary>
        /// Gets or sets the U.
        /// </summary>
        /// <value>
        /// The U.
        /// </value>
        [XmlAttribute]
        public float U
        {
            get { return u; }
            set { u = value; }
        }

        /// <summary>
        /// Gets or sets the V.
        /// </summary>
        /// <value>
        /// The V.
        /// </value>
        [XmlAttribute]
        public float V
        {
            get { return v; }
            set { v = value; }
        }
    }
}
