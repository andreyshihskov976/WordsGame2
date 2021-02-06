using System;
using System.Collections.Generic;
using System.Linq;
using WordsGame2.Interfaces;
using WordsGame2.SystemHandlers;

namespace WordsGame2.GameHandlers
{
    public class MainHandler : IMenu, IMultiplayer
    {
        private Settings _settings;
        private GameMechanic _gameMechanic;
        private GameInformation _infoCommand;
        private StorageService _storageService;
        private List<Players> _allPlayers;
        private List<Players> _chosenPlayers;

        public MainHandler()
        {
            Settings = new Settings();
            StorageService = new StorageService();
            AllPlayers = StorageService.DeserializePlayers(Settings.SavesPath);
            InfoCommand = new GameInformation(StorageService,Settings,AllPlayers);
            GameMechanic = new GameMechanic(Settings, InfoCommand);
        }

        public Settings Settings { get => _settings; set => _settings = value; }
        public GameInformation InfoCommand { get => _infoCommand; set => _infoCommand = value; }
        public GameMechanic GameMechanic { get => _gameMechanic; set => _gameMechanic = value; }
        public StorageService StorageService { get => _storageService; set => _storageService = value; }
        public List<Players> ChosenPlayers { get => _chosenPlayers; set => _chosenPlayers = value; }
        public List<Players> AllPlayers { get => _allPlayers; set => _allPlayers = value; }

        enum MainMenuActions
        {
            SetCustomSettings = 1,
            StartGame = 2,
            ExitGame = 3
        }

        public virtual void ShowMenu()
        {
            Console.WriteLine("Добро пожаловать в игру 'Слова'." + '\n' + "Для продолжения нажмите любую клавишу:");
            Console.ReadKey();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Нажмите 1 для изменения настроек." + '\n' + "Нажмите 2 для начала игры." + '\n' + "Нажмите 3 для выхода из игры.");
                if (Parsing.ParseInt(Console.ReadLine(), out int action))
                    switch ((MainMenuActions)action)
                    {
                        case MainMenuActions.SetCustomSettings:
                            Settings.SetCustomSettings();
                            break;
                        case MainMenuActions.StartGame:
                            InitializeGame();
                            break;
                        case MainMenuActions.ExitGame:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.Clear();
                            Console.Beep();
                            Console.WriteLine("Ошибка: 'Такого пункта не существует в меню.'" + '\n' +
                                                    "Нажмите любую клавишу для продолжения и повторите ввод.");
                            Console.ReadKey();
                            break;
                    }
            }
        }

        public virtual void ChoosePlayers()
        {
            ChosenPlayers = new List<Players>();
            while (ChosenPlayers.Count < 2)
            {
                Console.Clear();
                if (AllPlayers.Count > 0)
                {
                    Console.WriteLine("Игрок №{0}, выберите вашу запись игрока или добавьте новую." + '\n' + "Список записей:", ChosenPlayers.Count + 1);
                    foreach (Players player in AllPlayers)
                    {
                        Console.WriteLine("№: {0} {1}", AllPlayers.FindIndex(item => item.PlayerName == player.PlayerName) + 1, player.PlayerName);
                    }
                    Console.WriteLine("Для создания новой записи введите 0." + '\n' + "Введите ваш выбор:");
                    if (Parsing.ParseInt(Console.ReadLine(), out int chosenNumber))
                        if (chosenNumber != 0 && chosenNumber <= AllPlayers.Count)
                        {
                            if (!ChosenPlayers.Exists(item => item == AllPlayers[chosenNumber - 1]))
                                ChosenPlayers.Add(AllPlayers[chosenNumber - 1]);
                            else
                            {
                                Console.WriteLine("Данная запись уже была выбрана." + '\n' + "Нажмите любую клавишу для продолжения и повторите ввод.");
                                Console.ReadLine();
                            }
                        }
                        else if (chosenNumber == 0)
                            NewPlayer();
                }
                else
                {
                    Console.WriteLine("Ниодной записи игрока не было найдено.");
                    NewPlayer();
                }
            }
                    
        }

        public virtual void NewPlayer()
        {
            Console.WriteLine("Введите имя игрока:");
            string playerName = Console.ReadLine();
            if (!AllPlayers.Exists(item => item.PlayerName == playerName))
            {
                AllPlayers.Add(new Players(playerName));
                ChosenPlayers.Add(AllPlayers.Find(item => item.PlayerName == playerName));
            }
            else
            {
                Console.WriteLine("Данное имя уже использовалось. Придумайте другое имя игрока." + '\n' + "Нажмите любую клавишу для продолжения и повторите ввод.");
                Console.ReadLine();
            }
        }

        public virtual void InitializeGame()
        {
            ChoosePlayers();
            GameMechanic.BaseWordInput();
            InfoCommand.ChosenPlayers = ChosenPlayers;
            InfoCommand.baseWord = GameMechanic.baseWord;
            StartSession(ChosenPlayers);
            StorageService.SerializePlayers(AllPlayers, Settings.SavesPath);
            Console.WriteLine("Нажмите любую клавишу для перехода в меню.");
            Console.ReadKey();
        }


        public virtual void StartSession(List<Players> players)
        {
            Settings.TimerHandler.OutOfTime = false;
            foreach (var player in players)
            {
                player.IsAlive = true;
                player.ScoredWords.Clear();
                player.AmountRounds = 0;
            }
            bool firstPlayerNext = true;
            while (players[0].IsAlive && players[1].IsAlive)
                NextPlayerTurn(players, ref firstPlayerNext);
            FinalTurn(players);
            Console.Clear();
            InfoCommand.ShowGameInfo(players, GameMechanic.baseWord);
        }

        public virtual void FinalTurn(List<Players> players)
        {
            if (!GameMechanic.isGameExit)
                if (players[0].AmountRounds != players[1].AmountRounds)
                    if (players[0].AmountRounds > players[1].AmountRounds)
                        GameMechanic.RoundHandler(players, players[1], true);
                    else
                        GameMechanic.RoundHandler(players, players[0], true);
        }

        public virtual void NextPlayerTurn(List<Players> players, ref bool firstPlayerNext)
        {
            if (firstPlayerNext)
                GameMechanic.RoundHandler(players, players.First());
            else if (!firstPlayerNext)
                GameMechanic.RoundHandler(players, players.Last());
            firstPlayerNext = !firstPlayerNext;
        }
    }
}
