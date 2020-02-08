using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace slimCODE.Tools
{
    public class Logger
    {
        private List<ILogger> _loggers = new List<ILogger>();
        private long _uniqueId;

        public bool HasLoggers
        {
            get { return _loggers.Count > 0; }
        }

        internal void AddLogger(ILogger logger)
        {
            _loggers.Add(logger);
        }

        public void Report(LogLevel level, string message, Exception error)
        {
            var id = Interlocked.Increment(ref _uniqueId);
            var timeStamp = DateTimeOffset.UtcNow;  // I know, should use IScheduler...

            foreach(var logger in _loggers)
            {
                logger.Report(id, timeStamp, level, message, error);
            }
        }
    }
}
