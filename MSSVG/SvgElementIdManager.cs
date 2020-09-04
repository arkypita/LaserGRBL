using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace Svg
{
    /// <summary>
    /// Provides methods to ensure element ID's are valid and unique.
    /// </summary>
    public class SvgElementIdManager
    {
        private SvgDocument _document;
        private Dictionary<string, SvgElement> _idValueMap;

        /// <summary>
        /// Retrieves the <see cref="SvgElement"/> with the specified ID.
        /// </summary>
        /// <param name="id">A <see cref="string"/> containing the ID of the element to find.</param>
        /// <returns>An <see cref="SvgElement"/> of one exists with the specified ID; otherwise false.</returns>
        public virtual SvgElement GetElementById(string id)
        {
            if (id.StartsWith("url("))
            {
                id = id.Substring(4);
                id = id.TrimEnd(')');
            }
            if (id.StartsWith("#"))
            {
                id = id.Substring(1);
            }

            SvgElement element = null;
            this._idValueMap.TryGetValue(id, out element);

            return element;
        }

        public virtual SvgElement GetElementById(Uri uri)
        {
            if (uri.ToString().StartsWith("url(")) uri = new Uri(uri.ToString().Substring(4).TrimEnd(')'), UriKind.Relative);
            if (!uri.IsAbsoluteUri && this._document.BaseUri != null && !uri.ToString().StartsWith("#"))
            {
                var fullUri = new Uri(this._document.BaseUri, uri);
                var hash = fullUri.OriginalString.Substring(fullUri.OriginalString.LastIndexOf('#'));
                SvgDocument doc;
                switch (fullUri.Scheme.ToLowerInvariant())
                {
                    case "file":
                        doc = SvgDocument.Open<SvgDocument>(fullUri.LocalPath.Substring(0, fullUri.LocalPath.Length - hash.Length));
                        return doc.IdManager.GetElementById(hash);
                    case "http":
                    case "https":
                        var httpRequest = WebRequest.Create(uri);
                        using (WebResponse webResponse = httpRequest.GetResponse())
                        {
                            doc = SvgDocument.Open<SvgDocument>(webResponse.GetResponseStream());
                            return doc.IdManager.GetElementById(hash);
                        }
                    default:
                        throw new NotSupportedException();
                }

            }
            return this.GetElementById(uri.ToString());
        }

        /// <summary>
        /// Adds the specified <see cref="SvgElement"/> for ID management.
        /// </summary>
        /// <param name="element">The <see cref="SvgElement"/> to be managed.</param>
        public virtual void Add(SvgElement element)
        {
            AddAndForceUniqueID(element, null, false);
        }

        /// <summary>
        /// Adds the specified <see cref="SvgElement"/> for ID management. 
        /// And can auto fix the ID if it already exists or it starts with a number.
        /// </summary>
        /// <param name="element">The <see cref="SvgElement"/> to be managed.</param>
        /// <param name="autoForceUniqueID">Pass true here, if you want the ID to be fixed</param>
        /// <param name="logElementOldIDNewID">If not null, the action is called before the id is fixed</param>
        /// <returns>true, if ID was altered</returns>
        public virtual bool AddAndForceUniqueID(SvgElement element, SvgElement sibling, bool autoForceUniqueID = true, Action<SvgElement, string, string> logElementOldIDNewID = null)
        {
            var result = false;
            if (!string.IsNullOrEmpty(element.ID))
            {
                var newID = this.EnsureValidId(element.ID, autoForceUniqueID);
                if (autoForceUniqueID && newID != element.ID)
                {
                    if(logElementOldIDNewID != null)
                        logElementOldIDNewID(element, element.ID, newID);
                    element.ForceUniqueID(newID);
                    result = true;
                }
                this._idValueMap.Add(element.ID, element);
            }
            
            OnAdded(element);
            return result;
        }

        /// <summary>
        /// Removed the specified <see cref="SvgElement"/> from ID management.
        /// </summary>
        /// <param name="element">The <see cref="SvgElement"/> to be removed from ID management.</param>
        public virtual void Remove(SvgElement element)
        {
            if (!string.IsNullOrEmpty(element.ID))
            {
                this._idValueMap.Remove(element.ID);
            }
            
            OnRemoved(element);
        }

        /// <summary>
        /// Ensures that the specified ID is valid within the containing <see cref="SvgDocument"/>.
        /// </summary>
        /// <param name="id">A <see cref="string"/> containing the ID to validate.</param>
        /// <param name="autoForceUniqueID">Creates a new unique id <see cref="string"/>.</param>
        /// <exception cref="SvgException">
        /// <para>The ID cannot start with a digit.</para>
        /// <para>An element with the same ID already exists within the containing <see cref="SvgDocument"/>.</para>
        /// </exception>
        public string EnsureValidId(string id, bool autoForceUniqueID = false)
        {

            if (string.IsNullOrEmpty(id))
            {
                return id;
            }

            if (char.IsDigit(id[0]))
            {
                if (autoForceUniqueID)
                {
                    return EnsureValidId("id" + id, true);
                }
                throw new SvgIDWrongFormatException("ID cannot start with a digit: '" + id + "'.");
            }

            if (this._idValueMap.ContainsKey(id))
            {
                if(autoForceUniqueID)
                {
                    var match = regex.Match(id);

                    int number;
                    if (match.Success && int.TryParse(match.Value.Substring(1), out number))
                    {
                        id = regex.Replace(id, "#" + (number + 1));
                    }
                    else
                    {
                        id += "#1";
                    }

                    return EnsureValidId(id, true);
                }
                throw new SvgIDExistsException("An element with the same ID already exists: '" + id + "'.");
            }

            return id;
        }
        private static readonly Regex regex = new Regex(@"#\d+$");

        /// <summary>
        /// Initialises a new instance of an <see cref="SvgElementIdManager"/>.
        /// </summary>
        /// <param name="document">The <see cref="SvgDocument"/> containing the <see cref="SvgElement"/>s to manage.</param>
        public SvgElementIdManager(SvgDocument document)
        {
            this._document = document;
            this._idValueMap = new Dictionary<string, SvgElement>();
        }
        
        public event EventHandler<SvgElementEventArgs> ElementAdded;
        public event EventHandler<SvgElementEventArgs> ElementRemoved;
        
        protected void OnAdded(SvgElement element)
        {
        	var handler = ElementAdded;
        	if(handler != null)
        	{
        		handler(this._document, new SvgElementEventArgs{ Element = element });
        	}
        }
        
        protected void OnRemoved(SvgElement element)
        {
        	var handler = ElementRemoved;
        	if(handler != null)
        	{
        		handler(this._document, new SvgElementEventArgs{ Element = element });
        	}
        }
        
    }
    
    public class SvgElementEventArgs : EventArgs
    {
    	public SvgElement Element;
    }
}