using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Lexer
{
    public class Lang
    {
        /// <summary>
        /// Создание экземпляра обработчика.
        /// </summary>
        public Lang()
        {
            avalibleTerminals = new List<Terminal>(
            new Terminal[]
            {
                new Terminal("ASSIGN_OP", "^=$"),
                new Terminal("VAR", "^[a-zA-Z]+$"),
                new Terminal("DIGIT", "^0|([1-9][0-9]*)$"),
                new Terminal("OP", "^\\+|-|\\*|/"),
                new Terminal("WHILE_KW", "^while$"),
                new Terminal("PRINT_KW", "^print$"),
                new Terminal("FOR_KW", "^for$"),
                new Terminal("IF_KW", "^if$"),
                new Terminal("ELSE_KW", "^else$"),
                new Terminal("L_QB", "^{$"),
                new Terminal("R_QB", "^}$"),
                new Terminal("L_B", "^($"),
                new Terminal("R_B", "^)$"),
                /*
                 Те терминалы, которые ниже, по-сути нужны парсеру.
                 Для того, чтобы проанализировать выражение:
                 a = "Привет, мир!",
                 чтобы не было так:
                 a="Привет,мир!".
                 */
                new Terminal("CH_SPACE", "^ $"),
                new Terminal("CH_LEFTLINE", "^\r$"),
                new Terminal("CH_NEWLINE", "^\n$")
            }
            );
        }

        /// <summary>
        /// Создание экземпляра обработчика.
        /// </summary>
        /// <param name="avalibleTerminals">Набор разрешённых терминалов.</param>
        public Lang(List<Terminal> avalibleTerminals)
        {
            this.avalibleTerminals = avalibleTerminals
                ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Список поддерживаемых терминалов.
        /// </summary>
        private readonly List<Terminal> avalibleTerminals;

        /// <summary>
        /// Переобразование входного текста в лист токенов на основе
        /// правил терминалов.
        /// </summary>
        /// <param name="input">Входной поток текста.</param>
        /// <returns>Список найденных токенов.</returns>
        public virtual List<Token> SearchTokens(StreamReader input)
        {
            if (input == null)
                throw new ArgumentNullException("BufferedStream input = null");
            List<Token> output = new List<Token>(); // Сюда запишем вывод.
            StringBuilder bufferList = new StringBuilder(); // Строка из файла.
            char[] buffer = new char[1]; // Сюда попадает символ перед тем, как попасть в строку.
            List<Terminal> termsFound; // Сюда помещаются подходящие терминалы к строке bufferList.

            // True, если последняя итерация была с добавлением элемента в output. Иначе - False.
            bool lastAdd = false;
            while (!input.EndOfStream || bufferList.Length != 0)
            {
                if (!lastAdd)
                {
                    input.Read(buffer, 0, 1); // Чтение символа.
                    bufferList.Append(buffer[0]); // Запись символа в строку.
                }
                lastAdd = false;
                // Получение списка подходящих терминалов:
                termsFound = SearchInTerminals(bufferList.ToString());

                // Ура, мы что-то, кажется, нашли.
                if (termsFound.Count <= 1)
                {
                    if (termsFound.Count == 1 && !input.EndOfStream)
                        // Это ещё не конец файла и есть 1 прецидент. Ищем дальше.
                        continue;
                    int last = char.MaxValue + 1;
                    if (termsFound.Count == 0)
                    {
                        last = bufferList[bufferList.Length - 1]; // Запоминаем последний символ.
                        bufferList.Length--; // Уменьшаем длинну списка на 1.
                        termsFound = SearchInTerminals(bufferList.ToString()); // Теперь ищем терминалы.
                        if (termsFound.Count != 1) // Ой, должен был остаться только один.
                            throw new LexerException
                                ("Количество подходящих терменалов не равно 1: " + termsFound.Count);
                    }
                    // Всё идёт как надо
                    // Добавим в результаты
                    output.Add(
                        new Token(
                        termsFound.First(),
                        bufferList.ToString()
                        ));
                    bufferList.Clear();
                    lastAdd = true;
                    if (last != char.MaxValue + 1)
                        bufferList.Append((char)last);
                }
            }
            return output;
        }

        private List<Terminal> SearchInTerminals(string expression)
        {
            List<Terminal> output = new List<Terminal>();
            foreach (Terminal ter in avalibleTerminals)
            {
                Match mat = ter.RegularExpression.Match(expression);
                if (mat.Length > 0 && mat.Value.Equals(expression))
                    output.Add(ter);
            }
            return output;
        }
    }
}
