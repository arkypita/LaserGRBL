using System;
using System.ComponentModel;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Helpers;
using System.Xml.Serialization;

namespace SharpGL.SceneGraph.Quadrics
{
    /// <summary>
    /// The Cylinder class wraps the cylinder quadric.
    /// </summary>
    [Serializable()]
    public class Cylinder : Quadric, IVolumeBound
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cylinder"/> class.
        /// </summary>
        public Cylinder() 
        { 
            //  Set the name.
            Name = "Cylinder";

            //  Set the height via the property, sets the BV.
            Height = 1.0f;
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

            //	Draw a quadric with the current settings.
            gl.Cylinder(glQuadric, baseRadius, topRadius, height, slices, stacks);
        }

        /// <summary>
        /// Sphere data.
        /// </summary>
        private double baseRadius = 1.0;

        /// <summary>
        /// Top radius.
        /// </summary>
        private double topRadius = 0.0;

        /// <summary>
        /// The height.
        /// </summary>
        private double height = 1.0;

        /// <summary>
        /// The slices.
        /// </summary>
        private int slices = 16;

        /// <summary>
        /// The stacks.
        /// </summary>
        private int stacks = 20;

        /// <summary>
        /// Helps us implement IVolumeBound.
        /// </summary>
        private BoundingVolumeHelper boundingVolumeHelper = new BoundingVolumeHelper();

        /// <summary>
        /// Gets or sets the base radius.
        /// </summary>
        /// <value>
        /// The base radius.
        /// </value>
        [Description("Radius of the base of the cylinder."), Category("Quadric")]
        public double BaseRadius
        {
            get { return baseRadius; }
            set 
            { 
                baseRadius = value;
                BoundingVolume.FromCylindricalVolume(new Vertex(), (float)height, (float)baseRadius, (float)topRadius);
            }
        }

        /// <summary>
        /// Gets or sets the top radius.
        /// </summary>
        /// <value>
        /// The top radius.
        /// </value>
        [Description("Radius of the top of the cylinder."), Category("Quadric")]
        public double TopRadius
        {
            get { return topRadius; }
            set 
            {
                topRadius = value;
                BoundingVolume.FromCylindricalVolume(new Vertex(), (float)height, (float)baseRadius, (float)topRadius);
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        [Description("Height of the cylinder."), Category("Quadric")]
        public double Height
        {
            get { return height; }
            set 
            {
                height = value;
                BoundingVolume.FromCylindricalVolume(new Vertex(), (float)height, (float)baseRadius, (float)topRadius);
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