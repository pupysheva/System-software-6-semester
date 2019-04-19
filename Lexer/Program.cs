using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Lexer
{
    class Program
    {
        static async void writerAsync(string str)
        {
            await Task.Run(() => writer(str));
        }

        static void writer(string str)
        {
            do
            {
                Console.WriteLine(str);
                System.Threading.Thread.Sleep(100);
            } while (true);
        }

        static void fun2()
        {
            writerAsync("1");
            writer("2");
        }

        static int Main(string[] args)
        {

            fun2();








            FileInfo input;
            if (args.Length != 1)
            {
                Console.Write("Name file: ");
                input = new FileInfo(Console.ReadLine());
            }
            else
            {
                input = new FileInfo(args[0]);
            }
            if (!input.Exists)
                return 2;
            StreamReader stream;
            try
            {
                stream = input.OpenText();
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey(true);
                return 3;
            }
            BufferedStream bStream = new BufferedStream(stream.BaseStream);
            if (!bStream.CanRead)
                return 4;
            List<Token> tokens;
            try
            {
                //tokens = new Lang().SearchTokens(bStream); TODO
            }
            catch(LexerException e)
            {
                Console.WriteLine(e + "\n" + e.StackTrace);
                return 5;
            }
            //foreach (Token token in tokens)
            //    Console.WriteLine(token);
            return 0;
        }
    }
}
