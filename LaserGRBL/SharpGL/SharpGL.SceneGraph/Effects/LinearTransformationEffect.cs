
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph.Primitives;
using System.ComponentModel;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Transformations;

namespace SharpGL.SceneGraph.Effects
{

    /// <summary>
    /// A Linear Transformation is an effect that pushes a linear transformation (translate, scale, rotate)
    /// onto the stack.
    /// </summary>
    public class LinearTransformationEffect : Effect
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
            linearTransformation.Transform(gl);
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
        /// The linear transformation.
        /// </summary>
        private LinearTransformation linearTransformation = new LinearTransformation();

        /// <summary>
        /// Gets or sets the linear transformation.
        /// </summary>
        /// <value>
        /// The linear transformation.
        /// </value>
        [Description("The linear transformation."), Category("Effect")]
        public LinearTransformation LinearTransformation
        {
            get { return linearTransformation; }
            set { linearTransformation = value; }
        }
    }
}
