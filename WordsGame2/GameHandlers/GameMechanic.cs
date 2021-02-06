using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WordsGame2.Interfaces;
using WordsGame2.Other;

namespace WordsGame2
{
    public class GameMechanic: IMechanics, IRoundUI
    {
        private Settings _getSettings;
        private GameInformation _getInfoCommand;
        public string baseWord;
        public bool isGameExit;
        static Dictionary<char, int> baseWordDictionary;
        static string inputedWord;

        public GameMechanic(Settings getSettings, GameInformation getInfoCommand)
        {
            _getSettings = getSettings;
            _getInfoCommand = getInfoCommand;
        }

        public Settings GetSettings { get => _getSettings; set => _getSettings = value; }
        public GameInformation GetGameInformation { get => _getInfoCommand; set => _getInfoCommand = value; }

        public virtual void BaseWordInput()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Введите первоначальное слово:" + '\n' +
                    "Минимальная длина слова должна составлять: " + GetSettings.MinLength.ToString() + '\n' +
                    "Максимальная длина слова должна составлять: " + GetSettings.MaxLength.ToString());
                baseWord = Console.ReadLine();
                if (baseWord != string.Empty)
                    if (GetSettings.MinLength <= baseWord.Length && baseWord.Length <= GetSettings.MaxLength)
                    {
                        if (CheckBaseWord())
                        {
                            Console.Clear();
                            Console.Beep();
                            Console.WriteLine("Ошибка: Базовое слово не может содержать цифры и прочие знаки, кроме букв." + '\n' +
                                "Нажмите любую клавишу для продолжения и повторите ввод.");
                            Console.ReadKey();
                            continue;
                        }
                        int lettersCount = baseWord.ToCharArray().Distinct().Count();
                        baseWordDictionary = new Dictionary<char, int>(lettersCount);
                        foreach (var c in baseWord.ToUpper())
                        {
                            if (baseWordDictionary.Any(item => item.Key == c) != true)
                                baseWordDictionary.Add(c, baseWord.ToUpper().Count(letter => letter == c));
                        }
                        Console.Clear();
                        Console.WriteLine("Слово было сохранено." + '\n' +
                            "Нажмите любую клавишу для продолжения.");
                        Console.ReadKey();
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.Beep();
                        Console.WriteLine("Слово не подходит по правилам." + '\n' +
                            "Минимальная длина слова должна составлять: " + GetSettings.MinLength.ToString() + '\n' +
                            "Максимальная длина слова должна составлять: " + GetSettings.MaxLength.ToString() + '\n' +
                            "Нажмите любую клавишу для продолжения и повторите ввод.");
                        Console.ReadKey();
                        continue;
                    }
                else
                {
                    Console.Clear();
                    Console.Beep();
                    Console.WriteLine("Ошибка: Вы ввели пустую строку." + '\n' +
                "Минимальная длина слова должна составлять: " + GetSettings.MinLength.ToString() + '\n' +
                "Максимальная длина слова должна составлять: " + GetSettings.MaxLength.ToString() + '\n' +
                "Нажмите любую клавишу для продолжения и повторите ввод.");
                    Console.ReadKey();
                    continue;
                }
            }
        }

        public virtual void RoundHandler(List<Players> players, Players activePlayer, bool isLastRound = false)
        {
            GetSettings.TimerHandler.Timer.Start();
            activePlayer.AmountRounds++;
            RoundUI(activePlayer, isLastRound);
            if (GetSettings.TimerHandler.OutOfTime)
                activePlayer.IsAlive = false;
            if (activePlayer.IsAlive || isLastRound)
                HandleInputedWord(players, activePlayer);
        }

        public virtual void RoundUI(Players activePlayer, bool isLastRound)
        {
            Console.Clear();
            if (isLastRound) Console.WriteLine('\n' + "Последний раунд!" + '\n' + "{0}, Ваш ход." + '\n' + "Основное слово: {1}" + '\n' + "Для получения информации по игре введите следующие команды: ", activePlayer.PlayerName, baseWord);
            else Console.WriteLine('\n' +"{0}, Ваш ход." + '\n' + "Основное слово: {1}" + '\n' + "Для получения информации по игре введите следующие команды: ", activePlayer.PlayerName, baseWord);
            foreach (var command in GetGameInformation.Commands)
                Console.WriteLine(command);
            Console.WriteLine("Введите полученное Вами слово:");
            inputedWord = Console.ReadLine();
        }

        public virtual void HandleInputedWord(List<Players> players, Players activePlayer)
        {
            int lettersCount = 0;
            if (inputedWord.Length > 0)
            {
                if (inputedWord == "/exit")
                {
                    GetSettings.TimerHandler.Timer.Stop();
                    GetSettings.TimerHandler.TimeLeft = GetSettings.RoundDuration;
                    isGameExit = true;
                    foreach (var player in players)
                        player.IsAlive = false;
                    return;
                }
                if (!GetGameInformation.Commands.Contains(inputedWord))
                {
                    foreach (var c in inputedWord.ToUpper().Distinct())
                    {
                        if (baseWordDictionary.ContainsKey(c))
                            if (inputedWord.ToUpper().Count(letter => letter == c) <= baseWordDictionary[c])
                                lettersCount++;
                    }
                    if (lettersCount == inputedWord.Distinct().Count())
                    {
                        if (!players.Any(player => player.ScoredWords.Any(item => item == inputedWord)))
                        {
                            activePlayer.ScoredWords.Add(inputedWord);
                            Console.WriteLine("Слово засчитано.");
                            GetSettings.TimerHandler.Timer.Stop();
                            GetSettings.TimerHandler.TimeLeft = GetSettings.RoundDuration;
                            Console.ReadKey();
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Такое слово уже было.");
                            Console.ReadKey();
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Слово не засчитано.");
                        Console.ReadKey();
                        return;
                    }
                }
                else
                    GetGameInformation.ShowInfo(inputedWord);
            }
        }

        public virtual bool CheckBaseWord()
        {
            if (Regex.Match(baseWord, @"^[а-яА-Я]||[A-Za-z]||[/]+$").Success/* || Regex.Match(baseWord, @"^[A-Za-z]+$").Success*/)
                return false;
            else
                return true;
        }
    }
}
