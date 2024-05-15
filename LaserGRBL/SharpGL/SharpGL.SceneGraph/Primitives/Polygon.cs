using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using SharpGL.SceneGraph.Collections;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Lighting;
using SharpGL.SceneGraph.Raytracing;
using SharpGL.SceneGraph.Helpers;
using System.Xml.Serialization;
using SharpGL.SceneGraph.Transformations;
using SharpGL.SceneGraph.Assets;

namespace SharpGL.SceneGraph.Primitives
{
	/// <summary>
	/// A polygon contains a set of 'faces' which are indexes into a single list
	/// of vertices. The main thing about polygons is that they are easily editable
	/// by the user, depending on the Context they're in.
	/// </summary>
	[Serializable]
	public class Polygon : 
        SceneElement, 
        IHasObjectSpace,
        IRenderable,
        IRayTracable,
        IFreezable,
        IVolumeBound,
        IDeepCloneable<Polygon>,
        IHasMaterial 
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon"/> class.
        /// </summary>
		public Polygon() 
		{
			Name = "Polygon";
		}

		/// <summary>
		/// This function is cool, just stick in a set of points, it'll add them to the
		/// array, and create a face. It will take account of duplicate vertices too!
		/// </summary>
		/// <param name="vertexData">A set of vertices to make into a face.</param>
		public virtual void AddFaceFromVertexData(Vertex[] vertexData)
		{
			//	Create a face.
			Face newFace = new Face();

			//	Go through the vertices...
			foreach(Vertex v in vertexData)
			{
				//	Do we have this vertex already?
				int at = VertexSearch.Search(vertices, 0, v, 0.01f);
				
				//	Add the vertex, and index it.
                if (at == -1)
                {
                    newFace.Indices.Add(new Index(vertices.Count));
                    vertices.Add(v);
                }
                else
                {
                    newFace.Indices.Add(new Index(at));
                }
			}

			//	Add the face.
			faces.Add(newFace);
		}

		/// <summary>
		/// Triangulate this polygon.
		/// </summary>
		public void Triangulate()
		{
			List<Face> newFaces = new List<Face>();

			//	Go through each face...
			foreach(Face face in faces)
			{
				//	Number of triangles = vertices - 2.
				int triangles = face.Indices.Count - 2;

				//	Is it a triangle already?...
				if(triangles == 1)
				{
					newFaces.Add(face);
					continue;
				}

				//	Add a set of triangles.
				for(int i=0; i<triangles; i++)
				{
					Face triangle = new Face();
					triangle.Indices.Add(new Index(face.Indices[0]));
					triangle.Indices.Add(new Index(face.Indices[i+1]));
					triangle.Indices.Add(new Index(face.Indices[i+2]));
					triangle.Indices.Add(new Index(face.Indices[i+2]));
					triangle.Indices.Add(new Index(face.Indices[i+1]));
					newFaces.Add(triangle);
				}
			}

			faces.Clear();
			faces = newFaces;
		}

