using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Tree
{
    public interface ITreeNode<T> : IList<ITreeNode<T>>
    {
        T Current { get; set; }
    }
}
