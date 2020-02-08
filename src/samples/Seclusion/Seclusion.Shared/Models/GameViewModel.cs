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
                .CreateProperty(
                    "LandsViewModel",
                    this.AddChild(new LandsViewModel(
                        () => ObservableLogic.ObserveReset(stepProperty),
                        () => ObservableLogic.ObserveUntap(stepResultProperty),
                        () => ObservableLogic.ObservePlayLand(wouldPlayLandProperty, stepResultProperty))))
                .Latest;

            var wouldPlayCreatureProperty = this.CreateProperty(
                "WouldPlayCreature",
                () => ObservableLogic.ObserveWouldPlayCreature(
                    stepProperty,
                    enemyInHandProperty,
                    landsViewModel.UntappedLandsObservable.Select(lands => lands.Count()),
                    ObservableLogic.ObserveUntap(stepResultProperty)));

            this.CreateProperty(
                "Message", 
                () => ObservableLogic.ObserveMessage(stepProperty, wouldPlayLandProperty, wouldPlayCreatureProperty));

            var creaturesViewModel = this
                .CreateProperty(
                    "CreaturesViewModel",
                    this.AddChild(new CreaturesViewModel(
                        () => ObservableLogic.ObserveReset(stepProperty),
                        () => ObservableLogic.ObservePlayCreature(wouldPlayCreatureProperty, stepResultProperty))))
                .Latest;

            #region Old
            //var reset = this.CreateObservableCommand("Reset");
            ////-
            //var untapAll = this.CreateObservableCommand("UntapAll");

            //var enemyViewModel = new EnemyViewModel(reset);
            //this.CreateProperty("EnemyViewModel", this.AddChild(enemyViewModel));

            //var playLand = this.CreateObservableCommand<int, object, bool>(
            //    "PlayLand",
            //    enemyViewModel.InHandObservable,
            //    (ct, inHand, _) => this.PlayLand(ct, inHand));

            //var landsViewModel = this.AddChild(new LandsViewModel(reset, untapAll, playLand.WhereTrue().SelectUnit()));

            //var playCreature = this.CreateObservableCommand<int, int, object, CreatureViewModel>(
            //    "PlayCreature",
            //    enemyViewModel.InHandObservable,
            //    landsViewModel.UntappedLandsObservable.Select(u => u.Count()),
            //    (ct, inHand, untapped, _) => this.PlayCreature(ct, inHand, untapped));



            //this.CreateProperty("LandsViewModel", landsViewModel);
            //this.CreateProperty("CreaturesViewModel", this.AddChild(new CreaturesViewModel(reset, untapAll, landsViewModel)));
            //this.CreateProperty("PlayerViewModel", this.AddChild(new PlayerViewModel(reset)));
            #endregion
        }




        //private Task<bool> PlayLand(CancellationToken ct, int inHand)
        //{
        //    var roll = _random.Next(6);
        //    var index = Math.Min(_playLandChance.Length - 1, inHand);

        //    return Task.FromResult(_playLandChance[index] > roll);
        //}

        //private async Task<CreatureViewModel> PlayCreature(CancellationToken ct, int inHand, int untapped)
        //{
        //    if (untapped <= 0)
        //    {
        //        await this.ShowMessage(ct, "Sorry, the enemy doesn't have any untapped lands.");
        //        return null;
        //    }

        //    // TODO: Play or not.

        //    var power = _random.Next() % Math.Min(6, untapped) + 1;
        //    var abilities = new List<Ability>();

        //    var abilityRank = _random.Next(6) + _random.Next(6);

        //    System.Diagnostics.Debug.WriteLine(abilityRank);

        //    switch (_random.Next(6) + _random.Next(6))
        //    {
        //        case 0:
        //        case 1:
        //            abilities.Add(Ability.Deathtouch);
        //            break;

        //        case 2:
        //        case 3:
        //            abilities.Add(Ability.Lifelink);
        //            break;

        //        case 4:
        //        case 5:
        //            abilities.Add(Ability.Flying);
        //            break;

        //        case 6:
        //        case 7:
        //            abilities.Add(Ability.FirstStrike);
        //            abilities.Add(Ability.Evolve);
        //            break;

        //        case 8:
        //            abilities.Add(Ability.Vigilence);
        //            abilities.Add(Ability.Evolve);
        //            break;

        //        case 9:
        //            abilities.Add(Ability.Hexproof);
        //            break;

        //        case 10:
        //            abilities.Add(Ability.Indestructible);
        //            power = Math.Max(1, power - 1);
        //            break;
        //    }

        //    var creature = new CreatureViewModel(untapAll, power, power, abilities.ToArray());

        //    await this.ShowDialog(
        //        ct,
        //        "ConfirmCreature",
        //        creature,
        //        async ct2 =>
        //        {
        //            landsViewModel.TapLands(power);
        //            collectionProxy.Add(this.AddChild(creature));
        //        });

        //}
    }
}
