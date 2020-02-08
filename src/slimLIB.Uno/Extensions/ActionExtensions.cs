using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Core;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Action"/> instances.
    /// </summary>
    public static class ActionExtensions
    {
        /// <summary>
        /// Executes an action, logging an error if the current thread is not on the expected thread.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="dispatcher"></param>
        /// <param name="mustBeOnUIThread">Indicates if the expected thread is the main UI thread.</param>
        public static void VerifyThread(this Action action, CoreDispatcher dispatcher, bool mustBeOnUIThread)
        {
            dispatcher.VerifyThread(mustBeOnUIThread);
            action();
        }
    }
}
