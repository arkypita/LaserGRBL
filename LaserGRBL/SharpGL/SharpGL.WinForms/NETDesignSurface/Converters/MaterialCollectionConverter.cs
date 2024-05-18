using System;
using System.ComponentModel;
using System.Globalization;

namespace SharpGL.WinForms.NETDesignSurface.Converters
{
	/// <summary>
	/// This converts the Material Collection into something more functional.
	/// </summary>
	internal class MaterialCollectionConverter : System.ComponentModel.CollectionConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context,
			CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				if (value is System.Collections.ICollection)
					return "Materials";
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}