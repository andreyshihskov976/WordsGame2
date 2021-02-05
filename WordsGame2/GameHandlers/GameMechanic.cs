using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WordsGame2.Interfaces;
using WordsGame2.Other;

namespace WordsGame2
{
    public class GameMechanic: IGameMechanics
    {
        private Settings _getSettings;
        private InfoCommand _getInfoCommand;
        public string baseWord;
        static Dictionary<char, int> baseWordDictionary;
        static string inputedWord;

        public GameMechanic(Settings getSettings, InfoCommand getInfoCommand)
        {
            _getSettings = getSettings;
            _getInfoCommand = getInfoCommand;
        }

        public Settings GetSettings { get => _getSettings; set => _getSettings = value; }
        public InfoCommand GetInfoCommand { get => _getInfoCommand; set => _getInfoCommand = value; }

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

        public virtual void RoundHandler(List<Players> players, Players player, bool lastRound = false)
        {
            GetSettings.TimerHandler.Timer.Start();
            player.AmountRounds++;
            RoundUI();
            if (GetSettings.TimerHandler.OutOfTime)
                player.IsAlive = false;
            if (player.IsAlive || lastRound)
            {
                if (GetInfoCommand.ShowInfo(inputedWord, players, GetSettings.TimerHandler.Timer, baseWord))
                {
                    RoundUI();
                    CountInputedWord(players, player, lastRound);
                }
                else
                    CountInputedWord(players, player, lastRound);
            }
        }

        public void RoundUI()
        {
            Console.Clear();
            Console.WriteLine('\n' + "Основное слово: {0}" + '\n' + "Для получения информации по игре введите следующие команды: ", baseWord);
            foreach (var command in GetInfoCommand.Commands)
                Console.WriteLine(command);
            Console.WriteLine("Введите полученное Вами слово:");
            inputedWord = Console.ReadLine();
        }

        public virtual void CountInputedWord(List<Players> players, Players player, bool lastRound)
        {
            int lettersCount = 0;
            if (inputedWord.Length > 0)
            {
                foreach (var c in inputedWord.ToUpper().Distinct())
                {
                    if (baseWordDictionary.ContainsKey(c))
                        if (inputedWord.ToUpper().Count(letter => letter == c) <= baseWordDictionary[c])
                            lettersCount++;
                }
                if (lettersCount == inputedWord.Distinct().Count())
                {
                    if (!players[0].ScoredWords.Any(item => item == inputedWord) && !players[1].ScoredWords.Any(item => item == inputedWord))
                    {
                        player.ScoredWords.Add(inputedWord);
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
