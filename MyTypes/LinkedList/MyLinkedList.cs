using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTypes.LinkedList
{
    public class MyLinkedList<T> : ICollection<T>, ICollection
    {
        /// <summary>
        /// Возвращает последний узел <see cref="MyLinkedListNode{T}"/> из <see cref="MyLinkedList{T}"/>.
        /// </summary>
        public LinkedListNode<T> Last { get; private set; }

        /// <summary>
        /// Возвращает первый <see cref="MyLinkedListNode{T}"/> из <see cref="MyLinkedList{T}"/>.
        /// </summary>
        public LinkedListNode<T> First { get; private set; }

        /// <summary>
        /// Возвращает количество узлов, содержащихся в <see cref="MyLinkedList{T}"/>.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Последнее изменение этого листа.
        /// </summary>
        private DateTime lastModifed;

        public bool IsReadOnly => false;

        /// <summary>
        /// Получает объект, с помощью которого можно синхронизировать доступ к коллекции <see cref="MyLinkedList{T}"/>.
        /// </summary>
        public virtual object SyncRoot => this;

        /// <summary>
        /// Возвращает значение, показывающее, является ли доступ к коллекции <see cref="ICollection"/>
        /// синхронизированным (потокобезопасным).
        /// </summary>
        /// <returns>true, если доступ к классу <see cref="ICollection"/> является синхронизированным
        /// (потокобезопасным); в противном случае — false.</returns>
        public virtual bool IsSynchronized => false;

        void ICollection<T>.Add(T item)
        {
            lastModifed = DateTime.Now;
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
        {
            foreach(T elm in this)
            {
                if (item.Equals(elm))
                    return true;
            }
            return false;
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
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            lastModifed = DateTime.Now;
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public class Enumerator : IEnumerator<T>, IEnumerator
        {
            public Enumerator(MyLinkedList<T> baseList)
            {
                Base = baseList;
            }

            /// <summary>
            /// Основной лист.
            /// </summary>
            public MyLinkedList<T> Base;

            /// <summary>
            /// Последнее изменение.
            /// </summary>
            private readonly DateTime lastModifed;

            public T Current => throw new NotImplementedException();

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
