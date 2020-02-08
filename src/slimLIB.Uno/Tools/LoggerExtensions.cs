using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slimCODE.Tools
{
    public static partial class LoggerExtensions
    {
        public static void ReportCatastrophicError(this Logger logger, string message, Exception error = null)
        {
            logger.Report(LogLevel.CatastrophicError, message, error);
        }

        public static void ReportError(this Logger logger, string message, Exception error = null)
        {
            logger.Report(LogLevel.Error, message, error);
        }

        public static void ReportWarning(this Logger logger, string message, Exception error = null)
        {
            logger.Report(LogLevel.Warning, message, error);
        }

        public static void ReportInfo(this Logger logger, string message, Exception error = null)
        {
            logger.Report(LogLevel.Info, message, error);
        }

        public static void ReportDebug(this Logger logger, string message, Exception error = null)
        {
            logger.Report(LogLevel.Debug, message, error);
        }
    }
}
