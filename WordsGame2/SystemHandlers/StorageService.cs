using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordsGame2.Interfaces;

namespace WordsGame2.SystemHandlers
{
    public class StorageService: JsonSerialization
    {
        public override List<Players> DeserializePlayers(string filePath)
        {
            var players = base.DeserializePlayers(filePath);
            return players;
        }

        public override void SerializePlayers(List<Players> players, string filePath)
        {
            foreach (var player in players)
                player.TotalScore += player.ScoredWords.Count;
            base.SerializePlayers(players, filePath);
        }
    }
}
