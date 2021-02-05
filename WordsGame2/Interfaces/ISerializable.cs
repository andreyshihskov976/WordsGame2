using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsGame2.Interfaces
{
    interface ISerializable
    {
        void SerializePlayers(List<Players> players, string filePath);
        List<Players> DeserializePlayers(string filePath);
    }
}
