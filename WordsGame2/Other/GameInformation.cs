using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WordsGame2.Interfaces;
using WordsGame2.SystemHandlers;

namespace WordsGame2
{
    public class GameInformation: IInfo
    {
        private StorageService _getStorageService;
        private Settings _getSettings;
        private List<Players> _allPlayers;
        private List<Players> _chosenPlayers;
        public string baseWord;

        private readonly List<string> _commands = new List<string>() { "/show-words", "/score", "/total-score" };

        public GameInformation(StorageService getStorageService, Settings getSettings, List<Players> allPlayers, List<Players> chosenPlayers = null)
        {
            GetStorageService = getStorageService;
            GetSettings = getSettings;
            AllPlayers = allPlayers;
            ChosenPlayers = chosenPlayers;
        }

        public List<string> Commands => _commands;

        public StorageService GetStorageService { get => _getStorageService; set => _getStorageService = value; }
        public Settings GetSettings { get => _getSettings; set => _getSettings = value; }
        public List<Players> AllPlayers { get => _allPlayers; set => _allPlayers = value; }
        public List<Players> ChosenPlayers { get => _chosenPlayers; set => _chosenPlayers = value; }

        public virtual bool ShowInfo(string inputedCommand)
        {
            GetSettings.TimerHandler.Timer.Stop();
            Console.WriteLine("Игра приостановлена.");
            switch (inputedCommand)
            {
                case "/show-words":
                    ShowGameInfo(ChosenPlayers, baseWord);
                    break;
                case "/score":
                    ShowGameInfo(ChosenPlayers);
                    break;
                case "/total-score":
                    ShowGameInfo(AllPlayers);
                    break;
            }
            Console.WriteLine("Нажмите любую клавишу для перехода в меню.");
            Console.ReadKey();
            GetSettings.TimerHandler.Timer.Start();
            return true;
        }


        public virtual void ShowGameInfo(List<Players> players, string baseWord)
        {
            Console.Clear();
            Console.WriteLine("Результаты за текущую игру:");
            foreach (var player in players)
            {
                Console.WriteLine("{0} смог(-ла) составить: {1} слов(-а) из {2}", player.PlayerName, player.ScoredWords.Count, baseWord);
                foreach (var word in player.ScoredWords)
                    Console.Write(word+"; ");
                Console.WriteLine();
            }
        }

        public virtual void ShowGameInfo(List<Players> players)
        {
            Console.Clear();
            Console.WriteLine("Общее количество очков по всем играм:");
            foreach (var player in players.OrderBy(player => player.TotalScore))
                Console.WriteLine("Общее количество набранных очков для игрока {0} = {1}", player.PlayerName, player.TotalScore);
        }
    }
}
