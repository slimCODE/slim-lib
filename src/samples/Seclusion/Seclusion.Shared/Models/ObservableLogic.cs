using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using slimCODE.Extensions;
using slimCODE.Models;

namespace Seclusion.Models
{
    public static class ObservableLogic
    {
        private static readonly int[] _playLandChance = { 0, 0, 1, 3, 5, 6 };
        private static readonly int[] _playCreatureChance = { 0, 4, 5, 6 };
        private static readonly int[] _playCreatureAfterSorceryChance = { 0, 2, 3, 4, 5, 6 };
        private static readonly Ability[][] _randomAbilities = new[] {
            new[] { Ability.Deathtouch },
            new[] { Ability.Deathtouch },
            new[] { Ability.Lifelink },
            new[] { Ability.Lifelink },
            new[] { Ability.Flying },
            new[] { Ability.Flying },
            new[] { Ability.FirstStrike, Ability.Evolve },
            new[] { Ability.FirstStrike, Ability.Evolve },
            new[] { Ability.Vigilence },
            new[] { Ability.Hexproof },
            new[] { Ability.Indestructible },
        };

        private static Random _random = new Random();

        public static IObservable<Step> ObserveStep(IObservable<Unit> resultObservable)
        {
            return resultObservable
                .SkipHot()
                .Scan(Step.Start, (step, _) => step.Next());
        }

        public static IObservable<(Step Step, Result Result)> ObserveStepResult(
            IHotObservable<Step> stepObservable,
            IObservable<Unit> okObservable,
            IObservable<Unit> cancelObservable,
            IObservable<Unit> postponeObservable)
        {
            return stepObservable
                .SelectManyCancelPrevious(step => Observable
                    .Merge(
                        okObservable.Select(_ => Result.Ok),
                        cancelObservable.Select(_ => Result.Cancel),
                        postponeObservable.Select(_ => Result.Postpone))
                    .Select(result => (step, result)));
        }

        public static IObservable<bool> ObserveCancelVisible(IObservable<Step> stepObservable)
        {
            return stepObservable
                .Select(step => step.IsOneOf(
                    Step.Untap,
                    Step.Draw,
                    Step.PlayLand,
                    Step.PlaySorcery,
                    Step.PlayCreatureBefore,
                    Step.PlayInstant,
                    Step.PlayCreatureAfter));
        }

        public static IObservable<bool> ObservePostponeVisible(IObservable<Step> stepObservable)
        {
            return stepObservable
                .Select(step => step == Step.PlayCreatureBefore);
        }

        public static IObservable<Unit> ObserveReset(IHotObservable<Step> stepObservable)
        {
            return stepObservable
                .Scan(
                    new { IsReset = false, Step = Step.Start },
                    (previous, current) =>
                        new
                        {
                            IsReset = (current == Step.Start) && (previous.Step != Step.Start),
                            Step = current
                        })
                .Where(pair => pair.IsReset)
                .SelectUnit();
        }

        public static IObservable<Unit> ObserveUntap(IObservable<(Step Step, Result Result)> stepResultObservable)
        {
            return stepResultObservable
                .SkipHot()
                .Where(stepResult => (stepResult.Step == Step.Untap) && (stepResult.Result == Result.Ok))
                .SelectUnit();
        }

        public static IObservable<bool> ObserveWouldPlayLand(
            IObservable<Step> stepObservable, 
            IObservable<int> inHandObservable)
        {
            return inHandObservable
                .SelectManyCancelPrevious(inHand => stepObservable
                    .SkipHot()
                    .Select(step => step == Step.PlayLand
                        ? WouldPlayLand(inHand)
                        : false));
        }

        private static bool WouldPlayLand(int inHand)
        {
            return _random.Next(6) < _playLandChance[Math.Min(_playLandChance.Length - 1, inHand)];
        }

        public static IObservable<Unit> ObservePlayLand(IObservable<bool> wouldPlayLandObservable, IObservable<(Step Step, Result Result)> stepResultObservable)
        {
            return wouldPlayLandObservable
                .WhereTrue()
                .SelectManyCancelPrevious(_ => stepResultObservable
                    .SkipHot()
                    .Where(stepResult => (stepResult.Step == Step.PlayLand) && (stepResult.Result == Result.Ok)))
                .SelectUnit();
        }

        public static IObservable<CreatureViewModel> ObserveWouldPlayCreature(
            IObservable<Step> stepObservable,
            IObservable<int> inHandObservable,
            IObservable<int> untappedLandsObservable,
            IObservable<Unit> untapAllObservable,
            BaseViewModel parent)
        {
            return Observable
                .CombineLatest(
                    inHandObservable,
                    untappedLandsObservable,
                    (inHand, untapped) => (InHand: inHand, Untapped: untapped))
                .SelectManyCancelPrevious(info => stepObservable
                    .SkipHot()
                    .Select(step => (step == Step.PlayCreatureBefore)
                        ? WouldPlayCreature(info.InHand, info.Untapped, untapAllObservable, parent)
                        : null));
        }