        /// <summary>
        /// Render to the provided instance of OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="renderMode">The render mode.</param>
        public virtual void Render(OpenGL gl, RenderMode renderMode)
        {
            //  If we're frozen, use the helper.
            if (freezableHelper.IsFrozen)
            {
                freezableHelper.Render(gl);
                return;
            }
            
            //  Go through each face.
            foreach (Face face in faces)
            {
                //  If the face has its own material, push it.
                if (face.Material != null)
                    face.Material.Push(gl);

                //	Begin drawing a polygon.
                if (face.Indices.Count == 2)
                    gl.Begin(OpenGL.GL_LINES);
                else
                    gl.Begin(OpenGL.GL_POLYGON);

                foreach (Index index in face.Indices)
                {
                    //	Set a texture coord (if any).
                    if (index.UV != -1)
                        gl.TexCoord(uvs[index.UV]);

                    //	Set a normal, or generate one.
                    if (index.Normal != -1)
                        gl.Normal(normals[index.Normal]);
                    else
                    {
                        //	Do we have enough vertices for a normal?
                        if (face.Indices.Count >= 3)
                        {
                            //	Create a normal.
                            Vertex vNormal = face.GetSurfaceNormal(this);
                            vNormal.UnitLength();

                            // todo use auto smoothing instead
                            //	Add it to the normals, setting the index for next time.
                            normals.Add(vNormal);
                            index.Normal = normals.Count - 1;

                            gl.Normal(vNormal);
                        }
                    }

                    //	Set the vertex.
                    gl.Vertex(vertices[index.Vertex]);
                }

                gl.End();

                //  If the face has its own material, pop it.
                if (face.Material != null)
                    face.Material.Pop(gl);
            }

            //	Draw normals if we have to.
            if (drawNormals)
            {
                //	Set the colour to red.
                gl.PushAttrib(OpenGL.GL_ALL_ATTRIB_BITS);
                gl.Color(1, 0, 0, 1);
                gl.Disable(OpenGL.GL_LIGHTING);

                //	Go through each face.
                foreach (Face face in faces)
                {
                    //	Go though each index.
                    foreach (Index index in face.Indices)
                    {
                        //	Make sure it's got a normal, and a vertex.
                        if (index.Normal != -1 && index.Vertex != -1)
                        {
                            //	Get the vertex.
                            Vertex vertex = vertices[index.Vertex];

                            //	Get the normal vertex.
                            Vertex normal = normals[index.Normal];
                            Vertex vertex2 = vertex + normal;

                            gl.Begin(OpenGL.GL_LINES);
                            gl.Vertex(vertex);
                            gl.Vertex(vertex2);
                            gl.End();
                        }
                    }
                }

                //	Restore the attributes.
                gl.PopAttrib();
            }
        }

		

		/// <summary>
		/// This creates a polygon from a height map (any picture). Black is low, 
		/// and the colors are high (the lighter the color, the higher the surface).
		/// </summary>
		/// <param name="filename">Path of the image file.</param>
		/// <param name="xPoints">Number of points along X.</param>
		/// <param name="yPoints">Number of points along Y.</param>
		/// <returns>True if sucessful, false otherwise.</returns>
		public virtual bool CreateFromMap(string filename, int xPoints, int yPoints)
		{
			//	Try and load the image file.
			System.Drawing.Bitmap map = new System.Drawing.Bitmap(filename);
			if(map.Size.IsEmpty)
				return false;

			//	Set the descriptive name.
			Name = "Map created from '" + filename + "'";

			//	Get points.
			for(int y=0; y < yPoints; y++)
			{
				int yValue = (map.Height / yPoints) * y;

				for(int x=0; x < xPoints; x++)
				{
					int xValue = (map.Width / xPoints) * x;

					//	Get the pixel.
					System.Drawing.Color col = map.GetPixel(xValue, yValue);

					float xPos = (float)x / (float)xPoints;
					float yPos = (float)y / (float)yPoints;

					//	Create a control point from it.
					Vertex v = new Vertex(xPos, 0, yPos);

					//	Add the 'height', based on color.
					v.Y = (float)col.R / 255.0f + (float)col.G / 255.0f + 
						(float)col.B / 255.0f;
					
					//	Add this vertex to the vertices array.
					Vertices.Add(v);
				}
			}

			//	Create faces for the polygon.
			for(int y=0; y < (yPoints-1); y++)
			{
				for(int x=0; x < (xPoints-1); x++)
				{
					//	Create the face.
					Face face = new Face();

					//	Create vertex indicies.
					int nTopLeft = (y * xPoints) + x;
					int nBottomLeft = ((y + 1) * xPoints) + x;
					
					face.Indices.Add(new Index(nTopLeft));
					face.Indices.Add(new Index(nTopLeft + 1));
					face.Indices.Add(new Index(nBottomLeft + 1));
					face.Indices.Add(new Index(nBottomLeft));

					// Add the face.
					Faces.Add(face);
				}
			}

			return true;
		}

		/// <summary>
		/// This function performs lossless optimisation on the polygon, it should be 
		/// called when the geometry changes, and the polygon goes into static mode.
		/// </summary>
		/// <returns>The amount of optimisation (as a %).</returns>
		protected virtual float OptimisePolygon()
		{
			//	Check for any null faces.
			float facesBefore = faces.Count;

			for(int i=0; i<faces.Count; i++)
			{
				if(faces[i].Count == 0)
					faces.RemoveAt(i--);
			}
			float facesAfter = faces.Count;

			return (facesAfter / facesBefore) * 100;
		}

