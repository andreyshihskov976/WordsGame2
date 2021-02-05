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
        private Guid id;
        string playerName;
        bool isAlive;
        List<string> scoredWords = new List<string>();
        int amountRounds;
        int totalScore;

        public Players(string playerName)
        {
            id = Guid.NewGuid();
            this.playerName = playerName;
            isAlive = true;
            this.totalScore = 0;
        }

        public List<string> ScoredWords { get => scoredWords; set => scoredWords = value; }
        public bool IsAlive { get => isAlive; set => isAlive = value; }
        public string PlayerName { get => playerName; set => playerName = value; }
        public int AmountRounds { get => amountRounds; set => amountRounds = value; }
        public int TotalScore { get => totalScore; set => totalScore = value; }
        public Guid Id { get => id; set => id = value; }
    }
}
