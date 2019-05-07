using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Lexer;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Файл: ");
            try
            {
                using (StreamReader file = new StreamReader(Console.ReadLine()))
                {
                    Console.WriteLine(new ParserLang().Check(new LexerLang().SearchTokens(file)).ToString());
                }
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
