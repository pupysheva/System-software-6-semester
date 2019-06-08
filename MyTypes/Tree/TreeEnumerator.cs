using System;
using System.Collections;
using System.Collections.Generic;

namespace MyTypes.Tree
{
    public partial class TreeNode<T>
    {
        public class TreeEnumerator : IEnumerator<TreeNode<T>>
        {
            public TreeNode<T> Start;
            public LinkedList<IEnumerator<TreeNode<T>>> Line;
            private bool IsMoved = false;

            public TreeEnumerator(TreeNode<T> Start)
            {
                this.Start = Start;
                Line = new LinkedList<IEnumerator<TreeNode<T>>>();
            }

            public TreeNode<T> Current
                => Line.First.Value.Current;

            object IEnumerator.Current
                => Current;

            public void Dispose()
            {
                Start = null;
                Line = null;
            }


            public bool MoveNext()
            {
                if (!IsMoved)
                {
                    IsMoved = true;
                    if (Start != null)
                    {
                        Line.AddLast(new OnceEnumerator<TreeNode<T>>(Start));
                        foreach (TreeNode<T> node in Start.Children)
                        {
                            Line.AddLast(node.GetEnumerator());
                        }
                        return MoveNext();
                    }
                    return false;
                }
                else
                {
                    while (!Line.First.Value.MoveNext())
                    {
                        Line.RemoveFirst();
                        if (Line.Count <= 0)
                            return false;
                    }
                    return true;
                }
            }

            public void Reset()
            {
                Line.Clear();
                IsMoved = false;
            }
        }
    }

    internal class OnceEnumerator<J> : IEnumerator<J>
    {
        private J Start;
        private byte MoveCount = 0;

        public OnceEnumerator(J Start)
        {
            this.Start = Start;
        }

        public J Current
        {
            get
            {
                if (MoveCount == 1)
                    return Start;
                else
                    throw new NotSupportedException("Необходимо сначала вызвать MoveNext()");
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
            => Start = default(J);

        public bool MoveNext()
        {
            if (MoveCount > 0)
                return false;
            MoveCount += MoveCount < 2 ? (byte)1 : (byte)0;
            return true;
        }

        public void Reset()
        {
            MoveCount = 0;
        }
    }
}
