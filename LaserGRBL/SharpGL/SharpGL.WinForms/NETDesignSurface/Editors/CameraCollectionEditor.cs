using System;
using SharpGL.SceneGraph.Cameras;

namespace SharpGL.WinForms.NETDesignSurface.Editors
{
	/// <summary>
	/// This converts the Camera collection into something more usable (design time wise)
	/// by allowing all the types of camera to be added.
	/// </summary>
	internal class CameraCollectionEditor : System.ComponentModel.Design.CollectionEditor
	{
		public CameraCollectionEditor(Type type) : base(type) { }

		//	Return the types that you want to allow the user to add into your collection. 
		protected override Type[] CreateNewItemTypes()
		{
			return new Type[] { typeof(FrustumCamera), typeof(OrthographicCamera), typeof(PerspectiveCamera) };
		}
	}
}