using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slimCODE.Views
{
    public class Cancellable : ICancellable
    {
        private readonly Action _onCancel;

        public Cancellable(Action onCancel)
        {
            _onCancel = onCancel;
        }

        public void Cancel()
        {
            _onCancel();
        }
    }
}
