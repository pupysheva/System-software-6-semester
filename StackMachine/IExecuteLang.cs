using System.Collections.Generic;

namespace StackMachine
{
    public interface IExecuteLang
    {
        void Execute(IList<string> code);
    }
}