		/// <summary>
		/// Call this function as soon as you change the polygons geometry, it will
		/// re-generate normals, etc.
		/// </summary>
		/// <param name="regenerateNormals">Regenerate Normals.</param>
		public virtual void Validate(bool regenerateNormals)
		{
			if(regenerateNormals)
				normals.Clear();

			//	Go through each face.
			foreach(Face face in faces)
			{
				if(regenerateNormals)
				{
					//	Find a normal for the face.
                    Vertex normal = face.GetSurfaceNormal(this);

					//	Does this normal already exist?
					int index = VertexSearch.Search(normals, 0, normal, 0.001f);

                    if (index == -1)
                    {
                        index = normals.Count;
                        normals.Add(normal);
                    }
					//	Set the index normal.
					foreach(Index i in face.Indices)
						i.Normal = index;
				}
			}
		}
        
        /// <summary>
        /// This function tests to see if a ray interesects the polygon.
        /// </summary>
        /// <param name="ray">The ray you want to test.</param>
        /// <returns>
        /// The distance from the origin of the ray to the intersection, or -1 if there
        /// is no intersection.
        /// </returns>
		private Intersection TestIntersection(Ray ray)
		{
			Intersection intersect = new Intersection();

			//	This code came from jgt intersect_triangle code (search dogpile for it).
			foreach(Face face in faces)
			{
				//	Assert that it's a triangle.
				if(face.Count != 3)
					continue;
			
				//	Find the point of intersection upon the plane, as a point 't' along
				//	the ray.
				Vertex point1OnPlane = vertices[face.Indices[0].Vertex];
				Vertex point2OnPlane = vertices[face.Indices[1].Vertex];
				Vertex point3OnPlane = vertices[face.Indices[2].Vertex];
				Vertex midpointOpp1 = (point2OnPlane + point3OnPlane) / 2;
				Vertex midpointOpp2 = (point1OnPlane + point3OnPlane) / 2;
				Vertex midpointOpp3 = (point1OnPlane + point2OnPlane) / 2;
				
				Vertex planeNormal = face.GetSurfaceNormal(this);


				Vertex diff = point1OnPlane - ray.origin;
				float s1 = diff.ScalarProduct(planeNormal);
				float s2 =  ray.direction.ScalarProduct(planeNormal);

				if(s2 == 0)
					continue;
				float t = s1 / s2;
				if(t < 0)
					continue;
	
				float denomintor = planeNormal.ScalarProduct(ray.direction);
				if(denomintor < 0.00001f && denomintor > -0.00001f)
					continue;	//	doesn't intersect the plane.

			//	Vertex v = point1OnPlane - ray.origin;
			//	float t = (v.ScalarProduct(planeNormal)) / denomintor;

				//	Now we can get the point of intersection.
				Vertex vIntersect = ray.origin + (ray.direction * t);

				//	Do my cool test.
				Vertex vectorTo1 = vIntersect - point1OnPlane;
				Vertex vectorTo2 = vIntersect - point2OnPlane;
				Vertex vectorTo3 = vIntersect - point3OnPlane;
				Vertex vectorMidTo1 = midpointOpp1 - point1OnPlane;
				Vertex vectorMidTo2 = midpointOpp2 - point2OnPlane;
				Vertex vectorMidTo3 = midpointOpp3 - point3OnPlane;
				
				if(vectorTo1.Magnitude() > vectorMidTo1.Magnitude())
					continue;
				if(vectorTo2.Magnitude() > vectorMidTo2.Magnitude())
					continue;
				if(vectorTo3.Magnitude() > vectorMidTo3.Magnitude())
					continue;

				if(intersect.closeness == -1 || t < intersect.closeness) 
				{
					//	It's fucking intersection city man
					intersect.point = vIntersect;
					intersect.intersected = true;
					intersect.normal = planeNormal;
					intersect.closeness = t;
				}
			}

			return intersect;
		}

