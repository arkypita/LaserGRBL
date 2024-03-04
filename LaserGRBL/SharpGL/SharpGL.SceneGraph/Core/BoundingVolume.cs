using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SharpGL.SceneGraph.Core;
using SharpGL.Enumerations;

namespace SharpGL.SceneGraph.Core
{
	public class BoundingVolume : IRenderable
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingVolume"/> class.
        /// </summary>
        public BoundingVolume() 
		{
		}

        /// <summary>
        /// Creates the volume from vertices.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        public void FromVertices(IEnumerable<Vertex> vertices)
        {
            var vertexList = vertices.ToList();
            if (vertexList.Count < 2)
                return;

            lll = vertexList[0];
            hll = vertexList[0];
            lhl = vertexList[0];
            llh = vertexList[0];
            hhl = vertexList[0];
            hlh = vertexList[0];
            lhh = vertexList[0];
            hhh = vertexList[0];

            for (int i = 1; i < vertexList.Count; i++)
            {
                lll.X = Math.Min(lll.X, vertexList[i].X);
                lll.Y = Math.Min(lll.Y, vertexList[i].Y);
                lll.Z = Math.Min(lll.Z, vertexList[i].Z);

                hll.X = Math.Max(hll.X, vertexList[i].X);
                hll.Y = Math.Min(hll.Y, vertexList[i].Y);
                hll.Z = Math.Min(hll.Z, vertexList[i].Z);

                lhl.X = Math.Min(lhl.X, vertexList[i].X);
                lhl.Y = Math.Max(lhl.Y, vertexList[i].Y);
                lhl.Z = Math.Min(lhl.Z, vertexList[i].Z);

                llh.X = Math.Min(llh.X, vertexList[i].X);
                llh.Y = Math.Min(llh.Y, vertexList[i].Y);
                llh.Z = Math.Max(llh.Z, vertexList[i].Z);

                hhl.X = Math.Max(hhl.X, vertexList[i].X);
                hhl.Y = Math.Max(hhl.Y, vertexList[i].Y);
                hhl.Z = Math.Min(hhl.Z, vertexList[i].Z);

                hlh.X = Math.Max(hlh.X, vertexList[i].X);
                hlh.Y = Math.Min(hlh.Y, vertexList[i].Y);
                hlh.Z = Math.Max(hlh.Z, vertexList[i].Z);

                lhh.X = Math.Min(lhh.X, vertexList[i].X);
                lhh.Y = Math.Max(lhh.Y, vertexList[i].Y);
                lhh.Z = Math.Max(lhh.Z, vertexList[i].Z);

                hhh.X = Math.Max(hhh.X, vertexList[i].X);
                hhh.Y = Math.Max(hhh.Y, vertexList[i].Y);
                hhh.Z = Math.Max(hhh.Z, vertexList[i].Z);
            }
        }

        /// <summary>
        /// Creates the volume from a spherical volume.
        /// </summary>
        /// <param name="centre">The centre.</param>
        /// <param name="radius">The radius.</param>
        public void FromSphericalVolume(Vertex centre, float radius)
        {
            //  Set the centre.
            lll = centre;
            hll = centre;
            lhl = centre;
            llh = centre;
            hhl = centre;
            hlh = centre;
            lhh = centre;
            hhh = centre;

            //  Pad by the radius.
            Pad(radius);
        }

        /// <summary>
        /// Creates the volume from a cylindrical volume.
        /// </summary>
        /// <param name="baseline">The baseline.</param>
        /// <param name="height">The height.</param>
        /// <param name="baseRadius">The base radius.</param>
        /// <param name="topRadius">The top radius.</param>
        public void FromCylindricalVolume(Vertex baseline, float height, float baseRadius, float topRadius)
        {
            Vertex[] set = new Vertex[6];

            set[0] = baseline;
            set[1] = baseline + new Vertex(0, 0, height);

            set[2] = baseline + new Vertex(baseRadius, baseRadius , 0);
            set[3] = baseline + new Vertex(-baseRadius, -baseRadius, 0);

            set[4] = set[1] + new Vertex(topRadius, topRadius, 0);
            set[5] = set[1] + new Vertex(-topRadius, -topRadius, 0);

            FromVertices(set);            
        }

