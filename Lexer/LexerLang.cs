using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Lexer
{
    public class LexerLang
    {
        /// <summary>
        /// Создание экземпляра обработчика.
        /// </summary>
        public LexerLang()
        {
            avalibleTerminals = new List<Terminal>(
            new Terminal[]
            {
                new Terminal("ASSIGN_OP", "^=$"),
                new Terminal("VAR", "^[a-zA-Z]+$",uint.MaxValue),
                new Terminal("DIGIT", "^0|([1-9][0-9]*)$"),
                new Terminal("OP", "^\\+|-|\\*|/$"),
                new Terminal("LOGICAL_OP", "^>|<|>=|<=|==$"),
                new Terminal("WHILE_KW", "^while$", 0),
                new Terminal("PRINT_KW", "^print$",0),
                new Terminal("FOR_KW", "^for$",0),
                new Terminal("IF_KW", "^if$",0),
                new Terminal("ELSE_KW", "^else$",0),
                new Terminal("L_QB", "^{$"),
                new Terminal("R_QB", "^}$"),
                new Terminal("L_B", "^\\($"),
                new Terminal("R_B", "^\\)$"),
                new Terminal("COMMA", "^;$"),
                new Terminal("COM", "^,$"),
                /*
                 Те терминалы, которые ниже, по-сути нужны парсеру.
                 Для того, чтобы проанализировать выражение:
                 a = "Привет, мир!",
                 чтобы не было так:
                 a="Привет,мир!".
                 */
                new Terminal("CH_SPACE", "^ $"),
                new Terminal("CH_LEFTLINE", "^\r$"),
                new Terminal("CH_NEWLINE", "^\n$"),
                new Terminal("CH_TAB", "^\t$")
            }
            );
        }

        /// <summary>
        /// Создание экземпляра обработчика.
        /// </summary>
        /// <param name="avalibleTerminals">Набор разрешённых терминалов.</param>
        public LexerLang(IEnumerable<Terminal> avalibleTerminals)
        {
            this.avalibleTerminals = new List<Terminal>(avalibleTerminals ?? throw new ArgumentNullException());
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
                if (!lastAdd && !input.EndOfStream)
                {
                    input.Read(buffer, 0, 1); // Чтение символа.
                    bufferList.Append(buffer[0]); // Запись символа в строку.
                }
                lastAdd = false;
                // Получение списка подходящих терминалов:
                termsFound = SearchInTerminals(bufferList.ToString());

                // Ура, мы что-то, кажется, нашли.
                if (termsFound.Count <= 1 || input.EndOfStream)
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
                    }
                    if (termsFound.Count != 1) // Ой, должен был остаться только один.
                    {
                        if (termsFound.Count == 0)
                            throw new LexerException
                            ("Количество подходящих терменалов не равно 1: " + termsFound.Count);
                        Terminal need = termsFound.First();
                        Terminal oldNeed = null;
                        bool unical = true; // True, если необходимый терминал имеет самый высокий приоритет.
                        for (int i = 1; i < termsFound.Count; i++)
                        {
                            if (termsFound[i] > need)
                            {
                                need = termsFound[i];
                                unical = true;
                            }
                            else if (Terminal.PriorityEquals(termsFound[i], need))
                            {
                                oldNeed = termsFound[i];
                                unical = false;
                            }
                        }
                        if (!unical)
                            throw new LexerException
                                ($"Количество подходящих терменалов не равно 1: {termsFound.Count}" +
                                $", возможно был конфликт между: {oldNeed} и {need}");
                        termsFound.Clear();
                        termsFound.Add(need);
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