        /// <summary>
        /// Raytraces the specified ray. If an intersection is found, it is returned,
        /// otherwise null is returned.
        /// </summary>
        /// <param name="ray">The ray.</param>
        /// <param name="scene">The scene.</param>
        /// <returns>
        /// The intersection with the object, or null.
        /// </returns>
		public Intersection Raytrace(Ray ray, Scene scene)
		{
			//	First we see if this ray intersects this polygon.
			Intersection intersect = TestIntersection(ray);

			//	If there wasn't an intersection, return.
			if(intersect.intersected == false)
				return intersect;

			//	There was an intersection, find the color of this point on the 
			//	polygon.
            var lights = from se in scene.SceneContainer.Traverse()
                         where se is Light
                         select se;
			foreach(Light light in lights)
			{
				if(light.On)
				{
					//	Can we see this light? Cast a shadow ray.
					Ray shadowRay = new Ray();
					bool shadow = false;
					shadowRay.origin = intersect.point;
					shadowRay.direction = light.Position - shadowRay.origin;

					//	Test it with every polygon.
					foreach(Polygon p in scene.SceneContainer.Traverse<Polygon>())
					{
						if(p == this) continue;
						Intersection shadowIntersect = p.TestIntersection(shadowRay);
						if(shadowIntersect.intersected)
						{
							shadow = true;
							break;
						}
					}

					if(shadow == false)
					{
						//	Now find out what this light complements to our color.
						//todofloat angle = light.Direction.ScalarProduct(intersect.normal);
						//todo ray.light += material.CalculateLighting(light, angle);
						ray.light.Clamp();
					}
				}
			}

			return intersect;
		}

		/// <summary>
		/// This function subdivides the faces of this polygon.
		/// </summary>
		/// <returns>The number of faces in the new subdivided polygon.</returns>
		public int Subdivide()
		{
			List<Face> newFaces = new List<Face>();

			foreach(Face face in Faces)
			{
				//	Make sure the face is a triangle.
				if(face.Count != 3)
					continue;

				//	Now get the vertices of the face.
				Vertex v1 = Vertices[face.Indices[0].Vertex];
				Vertex v2 = Vertices[face.Indices[1].Vertex];
				Vertex v3 = Vertices[face.Indices[2].Vertex];

				//	Add the vertices to get a the midpoint of the edge formed by those
				//	vectors.
				Vertex vMidpoint = (v1 + v2 + v3) / 3;
				Index iMidpoint = new Index(Vertices.Count);
                Vertices.Add(vMidpoint);

				//	Now make three new faces from the old vertices and the midpoint.
				Face newFace = new Face();
				newFace.Indices.Add(new Index(face.Indices[0]));
				newFace.Indices.Add(new Index(face.Indices[1]));
				newFace.Indices.Add(iMidpoint);
				newFaces.Add(newFace);

				newFace = new Face();
				newFace.Indices.Add(new Index(face.Indices[1]));
				newFace.Indices.Add(new Index(face.Indices[2]));
				newFace.Indices.Add(iMidpoint);
				newFaces.Add(newFace);

				newFace = new Face();
				newFace.Indices.Add(new Index(face.Indices[2]));
				newFace.Indices.Add(new Index(face.Indices[0]));
				newFace.Indices.Add(iMidpoint);
				newFaces.Add(newFace);
			}

			faces = newFaces;

			return faces.Count;
		}

        /// <summary>
        /// Freezes this instance using the provided OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void Freeze(OpenGL gl)
        {
            //  Freeze using the helper.
            freezableHelper.Freeze(gl, this);
        }

        /// <summary>
        /// Unfreezes this instance using the provided OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void Unfreeze(OpenGL gl)
        {
            //  Unfreeze using the helper.
            freezableHelper.Unfreeze(gl);
        }

        /// <summary>
        /// Pushes us into Object Space using the transformation into the specified OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void PushObjectSpace(OpenGL gl)
        {
            //  Use the helper to push us into object space.
            hasObjectSpaceHelper.PushObjectSpace(gl);
        }

