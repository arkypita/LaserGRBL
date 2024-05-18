using System;
using System.Drawing;
using System.ComponentModel;

using SharpGL.SceneGraph;
using SharpGL.Enumerations;

namespace SharpGL.OpenGLAttributes
{
	/// <summary>
	/// This class has all the settings you can edit for fog.
	/// </summary>
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[Serializable()]
    public class StencilBufferAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="StencilBufferAttributes"/> class.
        /// </summary>
        public StencilBufferAttributes()
        {
            AttributeFlags = SharpGL.Enumerations.AttributeMask.StencilBuffer;
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if (enableStencilTest.HasValue) gl.EnableIf(OpenGL.GL_STENCIL_TEST, enableStencilTest.Value);
            if (stencilFunction.HasValue && stencilReference.HasValue && stencilMask.HasValue) 
                gl.StencilFunc(stencilFunction.Value, stencilReference.Value, stencilMask.Value);
            if (stencilClearIndex.HasValue) gl.ClearStencil(stencilClearIndex.Value);
            if (stencilWriteMask.HasValue) gl.StencilMask(stencilWriteMask.Value);
            if (operationFail.HasValue && operationDepthFail.HasValue && operationDepthPass.HasValue)
                gl.StencilOp(operationFail.Value, operationDepthFail.Value, operationDepthPass.Value);
            
        }

        /// <summary>
        /// Returns true if any attributes are set.
        /// </summary>
        /// <returns>
        /// True if any attributes are set
        /// </returns>
        public override bool AreAnyAttributesSet()
        {
            return
                enableStencilTest.HasValue ||
                stencilFunction.HasValue ||
                stencilReference.HasValue ||
                stencilMask.HasValue ||
                stencilClearIndex.HasValue ||
                stencilWriteMask.HasValue ||
                operationFail.HasValue ||
                operationDepthFail.HasValue ||
                operationDepthPass.HasValue;
        }

        private bool? enableStencilTest;
        private StencilFunction? stencilFunction;
        private int? stencilReference;
        private uint? stencilMask;
        private int? stencilClearIndex;
        private uint? stencilWriteMask;
        private StencilOperation? operationFail;
        private StencilOperation? operationDepthFail;
        private StencilOperation? operationDepthPass;

        /// <summary>
        /// Gets or sets the enable stencil test.
        /// </summary>
        /// <value>
        /// The enable stencil test.
        /// </value>
		[Description("."), Category("Stencil Buffer")]
        public bool? EnableStencilTest
		{
            get { return enableStencilTest; }
            set { enableStencilTest = value; }
        }

        /// <summary>
        /// Gets or sets the stencil function.
        /// </summary>
        /// <value>
        /// The stencil function.
        /// </value>
        [Description("."), Category("Stencil Buffer")]
        public StencilFunction? StencilFunction
        {
            get { return stencilFunction; }
            set { stencilFunction = value; }
        }

        /// <summary>
        /// Gets or sets the stencil reference.
        /// </summary>
        /// <value>
        /// The stencil reference.
        /// </value>
        [Description("."), Category("Stencil Buffer")]
        public int? StencilReference
        {
            get { return stencilReference; }
            set { stencilReference = value; }
        }

        /// <summary>
        /// Gets or sets the stencil mask.
        /// </summary>
        /// <value>
        /// The stencil mask.
        /// </value>
        [Description("."), Category("Stencil Buffer")]
        public uint? StencilMask
        {
            get { return stencilMask; }
            set { stencilMask = value; }
        }

        /// <summary>
        /// Gets or sets the index of the stencil clear.
        /// </summary>
        /// <value>
        /// The index of the stencil clear.
        /// </value>
        [Description("."), Category("Stencil Buffer")]
        public int? StencilClearIndex
        {
            get { return stencilClearIndex; }
            set { stencilClearIndex = value; }
        }

        /// <summary>
        /// Gets or sets the stencil write mask.
        /// </summary>
        /// <value>
        /// The stencil write mask.
        /// </value>
        [Description("."), Category("Stencil Buffer")]
        public uint? StencilWriteMask
        {
            get { return stencilWriteMask; }
            set { stencilWriteMask = value; }
        }

        /// <summary>
        /// Gets or sets the operation fail.
        /// </summary>
        /// <value>
        /// The operation fail.
        /// </value>
        [Description("."), Category("Stencil Buffer")]
        public StencilOperation? OperationFail
        {
            get { return operationFail; }
            set { operationFail = value; }
        }

        /// <summary>
        /// Gets or sets the operation depth pass.
        /// </summary>
        /// <value>
        /// The operation depth pass.
        /// </value>
        [Description("."), Category("Stencil Buffer")]
        public StencilOperation? OperationDepthFail
        {
            get { return operationDepthFail; }
            set { operationDepthFail = value; }
        }

        /// <summary>
        /// Gets or sets the operation depth pass.
        /// </summary>
        /// <value>
        /// The operation depth pass.
        /// </value>
        [Description("."), Category("Stencil Buffer")]
        public StencilOperation? OperationDepthPass
        {
            get { return operationDepthPass; }
            set { operationDepthPass = value; }
        }
	}
}
