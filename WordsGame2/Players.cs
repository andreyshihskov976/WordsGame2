using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WordsGame2
{
    public class Players
    {
        string playerName;
        bool isAlive;
        List<string> countedWords = new List<string>();
        int triesCount;

        public Players(string playerName)
        {
            this.playerName = playerName;
            isAlive = true;
        }

        public List<string> CountedWords { get => countedWords; set => countedWords = value; }
        public bool IsAlive { get => isAlive; set => isAlive = value; }
        public string PlayerName { get => playerName; set => playerName = value; }
        public int TriesCount { get => triesCount; set => triesCount = value; }
    }
}