        /// <summary>
        /// Pops us from Object Space using the transformation into the specified OpenGL instance.
        /// </summary>
        /// <param name="gl">The gl.</param>
        public void PopObjectSpace(OpenGL gl)
        {
            //  Use the helper to pop us from object space.
            hasObjectSpaceHelper.PopObjectSpace(gl);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public Polygon DeepClone()
        {
            //  Create a new polygon.
            Polygon polygon = new Polygon();

            //  Clone the data.
            polygon.hasObjectSpaceHelper = hasObjectSpaceHelper.DeepClone();
            polygon.freezableHelper = new FreezableHelper();

            //  TODO clone lists.
            return polygon;
        }
        
        /// <summary>
        /// The IHasObjectSpace helper.
        /// </summary>
        private HasObjectSpaceHelper hasObjectSpaceHelper = new HasObjectSpaceHelper();

        /// <summary>
        /// The freezable helper.
        /// </summary>
        private FreezableHelper freezableHelper = new FreezableHelper();

		/// <summary>
		/// The faces that make up the polygon.
		/// </summary>
		private List<Face> faces = new List<Face>();

		/// <summary>
		/// The vertices that make up the polygon.
		/// </summary>
		private List<Vertex> vertices = new List<Vertex>();

		/// <summary>
		/// The UV coordinates (texture coodinates) for the polygon.
		/// </summary>
		private List<UV> uvs = new List<UV>();

		/// <summary>
		/// The normals of the polygon object.
		/// </summary>
		private List<Vertex> normals = new List<Vertex>();

		/// <summary>
		/// Should the normals be drawn?
		/// </summary>
		private bool drawNormals = false;

        /// <summary>
        /// The bounding volume helper - used to ease implementation of IBoundVolume.
        /// </summary>
        private BoundingVolumeHelper boundingVolumeHelper = new BoundingVolumeHelper();

        /// <summary>
        /// Gets or sets the faces.
        /// </summary>
        /// <value>
        /// The faces.
        /// </value>
		[Description("The faces that make up the polygon."), Category("Polygon")]
		public List<Face> Faces
		{
			get {return faces;}
			set {faces = value; }
		}

        /// <summary>
        /// Gets or sets the vertices.
        /// </summary>
        /// <value>
        /// The vertices.
        /// </value>
		[Description("The vertices that make up the polygon."), Category("Polygon")]
		public List<Vertex> Vertices
		{
			get {return vertices;}
			set {vertices = value; }
		}

        /// <summary>
        /// Gets or sets the U vs.
        /// </summary>
        /// <value>
        /// The U vs.
        /// </value>
		[Description("The material coordinates."), Category("Polygon")]
		public List<UV> UVs
		{
			get {return uvs;}
			set {uvs = value; }
		}

        /// <summary>
        /// Gets or sets the normals.
        /// </summary>
        /// <value>
        /// The normals.
        /// </value>
		[Description("The normals."), Category("Normals")]
		public List<Vertex> Normals
		{
			get {return normals;}
			set {normals = value; }
		}
        
        /// <summary>
        /// Gets or sets a value indicating whether [draw normals].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [draw normals]; otherwise, <c>false</c>.
        /// </value>
		[Description("Should normals be drawn for each face?"), Category("Polygon")]
		public bool DrawNormals
		{
			get {return drawNormals;}
			set {drawNormals = value; }
		}

        /// <summary>
        /// Gets the transformation that pushes us into object space.
        /// </summary>
        [Description("The Polygon Object Space Transformation"), Category("Polygon")]
        public LinearTransformation Transformation
        {
            get { return hasObjectSpaceHelper.Transformation; }
            set { hasObjectSpaceHelper.Transformation = value; }
        }

        /// <summary>
        /// Gets the bounding volume.
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public BoundingVolume BoundingVolume
        {
            get 
            {
                //  todo; only create bv when vertices changed.
                boundingVolumeHelper.BoundingVolume.FromVertices(vertices);
                boundingVolumeHelper.BoundingVolume.Pad(0.1f); 
                return boundingVolumeHelper.BoundingVolume;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is frozen.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is frozen; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [XmlIgnore]
        public bool IsFrozen
        {
            get { return freezableHelper.IsFrozen; }
        }

        /// <summary>
        /// Material to be used when rendering the polygon in lighted mode.
        /// This material may be overriden on a per-face basis.
        /// </summary>
        /// <value>
        /// The material.
        /// </value>
        [XmlIgnore]
        public Material Material
        {
            get;
            set;
        }
    }

}
