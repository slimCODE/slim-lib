using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using slimCODE.Extensions;
using slimCODE.Models;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace Seclusion.Models
{
    public class GameViewModel : BaseViewModel
    {
        public GameViewModel()
        {
            var okCommand = this.CreateObservableCommand("OkCommand");
            var cancelCommand = this.CreateObservableCommand("CancelCommand");
            var postponeCommand = this.CreateObservableCommand("PostponeCommand");

            // Step and StepResult are interdependant.
            IObservable<Unit> resultObservable = null;

            var stepProperty = this.CreateProperty(
                "Step", 
                () => ObservableLogic.ObserveStep(resultObservable), 
                Step.Start);

            var stepResultProperty = this.CreateProperty(
                "StepResult",
                () => ObservableLogic.ObserveStepResult(stepProperty, okCommand, cancelCommand, postponeCommand));

            resultObservable = stepResultProperty.SkipHot().SelectUnit();

            this.CreateProperty(
                "IsCancelVisible",
                () => ObservableLogic.ObserveCancelVisible(stepProperty));

            this.CreateProperty(
                "IsPostponeVisible",
                () => ObservableLogic.ObservePostponeVisible(stepProperty));

            var enemyLifeProperty = this.CreateProperty(
                "EnemyLife", 
                () => ObservableLogic.ObserveReset(stepProperty).Select(_ => 20), 
                20);

            var yourLifeProperty = this.CreateProperty(
                "YourLife", 
                () => ObservableLogic.ObserveReset(stepProperty).Select(_ => 20), 
                20);

            var enemyInHandProperty = this.CreateProperty(
                "EnemyInHand", 
                externalInHand => ObservableLogic.ObserveInHand(externalInHand, stepProperty, stepResultProperty), 
                7);

            var wouldPlayLandProperty = this.CreateProperty(
                "WouldPlayLand", 
                () => ObservableLogic.ObserveWouldPlayLand(stepProperty, enemyInHandProperty));

            var landsViewModel = this
                .AddChildProperty(
                    "LandsViewModel",
                    new LandsViewModel(
                        () => ObservableLogic.ObserveReset(stepProperty),
                        () => ObservableLogic.ObserveUntap(stepResultProperty),
                        () => ObservableLogic.ObservePlayLand(wouldPlayLandProperty, stepResultProperty),
                        this));

            var wouldPlayCreatureProperty = this.CreateProperty(
                "WouldPlayCreature",
                () => ObservableLogic.ObserveWouldPlayCreature(
                    stepProperty,
                    enemyInHandProperty,
                    landsViewModel.UntappedLandsObservable.Select(lands => lands.Count()),
                    ObservableLogic.ObserveUntap(stepResultProperty),
                    this));

            this.CreateProperty(
                "Message", 
                () => ObservableLogic.ObserveMessage(stepProperty, wouldPlayLandProperty, wouldPlayCreatureProperty));

            var creaturesViewModel = this
                .AddChildProperty(
                    "CreaturesViewModel",
                    new CreaturesViewModel(
                        () => ObservableLogic.ObserveReset(stepProperty),
                        () => ObservableLogic.ObservePlayCreature(wouldPlayCreatureProperty, stepResultProperty),
                        this));
        }
    }
}
