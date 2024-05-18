using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using SharpGL.SceneGraph.Collections;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Lighting;

namespace SharpGL.SceneGraph.Primitives
{
	/// <summary>
	/// A Shadow object can be added as a child of a polygon.
	/// </summary>
	[Serializable]
	public class Shadow : SceneElement, IRenderable
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Shadow"/> class.
        /// </summary>
        public Shadow() 
		{
            Name = "Shadow";
		}

        /// <summary>
        /// Render to the provided instance of OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="renderMode">The render mode.</param>
        public void Render(OpenGL gl, RenderMode renderMode)
        {
            //  Cast the shadow.
            CastShadow(gl);
        }

		/// <summary>
		/// This function calculates the neighbours in a face.
		/// </summary>
		private void SetConnectivity()
		{
			if(connectivitySet == true)
				return;

            //  Get some useful references.
            var faces = ParentPolygon.Faces;

			//	Create space for edge indices.
			foreach(Face face in faces)
			{
				face.NeighbourIndices = new int[face.Indices.Count];
                for (int i = 0; i < face.NeighbourIndices.Length; i++)
                    face.NeighbourIndices[i] = -2;		//	-2 = no neighbour calculated.
			}

			for(int faceAindex = 0; faceAindex < faces.Count; faceAindex++)
			{
				Face faceA = faces[faceAindex];

                for (int edgeA = 0; edgeA < faceA.NeighbourIndices.Length; edgeA++)
				{
                    if (faceA.NeighbourIndices[edgeA] == -2)
					{
						//	We don't yet know the neighbor.
						bool found = false;
						for(int faceBindex = 0; faceBindex < faces.Count; faceBindex++)
						{
							if(faceBindex == faceAindex) continue;

							Face faceB = faces[faceBindex];


                            for (int edgeB = 0; edgeB < faceB.NeighbourIndices.Length; edgeB++)
							{
								int vA1 = faceA.Indices[edgeA].Vertex;
								int vA2 = faceA.Indices[(edgeA+1)%faceA.Indices.Count].Vertex;
								int vB1 = faceB.Indices[edgeB].Vertex;
								int vB2 = faceB.Indices[(edgeB+1)%faceB.Indices.Count].Vertex;

								//	Check if they're neighbours...
								if((vA1 == vB1 && vA2 == vB2) || (vA1 == vB2 && vA2 == vB1))
								{
                                    faceA.NeighbourIndices[edgeA] = faceBindex;
                                    faceB.NeighbourIndices[edgeB] = faceAindex;
									found = true;
									break;
								}
							}
							if(found)
								break;
						}
						if(found == false)
                            faceA.NeighbourIndices[edgeA] = -1;
					}
				}
			}

			connectivitySet = true;
		}