        /// <summary>
        /// Pads the bounding volume.
        /// </summary>
        /// <param name="padding">The padding.</param>
        public void Pad(float padding)
        {
            lll.X -= padding;
            lll.Y -= padding;
            lll.Z -= padding;

            hll.X += padding;
            hll.Y -= padding;
            hll.Z -= padding;

            lhl.X -= padding;
            lhl.Y += padding;
            lhl.Z -= padding;

            llh.X -= padding;
            llh.Y -= padding;
            llh.Z += padding;

            hhl.X += padding;
            hhl.Y += padding;
            hhl.Z -= padding;

            hlh.X += padding;
            hlh.Y -= padding;
            hlh.Z += padding;

            lhh.X -= padding;
            lhh.Y += padding;
            lhh.Z += padding;

            hhh.X += padding;
            hhh.Y += padding;
            hhh.Z += padding;
        }

        /// <summary>
        /// Gets the bound dimensions.
        /// </summary>
        /// <param name="x">The x size.</param>
        /// <param name="y">The y size.</param>
        /// <param name="z">The z size.</param>
        public void GetBoundDimensions(out float x, out float y, out float z)
        {
            x = Math.Abs(lll.X - hll.X);
            y = Math.Abs(lll.Y - lhl.Y);
            z = Math.Abs(lll.Z - llh.Z);
        }

        /// <summary>
        /// Render to the provided instance of OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="renderMode">The render mode.</param>
        public void Render(OpenGL gl, RenderMode renderMode)
        {
            //  Push attributes, disable lighting.
            gl.PushAttrib(OpenGL.GL_CURRENT_BIT | OpenGL.GL_ENABLE_BIT |
                OpenGL.GL_LINE_BIT | OpenGL.GL_POLYGON_BIT);
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.LineWidth(1.0f);
            gl.Color(1f, 0.2f, 0.2f, 0.6f);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, 
                renderMode == RenderMode.HitTest ? (uint)PolygonMode.Filled : (uint)PolygonMode.Lines);
            
            gl.Begin(OpenGL.GL_QUADS);		// Draw The Cube Using quads
            gl.Vertex(hhl);	// Top Right Of The Quad (Top)
            gl.Vertex(lhl);	// Top Left Of The Quad (Top)
            gl.Vertex(lhh);	// Bottom Left Of The Quad (Top)
            gl.Vertex(hhh);	// Bottom Right Of The Quad (Top)
            gl.Vertex(hlh);	// Top Right Of The Quad (Bottom)
            gl.Vertex(llh);	// Top Left Of The Quad (Bottom)
            gl.Vertex(lll);	// Bottom Left Of The Quad (Bottom)
            gl.Vertex(hll);	// Bottom Right Of The Quad (Bottom)
            gl.Vertex(hhh);	// Top Right Of The Quad (Front)
            gl.Vertex(lhh);	// Top Left Of The Quad (Front)
            gl.Vertex(llh);	// Bottom Left Of The Quad (Front)
            gl.Vertex(hlh);	// Bottom Right Of The Quad (Front)
            gl.Vertex(hll);	// Top Right Of The Quad (Back)
            gl.Vertex(lll);	// Top Left Of The Quad (Back)
            gl.Vertex(lhl);	// Bottom Left Of The Quad (Back)
            gl.Vertex(hhl);	// Bottom Right Of The Quad (Back)
            gl.Vertex(lhh);	// Top Right Of The Quad (Left)
            gl.Vertex(lhl);	// Top Left Of The Quad (Left)
            gl.Vertex(lll);	// Bottom Left Of The Quad (Left)
            gl.Vertex(llh);	// Bottom Right Of The Quad (Left)
            gl.Vertex(hhl);	// Top Right Of The Quad (Right)
            gl.Vertex(hhh);	// Top Left Of The Quad (Right)
            gl.Vertex(hlh);	// Bottom Left Of The Quad (Right)
            gl.Vertex(hll);	// Bottom Right Of The Quad (Right)
            gl.End();			// End Drawing The Cube

            //  Pop attributes.
            gl.PopAttrib();
        }

        private Vertex lll;
        private Vertex hll;
        private Vertex lhl;
        private Vertex llh;
        private Vertex hhl;
        private Vertex hlh;
        private Vertex lhh;
        private Vertex hhh;
    }
}
