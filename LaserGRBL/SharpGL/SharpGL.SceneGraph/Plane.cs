using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.SceneGraph
{
  /// <summary>
  /// A plane.
  /// </summary>
    [Serializable()]
    public class Plane
    {
      /// <summary>
      /// Initializes a new instance of the <see cref="Plane"/> class.
      /// </summary>
        public Plane() 
        {
        }

        /// <summary>
        /// This finds out if a point is in front of, behind, or on this plane.
        /// </summary>
        /// <param name="point">The point to classify.</param>
        /// <returns>
        /// Less than 0 if behind, 0 if on, Greater than 0 if in front.
        /// </returns>
        public float ClassifyPoint(Vertex point)
        {
            //	(X-P)*N = 0. Where, X is a point to test, P is a point
            //	on the plane, and N is the normal to the plane.

            return normal.ScalarProduct(point);
        }

        /// <summary>
        /// The position.
        /// </summary>
        public Vertex position = new Vertex(0, 0, 0);

        /// <summary>
        /// The normal.
        /// </summary>
      public Vertex normal = new Vertex(0, 0, 0);

      /// <summary>
      /// The equation.
      /// </summary>
        public float[] equation; // ax + by + cz + d = 0.
    }
}
