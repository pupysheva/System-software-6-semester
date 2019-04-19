using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lexer
{
    public class Lang
    {
        public Lang()
        {
        }

        /// <summary>
        /// Список поддерживаемых терминалов.
        /// </summary>
        private readonly List<Terminal> avalibleTerminals = new List<Terminal>(
           new Terminal[]
           {
               new Terminal("ASSIGN_OP", "^=$"),
               new Terminal("VAR", "^[a-zA-Z]+$"),
               new Terminal("DIGIT", "^0|([1-9][0-9]+)$")
           }
            );

        public async Task<List<Token>> SearchTokens(BufferedStream input)
        {
            if (input == null)
                throw new ArgumentNullException("BufferedStream input = null");
            if (!input.CanRead)
                throw new ArgumentException("BufferedStream can't read.");
            char[] a = new char[1];
            await new StreamReader(input).ReadBlockAsync(a, 0, 1);
            int w = 2;
            throw new NotImplementedException();
        }
    }
}
