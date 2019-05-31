using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Tree
{
    public interface IReadOnlyTreeNode<out T> : IReadOnlyList<IReadOnlyTreeNode<T>>
    {
        T Current { get; }
    }
}