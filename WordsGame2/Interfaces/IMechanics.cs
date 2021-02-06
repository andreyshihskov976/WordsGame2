using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsGame2.Interfaces
{
    interface IMechanics
    {
        void BaseWordInput();
        void RoundHandler(List<Players> players, Players activePlayer, bool isLastRound = false);
        void HandleInputedWord(List<Players> players, Players activePlayer);
        bool CheckBaseWord();
    }
}
