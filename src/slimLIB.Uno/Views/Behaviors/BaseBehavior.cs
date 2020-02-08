using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xaml.Interactivity;
using slimCODE.Extensions;
using slimCODE.Tools;
using Windows.UI.Xaml;

namespace slimCODE.Views.Behaviors
{
    public abstract class BaseBehavior<T> : Behavior
        where T : FrameworkElement
    {
        private SerialDisposable _attachedSubscription = new SerialDisposable();

        public T AssociatedElement
        {
            get { return this.AssociatedObject as T; }
        }

        protected override void OnAttached()
        {
            if (this.AssociatedElement == null)
            {
                throw new NotSupportedException($"The attached element is not of the correct type. A {typeof(T).Name} was expected.");
            }

            _attachedSubscription.Disposable = new CompositeDisposable(
                this.Attach(),
                this.ReattachOnLoaded());
        }

        protected override void OnDetaching()
        {
            _attachedSubscription.Disposable = null;
        }

        protected abstract IDisposable Attach();

        private IDisposable ReattachOnLoaded()
        {
            return this
                .AssociatedElement
                .ObserveLoaded()
                .Subscribe(
                    _ => OnAttached(),
                    error => Logs.UseLogger(l => l.ReportError("Observing Loaded on associated object.", error)));
        }
    }
}
