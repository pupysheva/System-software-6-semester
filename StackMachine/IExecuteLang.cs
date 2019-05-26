using System.Collections.Generic;

namespace StackMachine
{
    public interface IExecuteLang
    {
        /// <summary>
        /// Выполняет код на данном компьютере.
        /// </summary>
        /// <param name="code">Список команд.</param>
        void Execute(IList<string> code);
    }
}