using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTypes.LinkedList
{
    public class MyLinkedList<T> : ICollection<T>, ICollection, IEnumerable<MyLinkedListNode<T>>
    {
        /// <summary>
        /// Возвращает последний узел <see cref="MyLinkedListNode{T}"/> из <see cref="MyLinkedList{T}"/>.
        /// </summary>
        public MyLinkedListNode<T> Last { get; private set; }

        /// <summary>
        /// Возвращает первый <see cref="MyLinkedListNode{T}"/> из <see cref="MyLinkedList{T}"/>.
        /// </summary>
        public MyLinkedListNode<T> First { get; private set; }

        /// <summary>
        /// Возвращает количество узлов, содержащихся в <see cref="MyLinkedList{T}"/>.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Последнее изменение этого листа.
        /// </summary>
        private DateTime lastModifed;

        bool ICollection<T>.IsReadOnly => false;

        /// <summary>
        /// Получает объект, с помощью которого можно синхронизировать доступ к коллекции <see cref="MyLinkedList{T}"/>.
        /// </summary>
        object ICollection.SyncRoot => this;

        /// <summary>
        /// Возвращает значение, показывающее, является ли доступ к коллекции <see cref="ICollection"/>
        /// синхронизированным (потокобезопасным).
        /// </summary>
        /// <returns>true, если доступ к классу <see cref="ICollection"/> является синхронизированным
        /// (потокобезопасным); в противном случае — false.</returns>
        public virtual bool IsSynchronized => false;

        void ICollection<T>.Add(T item)
            => AddLast(item);
        
        /// <summary>
        /// Добавляет новый узел, содержащий указанное значение после указанного существующего
        /// узла в <see cref="MyLinkedList{T}"/>.
        /// </summary>
        /// <param name="node"><see cref="MyLinkedListNode{T}"/>, после которого необходимо
        /// вставить новый <see cref="MyLinkedListNode{T}"/> содержащий value.</param>
        /// <param name="value">Значение для добавления <see cref="MyLinkedList{T}"/>.</param>
        /// <returns>Новый <see cref="MyLinkedListNode{T}"/> содержащий value.</returns>
        /// <exception cref="ArgumentNullException">Свойство node имеет значение null.</exception>
        /// <exception cref="InvalidOperationException">node не в текущем <see cref="MyLinkedList{T}"/>.</exception>
        public LinkedListNode<T> AddAfter(MyLinkedListNode<T> node, T value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Добавляет указанный новый узел после указанного существующего узла в <see cref="MyLinkedList{T}"/>.
        /// </summary>
        /// <param name="node"><see cref="MyLinkedListNode{T}"/> После вставки newNode.</param>
        /// <param name="newNode">Новый <see cref="MyLinkedListNode{T}"/> Добавление <see cref="MyLinkedList{T}"/>.</param>
        /// <exception cref="ArgumentNullException">Свойство node имеет значение null. -или- Свойство newNode имеет значение null.</exception>
        /// <exception cref="InvalidOperationException">node не в текущем <see cref="MyLinkedList{T}"/>. -или- newNode принадлежит
        /// к другому <see cref="MyLinkedListNode{T}"/>.</exception>
        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Добавляет новый узел, содержащий указанное значение перед указанным узлом, существующие
        /// в <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="node"><see cref="MyLinkedListNode{T}"/> перед которым необходимо вставить
        /// новый <see cref="MyLinkedListNode{T}"/> содержащий value.</param>
        /// <param name="value">Значение для добавления <see cref="MyLinkedList{T}"/>.</param>
        /// <returns>Новый <see cref="MyLinkedListNode{T}"/> содержащий value.</returns>
        /// <exception cref="ArgumentNullException">Свойство node имеет значение null.</exception>
        /// <exception cref="InvalidOperationException">node не в текущем <see cref="MyLinkedList{T}"/>.</exception>
        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Добавляет указанный новый узел перед указанным узлом, существующие в <see cref="MyLinkedList{T}"/>.
        /// </summary>
        /// <param name="node"><see cref="MyLinkedListNode{T}"/> до вставки newNode.</param>
        /// <param name="newNode">Новый <see cref="MyLinkedListNode{T}"/> Добавление <see cref="MyLinkedList{T}"/>.</param>
        /// <exception cref="ArgumentNullException">Свойство node имеет значение null. -или- Свойство newNode имеет значение null.</exception>
        /// <exception cref="InvalidOperationException">node не в текущем <see cref="MyLinkedList{T}"/>. -или- newNode принадлежит
        /// к другому <see cref="MyLinkedList{T}"/>.</exception>
        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Добавляет новый узел, содержащий указанное значение в начале <see cref="MyLinkedList{T}"/>.
        /// </summary>
        /// <param name="value">Добавляемое значение в начале <see cref="MyLinkedList{T}"/>.</param>
        /// <returns>Новый <see cref="MyLinkedListNode{T}"/> содержащий value.</returns>
        public LinkedListNode<T> AddFirst(T value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Добавляет указанный новый узел в начале <see cref="MyLinkedList{T}"/>.
        /// </summary>
        /// <param name="node">Новый <see cref="MyLinkedListNode{T}"/> для добавления в начале <see cref="MyLinkedList{T}"/>.</param>
        /// <exception cref="ArgumentNullException">Свойство node имеет значение null.</exception>
        /// <exception cref="InvalidOperationException">node принадлежит к другому <see cref="MyLinkedList{T}"/>.</exception>
        public void AddFirst(LinkedListNode<T> node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Добавляет новый узел, содержащий указанное значение в конце <see cref="MyLinkedList{T}"/>.
        /// </summary>
        /// <param name="value">Значение, добавляемое в конце <see cref="MyLinkedList{T}"/>.</param>
        /// <returns>Новый <see cref="MyLinkedListNode{T}"/> содержащий value.</returns>
        public LinkedListNode<T> AddLast(T value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Добавляет указанный новый узел в конце <see cref="MyLinkedList{T}"/>.
        /// </summary>
        /// <param name="node">Новый <see cref="MyLinkedListNode{T}"/> для добавления в конце <see cref="MyLinkedList{T}"/>.</param>
        /// <exception cref="ArgumentNullException">Свойство node имеет значение null.</exception>
        /// <exception cref="InvalidOperationException">node принадлежит к другому <see cref="MyLinkedList{T}"/>.</exception>
        public void AddLast(LinkedListNode<T> node)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            lastModifed = DateTime.Now;
            Last = null;
            First = null;
            Count = 0;
        }

        public bool Contains(T item)
            => Find(item) != null;

        public MyLinkedListNode<T> Find(T item)
        {
            foreach (MyLinkedListNode<T> elm in (IEnumerable<MyLinkedListNode<T>>)this)
            {
                if (item.Equals(elm))
                    return elm;
            }
            return null;
        }

        public MyLinkedListNode<T> FindLast(T item)
        {
            IEnumerator<MyLinkedListNode<T>> enume = GetEnumeratorNode(true);
            while(enume.MoveNext())
            {
                if (item.Equals(enume.Current))
                    return enume.Current;
            }
            return null;
        }

        public void CopyTo(T[] array, int arrayIndex)
            => CopyTo(array, arrayIndex);

        public void CopyTo(Array array, int index)
        {
            foreach (T elm in this)
            {
                array.SetValue(elm, index++);
            }
        }

        public IEnumerator<T> GetEnumerator()
            => new Enumerator(this);

        public bool Remove(T item)
        {
            lastModifed = DateTime.Now;
            MyLinkedListNode<T> ToRemove = Find(item);
            if (ToRemove == null)
                return false;
            if(ToRemove.Previous != null)
                ToRemove.Previous.Next = ToRemove.Next;
            if(ToRemove.Next != null)
                ToRemove.Next.Previous = ToRemove.Previous;
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        IEnumerator<MyLinkedListNode<T>> GetEnumeratorNode(bool IsReverse = false)
            => new Enumerator(this, IsReverse);

        IEnumerator<MyLinkedListNode<T>> IEnumerable<MyLinkedListNode<T>>.GetEnumerator()
            => GetEnumeratorNode();

        public struct Enumerator : IEnumerator<T>, IEnumerator, IEnumerator<MyLinkedListNode<T>>
        {
            public Enumerator(MyLinkedList<T> baseList, bool isReverse = false)
            {
                Base = baseList;
                lastModifed = baseList.lastModifed;
                IsMoved = false;
                CurrentNode = null;
                IsReverse = isReverse;
            }

            /// <summary>
            /// Основной лист.
            /// </summary>
            private MyLinkedList<T> Base;

            /// <summary>
            /// Текущий узел.
            /// </summary>
            private MyLinkedListNode<T> CurrentNode { get; set; }

            /// <summary>
            /// True, если двигался. False, если нет.
            /// </summary>
            private bool IsMoved;

            /// <summary>
            /// Последнее изменение.
            /// </summary>
            private readonly DateTime lastModifed;

            public bool IsReverse { get; }

            public T Current => CurrentNode.Value;

            object IEnumerator.Current => Current;

            MyLinkedListNode<T> IEnumerator<MyLinkedListNode<T>>.Current => CurrentNode;

            public void Dispose()
            {
                Base = null;
                CurrentNode = null;
            }

            public bool MoveNext()
            {
                if (Base.lastModifed != lastModifed)
                    throw new InvalidOperationException("Коллекция была изменена после создания перечислителя.");
                if (CurrentNode == null)
                {
                    if (IsMoved)
                        return false;
                    IsMoved = true;
                    if (IsReverse)
                        CurrentNode = Base.Last;
                    else
                        CurrentNode = Base.First;
                    return CurrentNode != null;
                }
                if (IsReverse)
                    CurrentNode = CurrentNode.Previous;
                else
                    CurrentNode = CurrentNode.Next;
                return CurrentNode != null;
            }

            public void Reset()
            {
                IsMoved = false;
                CurrentNode = null;
            }
        }
    }
}
