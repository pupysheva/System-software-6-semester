using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Parser.Tree
{
    public class TreeNode<T> : ITreeNode<T>
    {
        protected List<ITreeNode<T>> Children
            = new List<ITreeNode<T>>();

        public TreeNode(T Current = default(T))
        {
            this.Current = Current;
        }

        /// <summary>
        /// Возвращает потомков данной ветви.
        /// </summary>
        /// <param name="index">Номер потомка, который вам нужен.</param>
        public ITreeNode<T> this[int index]
        {
            get => Children[index];
            set => Children[index] = value;
        }

        public T Current { get; set; }

        public int Count => Children.Count;

        public bool IsReadOnly => ((IList<ITreeNode<T>>)Children).IsReadOnly;

        public void Add(ITreeNode<T> item)
        {
            Children.Add(item);
        }

        public void Clear()
        {
            Children.Clear();
        }

        public bool Contains(ITreeNode<T> item)
        {
            return Children.Contains(item);
        }

        public void CopyTo(ITreeNode<T>[] array, int arrayIndex)
        {
            Children.CopyTo(array, arrayIndex);
        }

        public int IndexOf(ITreeNode<T> item)
        {
            return Children.IndexOf(item);
        }

        public void Insert(int index, ITreeNode<T> item)
        {
            Children.Insert(index, item);
        }

        public bool Remove(ITreeNode<T> item)
        {
            return Children.Remove(item);
        }

        public void RemoveAt(int index)
        {
            Children.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        IEnumerator<ITreeNode<T>> IEnumerable<ITreeNode<T>>.GetEnumerator()
            => GetEnumerator();

        public TreeEnumerator GetEnumerator()
            => new TreeEnumerator(this);

        public override string ToString()
            => ToString(StringFormat.Default);

        public string ToString(StringFormat sf)
        {
            switch (sf)
            {
                case StringFormat.Default:
                    return $"{base.ToString()}: cur: {Current}, deep: [{string.Join(", ", Children)}]";
                case StringFormat.NewLine:
                    {
                        string[] lines = $"{Current}: \n<NEEDTAB_TreeNode>{string.Join("\n", Children)}\n</NEEDTAB_TreeNode>".Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        int count = 0, countOpen, countClose;
                        Regex regOpen = new Regex("<NEEDTAB_TreeNode>");
                        Regex regClose = new Regex("</NEEDTAB_TreeNode>");
                        for (long i = 0; i < lines.LongLength; i++)
                        {
                            countOpen = regOpen.Match(lines[i]).Groups.Count;
                            countClose = regClose.Match(lines[i]).Groups.Count;
                            count += countOpen - countClose;
                            if (count > 0)
                                lines[i] = lines[i].Insert(0, new string('\t', count));
                            if (countOpen != 0)
                                lines[i] = lines[i].Replace("<NEEDTAB_TreeNode>", "");
                            if(countClose != 0)
                                lines[i] = lines[i].Replace("</NEEDTAB_TreeNode>", "");
                        }
                        return string.Join("\n", lines);
                    }
                case StringFormat.Base:
                    return base.ToString();
                default:
                    throw new NotSupportedException($"Формат {sf} не поддерживается.");
            }
        }

        public enum StringFormat
        {
            Default,
            NewLine,
            Base
        }

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
            if (MoveCount != 1)
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
