
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph.Primitives;
using System.ComponentModel;
using SharpGL.SceneGraph.Core;

namespace SharpGL.SceneGraph.Effects
{

    /// <summary>
    /// An ArcBall is an effect that pushes an arcball transformation 
    /// onto the stack.
    /// </summary>
    public class ArcBallEffect : Effect
    {
        /// <summary>
        /// Pushes the effect onto the specified parent element.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="parentElement">The parent element.</param>
        public override void Push(OpenGL gl, Core.SceneElement parentElement)
        {
            //  Push the stack.
            gl.PushMatrix();

            //  Perform the transformation.
            arcBall.TransformMatrix(gl);
        }

        /// <summary>
        /// Pops the specified parent element.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="parentElement">The parent element.</param>
        public override void Pop(OpenGL gl, Core.SceneElement parentElement)
        {
            //  Pop the stack.
            gl.PopMatrix();
        }

        /// <summary>
        /// The arcball.
        /// </summary>
        private ArcBall arcBall = new ArcBall();

        /// <summary>
        /// Gets or sets the linear transformation.
        /// </summary>
        /// <value>
        /// The linear transformation.
        /// </value>
        [Description("The ArcBall."), Category("Effect")]
        public ArcBall ArcBall
        {
            get { return arcBall; }
            set { arcBall = value; }
        }
    }
}
