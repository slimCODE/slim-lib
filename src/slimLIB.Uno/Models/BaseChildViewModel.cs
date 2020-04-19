using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Core;

namespace slimCODE.Models
{
    /// <summary>
    /// Represents a view-model that is created by another view-model. As opposed to main view-models,
    /// child view-models do not get injected a CoreDispatcher automatically. In order to enforce this
    /// propagation, child view-models must have this base type.
    /// </summary>
    public abstract class BaseChildViewModel : BaseViewModel
    {
        protected BaseChildViewModel(BaseViewModel parent)
            : base (parent.Dispatcher)
        {
            parent.AddChild(this);
        }
    }
}
