using Lexer;
using System;
using System.Collections.Generic;
using System.IO;

namespace StackMachine
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            StreamReader stream = OpenFileVisualInterface(args);
            List<Token> tokens;
            try
            {
                // Получаем жетоны.
                tokens = Lang.lexerLang.SearchTokens(stream);
            }
            catch (LexerException e)
            {
                // Ошибка.
                Console.WriteLine(e + "\n" + e.StackTrace);
                Console.ReadKey();
                return 5;
            }
            stream.Close();
            tokens.RemoveAll((Token t) => t.Type.Name.Contains("CH_"));
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (Token token in tokens)
                // Печатаем жетоны.
                Console.WriteLine(token);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("-----\nОтчёт компиляции:\n-----");
            var report = Lang.parserLang.Check(tokens);
            Console.WriteLine(string.Join("\n", report.Info));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-----\nДерево компиляции:\n-----");
            Console.WriteLine(report.Compile);
            if(!report.IsSuccess)
            {
                Console.WriteLine("Не могу запустить...");
                Console.ReadKey();
                return 6;
            }
            List<string> commands = Lang.parserLang.Compile(tokens);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("-----\nПольская запись:\n-----");
            Console.WriteLine(string.Join(", ", commands));
            Console.ForegroundColor = ConsoleColor.White;
            Lang.stackMachine.Execute(commands);
            Console.Write("Press eny key...");
            Console.ReadLine();
            return 0;
        }

        /// <summary>
        /// Пытается открыть файл из аргумента командной строки.
        /// Если не получится, спрашивает у входящего потока.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        /// <returns>Открытый файл для чтения.</returns>
        static StreamReader OpenFileVisualInterface(string[] args)
        {
            FileInfo input;
            if (args.Length != 1)
            { // Если файл из аргументов программы не взят
                Console.Write("Name file: ");
                input = new FileInfo(Console.ReadLine());
            }
            else
            { // Если есть аргументы, то берём из аргументов.
                input = new FileInfo(args[0]);
            }
            return input.OpenText();
        }
    }
}
