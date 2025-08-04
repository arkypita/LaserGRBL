using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace LaserGRBL.SvgConverter
{
    /// <summary>
    /// Utility class that can adjust a SVG to replace cloned elements with their originals.
    /// </summary>
    public class CloneReplacer
    {
        private const int CLONE_NESTING_LIMIT = 20;
        public CloneReplacer() {}

        #region ScanSVGAndReplaceClonedElements
        /// <summary>
        /// Scans the SVG for any "use" element (a clone) and replaces it with its original source element. Adds
        /// the cloned element's "transform" attribute so that its replacement inherits all its position and scaling.
        /// The "use" element is a SVG mechanism to clone an object. Inkscape leverages "use" for its object
        /// cloning mechanism.
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/SVG/Reference/Element/use">See Mozilla documentation</see>.
        /// </summary>
        /// <param name="svgRoot">SVG root element of where to start the scan. Note that the parameter is internally mutated.</param>
        public void ScanSVGAndReplaceClonedElements(XElement svgRoot)
        {
            if (svgRoot == null) return;

            //Build the map of SVG elements and their source link, if they exist
            Dictionary<string, TraversalElementInfo> svgElementMap = new Dictionary<string, TraversalElementInfo>();
            TraverseSVGToAssembleElementMap(svgRoot, svgElementMap);

            // Now iterate our map, looking for any element that has a source link,
            // indicating that it is a clone that needs to be decloned
            foreach (var item in svgElementMap.Where(x => !string.IsNullOrWhiteSpace(x.Value.SourceID)))
            {
                TraversalTransformStack stack = FindSourceElement(svgElementMap, item.Key);
                if (stack == null || stack.OrginalElement == null) continue;

                XElement srcNode = stack.OrginalElement;
                XAttribute srcTransform = srcNode.Attribute("transform");

                // Assemble the stack of transforms to ensure each clone is positioned/scaled/etc properly
                // And tack on the final source element transform too.
                var transformSB = new StringBuilder();
                while (stack.Transforms.Count > 0)
                {
                    if (transformSB.Length > 0)
                    {
                        transformSB.Append(" ");
                    }
                    transformSB.Append(stack.Transforms.Pop());
                }
                if (srcTransform != null)
                {
                    if (transformSB.Length > 0)
                    {
                        transformSB.Append(" ");
                    }
                    transformSB.Append(srcTransform.Value);
                }

                // Create the new "decloned" element based on the source,
                // replacing the transform attribute with the stack we built
                XElement declonedElement = new XElement(srcNode);
                declonedElement.SetAttributeValue(
                        "transform", transformSB.ToString()
                        );

                // Now, swap out the existing "use" element with the "copy" of the source
                item.Value.XElement.ReplaceWith(declonedElement);
            }
        }
        #endregion

        #region FindSourceElement
        /// <summary>
        /// Recursively traverses a map of SVG elements to locate the original source element 
        /// for a given clone ID. Accumulates any transform attributes along the traversal path 
        /// into a stack within the resulting <c>TraversalTransformStack</c>.
        /// Returns <c>null</c> if the source cannot be found or recursion exceeds the safety limit.
        /// </summary>
        /// <param name="svgElementMap">
        /// A dictionary mapping element IDs to <c>TraversalElementInfo</c> instances, 
        /// representing the available SVG elements and their metadata.
        /// </param>
        /// <param name="idOfClone">
        /// The ID of the cloned element whose original source is to be resolved.
        /// </param>
        /// <param name="recurseCount">
        /// Optional parameter to track recursion depth for safety checks. 
        /// Defaults to 0 and should not be manually set by consumers.
        /// </param>
        /// <returns>
        /// A <c>TraversalTransformStack</c> initialized with the source element and accumulated transforms,
        /// or <c>null</c> if the source is invalid, unreachable, or exceeds recursion limits.
        /// </returns>
        protected TraversalTransformStack FindSourceElement(Dictionary<string, TraversalElementInfo> svgElementMap,
            string idOfPotentialClone, int recurseCount = 0)
        {
            var isFound = svgElementMap.TryGetValue(idOfPotentialClone, out TraversalElementInfo cloneInfo);
            recurseCount = recurseCount + 1;
            if (recurseCount > CLONE_NESTING_LIMIT)
            {
                // Assume the worse. It's a corrupt SVG with an infinite loop reference problem.
                // Bail out to prevent stack overflow.
                Logger.LogMessage("CloneReplace", "Aborting clone nesting deeper than {0}. Clone: '{1}' Source: '{2}'", CLONE_NESTING_LIMIT, idOfPotentialClone, cloneInfo.SourceID);
                cloneInfo.MarkDeadEnd();
                return null;
            }
            if (isFound)
            {
                if (cloneInfo.SourceID != null)
                {
                    var srcElement = FindSourceElement(svgElementMap, cloneInfo.SourceID, recurseCount);
                    var transform = cloneInfo.XElement.Attribute("transform");
                    if (srcElement != null && transform != null)
                    {
                        srcElement.Transforms.Push(transform.Value);
                    }
                    return srcElement;
                }
                else
                {
                    return new TraversalTransformStack(cloneInfo.XElement);
                }
            }
            else
            {
                // Probably a bogus/missing/broken/external href/link that we don't handle. Skip it.
                // Could be something like an embedded image too:  xlink:href="data:image/png;base64...
                return null;
            }
        }
        #endregion

        #region TraverseSVGToAssembleElementMap
        /// <summary>
        /// Recursively traverses the SVG to assemble a dictionary of all elements with an "id" attribute.
        /// The dictionary is essential for the clone replacement process.
        /// </summary>
        /// <param name="svgElement">The SVG element to traverse</param>
        /// <param name="svgElementMap">Dictionary/map used to house the elements.</param>
        protected void TraverseSVGToAssembleElementMap(XElement svgElement, Dictionary<string, TraversalElementInfo> svgElementMap)
        {
            var idAttr = svgElement.Attribute("id");
            if (idAttr != null)
            {
                // Ideally we wouldn't need this check, but being defensive
                if (!svgElementMap.ContainsKey(idAttr.Value))
                {
                    svgElementMap[idAttr.Value] = new TraversalElementInfo(svgElement);
                }
            }
            foreach (var child in svgElement.Elements())
            {
                TraverseSVGToAssembleElementMap(child, svgElementMap);
            }
        }
        #endregion
    }

    #region TraversalTransformStack Class
    public class TraversalTransformStack
    {
        public Stack<string> Transforms { get; } = new Stack<string>();
        public readonly XElement OrginalElement = null;
        public TraversalTransformStack(XElement originalElement)
        {
            OrginalElement = originalElement;
        }
    }
    #endregion

    #region TraversalElementInfo Class
    /// <summary>
    /// Used to store easy to access information about the XElement and if it's a clone.
    /// Helps during the SVG ingestion process to remap "use" elements (a.k.a. clones) to their source.
    /// </summary>
    public class TraversalElementInfo
    {
        /// <summary>
        /// Namespace for the "xlink:href" attribute used by a cloned SVG element to "point" at its source.
        /// Note SVG2 supposedly doesn't need the namespace.
        /// </summary>
        private static XNamespace xlink = "http://www.w3.org/1999/xlink";

        public XElement XElement { get; private set; }
        public string SourceID { get; private set; } = null;

        /// <summary>
        /// Constructs and computes any LinkedToID if necessary.
        /// </summary>
        /// <param name="svgElement"></param>
        public TraversalElementInfo(XElement svgElement)
        {
            XElement = svgElement;

            var linkAtribute = svgElement.Attribute(xlink + "href") ?? svgElement.Attribute("href"); //handle both SVG1.1 and SVG 2
            if (linkAtribute != null)
            {
                var v = linkAtribute.Value ?? "";
                SourceID = v.StartsWith("#") ? v.Substring(1) : v; // Remove the leading "#" if it exists
            }
        }

        /// <summary>
        /// Marks the element as a dead end. Useful in the case of a recursion problem.
        /// </summary>
        public void MarkDeadEnd()
        {
            this.SourceID = null;
        }
    } 
    #endregion
}
