using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WordsGame2
{
    static class Settings
    {
        const double interval = 1000;
        const int defaultMinLength = 8;
        const int defaultMaxLength = 30;
        const int defaultRoundDuration = 10;
        static int minLength;
        static int maxLength;
        static Timer timer;
        static int timeLeft;
        static int roundDuration;

        public static int MinLength { get => minLength; set => minLength = value; }
        public static int MaxLength { get => maxLength; set => maxLength = value; }
        public static double Interval => interval;
        public static int RoundDuration { get => roundDuration; set => roundDuration = value; }
        public static int TimeLeft { get => timeLeft; set => timeLeft = value; }
        public static Timer Timer { get => timer; set => timer = value; }

        enum SettingsMenuActions
        {
            ChangeMinLength = 1,
            ChangeMaxLength = 2,
            ChangeRoundDuration = 3,
            ExitSettingsMenu = 4
        }

        public static void SetDefaultSettings()
        {
            minLength = defaultMinLength;
            maxLength = defaultMaxLength;
            roundDuration = timeLeft = defaultRoundDuration;
            timer = new Timer(interval);
            timer.Elapsed += GameMechanic.TimerTick;
            timer.AutoReset = true;
        }

        public static void SetCustomSettings()
        {
            int Action;
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Нажмите 1 для изменения минимальной длины слова." + '\n' + "Нажмите 2 для изменения максимальной длины слова." + '\n' + "Нажмите 3 для изменения длительности раунда." + '\n' + "Нажмите 4 для возврата в главное меню.");
                if (!Parsing.ParseInt(Console.ReadLine(), out Action))
                    continue;
                else
                {
                    switch ((SettingsMenuActions)Action)
                    {
                        case SettingsMenuActions.ChangeMinLength:
                            while (true)
                            {
                                Console.Clear();
                                Console.WriteLine("Введите минимальную длину слова:");
                                if (!Parsing.ParseInt(Console.ReadLine(), out minLength))
                                    continue;
                                else
                                    break;
                            }
                            break;
                        case SettingsMenuActions.ChangeMaxLength:
                            while (true)
                            {

                                Console.Clear();
                                Console.WriteLine("Введите максимальную длину слова:");
                                if (!Parsing.ParseInt(Console.ReadLine(), out maxLength))
                                    continue;
                                else
                                    break;
                            }
                            break;
                        case SettingsMenuActions.ChangeRoundDuration:
                            while (true)
                            {
                                Console.Clear();
                                Console.WriteLine("Задайте длительность раунда в секундах:");
                                if (!Parsing.ParseInt(Console.ReadLine(), out roundDuration))
                                    continue;
                                else
                                {
                                    timeLeft = roundDuration;
                                    break;
                                }
                            }
                            break;
                        case SettingsMenuActions.ExitSettingsMenu:
                            exit = !exit;
                            Console.Clear();
                            Console.WriteLine("Параметры сохранены." + '\n' + "Нажмите любую клавишу для продолжения.");
                            Console.ReadKey();
                            break;
                        default:
                            ExceptionsMessages.ShowExceptionMessage(ExceptionsMessages.ExceptionMessage["OutOfMenuException"]);
                            continue;
                    }
                }
            }
        }
    }
}
