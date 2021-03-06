﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordsGame2.Interfaces;

namespace WordsGame2
{
    public class JsonSerialization: ISerializable
    {
        public virtual void SerializePlayers(List<Players> players, string filePath)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            
            using (StreamWriter sw = new StreamWriter(filePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, players);
            }
        }

        public virtual List<Players> DeserializePlayers(string filePath)
        {
            List<Players> players = new List<Players>();
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            using (StreamReader sr = new StreamReader(filePath))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                players = serializer.Deserialize<List<Players>>(reader);
            }
            return players;
        }
    }
}