        private static CreatureViewModel WouldPlayCreature(int inHand, int untapped, IObservable<Unit> untapAllObservable, BaseViewModel parent)
        {
            if ((untapped > 0) &&
                (_random.Next(6) < _playCreatureChance[Math.Min(_playCreatureChance.Length - 1, inHand)]))
            {
                var power = _random.Next(untapped) + 1;
                var abilities = _randomAbilities[_random.Next(6) + _random.Next(6)];

                return new CreatureViewModel(untapAllObservable, power, power, parent, abilities);
            }

            return null;
        }

        public static IObservable<CreatureViewModel> ObservePlayCreature(
            IObservable<CreatureViewModel> wouldPlayCreatureObservable, 
            IObservable<(Step Step, Result Result)> stepResultObservable)
        {
            return wouldPlayCreatureObservable
                .WhereNotNull()
                .SelectManyCancelPrevious(creature => stepResultObservable
                    .SkipHot()
                    .Where(stepResult => (stepResult.Step == Step.PlayCreatureBefore) 
                        && (stepResult.Result == Result.Ok))
                    .Select(_ => creature));
        }

        public static IObservable<string> ObserveMessage(
            IObservable<Step> stepObservable, 
            IObservable<bool> wouldPlayLandObservable,
            IObservable<CreatureViewModel> wouldPlayCreatureObservable)
        {
            return Observable
                .CombineLatest(
                    stepObservable,
                    wouldPlayLandObservable,
                    wouldPlayCreatureObservable,
                    (step, playLand, creature) =>
                    {
                        switch (step)
                        {
                            case Step.Start:
                                return "Start a new game";

                            case Step.Untap:
                                return "Untap lands and creatures";

                            case Step.Upkeep:
                                return "Upkeep";

                            case Step.Draw:
                                return "Draw";

                            case Step.PlayLand:
                                return playLand ? "Play a land" : "No land this turn";

                            case Step.PlaySorcery:
                                return "TODO: Play sorcery";

                            case Step.PlayCreatureBefore:
                                return (creature != null)
                                    ? "Play a " + creature.ToString() + " creature?"
                                    : "No creature this turn";

                            case Step.Attack:
                                return "The enemy is attacking";

                            case Step.PlayInstant:
                                return "TODO: Play an instant";

                            case Step.PlayCreatureAfter:
                                return "TODO: Play postponed creature";

                            case Step.EndOfTurn:
                                return "End of the enemy's turn";

                            case Step.YourTurn:
                                return "Your turn";

                            // TODO: YourAttack, PlayDefenseInstant, EndOfGame 
                            default:
                                return "---";
                        }
                    });
        }

        enum Effect
        {
            Set,
            Add,
            Clip,
        }

        public static IObservable<int> ObserveInHand(
            IObservable<int> externalInHandObservable,
            IHotObservable<Step> stepObservable, 
            IObservable<(Step Step, Result Result)> stepResultObservable)
        {
            IObservable<(Effect Effect, int Value)> decrease = stepResultObservable
                .SkipHot()
                .Where(stepResult => (stepResult.Result == Result.Ok)
                    && (stepResult.Step.IsOneOf(Step.PlayLand, Step.PlaySorcery, Step.PlayCreatureBefore, Step.PlayInstant, Step.PlayCreatureAfter)))
                .Select(_ => (Effect.Add, -1));

            IObservable<(Effect Effect, int Value)> increase = stepResultObservable
                .SkipHot()
                .Where(stepResult => (stepResult.Result == Result.Ok)
                    && (stepResult.Step == Step.Draw))
                .Select(_ => (Effect.Add, 1));

            IObservable<(Effect Effect, int Value)> limit = stepResultObservable
                .SkipHot()
                .Where(stepResult => (stepResult.Result == Result.Ok)
                    && (stepResult.Step == Step.EndOfTurn))
                .Select(_ => (Effect.Clip, 7));

            IObservable<(Effect Effect, int Value)> reset = ObserveReset(stepObservable)
                .Select(_ => (Effect.Set, 7));

            return externalInHandObservable
                .StartWith(7)
                .SelectManyCancelPrevious(inHand => Observable
                    .Merge(decrease, increase, limit, reset)
                    .Scan(inHand, (value, effect) =>
                    {
                        switch (effect.Effect)
                        {
                            case Effect.Set:
                                return 7;

                            case Effect.Add:
                                return value + effect.Value;

                            case Effect.Clip:
                                return Math.Min(value, effect.Value);

                            default:
                                return value;
                        }
                    }));
        }
    }
}
