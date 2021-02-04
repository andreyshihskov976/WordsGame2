using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;

namespace WordsGame2
{
    static class GameMechanic
    {
        
        static string baseWord;
        static Dictionary<char, int> baseWordDictionary;
        static string inputedWord;
        static bool gameEnd;
        

        public static void BaseWordInput()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Введите первоначальное слово:" + '\n' +
                    "Минимальная длина слова должна составлять: " + Settings.MinLength.ToString() + '\n' +
                    "Максимальная длина слова должна составлять: " + Settings.MaxLength.ToString());
                baseWord = Console.ReadLine();
                if (baseWord.Length > 0)
                    if (Settings.MinLength <= baseWord.Length && baseWord.Length <= Settings.MaxLength)
                    {
                        if (CheckBaseWord())
                        {
                            ExceptionsMessages.ShowExceptionMessage(ExceptionsMessages.ExceptionMessage["CheckBaseWordException"]);
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
                        ExceptionsMessages.ShowExceptionMessage(ExceptionsMessages.ExceptionMessage["BaseWordLengthException"]);
                        continue;
                    }
                else
                {
                    ExceptionsMessages.ShowExceptionMessage(ExceptionsMessages.ExceptionMessage["EmptyBaseWordException"]);
                    continue;
                }
            }
        }

        static void Round(List<Players> players,Players player, bool lastRound = false)
        {
            Console.Clear();
            Settings.Timer.Start();
            player.TriesCount++;
            Console.WriteLine('\n' + "Основное слово: {0}" + '\n' +
                "Введите полученное слово:", baseWord);
            inputedWord = Console.ReadLine();
            if (gameEnd)
                player.IsAlive = false;
            if (player.IsAlive || lastRound)
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
                        if (!players[0].CountedWords.Any(item => item == inputedWord) && !players[1].CountedWords.Any(item => item == inputedWord))
                        {
                            player.CountedWords.Add(inputedWord);
                            Console.WriteLine("Слово засчитано.");
                            Settings.Timer.Stop();
                            Settings.TimeLeft = Settings.RoundDuration;
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
        }

        public static void StartGame(List<Players> players)
        {
            gameEnd = false;
            BaseWordInput();
            Players player = players[1];
            while (players[0].IsAlive || players[1].IsAlive)
            {
                SwitchPlayers(players, ref player);
                if (gameEnd && players[0].TriesCount != players[1].TriesCount)
                    if (players[0].TriesCount > players[1].TriesCount)
                        Round(players, players[1], true);
                    else
                        Round(players, players[1], true);
            }
            ShowResults(players);
            Console.WriteLine("Нажмите любую клавишу для перехода в меню.");
            Console.ReadKey();
        }

        static void SwitchPlayers(List<Players> players, ref Players player)
        {
            if (player == players[1])
            {
                player = players[0];
                Round(players, player);
            }
            else if (player == players[0])
            {
                player = players[1];
                Round(players, player);
            }
        }

        public static void TimerTick(Object source, ElapsedEventArgs e)
        {
            Console.Beep();
            int currentLineCursorTop = Console.CursorTop;
            int currentLineCursorLeft = Console.CursorLeft;
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 0);
            Console.Write("Осталось времени: {0} секунд(-ы)", Settings.TimeLeft);
            Console.SetCursorPosition(currentLineCursorLeft, currentLineCursorTop);
            Console.CursorVisible = true;
            Settings.TimeLeft -= 1;
            if (Settings.TimeLeft == -1)
                StopRound();
        }

        private static void StopRound()
        {
            gameEnd = true;
            Settings.Timer.Stop();
            Settings.TimeLeft = Settings.RoundDuration;
            Console.Clear();
            Console.WriteLine("Время истекло." + '\n' + "Нажмите 'Ввод' для продолжения.");
        }

        private static void ShowResults(List<Players> players)
        {
            Console.Clear();
            foreach (var player in players)
            {
                Console.WriteLine("{0} смог(-ла) составить: {1} слов(-а) из {2}", player.PlayerName, player.CountedWords.Count, baseWord);
                foreach (var word in player.CountedWords)
                    Console.WriteLine(word);
                Console.WriteLine();
            }
        }

        private static bool CheckBaseWord()
        {
            if (Regex.Match(baseWord, @"^[а-яА-Я]+$").Success || Regex.Match(baseWord, @"^[A-Za-z]+$").Success)
                return false;
            else
                return true;
        }
    }
}
