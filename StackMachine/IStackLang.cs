using System.Collections.Generic;

namespace StackMachine
{
    public interface IStackLang
    {
        void Execute(IList<string> code);
    }
}