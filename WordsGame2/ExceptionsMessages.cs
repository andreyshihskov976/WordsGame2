﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsGame2
{
    static class ExceptionsMessages
    {
        public static Dictionary<string, string> ExceptionMessage = new Dictionary<string, string>()
        {
            {"EmptyBaseWordException","Ошибка: Вы ввели пустую строку." + '\n' +
                "Минимальная длина слова должна составлять: " + Settings.MinLength.ToString() + '\n' +
                "Максимальная длина слова должна составлять: " + Settings.MaxLength.ToString() + '\n' +
                "Нажмите любую клавишу для продолжения и повторите ввод."},
            { "BaseWordLengthException", "Слово не подходит по правилам." + '\n' +
                "Минимальная длина слова должна составлять: " + Settings.MinLength.ToString() + '\n' +
                "Максимальная длина слова должна составлять: " + Settings.MaxLength.ToString() + '\n' +
                "Нажмите любую клавишу для продолжения и повторите ввод."},
            { "CheckBaseWordException", "Ошибка: Базовое слово не может содержать цифры и прочие знаки, кроме букв." + '\n' +
                "Нажмите любую клавишу для продолжения и повторите ввод."},
            { "ParseIntException", "Ошибка: 'Вводимое значение должно быть целым числом.'" + '\n' +
                                    "Нажмите любую клавишу для продолжения и повторите ввод."},
            { "OutOfMenuException","Ошибка: 'Такого пункта не существует в меню.'" + '\n' +
                                    "Нажмите любую клавишу для продолжения и повторите ввод."}
        };

        public static void ShowExceptionMessage(string message)
        {
            Console.Clear();
            Console.Beep();
            Console.WriteLine(message);
            Console.ReadKey();
        }
    }
}
