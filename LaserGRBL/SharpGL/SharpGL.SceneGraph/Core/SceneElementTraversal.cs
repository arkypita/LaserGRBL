using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.SceneGraph.Core
{
    public partial class SceneElement
    {
        /// <summary>
        /// Traverses this instance. Traversing simply returns 
        /// the children and all descendents in the correct order.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SceneElement> Traverse()
        {
            //  Go through each child element...
            foreach (var sceneElement in children)
            {
                //  ...yield it...
                yield return sceneElement;

                //  ...and recurse.
                foreach (var descenedent in sceneElement.Traverse())
                    yield return descenedent;
            }
        }

        /// <summary>
        /// Traverses this instance. Traversing simply returns
        /// the children and all descendents in the correct order.
        /// This traversal allows for a predicate to be used.
        /// </summary>
        /// <param name="predicate">The predicate for traversal.</param>
        /// <returns></returns>
        public IEnumerable<SceneElement> Traverse(Predicate<SceneElement> predicate)
        {
            //  Go through each child element...
            foreach (var sceneElement in children)
            {
                //  ...yield it...
                if (predicate(sceneElement))
                    yield return sceneElement;

                //  ...and recurse.
                foreach (var descenedent in sceneElement.Traverse())
                    if (predicate(descenedent))
                        yield return descenedent;
            }
        }

        /// <summary>
        /// Traverses this instance. Traversing simply returns 
        /// the children and all descendents of type T in the correct order.
        /// </summary>
        /// <typeparam name="T">The type of object to traverse for.</typeparam>
        /// <returns>The full set of T objects in the scene.</returns>
        public IEnumerable<T> Traverse<T>() where T : SceneElement
        {
            //  Go through each child element...
            foreach (var sceneElement in children)
            {
                //  ...yield it...
                if (sceneElement is T)
                    yield return (T)sceneElement;

                //  ...and recurse.
                foreach (var descenedent in sceneElement.Traverse())
                    if (descenedent is T)
                        yield return (T)descenedent;
            }
        }

        /// <summary>
        /// Traverses this instance. Traversing simply returns
        /// the children and all descendents of type T in the correct order.
        /// </summary>
        /// <typeparam name="T">The type of object to traverse for.</typeparam>
        /// <param name="predicate">The predicate for traversal.</param>
        /// <returns>
        /// The full set of T objects in the scene.
        /// </returns>
        public IEnumerable<T> Traverse<T>(Predicate<T> predicate) where T : SceneElement
        {
            //  Go through each child element...
            foreach (var sceneElement in children)
            {
                //  ...yield it...
                if (sceneElement is T)
                    if(predicate(sceneElement as T))
                        yield return (T)sceneElement;

                //  ...and recurse.
                foreach (var descenedent in sceneElement.Traverse())
                    if (descenedent is T)
                        if(predicate(descenedent as T))
                            yield return (T)descenedent;
            }
        }

        /// <summary>
        /// Traverses to root element.
        /// </summary>
        /// <returns></returns>
        public SceneContainer TraverseToRootElement()
        {
            //  Go through elements until we find the top level one.
            var current = this;
            while (current.Parent != null)
                current = current.Parent;
            
            //  Return the root element.
            return current as SceneContainer;
        }
    }
}
