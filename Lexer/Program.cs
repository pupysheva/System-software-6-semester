using System;
using System.IO;

namespace Lexer
{
    class Program
    {
        static int Main(string[] args)
        {
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

        }
    }
}
