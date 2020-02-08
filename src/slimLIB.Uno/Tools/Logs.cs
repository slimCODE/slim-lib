using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace slimCODE.Tools
{
    // TODO: Logs.GetLogger(log => log.ReportError("foo bar", error));
    //       The lambda is not called when no logger active.
    public static partial class Logs
    {
        private static readonly Logger __logger = new Logger();

        static Logs()
        {
            AddDebugLogger();
        }

        public static void AddLogger(ILogger logger)
        {
            __logger.AddLogger(logger);
        }

        public static void UseLogger(Action<Logger> loggerAction)
        {
            if(__logger.HasLoggers)
            {
                loggerAction(__logger);
            }
        }

		public static IDisposable UseLogger(Action<Logger> beginLoggerAction, Action<Logger, Stopwatch> endLoggerAction)
		{
			if (__logger.HasLoggers)
			{
				beginLoggerAction(__logger);

				var stopWatch = Stopwatch.StartNew();

				return Disposable.Create(() =>
				{
					stopWatch.Stop();
					endLoggerAction(__logger, stopWatch);
				});
			}

			return Disposable.Empty;
		}

		[Conditional("DEBUG")]
        private static void AddDebugLogger()
        {
            if(System.Diagnostics.Debugger.IsAttached)
            {
                AddLogger(new DebuggerLogger());
            }
        }
    }
}
