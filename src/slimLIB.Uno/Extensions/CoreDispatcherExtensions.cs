using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="CoreDispatcher"/> instances.
    /// </summary>
    public static partial class CoreDispatcherExtensions
    {
        /// <summary>
        /// Runs the provided <see cref="DispatchedHandler"/> action on a <see cref="CoreDispatcher"/> with normal priority.
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="ct"></param>
        /// <param name="agileCallback"></param>
        /// <returns></returns>
        public static Task RunNormalAsync(this CoreDispatcher dispatcher, CancellationToken ct, DispatchedHandler agileCallback)
        {
            return dispatcher
                .RunAsync(CoreDispatcherPriority.Normal, agileCallback)
                .AsTask(ct);
        }

        /// <summary>
        /// Runs the provided <see cref="Func{TResult}"/> on a <see cref="CoreDispatcher"/> in normal priority.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dispatcher"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task<TResult> RunNormalAsync<TResult>(
            this CoreDispatcher dispatcher,
            Func<TResult> func)
        {
            var source = new TaskCompletionSource<TResult>();

            await dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    try
                    {
                        source.SetResult(func());
                    }
                    catch (Exception error)
                    {
                        source.SetException(error);
                    }
                });

            return await source.Task;
        }

        /// <summary>
        /// Runs an async function on a <see cref="CoreDispatcher"/> in normal priority.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dispatcher"></param>
        /// <param name="ct"></param>
        /// <param name="asyncFunc"></param>
        /// <returns></returns>
        public static async Task<TResult> RunNormalAsync<TResult>(
            this CoreDispatcher dispatcher,
            CancellationToken ct,
            Func<CancellationToken, Task<TResult>> asyncFunc)
        {
            var source = new TaskCompletionSource<TResult>();

            await dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                async () =>
                {
                    try
                    {
                        source.SetResult(await asyncFunc(ct));
                    }
                    catch (Exception error)
                    {
                        source.SetException(error);
                    }
                });

            // TODO: ct
            return await source.Task;
        }

        /// <summary>
        /// Logs an error if the current thread is not the expected thread compared to a <see cref="CoreDispatcher"/>.
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="mustBeOnUIThread"></param>
        public static void VerifyThread(this CoreDispatcher dispatcher, bool mustBeOnUIThread)
        {
            if (dispatcher.HasThreadAccess != mustBeOnUIThread)
            {
                /*
                Logs.UseLogger(logger =>
                    logger.ReportError(
                        "Wrong thread. Was expecting {0}.".FormatInvariant(mustBeOnUIThread ? "dispatcher" : "!dispatcher")));
                */
            }
        }
    }
}
