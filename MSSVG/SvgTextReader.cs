using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Specialized;

namespace Svg
{
    internal sealed class SvgTextReader : XmlTextReader
    {
        private Dictionary<string, string> _entities;
        private string _value;
        private bool _customValue = false;
        private string _localName;

        public SvgTextReader(Stream stream, Dictionary<string, string> entities)
            : base(stream)
        {
            this.EntityHandling = EntityHandling.ExpandEntities;
            this._entities = entities;
        }

        public SvgTextReader(TextReader reader, Dictionary<string, string> entities)
            : base(reader)
        {
            this.EntityHandling = EntityHandling.ExpandEntities;
            this._entities = entities;
        }

        /// <summary>
        /// Gets the text value of the current node.
        /// </summary>
        /// <value></value>
        /// <returns>The value returned depends on the <see cref="P:System.Xml.XmlTextReader.NodeType"/> of the node. The following table lists node types that have a value to return. All other node types return String.Empty.Node Type Value AttributeThe value of the attribute. CDATAThe content of the CDATA section. CommentThe content of the comment. DocumentTypeThe internal subset. ProcessingInstructionThe entire content, excluding the target. SignificantWhitespaceThe white space within an xml:space= 'preserve' scope. TextThe content of the text node. WhitespaceThe white space between markup. XmlDeclarationThe content of the declaration. </returns>
        public override string Value
        {
            get
            {
                return (this._customValue) ? this._value : base.Value;
            }
        }

        /// <summary>
        /// Gets the local name of the current node.
        /// </summary>
        /// <value></value>
        /// <returns>The name of the current node with the prefix removed. For example, LocalName is book for the element &lt;bk:book&gt;.For node types that do not have a name (like Text, Comment, and so on), this property returns String.Empty.</returns>
        public override string LocalName
        {
            get
            {
                return (this._customValue) ? this._localName : base.LocalName;
            }
        }

        private IDictionary<string, string> Entities
        {
            get
            {
                if (this._entities == null)
                {
                    this._entities = new Dictionary<string, string>();
                }

                return this._entities;
            }
        }

        /// <summary>
        /// Moves to the next attribute.
        /// </summary>
        /// <returns>
        /// true if there is a next attribute; false if there are no more attributes.
        /// </returns>
        public override bool MoveToNextAttribute()
        {
            bool moved = base.MoveToNextAttribute();

            if (moved)
            {
                this._localName = base.LocalName;

                if (this.ReadAttributeValue())
                {
                    if (this.NodeType == XmlNodeType.EntityReference)
                    {
                        this.ResolveEntity();
                    }
                    else
                    {
                        this._value = base.Value;
                    }
                }
                this._customValue = true;
            }

            return moved;
        }

        /// <summary>
        /// Reads the next node from the stream.
        /// </summary>
        /// <returns>
        /// true if the next node was read successfully; false if there are no more nodes to read.
        /// </returns>
        /// <exception cref="T:System.Xml.XmlException">An error occurred while parsing the XML. </exception>
        public override bool Read()
        {
            this._customValue = false;
            bool read = base.Read();

            if (this.NodeType == XmlNodeType.DocumentType)
            {
                this.ParseEntities();
            }

            return read;
        }

        private void ParseEntities()
        {
            const string entityText = "<!ENTITY";
            string[] entities = this.Value.Split(new string[]{entityText}, StringSplitOptions.None);
            string name = null;
            string value = null;
            int quoteIndex;

            foreach (string entity in entities)
            {
                if (string.IsNullOrEmpty(entity.Trim()))
                {
                    continue;
                }

                name = entity.Trim();
                quoteIndex = name.IndexOf(this.QuoteChar);
                if (quoteIndex > 0)
                {
                    value = name.Substring(quoteIndex + 1, name.LastIndexOf(this.QuoteChar) - quoteIndex - 1);
                    name = name.Substring(0, quoteIndex).Trim();
                    this.Entities.Add(name, value);
                }
            }
        }

        /// <summary>
        /// Resolves the entity reference for EntityReference nodes.
        /// </summary>
        public override void ResolveEntity()
        {
            if (this.NodeType == XmlNodeType.EntityReference)
            {
                if (this._entities.ContainsKey(this.Name))
                {
                    this._value = this._entities[this.Name];
                }
                else
                {
                    this._value = string.Empty;
                }

                this._customValue = true;
            }
        }
    }
}