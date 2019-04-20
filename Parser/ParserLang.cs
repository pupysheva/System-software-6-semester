using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lexer;

namespace Parser
{
    /// <summary>
    /// Представляет собой класс, который реализует парсер.
    /// </summary>
    class ParserLang
    {
        public ParserLang() { }

        /// <summary>
        /// Проверяет, соответсвует ли заданый язык программирования граматике.
        /// </summary>
        /// <param name="tokens">Список терминалов входного файла.</param>
        /// <returns>Отчёт об ошибках.</returns>
        public ReportParser Check(List<Token> tokens)
        {
            throw new NotImplementedException();
        }
    }
}
