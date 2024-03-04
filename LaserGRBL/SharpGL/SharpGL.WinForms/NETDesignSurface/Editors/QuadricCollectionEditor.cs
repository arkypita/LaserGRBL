using SharpGL.SceneGraph.Quadrics;
using System;

namespace SharpGL.WinForms.NETDesignSurface.Editors
{
	/// <summary>
	/// This converts the Quadric Collection into something more functional, and
	/// allows you to add many types of quadrics.
	/// </summary>
	internal class QuadricCollectionEditor : System.ComponentModel.Design.CollectionEditor
	{
		public QuadricCollectionEditor(Type type) : base(type) { }

		//	Return the types that you want to allow the user to add into your collection. 
		protected override Type[] CreateNewItemTypes()
		{
			return new Type[] { typeof(Cylinder), typeof(Disk), typeof(Sphere) };
		}
	}
}