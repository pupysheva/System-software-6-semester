using System.Collections.Generic;

namespace MyTypes.Tree
{
    public interface IReadOnlyTreeNode<out T> : IReadOnlyList<IReadOnlyTreeNode<T>>
    {
        T Current { get; }
    }
}