using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>
    /// A collection of Scalable Vector Attributes that can be inherited from the owner elements ancestors.
    /// </summary>
    public sealed class SvgAttributeCollection : Dictionary<string, object>
    {
        private SvgElement _owner;

        /// <summary>
        /// Initialises a new instance of a <see cref="SvgAttributeCollection"/> with the given <see cref="SvgElement"/> as the owner.
        /// </summary>
        /// <param name="owner">The <see cref="SvgElement"/> owner of the collection.</param>
        public SvgAttributeCollection(SvgElement owner)
        {
            this._owner = owner;
        }

        /// <summary>
        /// Gets the attribute with the specified name.
        /// </summary>
        /// <typeparam name="TAttributeType">The type of the attribute value.</typeparam>
        /// <param name="attributeName">A <see cref="string"/> containing the name of the attribute.</param>
        /// <returns>The attribute value if available; otherwise the default value of <typeparamref name="TAttributeType"/>.</returns>
        public TAttributeType GetAttribute<TAttributeType>(string attributeName)
        {
            if (this.ContainsKey(attributeName) && base[attributeName] != null)
            {
                return (TAttributeType)base[attributeName];
            }

            return this.GetAttribute<TAttributeType>(attributeName, default(TAttributeType));
        }

        /// <summary>
        /// Gets the attribute with the specified name.
        /// </summary>
        /// <typeparam name="T">The type of the attribute value.</typeparam>
        /// <param name="attributeName">A <see cref="string"/> containing the name of the attribute.</param>
        /// <param name="defaultValue">The value to return if a value hasn't already been specified.</param>
        /// <returns>The attribute value if available; otherwise the default value of <typeparamref name="T"/>.</returns>
        public T GetAttribute<T>(string attributeName, T defaultValue)
        {
            if (this.ContainsKey(attributeName) && base[attributeName] != null)
            {
                return (T)base[attributeName];
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the attribute with the specified name and inherits from ancestors if there is no attribute set.
        /// </summary>
        /// <typeparam name="TAttributeType">The type of the attribute value.</typeparam>
        /// <param name="attributeName">A <see cref="string"/> containing the name of the attribute.</param>
        /// <returns>The attribute value if available; otherwise the ancestors value for the same attribute; otherwise the default value of <typeparamref name="TAttributeType"/>.</returns>
        public TAttributeType GetInheritedAttribute<TAttributeType>(string attributeName)
        {
            if (this.ContainsKey(attributeName) && !IsInheritValue(base[attributeName]))
            {
                var result = (TAttributeType)base[attributeName];
                var deferred = result as SvgDeferredPaintServer;
                if (deferred != null) deferred.EnsureServer(_owner);
                return result;
            }

            if (this._owner.Parent != null)
            {
                var parentAttribute = this._owner.Parent.Attributes[attributeName];
                if (parentAttribute != null)
                {
                    return (TAttributeType)parentAttribute;
                }
            }

            return default(TAttributeType);
        }

        private bool IsInheritValue(object value)
        {
            return (value == null ||
                    (value is SvgFontWeight && (SvgFontWeight)value == SvgFontWeight.Inherit) ||
                    (value is SvgTextAnchor && (SvgTextAnchor)value == SvgTextAnchor.Inherit) ||
                    (value is SvgFontVariant && (SvgFontVariant)value == SvgFontVariant.Inherit) || 
                    (value is SvgTextDecoration && (SvgTextDecoration)value == SvgTextDecoration.Inherit) ||
                    (value is XmlSpaceHandling && (XmlSpaceHandling)value == XmlSpaceHandling.inherit) ||
                    (value is SvgOverflow && (SvgOverflow)value == SvgOverflow.Inherit) ||
                    (value is SvgColourServer && (SvgColourServer)value == SvgColourServer.Inherit) ||
                    (value is SvgShapeRendering && (SvgShapeRendering)value == SvgShapeRendering.Inherit) ||
                    (value is SvgTextRendering && (SvgTextRendering)value == SvgTextRendering.Inherit) ||
                    (value is SvgImageRendering && (SvgImageRendering)value == SvgImageRendering.Inherit) ||
                    (value is string && ((string)value).ToLower() == "inherit")
                   );
        }

        /// <summary>
        /// Gets the attribute with the specified name.
        /// </summary>
        /// <param name="attributeName">A <see cref="string"/> containing the attribute name.</param>
        /// <returns>The attribute value associated with the specified name; If there is no attribute the parent's value will be inherited.</returns>
        public new object this[string attributeName]
        {
            get { return this.GetInheritedAttribute<object>(attributeName); }
            set 
            {
            	if(base.ContainsKey(attributeName))
            	{
	            	var oldVal = base[attributeName];	            	
	            	if(TryUnboxedCheck(oldVal, value))
	            	{
	            		base[attributeName] = value;
	            		OnAttributeChanged(attributeName, value);
	            	}
            	}
            	else
            	{
            		base[attributeName] = value;
	            	OnAttributeChanged(attributeName, value);
            	}
            }
        }
        
        private bool TryUnboxedCheck(object a, object b)
        {
        	if(IsValueType(a))
        	{
        		if(a is SvgUnit)
        			return UnboxAndCheck<SvgUnit>(a, b);
        		else if(a is bool)
        			return UnboxAndCheck<bool>(a, b);
        		else if(a is int)
        			return UnboxAndCheck<int>(a, b);
        		else if(a is float)
        			return UnboxAndCheck<float>(a, b);
        		else if(a is SvgViewBox)
        			return UnboxAndCheck<SvgViewBox>(a, b);
        		else
        			return true;
        	}
        	else
        	{
        		return a != b;
        	}
        }
        
        private bool UnboxAndCheck<T>(object a, object b)
        {
        	return !((T)a).Equals((T)b);
        }
        
        private bool IsValueType(object obj) 
        {
        	return obj != null && obj.GetType().IsValueType;
        }
        
        /// <summary>
        /// Fired when an Atrribute has changed
        /// </summary>
        public event EventHandler<AttributeEventArgs> AttributeChanged;
        
        private void OnAttributeChanged(string attribute, object value)
        {
        	var handler = AttributeChanged;
        	if(handler != null)
        	{
        		handler(this._owner, new AttributeEventArgs { Attribute = attribute, Value = value });
        	}
        }
    }
    
    
    /// <summary>
    /// A collection of Custom Attributes
    /// </summary>
    public sealed class SvgCustomAttributeCollection : Dictionary<string, string>
    {
        private SvgElement _owner;

        /// <summary>
        /// Initialises a new instance of a <see cref="SvgAttributeCollection"/> with the given <see cref="SvgElement"/> as the owner.
        /// </summary>
        /// <param name="owner">The <see cref="SvgElement"/> owner of the collection.</param>
        public SvgCustomAttributeCollection(SvgElement owner)
        {
            this._owner = owner;
        }

        /// <summary>
        /// Gets the attribute with the specified name.
        /// </summary>
        /// <param name="attributeName">A <see cref="string"/> containing the attribute name.</param>
        /// <returns>The attribute value associated with the specified name; If there is no attribute the parent's value will be inherited.</returns>
        public new string this[string attributeName]
        {
        	get { return base[attributeName]; }
            set 
            {
            	if(base.ContainsKey(attributeName))
            	{
	            	var oldVal = base[attributeName];
	            	base[attributeName] = value;
	            	if(oldVal != value) OnAttributeChanged(attributeName, value);
            	}
            	else
            	{
            		base[attributeName] = value;
	            	OnAttributeChanged(attributeName, value);
            	}
            }
        }
        
        /// <summary>
        /// Fired when an Atrribute has changed
        /// </summary>
        public event EventHandler<AttributeEventArgs> AttributeChanged;
        
        private void OnAttributeChanged(string attribute, object value)
        {
        	var handler = AttributeChanged;
        	if(handler != null)
        	{
        		handler(this._owner, new AttributeEventArgs { Attribute = attribute, Value = value });
        	}
        }
    }
}