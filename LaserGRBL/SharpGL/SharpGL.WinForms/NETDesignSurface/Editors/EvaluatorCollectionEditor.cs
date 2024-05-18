using System;
using System.Collections;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Evaluators;

namespace SharpGL.WinForms.NETDesignSurface.Editors
{
	/// <summary>
	/// This converts the evaluator collection into something more usable (design time wise)
	/// by allowing all the types of evaluator to be added.
	/// </summary>
	internal class EvaluatorCollectionEditor : System.ComponentModel.Design.CollectionEditor
	{
		public EvaluatorCollectionEditor(Type type) : base(type) { }

		//	Return the types that you want to allow the user to add into your collection. 
		protected override Type[] CreateNewItemTypes()
		{
			return new Type[] { typeof(Evaluator1D), typeof(Evaluator2D), typeof(NurbsCurve), typeof(NurbsSurface) };
		}
	}
}