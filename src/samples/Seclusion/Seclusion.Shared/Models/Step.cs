using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seclusion.Models
{
    public enum Step
    {
        Start,
        Untap,
        Upkeep,
        Draw,
        PlayLand,
        PlaySorcery,
        PlayCreatureBefore,
        Attack,
        PlayInstant,
        PlayCreatureAfter,
        EndOfTurn,
        YourTurn,
    }
}
