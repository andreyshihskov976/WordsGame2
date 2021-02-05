using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsGame2
{
    static class Parsing
    {
        public static bool ParseInt(string inputString, out int outputInt)
        {
            int tryParse;
            if (int.TryParse(inputString, out tryParse))
            {
                outputInt = tryParse;
                return true;
            }
            else
            {
                Console.Clear();
                Console.Beep();
                Console.WriteLine("Ошибка: 'Вводимое значение должно быть целым числом.'" + '\n' +
                                        "Нажмите любую клавишу для продолжения и повторите ввод.");
                Console.ReadKey();
                outputInt = 0;
                return false;
            }
        }
    }
}
