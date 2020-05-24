using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using slimCODE.Extensions;
using slimCODE.Models;

namespace Plain.Shared.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            this.CreateProperty("Time", () => Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1)).Select(_ => DateTimeOffset.Now.ToString("T")));
        }
    }
}
