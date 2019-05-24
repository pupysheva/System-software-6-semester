using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackMachine
{
    public abstract class AbstractStackExecuteLang : IExecuteLang
    {
        protected readonly IDictionary<string, double> Variables
            = new Dictionary<string, double>();

        protected readonly Stack<string> Stack
            = new Stack<string>();

        public int InstructionPointer { get; protected set; }
            = -1;

        public virtual void Execute(IList<string> code)
        {
            while (++InstructionPointer < code.Count)
                ExecuteCommand(code[InstructionPointer]);
        }

        protected abstract void ExecuteCommand(string command);
    }
}
