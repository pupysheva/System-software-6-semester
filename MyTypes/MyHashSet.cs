using MyTypes.LinkedList;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyTypes
{
    /// <summary>
    /// Представляет набор значений.
    /// Класс не является потокобезопасным.
    /// </summary>
    /// <typeparam name="T">Тип элементов в коллекции.</typeparam>
    public class MyHashSet<T> : ISet<T>, ICollection
    {
        private ICollection<T>[] cards;

        private DateTime lastModifed;

        public MyHashSet(int countCards = 15)
        {
            lastModifed = DateTime.Now;
            cards = new ICollection<T>[countCards];
            for (int i = 0; i < cards.Length; i++)
                cards[i] = new MyLinkedList<T>();
        }
        

        /// <summary>
        /// Возвращает число элементов, содержащихся в наборе.
        /// </summary>
        /// <returns>Число элементов, содержащихся в наборе.</returns>
        public int Count
        {
            get
            {
                int count = 0;
                foreach (ICollection<T> list in cards)
                    count += list.Count;
                return count;
            }
        }

        /// <summary>
        /// Возвращает, является ли коллекция только для чтения. Возвращается false.
        /// </summary>
        bool ICollection<T>.IsReadOnly => false;

        object ICollection.SyncRoot => cards;

        bool ICollection.IsSynchronized => cards.IsSynchronized;

        /// <summary>
        /// Добавляет указанный элемент в набор.
        /// </summary>
        /// <param name="item">Элемент, добавляемый в набор.</param>
        void ICollection<T>.Add(T item)
            => Add(item);

        /// <summary>
        /// Добавляет указанный элемент в набор.
        /// </summary>
        /// <param name="item">Элемент, добавляемый в набор.</param>
        /// <returns>Значение true, если элемент добавлен в объект <see cref="MyHashSet{T}"/>,
        /// значение false, если элемент уже присутствует в нем.</returns> 
        public bool Add(T item)
        {
            if (item == null)
                return false;
            if (Contains(item))
                return false;
            lastModifed = DateTime.Now;
            uint pos = (uint)item.GetHashCode() % (uint)cards.LongLength;
            cards[pos].Add(item);
            return true;
        }

        /// <summary>
        /// Удаляет все элементы из объекта <see cref="MyHashSet{T}"/>.
        /// </summary>
        public void Clear()
        {
            lastModifed = DateTime.Now;
            foreach (ICollection<T> list in cards)
            {
                list.Clear();
            }
        }

        /// <summary>
        /// Определяет, содержит ли объект <see cref="MyHashSet{T}"/> указанный элемент.
        /// </summary>
        /// <param name="item">Элемент, который нужно найти в объекте <see cref="MyHashSet{T}"/>.</param>
        /// <returns>Значение true, если объект <see cref="MyHashSet{T}"/> содержит указанный
        /// элемент; в противном случае — значение false.</returns>
        public bool Contains(T item)
        {
            if (item == null)
                return false;
            uint pos = (uint)item.GetHashCode() % (uint)cards.LongLength;
            return cards[pos].Contains(item);
        }

        /// <summary>
        /// Копирует элементы объекта <see cref="MyHashSet{T}"/> в массив, начиная
        /// с указанного индекса массива.
        /// </summary>
        /// <param name="array">Одномерный массив, являющийся назначением элементов, копируемых из объекта <see cref="MyHashSet{T}"/>.
        /// Индекс в массиве должен начинаться с нуля.</param>
        /// <param name="arrayIndex">Отсчитываемый от нуля индекс в массиве array, указывающий начало копирования.</param>
        /// <exception cref="ArgumentNullException">Свойство array имеет значение null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Значение параметра arrayIndex меньше 0.</exception>
        /// <exception cref="ArgumentException">arrayIndex больше, чем длина назначения array.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(array, arrayIndex, Count);
        }

        /// <summary>
        /// Копирует элементы объекта <see cref="MyHashSet{T}"/> в массив, начиная
        /// с указанного индекса массива.
        /// </summary>
        /// <param name="array">Одномерный массив, являющийся назначением элементов, копируемых из объекта <see cref="MyHashSet{T}"/>.
        /// Индекс в массиве должен начинаться с нуля.</param>
        /// <param name="arrayIndex">Отсчитываемый от нуля индекс в массиве array, указывающий начало копирования.</param>
        /// <param name="count">Число элементов, копируемых в массив array.</param>
        /// <exception cref="ArgumentNullException">Свойство array имеет значение null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Значение параметра arrayIndex меньше 0.</exception>
        /// <exception cref="ArgumentException">arrayIndex + count больше, чем длина назначения array.</exception>
        public void CopyTo(T[] array, int arrayIndex, int count)
        {
            if (array == null)
                throw new ArgumentNullException("Свойство array имеет значение null.");
            if (arrayIndex < 0 || count < 0 || arrayIndex + count > array.Length)
                if (arrayIndex < 0)
                    throw new ArgumentOutOfRangeException("Значение параметра arrayIndex меньше 0.");
                else
                    throw new ArgumentException("arrayIndex + count больше, чем длина назначения array.");
            foreach (T elm in this)
            {
                array[arrayIndex++] = elm;
            }
        }

        /// <summary>
        /// Копирует элементы объекта <see cref="MyHashSet{T}"/> в массив.
        /// </summary>
        /// <param name="array">Одномерный массив, являющийся назначением элементов, копируемых из объекта <see cref="MyHashSet{T}"/>. Индекс в массиве должен начинаться с нуля.</param>
        public void CopyTo(T[] array)
            => CopyTo(array, 0, Count);

        /// <summary>
        /// Удаляет все элементы в указанной коллекции из текущего объекта <see cref="MyHashSet{T}"/>.
        /// </summary>
        /// <param name="other">Коллекция элементов, удаляемая из объекта <see cref="MyHashSet{T}"/>.</param>
        /// <exception cref="ArgumentNullException">Свойство other имеет значение null.</exception>
        public void ExceptWith(IEnumerable<T> other)
        {
            foreach(T oth in other)
            {
                Remove(oth);
            }
        }

        /// <summary>
        /// Возвращает перечислитель, выполняющий итерацию элементов объекта <see cref="MyHashSet{T}"/>.
        /// </summary>
        /// <returns>Объект <see cref="MyHashSet{T}.Enumerator"/> для объекта <see cref="MyHashSet{T}"/>.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Изменяет текущий объект <see cref="MyHashSet{T}"/> так, чтобы он содержал
        /// все элементы, имеющиеся в нём или в указанной коллекции либо как в нём, так и
        /// в указанной коллекции.
        /// </summary>
        /// <param name="other">Коллекция для сравнения с текущим объектом <see cref="MyHashSet{T}"/>.</param>
        /// <exception cref="ArgumentNullException">Свойство other имеет значение null.</exception>
        public void UnionWith(IEnumerable<T> other)
        {
            lastModifed = DateTime.Now;
            foreach(T oth in other)
            {
                Add(oth);
            }
        }

        /// <summary>
        /// Изменяет текущий объект <see cref="MyHashSet{T}"/> так, чтобы он содержал
        /// только элементы, которые имеются в этом объекте и в указанной коллекции.
        /// </summary>
        /// <param name="other">Коллекция для сравнения с текущим объектом <see cref="MyHashSet{T}"/>.</param>
        /// <exception cref="ArgumentNullException">Свойство other имеет значение null.</exception>
        public void IntersectWith(IEnumerable<T> other)
        {
            lastModifed = DateTime.Now;
            MyHashSet<T> toAdd = new MyHashSet<T>(cards.Length);
            foreach(T oth in other)
            {
                if (Contains(oth))
                    toAdd.Add(oth);
            }
            cards = toAdd.cards;
        }

        /// <summary>
        /// Изменяет текущий объект <see cref="MyHashSet{T}"/> так, чтобы он содержал
        /// только элементы, которые имеются либо в этом объекте, либо в указанной коллекции,
        /// но не одновременно в них обоих.
        /// </summary>
        /// <param name="other">Коллекция для сравнения с текущим объектом <see cref="MyHashSet{T}"/>.</param>
        /// <exception cref="ArgumentNullException">Свойство other имеет значение null.</exception>
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            lastModifed = DateTime.Now;
            foreach(T oth in other)
            {
                if (!Remove(oth))
                    Add(oth);
            }
        }

        /// <summary>
        /// Определяет, является ли объект <see cref="MyHashSet{T}"/> подмножеством
        /// указанной коллекции.
        /// </summary>
        /// <param name="other">Коллекция для сравнения с текущим объектом <see cref="MyHashSet{T}"/>.</param>
        /// <returns>Значение true, если объект <see cref="MyHashSet{T}"/> является подмножеством
        /// other; в противном случае — значение false.</returns>
        /// <exception cref="ArgumentNullException">Свойство other имеет значение null.</exception>
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            int count = 0;
            foreach(T oth in other)
            {
                if (Contains(oth))
                    count++;
            }
            return count == Count;
        }

        /// <summary>
        /// Определяет, является ли объект <see cref="MyHashSet{T}"/> супермножеством
        /// указанной коллекции.
        /// </summary>
        /// <param name="other">Коллекция для сравнения с текущим объектом <see cref="MyHashSet{T}"/>.</param>
        /// <returns>Значение true, если объект <see cref="MyHashSet{T}"/> является супермножеством
        /// other; в противном случае — значение false.</returns>
        /// <exception cref="ArgumentNullException">Свойство other имеет значение null.</exception>
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException("Я не знаю, что такое супермножество");
        }

        /// <summary>
        /// Определяет, является ли объект <see cref="MyHashSet{T}"/> строгим супермножеством
        /// указанной коллекции.
        /// </summary>
        /// <param name="other">Коллекция для сравнения с текущим объектом <see cref="MyHashSet{T}"/>.</param>
        /// <returns>Значение true, если объект <see cref="MyHashSet{T}"/> является строгим
        /// супермножеством other; в противном случае — значение false.</returns>
        /// <exception cref="ArgumentNullException">Свойство other имеет значение null.</exception>
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException("Я не знаю, что такое супермножество.");
        }

        /// <summary>
        /// Определяет, является ли объект <see cref="MyHashSet{T}"/> строгим подмножеством
        /// указанной коллекции.
        /// </summary>
        /// <param name="other">Коллекция для сравнения с текущим объектом <see cref="MyHashSet{T}"/>.</param>
        /// <returns>Значение true, если объект <see cref="MyHashSet{T}"/> является строгим
        /// подмножеством объекта other; в противном случае — значение false.</returns>
        /// <exception cref="ArgumentNullException">Свойство other имеет значение null.</exception>
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException("Я не знаю, что такое строгое подмножество.");
        }

        /// <summary>
        /// Определяет, имеются ли общие элементы в текущем объекте <see cref="MyHashSet{T}"/>
        /// и в указанной коллекции.
        /// </summary>
        /// <param name="other">Коллекция для сравнения с текущим объектом <see cref="MyHashSet{T}"/>.</param>
        /// <returns>Значение true, если объект <see cref="MyHashSet{T}"/> и коллекция other
        /// имеют по крайней мере один общий элемент; в противном случае — значение false.</returns>
        /// <exception cref="ArgumentNullException">Свойство other имеет значение null.</exception>
        public bool Overlaps(IEnumerable<T> other)
        {
            foreach(T oth in other)
            {
                if (Contains(oth))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Определяет, содержат ли объект <see cref="MyHashSet{T}"/> и указанная
        /// коллекция одни и те же элементы.
        /// </summary>
        /// <param name="other">Коллекция для сравнения с текущим объектом <see cref="MyHashSet{T}"/>.</param>
        /// <returns>Значение true, если объект <see cref="MyHashSet{T}"/> равен other;
        /// в противном случае — значение false.</returns>
        /// <exception cref="ArgumentNullException">Свойство other имеет значение null.</exception>
        public bool SetEquals(IEnumerable<T> other)
        {
            foreach(T oth in other)
            {
                if (!Contains(oth))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Удаляет указанный элемент из объекта <see cref="MyHashSet{T}"/>.
        /// </summary>
        /// <param name="item">Подлежащий удалению элемент.</param>
        /// <returns>Значение true, если элемент был найден и удален; в противном случае — значение
        /// false. Этот метод возвращает значение false, если элемент item не удалось найти
        /// в объекте <see cref="MyHashSet{T}"/>.</returns>
        public bool Remove(T item)
        {
            if (item == null)
                return false;
            uint pos = (uint)item.GetHashCode() % (uint)cards.LongLength;
            return cards[pos].Remove(item);
        }

        /// <summary>
        /// Возвращает перечислитель, который осуществляет итерацию по коллекции.
        /// </summary>
        /// <returns>Объект <see cref="IEnumerator"/>, который используется для прохода по коллекции.</returns>
        IEnumerator IEnumerable.GetEnumerator()
            => new Enumerator(this);

        void ICollection.CopyTo(Array array, int index)
        {
            foreach(T elm in this)
            {
                array.SetValue(elm, index++);
            }
        }

        /// <summary>
        /// Показывает разницу в балансировке корзин.
        /// Чем ближе к 0, тем лучше балансировка.
        /// Чем ближе к 1, тем хуже балансировка.
        /// </summary>
        public double Balancing
        {
            get
            {
                int countHashSet = Count;
                if (countHashSet == 0)
                    return 0;
                double mustCount = countHashSet / (double)cards.Length;
                double difference = 0;
                foreach(ICollection<T> card in cards)
                {
                    difference += Math.Abs(mustCount - card.Count);
                }
                return difference / (mustCount * cards.Length);
            }
        }

        /// <summary>
        /// Перечисляет элементы объекта <see cref="MyHashSet{T}"/>.
        /// </summary>
        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private MyHashSet<T> myHashSet;
            private readonly DateTime myLastModifed;
            private IEnumerator enumeratorCards;
            private IEnumerator<T> enumeratorList;

            public Enumerator(MyHashSet<T> myHashSet) : this()
            {
                this.myHashSet = myHashSet;
                this.myLastModifed = myHashSet.lastModifed;
            }

            /// <summary>
            /// Возвращает элемент, расположенный в текущей позиции перечислителя.
            /// </summary>
            /// <returns>Элемент в <see cref="MyHashSet{T}"/> коллекции, соответствующий текущей
            //  позиции перечислителя.</returns>
            public T Current => enumeratorList.Current;

            object IEnumerator.Current => Current;

            /// <summary>
            /// Освобождает все ресурсы, используемые <see cref="Enumerator"/>
            /// объекта.
            /// </summary>
            public void Dispose()
            {
                myHashSet = null;
                enumeratorCards = null;
                enumeratorList = null;
            }

            /// <summary>
            /// Перемещает перечислитель к следующему элементу <see cref="MyHashSet{T}"/>
            /// коллекции.
            /// </summary>
            /// <returns>Значение true, если перечислитель был успешно перемещен к следующему элементу;
            /// значение false, если перечислитель достиг конца коллекции.</returns>
            /// <exception cref="InvalidOperationException">Коллекция была изменена после создания перечислителя.</exception>
            public bool MoveNext()
            {
                if (myHashSet.lastModifed != myLastModifed)
                    throw new InvalidOperationException("Коллекция была изменена после создания перечислителя.");
                if(enumeratorCards == null)
                {
                    enumeratorCards = myHashSet.cards.GetEnumerator();
                    if (!enumeratorCards.MoveNext())
                        return false;
                    enumeratorList = ((IEnumerable<T>)enumeratorCards.Current).GetEnumerator();
                    return MoveNext();
                }
                if (enumeratorList.MoveNext())
                    return true;
                if (!enumeratorCards.MoveNext())
                    return false;
                enumeratorList = ((IEnumerable<T>)enumeratorCards.Current).GetEnumerator();
                return MoveNext();
            }

            /// <summary>
            /// Устанавливает перечислитель в его начальное положение, т. е. перед первым элементом
            /// коллекции.
            /// </summary>
            /// <exception cref="InvalidOperationException">Коллекция была изменена после создания перечислителя.</exception>
            public void Reset()
            {
                enumeratorCards = null;
                enumeratorList = null;
            }
        }
    }
}
