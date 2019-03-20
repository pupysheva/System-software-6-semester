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
        private List<Terminal> avalibleTerminals = new List<Terminal>(
           new Terminal[]
           {
               new Terminal("ASSIGN_OP", "^=$"),
               new Terminal("VAR", "^[a-zA-Z]+$"),
               new Terminal("DIGIT", "^0|([1-9][0-9]+)$")
           }
            );

        public List<Token> SearchTokens(BufferedStream input)
        {

        }
    }
}
