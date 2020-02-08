using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using slimCODE.Models;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods on <see cref="IObservable{T}"/> instances.
    /// </summary>
    public static partial class ObservableExtensions
    {
        /// <summary>
        /// Executes an action for each element of an <see cref="IObservable{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable"></param>
        /// <param name="asyncAction"></param>
        /// <returns></returns>
        public static IObservable<T> Do<T>(
            this IObservable<T> observable,
            Func<CancellationToken, T, Task> asyncAction)
        {
            return observable
                .SelectMany(async (value, ct) =>
                {
                    await asyncAction(ct, value);
                    return value;
                });
        }

        /// <summary>
        /// Executes one of two actions for each element of an <see cref="IObservable{T}"/>, depending
        /// whether the item is null or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable"></param>
        /// <param name="whenNotNull"></param>
        /// <param name="whenNull"></param>
        /// <returns></returns>
        public static IObservable<T> DoWhen<T>(
            this IObservable<T> observable,
            Action<T> whenNotNull,
            Action whenNull = null)
            where T : class
        {
            return observable
                .Do(value => value.DoWhen(whenNotNull, whenNull));
        }

        /// <summary>
        /// Executes one of two actions for each element of an <see cref="IObservable{Boolean}"/>, depending
        /// whether the value is true or false.
        /// </summary>
        /// <param name="observable"></param>
        /// <param name="whenTrue"></param>
        /// <param name="whenFalse"></param>
        /// <returns></returns>
        public static IObservable<bool> DoWhen(
            this IObservable<bool> observable,
            Action whenTrue = null,
            Action whenFalse = null)
        {
            return observable
                .Do(value =>
                {
                    if (value)
                    {
                        whenTrue?.Invoke();
                    }
                    else
                    {
                        whenFalse?.Invoke();
                    }
                });
        }

        /// <summary>
        /// Transforms each elememt of an <see cref="IObservable{T}"/> into a <see cref="Unit"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable"></param>
        /// <returns></returns>
        public static IObservable<Unit> SelectUnit<T>(this IObservable<T> observable)
        {
            return observable
                .Select(_ => Unit.Default);
        }

        /// <summary>
        /// Transforms an <see cref="IObservable{Unit}"/> into a hot observable that returns a <see cref="Unit"/> on subscription.
        /// </summary>
        /// <param name="observable"></param>
        /// <returns></returns>
        public static IObservable<Unit> StartWithUnit(this IObservable<Unit> observable)
        {
            return observable
                .StartWith(Unit.Default);
        }

        /// <summary>
        /// Starts observing a new <see cref="IObservable{TTarget}"/> selected from a <typeparamref name="TSource"/>
        /// fired by a source <see cref="IObservable{TSource}"/>, unsubscribing from any previous observable.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="observable"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IObservable<TTarget> SelectManyCancelPrevious<TSource, TTarget>(
            this IObservable<TSource> observable,
            Func<TSource, IObservable<TTarget>> selector)
        {
            return observable
                .Select(source => selector(source))
                .Switch();
        }

        /// <summary>
        /// Executes and async function that returns a <typeparamref name="TTarget"/> every time an 
        /// <see cref="IObservable{TSource}"/> produces a value, cancelling any pending previous async operation.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="observable"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IObservable<TTarget> SelectManyCancelPrevious<TSource, TTarget>(
            this IObservable<TSource> observable,
            Func<CancellationToken, TSource, Task<TTarget>> selector)
        {
            return observable
                .Select(source => Observable.FromAsync(ct => selector(ct, source)))
                .Switch();
        }

        /// <summary>
        /// Executes and async action every time an         /// <see cref="IObservable{TSource}"/> 
        /// produces a value, cancelling any pending previous async operation.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="observable"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IObservable<Unit> SelectManyCancelPrevious<TSource>(
            this IObservable<TSource> observable,
            Func<CancellationToken, TSource, Task> selector)
        {
            return observable
                .Select(source => Observable.FromAsync(ct => selector(ct, source)))
                .Switch();
        }

        /// <summary>
        /// Makes an <see cref="IObservable{TSource}"/> throw a <typeparamref name="TException"/> selected from 
        /// the next <typeparamref name="TSource"/>.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="observable"></param>
        /// <param name="exceptionSelector"></param>
        /// <returns></returns>
        public static IObservable<TSource> Throw<TSource, TException>(
            this IObservable<TSource> observable,
            Func<TSource, TException> exceptionSelector)
            where TException : Exception
        {
            return observable
                .Do(value => { throw exceptionSelector(value); });
        }

        /// <summary>
        /// Subscribes to an <see cref="IObservable{T}"/> where <typeparamref name="T"/> implements 
        /// <see cref="IDisposable"/> and makes sure to dispose each item when the next comes in.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable"></param>
        /// <returns></returns>
        public static IDisposable SubscribeEnsurePreviousIsDisposed<T>(this IObservable<T> observable)
            where T : IDisposable
        {
            return observable
                .Scan(default(T), (previous, current) => previous)
                .Subscribe(previous => previous.SafeDispose());
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TWatch"></typeparam>
        /// <param name="observable"></param>
        /// <param name="watchedSelector"></param>
        /// <returns></returns>
        public static IObservable<TSource> Watch<TSource, TWatch>(
            this IObservable<TSource> observable,
            Func<TSource, IObservable<TWatch>> watchedSelector)
        {
            return observable
                .SelectManyCancelPrevious(source => watchedSelector(source)
                    .Where(_ => false)
                    .Select(_ => default(TSource))
                    .StartWith(source));
        }

        /// <summary>
        /// Observes two <see cref="IObservable{TSource}"/>, cumulating all items from the first in an array,
        /// and removing all items from the second from that array.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="addingObservable"></param>
        /// <param name="removingObservable"></param>
        /// <returns></returns>
        public static IObservable<TSource[]> Except<TSource>(
            this IObservable<TSource> addingObservable,
            IObservable<TSource> removingObservable)
        {
            return Observable
                .Merge(
                    addingObservable.Select(item => new { Item = item, Added = true }),
                    removingObservable.Select(item => new { Item = item, Added = false }))
                .Scan(
                    new TSource[0],
                    (items, info) => info.Added
                        ? items.Concat(info.Item).ToArray()
                        : items.Except(info.Item).ToArray());
        }

        /// <summary>
        /// Filters out all true values from an <see cref="IObservable{Boolean}"/>.
        /// </summary>
        /// <param name="observable"></param>
        /// <returns></returns>
        public static IObservable<bool> WhereFalse(this IObservable<bool> observable)
        {
            return observable.Where(value => !value);
        }

        /// <summary>
        /// Filters out all false values from an <see cref="IObservable{Boolean}"/>.
        /// </summary>
        /// <param name="observable"></param>
        /// <returns></returns>
        public static IObservable<bool> WhereTrue(this IObservable<bool> observable)
        {
            return observable.Where(value => value);
        }

        /// <summary>
        /// Filters out all null values from an <see cref="IObservable{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable"></param>
        /// <returns></returns>
        public static IObservable<T> WhereNotNull<T>(this IObservable<T> observable)
            where T : class
        {
            return observable.Where(value => value != null);
        }

        /// <summary>
        /// Transforms each element in an <see cref="IObservable{String}"/> into a <see cref="Boolean"/>
        /// that indicates if the element was not null, empty or only whitespaces.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IObservable<bool> SelectNotNullOrWhiteSpace(this IObservable<string> source)
        {
            return source
                .Select(value => !string.IsNullOrWhiteSpace(value));
        }

        /// <summary>
        /// Transforms each element in an <see cref="IObservable{String}"/> into a <see cref="Boolean"/>
        /// that indicates if the element was not null or empty.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IObservable<bool> SelectNotNullOrEmpty(this IObservable<string> source)
        {
            return source
                .Select(value => !string.IsNullOrEmpty(value));
        }

        /// <summary>
        /// Inverts each element of an <see cref="IObservable{Boolean}"/>.
        /// </summary>
        /// <param name="observable"></param>
        /// <returns></returns>
        public static IObservable<bool> Negate(this IObservable<bool> observable)
        {
            return observable.Select(value => !value);
        }

        /*
        public static IObservable<T> Debug<T>(this IObservable<T> observable, string descriptor = null)
        {
            return Observable
                .Create<T>(o =>
                {
                    descriptor = descriptor ?? Guid.NewGuid().ToString();

                    Logs.UseLogger(l => l.ReportDebug($"Subscribe {descriptor}"));

                    return new CompositeDisposable(
                        observable
                            .Subscribe(
                                next =>
                                {
                                    Logs.UseLogger(l => l.ReportDebug($"OnNext {descriptor} = {next}"));
                                    o.OnNext(next);
                                },
                                error =>
                                {
                                    Logs.UseLogger(l => l.ReportDebug($"OnError {descriptor} = {error.Message}"));
                                    o.OnError(error);
                                },
                                () =>
                                {
                                    Logs.UseLogger(l => l.ReportDebug($"OnCompleted {descriptor}"));
                                    o.OnCompleted();
                                }),
                        Disposable.Create(() => Logs.UseLogger(l => l.ReportDebug($"Disposed {descriptor}")))
                    );
                });
        }
        */

        /// <summary>
        /// Combines the latest elements of two <see cref="IObservable{Boolean}"/> into their "&&" combination.
        /// </summary>
        /// <param name="firstSource"></param>
        /// <param name="secondSource"></param>
        /// <returns></returns>
        public static IObservable<bool> CombineLatestAnd(this IObservable<bool> firstSource, IObservable<bool> secondSource)
        {
            return firstSource
                .CombineLatest(secondSource, (firstValue, secondValue) => firstValue && secondValue);
        }

        /// <summary>
        /// Combines the latest elements of two <see cref="IObservable{Boolean}"/> into their "||" combination.
        /// </summary>
        /// <param name="firstSource"></param>
        /// <param name="secondSource"></param>
        /// <returns></returns>
        public static IObservable<bool> CombineLatestOr(this IObservable<bool> firstSource, IObservable<bool> secondSource)
        {
            return firstSource
                .CombineLatest(secondSource, (firstValue, secondValue) => firstValue || secondValue);
        }

        /// <summary>
        /// Combines the latest elements of two <see cref="IObservable{Boolean}"/> into their "!=" comparison.
        /// </summary>
        /// <param name="firstSource"></param>
        /// <param name="secondSource"></param>
        /// <returns></returns>
        public static IObservable<bool> CombineLatestXor(this IObservable<bool> firstSource, IObservable<bool> secondSource)
        {
            return firstSource
                .CombineLatest(secondSource, (firstValue, secondValue) => firstValue != secondValue);
        }

        /// <summary>
        /// Observes elements of an <see cref="IObservable{T}"/> on the default <see cref="IScheduler"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable"></param>
        /// <returns></returns>
        public static IObservable<T> OnBackground<T>(this IObservable<T> observable)
        {
            return observable
                .ObserveOn(Scheduler.Default);
        }

        /// <summary>
        /// Skips the first element of an <see cref="IObservable{T}"/> only if that observable
        /// also implements <see cref="IHotObservable{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable"></param>
        /// <returns></returns>
        public static IObservable<T> SkipHot<T>(this IObservable<T> observable)
        {
            return (observable as IHotObservable<T>)?.Skip(1)
                ?? observable;
        }
    }
}
