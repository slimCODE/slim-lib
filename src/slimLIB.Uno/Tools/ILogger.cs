using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slimCODE.Tools
{
    public interface ILogger
    {
        void Report(long id, DateTimeOffset timeStamp, LogLevel level, string message, Exception error);
    }
}
