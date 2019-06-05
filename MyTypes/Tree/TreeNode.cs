using System;
using System.Collections;
using System.Collections.Generic;

namespace MyTypes.Tree
{
    public partial class TreeNode<T> : ITreeNode<T>
    {
        protected List<ITreeNode<T>> Children
            = new List<ITreeNode<T>>();

        public TreeNode(T Current = default(T))
        {
            this.Current = Current;
        }

        /// <summary>
        /// True, если при вызове <see cref="Add(T)"/> нужно проверять,
        /// является ли это деревом и тогда отправлять аргумент в <see cref="Add(ITreeNode{T})"/>.
        /// По умолчанию: true.
        /// </summary>
        public bool IsNeedAddSwap { get; set; } = true;

        /// <summary>
        /// Возвращает потомков данной ветви.
        /// </summary>
        /// <param name="index">Номер потомка, который вам нужен.</param>
        ITreeNode<T> IList<ITreeNode<T>>.this[int index]
        {
            get => Children[index];
            set => Children[index] = value;
        }

        /// <summary>
        /// Возвращает потомков данной ветви.
        /// </summary>
        /// <param name="index">Номер потомка, который вам нужен.</param>
        public TreeNode<T> this[int index]
        {
            get => (TreeNode<T>)Children[index];
            set => Children[index] = value;
        }

        public T Current { get; set; }

        public int Count => Children.Count;

        public bool IsReadOnly => ((IList<ITreeNode<T>>)Children).IsReadOnly;

        public void Add(T item)
        {
            if (IsNeedAddSwap && item is ITreeNode<T> itemTree)
                Add(itemTree);
            else
                Children.Add(new TreeNode<T>(item));
        }

        public void Add(ITreeNode<T> item)
        {
            if (IsNeedAddSwap && item.Current is ITreeNode<T>)
                throw new NotSupportedException($"Операция не поддерживается. Отключите {nameof(IsNeedAddSwap)}");
            else
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
            => this.ToString(StringFormat.NewLine);

        public IEnumerable<ITreeNode<T>> GetEnumerableOnlyNeighbors()
            => Children;
    }
}
