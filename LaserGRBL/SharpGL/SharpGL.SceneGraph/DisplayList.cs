using System;

namespace SharpGL.SceneGraph
{
	/// <summary>
	/// The display list class basicly wraps an OpenGL display list, making them easier
	/// to manage. Remember this class is completely OpenGL dependant. In time this class
	/// will derive from the IOpenGLDependant interface.
	/// </summary>
	public class DisplayList
	{
		public enum DisplayListMode
		{
            Compile = (int)OpenGL.GL_COMPILE,
			CompileAndExecute = (int)OpenGL.GL_COMPILE_AND_EXECUTE,
		}

		public DisplayList()
		{
		}

		/// <summary>
		/// This function generates the display list. You must call it before you call
		/// anything else!
		/// </summary>
		/// <param name="gl">OpenGL</param>
		public virtual void Generate(OpenGL gl)
		{
			//	Generate one list.
			list = gl.GenLists(1);
		}

		/// <summary>
		/// This function makes the display list.
		/// </summary>
		/// <param name="gl">OpenGL</param>
		/// <param name="mode">The mode, compile or compile and execute.</param>
		public virtual void New(OpenGL gl, DisplayListMode mode)
		{
			//	Start the list.
			gl.NewList(list, (uint)mode);
		}

		/// <summary>
		/// This function ends the compilation of a list.
		/// </summary>
		/// <param name="gl"></param>
		public virtual void End(OpenGL gl)
		{
			//	This function ends the display list
			gl.EndList();
		}

		public virtual bool IsList(OpenGL gl)
		{
			//	Is the list a proper display list?
			if(gl.IsList(list) == OpenGL.GL_TRUE)
				return true;
			else
				return false;
		}

		public static bool IsList(OpenGL gl, DisplayList displayList)
		{
			//	Is the specified list a proper display list?
			if(gl.IsList(displayList.list) == OpenGL.GL_TRUE)
				return true;
			else
				return false;
		}

		public virtual void Call(OpenGL gl)
		{
			gl.CallList(list);
		}

		public virtual void Delete(OpenGL gl)
		{
			gl.DeleteLists(list, 1);
			list = 0;
		}

		protected uint list = 0;

		public uint List
		{
			get {return list;}
		}
	}
}
