using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using DependencyObjectAndLevel = System.Tuple<Windows.UI.Xaml.DependencyObject, int>;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension metods for <see cref="DependencyObject"/> instances.
    /// </summary>
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Enumerates child objects of a <see cref="DependencyObject"/> in a breadth-first search, 
        /// up to a maximum level of deepness.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isRecursive"></param>
        /// <param name="maxLevels"></param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> GetChildren(this DependencyObject obj, bool isRecursive = true, int maxLevels = 10)
        {
            // For performance, we don't want a depth-first tree search.
            var queue = new Queue<DependencyObjectAndLevel>();
            queue.Enqueue(new DependencyObjectAndLevel(obj, 0));

            while (queue.Count > 0)
            {
                var info = queue.Dequeue();
                var count = VisualTreeHelper.GetChildrenCount(info.Item1);

                // Avoid "if" in loop, better to loop twice.
                for (int i = 0; i < count; i++)
                {
                    yield return VisualTreeHelper.GetChild(info.Item1, i);
                }

                if (isRecursive && (info.Item2 < maxLevels - 1))
                {
                    for (int i = 0; i < count; i++)
                    {
                        queue.Enqueue(new DependencyObjectAndLevel(VisualTreeHelper.GetChild(info.Item1, i), info.Item2 + 1));
                    }
                }
            }
        }

        /// <summary>
        /// Enumerates all parents of a <see cref="DependencyObject"/> from the most immedate to the topmost.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> GetParents(this DependencyObject obj)
        {
            obj = VisualTreeHelper.GetParent(obj);

            while (obj != null)
            {
                yield return obj;

                obj = VisualTreeHelper.GetParent(obj);
            }
        }

        /// <summary>
        /// Searches for the first object of type <typeparamref name="T"/> in children 
        /// (or optionally self) of a <see cref="DependencyObject"/>, in a breadth-first search.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="includeSelf"></param>
        /// <returns></returns>
        public static T FindFirstChild<T>(this DependencyObject obj, bool includeSelf = false)
            where T : DependencyObject
        {
            if (includeSelf && (obj is T))
            {
                // Uno
                // return obj as T;
                return (T)obj;
            }

            return obj
                .GetChildren()
                .OfType<T>()
                .FirstOrDefault();
        }

        /// <summary>
        /// Searches for the first object of type <typeparamref name="T"/> in parents (or self) 
        /// of a <see cref="DependencyObject"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="includeSelf"></param>
        /// <returns></returns>
        public static T FindFirstParent<T>(this DependencyObject obj, bool includeSelf = false)
            where T : DependencyObject
        {
            if (includeSelf && (obj is T))
            {
                // Uno
                // return obj as T;
                return (T)obj;
            }

            return obj
                .GetParents()
                .OfType<T>()
                .FirstOrDefault();
        }
    }
}
