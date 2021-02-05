using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsGame2.Interfaces
{
    interface IGameMechanics
    {
        void BaseWordInput();
        void RoundHandler(List<Players> players, Players player, bool lastRound = false);
        void RoundUI();
        void CountInputedWord(List<Players> players, Players player, bool lastRound);
        bool CheckBaseWord();
    }
}
