using System;
using System.Collections;
using System.Collections.Generic;

namespace MyTypes.Tree
{
    /// <summary>
    /// Класс реализует дерево.
    /// Класс не является потокобезопасным.
    /// </summary>
    /// <typeparam name="T">Тип, который хранится в каждом узле дерева.</typeparam>
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
        /// Получение доступа к соседним потомкам данного узла.
        /// </summary>
        /// <param name="index">Номер соседнего потомка, который вам нужен.
        /// Для того, чтобы узнать количество соседних потомков, воспользуйтесь <see cref="Count"/>.</param>
        public TreeNode<T> this[int index]
        {
            get => (TreeNode<T>)Children[index];
            set => Children[index] = value;
        }

        /// <summary>
        /// Значение текущего узла.
        /// </summary>
        public T Current { get; set; }

        /// <summary>
        /// Возвращает количество соседних потомков.
        /// </summary>
        public int Count => Children.Count;

        bool ICollection<ITreeNode<T>>.IsReadOnly => false;

        /// <summary>
        /// Добавить потомка.
        /// </summary>
        /// <param name="item">Значение узла, который должен стать потомком текущего дерева.</param>
        public void Add(T item)
        {
            if (IsNeedAddSwap && item is ITreeNode<T> itemTree)
                Add(itemTree);
            else
                Children.Add(new TreeNode<T>(item));
        }

        /// <summary>
        /// Добавить узел потомка к текущему узлу.
        /// </summary>
        /// <param name="item">Узел, который должен стать потомком.</param>
        public void Add(ITreeNode<T> item)
        {
            if (IsNeedAddSwap && item.Current is ITreeNode<T>)
                throw new NotSupportedException($"Операция не поддерживается. Отключите {nameof(IsNeedAddSwap)}");
            else
                Children.Add(item);
        }

        /// <summary>
        /// Очищает список потомка данного узла.
        /// </summary>
        public void Clear()
        {
            Children.Clear();
        }

        /// <summary>
        /// Определяет, содержится ли узел среди соседей.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Получает перечеслитель, который проходится по всем узлам дерева и поддеревьев, включая корень дерева.
        /// </summary>
        public TreeEnumerator GetEnumerator()
            => new TreeEnumerator(this);

        /// <summary>
        /// Рисование дерева в виде строки.
        /// По факту вызывается <see cref="TreeTools.ToString{T}(ITreeNode{T}, StringFormat)"/>
        /// с параметром <see cref="StringFormat.NewLine"/>.
        /// </summary>
        public override string ToString()
            => this.ToString(StringFormat.NewLine);

        /// <summary>
        /// Получает перечислитель, который перечисляет только соседних потомков.
        /// </summary>
        public IEnumerable<ITreeNode<T>> GetEnumerableOnlyNeighbors()
            => Children;
    }
}
