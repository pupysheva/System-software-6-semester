using System.Collections.Generic;

namespace MyTypes.Tree
{
    public interface ITreeNode<T> : IList<ITreeNode<T>>
    {
        /// <summary>
        /// Значение корня дерева.
        /// </summary>
        T Current { get; set; }

        /// <summary>
        /// Добавляет соседа к корню дерева.
        /// </summary>
        /// <param name="toAdd">Новый сосед.</param>
        void Add(T toAdd);

        /// <summary>
        /// Добавляет соседа-дерева к корню дерева.
        /// </summary>
        /// <param name="toAdd">Новый сосед со своими потомками.</param>
        void AddTreeNode(ITreeNode<T> toAdd);

        /// <summary>
        /// Получает перечисление только соседних детей корня.
        /// Для перечисления всех узлов используйте GetEnumerator.
        /// </summary>
        IEnumerable<ITreeNode<T>> GetEnumerableOnlyNeighbors();
    }
}
