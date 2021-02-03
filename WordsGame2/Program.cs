using System;

namespace WordsGame2
{
    class Program
    {
        enum MainMenuActions
        {
            SetCustomSettings = 1,
            StartGame = 2,
            ExitGame = 3
        }

        static void Main(string[] args)
        {
            int Action;
            Console.WriteLine("Добро пожаловать в игру 'Слова'." + '\n' + "Для продолжения нажмите любую клавишу:");
            Console.ReadKey();
            Settings.SetDefaultSettings();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Нажмите 1 для изменения настроек." + '\n' + "Нажмите 2 для начала игры." + '\n' + "Нажмите 3 для выхода из игры.");
                if (Parsing.ParseInt(Console.ReadLine(), out Action))
                    switch ((MainMenuActions)Action)
                    {
                        case MainMenuActions.SetCustomSettings:
                            Settings.SetCustomSettings();
                            break;
                        case MainMenuActions.StartGame:
                            //Console.WriteLine("Введите имя первого пользователя:");
                            Players player1 = new Players("Игрок 1");
                            Players player2 = new Players("Игрок 2");
                            Players[] players = new Players[] { player1, player2 };
                            //создание пользователей
                            GameMechanic.StartGame(players);
                            break;
                        case MainMenuActions.ExitGame:
                            Environment.Exit(0);
                            break;
                        default:
                            ExceptionsMessages.ShowExceptionMessage(ExceptionsMessages.ExceptionMessage["OutOfMenuException"]);
                            break;
                    }

            }
        }
    }
}
