using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WordsGame2.Interfaces;

namespace WordsGame2.Other
{
    public class TimerHandler: ITimer
    {
        const double interval = 1000;
        private Timer _timer;
        private Settings _getSettings;
        private int _timeLeft;
        private bool _outOfTime;

        public Timer Timer { get => _timer; set => _timer = value; }
        public Settings GetSettings { get => _getSettings; set => _getSettings = value; }
        public int TimeLeft { get => _timeLeft; set => _timeLeft = value; }
        public bool OutOfTime { get => _outOfTime; set => _outOfTime = value; }

        public TimerHandler(Settings settings)
        {
            _timer = new Timer(interval);
            _timer.Elapsed += TimerTick;
            _timer.AutoReset = true;
            _getSettings = settings;
            _timeLeft = settings.RoundDuration;
        }

        public virtual void TimerTick(Object source, ElapsedEventArgs e)
        {
            Console.Beep();
            int currentLineCursorTop = Console.CursorTop;
            int currentLineCursorLeft = Console.CursorLeft;
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 0);
            Console.Write("Осталось времени: {0} секунд(-ы)", TimeLeft);
            Console.SetCursorPosition(currentLineCursorLeft, currentLineCursorTop);
            Console.CursorVisible = true;
            TimeLeft -= 1;
            if (TimeLeft == -1)
                TimeIsOver();
        }

        public virtual void TimeIsOver()
        {
            OutOfTime = true;
            Timer.Stop();
            TimeLeft = GetSettings.RoundDuration;
            Console.Clear();
            Console.WriteLine("Время истекло." + '\n' + "Нажмите 'Ввод' для продолжения.");
        }
    }
}
