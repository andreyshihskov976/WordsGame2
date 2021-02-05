using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsGame2.Interfaces
{
    interface IGame
    {
        void Game(List<Players> players);
        void StartSession(List<Players> players);
    }
}
