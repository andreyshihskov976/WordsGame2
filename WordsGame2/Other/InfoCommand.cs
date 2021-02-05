using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WordsGame2
{
    public class InfoCommand
    {
        private readonly List<string> _commands = new List<string>() { "/show-words", "/score", "/total-score" };

        public List<string> Commands => _commands;

        public virtual bool ShowInfo(string inputedCommand, List<Players> players, Timer timer, string baseWord)
        {
            if (Commands.Contains(inputedCommand))
            {
                timer.Stop();
                Console.WriteLine("Игра приостановлена.");
                switch (inputedCommand)
                {
                    case "/show-words":
                        ShowSessionResults(players, baseWord);
                        break;
                    case "/score":
                        //общий счет по играм для игроков текущей сессии
                        break;
                    case "/total-score":
                        //общий счет по играм для всех игроков
                        break;
                }
                Console.WriteLine("Нажмите любую клавишу для перехода в меню.");
                Console.ReadKey();
                timer.Start();
                return true;
            }
            else
                return false;
        }


        public virtual void ShowSessionResults(List<Players> players, string baseWord)
        {
            Console.WriteLine("Результаты за текущую игру:");
            foreach (var player in players)
            {
                Console.WriteLine("{0} смог(-ла) составить: {1} слов(-а) из {2}", player.PlayerName, player.ScoredWords.Count, baseWord);
                foreach (var word in player.ScoredWords)
                    Console.Write(word+"; ");
                Console.WriteLine();
            }
        }

    }
}
