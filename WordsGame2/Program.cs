using System;
using System.Collections.Generic;
using WordsGame2.GameHandlers;

namespace WordsGame2
{
    class Program
    {
        static void Main(string[] args)
        {
            MainHandler gameHandler = new MainHandler();
            gameHandler.ShowMenu();
        }
    }
}
