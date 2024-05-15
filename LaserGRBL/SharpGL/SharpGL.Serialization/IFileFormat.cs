using System;
using System.IO;

using SharpGL.SceneGraph;

namespace SharpGL.Serialization
{
	/// <summary>
	/// A Format class has the functionality to load data from a certain type of file.
	/// </summary>
	public interface IFileFormat
	{
        /// <summary>
        /// Load the data from the specified file stream. The data
        /// should be loaded into a scene object. Also, for consistency
        /// the ObjectLoaded event should be fired every time an object
        /// (such as a polygon or material) is loaded.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The scene or null if loading failed.</returns>
		Scene LoadData(string path);

        /// <summary>
        /// Saves the scene to the specified stream.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <param name="path">The path.</param>
        /// <returns>True if saved correctly.</returns>
		bool SaveData(Scene scene, string path);
        
		/// <summary>
		/// This property returns an array of file types that can be used with this
		/// format, e.g the CaligariFormat would return "cob", "scn".
		/// </summary>
	    string[] FileTypes
		{
			get;
		}

		/// <summary>
		/// This gets a filter suitable for a file open/save dialog, e.g 
		/// "Caligari trueSpace Files (*.cob, *.scn)|*.cob;*.scn".
		/// </summary>
		string Filter
		{
			get;
		}
	}
}