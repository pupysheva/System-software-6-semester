using System;
using System.Collections.Generic;

namespace Lexer
{
    /// <summary>
    /// Описывает поиск терминалов.
    /// </summary>
    class StateOfSearchTerminal
    {
        private StateOfSearchTerminal() : this(new List<Terminal>()) { }
        private StateOfSearchTerminal(IList<Terminal> Terminals, uint Count = 0)
        {
            this.Count = Count;
            this.Terminals = Terminals ?? throw new ArgumentNullException();
        }
        /// <summary>
        /// Найденные терминалы.
        /// </summary>
        public IList<Terminal> Terminals { get; }
        /// <summary>
        /// Количество найденных.
        /// </summary>
        public uint Count { private set; get; }

        
    }
}
