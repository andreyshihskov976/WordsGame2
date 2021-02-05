using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsGame2.Interfaces
{
    interface IMultiplayer
    {
        void NextPlayerTurn(List<Players> players, ref bool firstPlayerNext);
        void NewPlayer();
        void ChoicePlayers();
    }
}
