using System;
using System.ComponentModel;
using SharpGL.SceneGraph.Core;

namespace SharpGL.SceneGraph.Quadrics
{
    /// <summary>
    /// The Disk class wraps both the disk and partial disk quadrics.
    /// </summary>
    [Serializable()]
    public class Disk : Quadric
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Disk"/> class.
        /// </summary>
        public Disk() 
        { 
            Name = "Disk";
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
            gl.PartialDisk(glQuadric, innerRadius, outerRadius,
                slices, loops, startAngle, sweepAngle);
        }

        //	Sphere data.
        double innerRadius = 0.0;
        double outerRadius = 2.0;
        double startAngle = 0.0;
        double sweepAngle = 360.0;
        int slices = 16;
        int loops = 20;

        /// <summary>
        /// Gets or sets the inner radius.
        /// </summary>
        /// <value>
        /// The inner radius.
        /// </value>
        [Description("Radius of the hole on the inside of the disk."), Category("Quadric")]
        public double InnerRadius
        {
            get { return innerRadius; }
            set { innerRadius = value; }
        }

        /// <summary>
        /// Gets or sets the outer radius.
        /// </summary>
        /// <value>
        /// The outer radius.
        /// </value>
        [Description("Radius of the disk."), Category("Quadric")]
        public double OuterRadius
        {
            get { return outerRadius; }
            set { outerRadius = value; }
        }

        /// <summary>
        /// Gets or sets the start angle.
        /// </summary>
        /// <value>
        /// The start angle.
        /// </value>
        [Description("Start angle of the partial disk."), Category("Quadric")]
        public double StartAngle
        {
            get { return startAngle; }
            set { startAngle = value; }
        }

        /// <summary>
        /// Gets or sets the sweep angle.
        /// </summary>
        /// <value>
        /// The sweep angle.
        /// </value>
        [Description("Size of the partial disk."), Category("Quadric")]
        public double SweepAngle
        {
            get { return sweepAngle; }
            set { sweepAngle = value; }
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
        /// Gets or sets the loops.
        /// </summary>
        /// <value>
        /// The loops.
        /// </value>
        [Description("Number of loops."), Category("Quadric")]
        public int Loops
        {
            get { return loops; }
            set { loops = value; }
        }
    }
}