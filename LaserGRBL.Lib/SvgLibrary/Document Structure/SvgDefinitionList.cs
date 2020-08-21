using System;
using System.Collections.Generic;
using System.Text;

namespace Svg
{
    /// <summary>
    /// Represents a list of re-usable SVG components.
    /// </summary>
    [SvgElement("defs")]
    public class SvgDefinitionList : SvgElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SvgDefinitionList"/> class.
        /// </summary>
        public SvgDefinitionList()
        {
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            // Do nothing. Children should NOT be rendered.
        }

		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgDefinitionList>();
		}

    }
}