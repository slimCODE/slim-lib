using System;
using System.Collections.Generic;
using System.Text;

namespace slimCODE.Models
{
    /// <summary>
    /// Enumeration of possible view-model states.
    /// </summary>
    public enum ViewModelState
    {
        /// <summary>
        /// The view-model was never loaded yet.
        /// </summary>
        NotLoaded = 0,
        /// <summary>
        /// The view-model is loaded and active.
        /// </summary>
        Loaded = 1,
        /// <summary>
        /// The view-model was previously loaded, but is now unloaded.
        /// </summary>
        Unloaded = 2,
    }
}