        /// <summary>
        /// Casts a real time 3D shadow.
        /// </summary>
        /// <param name="gl">The OpenGL object.</param>
		private void CastShadow(OpenGL gl)
		{
			//	Set the connectivity, (calculate the neighbours of each face).
			SetConnectivity();

            //  Get the lights in the scene.
            var lights = TraverseToRootElement().Traverse<Light>(l => l.IsEnabled && l.On && l.CastShadow);

            //  Get some useful references.
            var faces = ParentPolygon.Faces;

			//	Go through every light in the scene.
			foreach(var light in lights)
			{
				//	Every face will have a visibility setting.
				bool[] facesVisible = new bool[faces.Count];

				//	Get the light position relative to the polygon.
				Vertex lightPos = light.Position;
				lightPos = lightPos - ParentPolygon.Transformation.TranslationVertex;

				//	Go through every face, finding out whether it's visible to the light.
				for(int nFace = 0; nFace < faces.Count; nFace++)
				{
					//	Get a reference to the face.
					Face face = faces[nFace];

					//	Is this face facing the light?
					float[] planeEquation = face.GetPlaneEquation(ParentPolygon);
					float side = planeEquation[0] * lightPos.X + 
						planeEquation[1] * lightPos.Y + 
						planeEquation[2] * lightPos.Z + planeEquation[3];
					facesVisible[nFace] = (side > 0) ? true : false;
				}

				//	Save all the attributes.
                gl.PushAttrib(OpenGL.GL_ALL_ATTRIB_BITS);
			
				//	Turn off lighting.
                gl.Disable(OpenGL.GL_LIGHTING);

				//	Turn off writing to the depth mask.
				gl.DepthMask(0);
                gl.DepthFunc(OpenGL.GL_LEQUAL);

				//	Turn on stencil buffer testing.
                gl.Enable(OpenGL.GL_STENCIL_TEST);

				//	Translate our shadow volumes.
				ParentPolygon.PushObjectSpace(gl);

				//	Don't draw to the color buffer.
				gl.ColorMask(0, 0, 0, 0);
                gl.StencilFunc(OpenGL.GL_ALWAYS, 1, 0xFFFFFFFF);

                gl.Enable(OpenGL.GL_CULL_FACE);
			
				//	First Pass. Increase Stencil Value In The Shadow
                gl.FrontFace(OpenGL.GL_CCW);
                gl.StencilOp(OpenGL.GL_KEEP, OpenGL.GL_KEEP, OpenGL.GL_INCR);
				DoShadowPass(gl, lightPos, facesVisible);

				//	Second Pass. Decrease Stencil Value In The Shadow
                gl.FrontFace(OpenGL.GL_CW);
                gl.StencilOp(OpenGL.GL_KEEP, OpenGL.GL_KEEP, OpenGL.GL_DECR);
				DoShadowPass(gl, lightPos, facesVisible);

                gl.FrontFace(OpenGL.GL_CCW);

				ParentPolygon.PopObjectSpace(gl);
			
				//	Enable writing to the color buffer.
				gl.ColorMask(1, 1, 1, 1); 
            
				// Draw A Shadowing Rectangle Covering The Entire Screen
				gl.Color(light.ShadowColor);
                gl.Enable(OpenGL.GL_BLEND);
                gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
                gl.StencilFunc(OpenGL.GL_NOTEQUAL, 0, 0xFFFFFFF);
                gl.StencilOp(OpenGL.GL_KEEP, OpenGL.GL_KEEP, OpenGL.GL_KEEP);
				
				Quadrics.Sphere shadow = new Quadrics.Sphere();
                shadow.Transformation.ScaleX = shadowSize;
                shadow.Transformation.ScaleY = shadowSize;
                shadow.Transformation.ScaleZ = shadowSize;
				shadow.Render(gl, RenderMode.Design);
	
				gl.PopAttrib();
			}
		}


		/// <summary>
		/// This is part of the shadow casting code, it does a shadowpass for the
		/// polygon using the specified light.
		/// </summary>
		/// <param name="gl">The OpenGL object.</param>
		/// <param name="lightPos">The position of the light casting the shadow.</param>
		/// <param name="visibleArray">An array of bools indicating the visible faces.</param>
		private void DoShadowPass(OpenGL gl, Vertex lightPos, bool[] visibleArray)
		{
            //  Helpful references.
            var faces = ParentPolygon.Faces;
            var vertices = ParentPolygon.Vertices;

			//	Go through each face.
			for(int i = 0; i < faces.Count; i++)
			{
				//	Get a reference to the face.
				Face face = faces[i];
				
				//	Is the face visible...
				if(visibleArray[i])
				{
					//	Go through each edge...
					for(int j = 0; j < face.Indices.Count; j++)
					{
						//	Get the neighbour of this edge.
                        int neighbourIndex = face.NeighbourIndices[j];

						//	If there's no neighbour, or the neighbour ain't visible, it's
						//	an edge...
						if(neighbourIndex == -1 || visibleArray[neighbourIndex] == false )
						{
							//	Get the edge vertices.
							Vertex v1 = vertices[face.Indices[j].Vertex];
							Vertex v2 = vertices[face.Indices[(j+1)%face.Indices.Count].Vertex];
						
							//	Create the two distant vertices.
							Vertex v3 = (v1 - lightPos) * 100;
							Vertex v4 = (v2 - lightPos) * 100;
							
							//	Draw the shadow volume.
                            gl.Begin(OpenGL.GL_TRIANGLE_STRIP);
							gl.Vertex(v1);
							gl.Vertex(v1 + v3);
							gl.Vertex(v2);
							gl.Vertex(v2 + v4);
							gl.End();
						}
					}
				}
			}			
		}
        	
		/// <summary>
		/// Are the face neighbours calculated?
		/// </summary>
		private bool connectivitySet = false;
        		
		/// <summary>
		/// The size of the shadow in each direction.
		/// </summary>
		private float shadowSize = 10;

        /// <summary>
        /// Gets the parent polygon.
        /// </summary>
        private Polygon ParentPolygon
        {
            get { return (Polygon)Parent; }
        }

        /// <summary>
        /// Gets or sets the size of the shadow.
        /// </summary>
        /// <value>
        /// The size of the shadow.
        /// </value>
		[Description("How large should the shadow be (how far does it extend in each direction)."), Category("Shadow")]
		public float ShadowSize
		{
			get {return shadowSize;}
			set {shadowSize = value; }
		}
    }

}
