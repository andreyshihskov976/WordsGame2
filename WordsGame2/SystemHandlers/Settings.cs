using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WordsGame2.Other;
using WordsGame2.Interfaces;

namespace WordsGame2
{
    public class Settings: ICustomSettings, IDefaultSettings, IMenu
    {
        const int defaultMinLength = 8;
        const int defaultMaxLength = 30;
        const int defaultRoundDuration = 5;
        private TimerHandler _timer;
        private int _minLength;
        private int _maxLength;
        private int _roundDuration;

        public Settings()
        {
            SetDefaultSettings();
            _timer = new TimerHandler(this);
        }

        public int MinLength { get => _minLength; set => _minLength = value; }
        public int MaxLength { get => _maxLength; set => _maxLength = value; }
        public int RoundDuration { get => _roundDuration; set => _roundDuration = value; }
        public TimerHandler TimerHandler { get => _timer; set => _timer = value; }

        enum SettingsMenuActions
        {
            ChangeMinLength = 1,
            ChangeMaxLength = 2,
            ChangeRoundDuration = 3,
            ExitSettingsMenu = 4
        }

        public virtual void SetDefaultSettings()
        {
            _minLength = defaultMinLength;
            _maxLength = defaultMaxLength;
            _roundDuration = defaultRoundDuration;
        }

        public virtual void SetCustomSettings()
        {
            ShowMenu();
        }

        public void ShowMenu()
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
                            SetMinLength();
                            break;
                        case SettingsMenuActions.ChangeMaxLength:
                            SetMaxLength();
                            break;
                        case SettingsMenuActions.ChangeRoundDuration:
                            SetRoundDuration();
                            break;
                        case SettingsMenuActions.ExitSettingsMenu:
                            exit = !exit;
                            Console.Clear();
                            Console.WriteLine("Параметры сохранены." + '\n' + "Нажмите любую клавишу для продолжения.");
                            Console.ReadKey();
                            break;
                        default:
                            Console.Clear();
                            Console.Beep();
                            Console.WriteLine("Ошибка: 'Такого пункта не существует в меню.'" + '\n' +
                                                    "Нажмите любую клавишу для продолжения и повторите ввод.");
                            Console.ReadKey();
                            continue;
                    }
                }
            }
        }

        public void SetRoundDuration()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Задайте длительность раунда в секундах:");
                if (!Parsing.ParseInt(Console.ReadLine(), out _roundDuration))
                    continue;
                else
                {
                    TimerHandler.TimeLeft = RoundDuration;
                    break;
                }
            }
        }

        public void SetMaxLength()
        {
            while (true)
            {

                Console.Clear();
                Console.WriteLine("Введите максимальную длину слова:");
                if (!Parsing.ParseInt(Console.ReadLine(), out _maxLength))
                    continue;
                else
                    break;
            }
        }

        public void SetMinLength()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Введите минимальную длину слова:");
                if (!Parsing.ParseInt(Console.ReadLine(), out _minLength))
                    continue;
                else
                    break;
            }
        }
    }
}
