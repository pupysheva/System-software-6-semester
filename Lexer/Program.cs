using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Lexer
{
    class Program
    {
        static int Main(string[] args)
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
            if (!input.Exists)
                // Файл не найден.
                return 2;
            StreamReader stream;
            try
            {
                // Читаем текст.
                stream = input.OpenText();
            } catch (Exception e)
            {
                // Ошибка при чтении.
                Console.WriteLine(e.Message);
                Console.ReadKey(true);
                return 3;
            }
            List<Token> tokens;
            try
            {
                // Получаем токены.
                tokens = new Lang().SearchTokens(stream);
            }
            catch(LexerException e)
            {
                // Ошибка.
                Console.WriteLine(e + "\n" + e.StackTrace);
                return 5;
            }
            foreach (Token token in tokens)
                // Печатаем токины.
                Console.WriteLine(token);
            Console.Write("Press eny key...");
            Console.ReadLine();
            return 0;
        }
    }
}
