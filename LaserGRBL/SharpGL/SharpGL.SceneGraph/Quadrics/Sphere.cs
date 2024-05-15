using System;
using System.ComponentModel;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Helpers;
using System.Xml.Serialization;

namespace SharpGL.SceneGraph.Quadrics
{
    /// <summary>
    /// The Sphere class wraps the sphere quadric.
    /// </summary>
    [Serializable()]
    public class Sphere : Quadric, IVolumeBound
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sphere"/> class.
        /// </summary>
        public Sphere() 
        { 
            //  Set the name.
            Name = "Sphere";

            //  Set the radius (builds the BV).
            Radius = 1.0f;
        }

        /// <summary>
        /// Render to the provided instance of OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="renderMode">The render mode.</param>
        public override void Render(OpenGL gl, RenderMode renderMode)
        {
            //  Call the base.
            base.Render(gl, renderMode);

            //	Draw a sphere with the current settings.
            gl.Sphere(glQuadric, radius, slices, stacks);
        }

        /// <summary>
        /// The radius.
        /// </summary>
        private double radius = 1.0;

        /// <summary>
        /// The slices.
        /// </summary>
        private int slices = 16;

        /// <summary>
        /// The stacks.
        /// </summary>
        private int stacks = 20;

        /// <summary>
        /// The bounding volume helper, used to aid implementation of IVolumeBound.
        /// </summary>
        private BoundingVolumeHelper boundingVolumeHelper = new BoundingVolumeHelper();

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        [Description("Radius of the Sphere."), Category("Quadric")]
        public double Radius
        {
            get { return radius; }
            set
            {
                //  Set the radius.
                radius = value;

                //  Update the bounding volume.
                boundingVolumeHelper.BoundingVolume.FromSphericalVolume(
                    new Vertex(0, 0, 0), (float)value + 0.1f);
            }
        }

        /// <summary>
        /// Gets or sets the slices.
        /// </summary>
        /// <value>
        /// The slices.
        /// </value>
        [Description("Number of slices."), Category("Quadric")]
        public int Slices
        {
            get { return slices; }
            set { slices = value; }
        }

        /// <summary>
        /// Gets or sets the stacks.
        /// </summary>
        /// <value>
        /// The stacks.
        /// </value>
        [Description("Number of stacks."), Category("Quadric")]
        public int Stacks
        {
            get { return stacks; }
            set { stacks = value; }
        }

        /// <summary>
        /// Gets the bounding volume.
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public BoundingVolume BoundingVolume
        {
            get { return boundingVolumeHelper.BoundingVolume; }
        }
    }
}