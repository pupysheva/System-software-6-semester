using System;
using System.Collections.Generic;
using System.IO;
using Lexer;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Файл: ");
            string filename;
            if (args.Length > 0)
                filename = args[0];
            else
                filename = Console.ReadLine();
            try
            {
                using StreamReader file = new StreamReader(filename);
                List<Token> tokens = Lexer.ExampleLang.Lang.SearchTokens(file);
                tokens.RemoveAll((Token t) => t.Type.Name.Contains("CH_"));
                var report = Parser.ExampleLang.Lang.Check(tokens);
                Console.WriteLine(report.ToString());
            }
            catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
            Console.ReadKey(true);
        }
    }
}
