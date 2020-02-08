using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seclusion.Models
{
    public static partial class StepExtensions
    {
        public static Step Next(this Step step)
        {
            if (step == Step.YourTurn)
            {
                return Step.Untap;
            }

            return (Step)(step + 1);
        }
    }
}
