using slimCODE.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slimCODE.Tools
{
    public class DebuggerLogger : ILogger
    {
        public void Report(long id, DateTimeOffset timeStamp, LogLevel level, string message, Exception error)
        {
            System.Diagnostics.Debug.WriteLine(
                "[{0},{1}] {2} = {3}{4}", 
                id,
                timeStamp,
                level, 
                message,
                error.SelectWhen(e => string.Format("\n{0}: {1}", e.GetType().Name, e.Message), () => ""));
        }
    }
}
