using Lexer;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Описывает одно правило парсера.
/// </summary>
namespace Parser
{
    /// <summary>
    /// Хранит в себе одно правило грамматики.
    /// </summary>
    class Nonterminal : IList, IList<object>
    {
        /// <summary>
        /// Лист содержит операторы нетерминалов или терминалы, образуя единый терминал.
        /// </summary>
        private readonly List<object> list = new List<object>();

        public Nonterminal(params object[] operatorsWithTerminals)
            => AddRange(operatorsWithTerminals ?? throw new ArgumentNullException("Невероятная ошибка понимания ситаксиса C# достигнута."));

        public bool CheckRule(List<Token> tokens)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Проверяет, что элемент подходит для данного списка.
        /// Подходят объекты только Token и RuleOperator.
        /// </summary>
        /// <param name="value">Объект на проверку.</param>
        private void CheckSet(object value)
        {
            if (!IsCanAdd(value))
                throw new ArgumentException(
                    "Ожидался токен или оператор. Фактически: " + value.ToString());
        }

        /// <summary>
        /// Определяет, может ли элемент быть добавлен в данное правило.
        /// </summary>
        /// <param name="value">Объект, рассматреть который необходимо.</param>
        /// <returns>True, если это <see cref="RuleOperator"/> или <see cref="Terminal">.
        /// В противном случае, False. Если value = null, то возвращает False.</returns>
        public bool IsCanAdd(object value)
         => value != null && (value is Terminal || value is RuleOperator);

        /// <summary>
        /// Добавляет множество операторов или терминалов в правило.
        /// </summary>
        /// <param name="operatorsWithTerminals">Перечень объектов,
        /// соответсвующих <see cref="IsCanAdd(object)"/>.</param>
        public void AddRange(IEnumerable<object> operatorsWithTerminals)
        {
            foreach (object opOrTer in operatorsWithTerminals)
                CheckSet(opOrTer);
            list.AddRange(operatorsWithTerminals);
        }

        #region IList

        public object this[int index]
        {
            get => list[index];
            set
            {
                CheckSet(value);
                list[index] = value;
            }
        }

        public int Count => list.Count;

        public bool IsReadOnly => ((IList)list).IsReadOnly;

        public bool IsFixedSize => ((IList)list).IsFixedSize;

        public object SyncRoot => ((IList)list).SyncRoot;

        public bool IsSynchronized => ((IList)list).IsSynchronized;

        /// <summary>
        /// Дополнить правило нетерминала.
        /// </summary>
        /// <param name="item">Объект, соответсвующий <see cref="IsCanAdd(object)"/>.</param>
        public void Add(object item)
        {
            CheckSet(item);
            list.Add(item);
        }

        public void Clear() => list.Clear();

        public bool Contains(object item) => list.Contains(item);

        public void CopyTo(object[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

        public IEnumerator<object> GetEnumerator() => list.GetEnumerator();

        public int IndexOf(object item)
        {
            if (IsCanAdd(item))
                // Гарантируется, что все объекты в списке соответсвуют критерию IsCanAdd(object)
                return list.IndexOf(item);
            else
                return -1;
        }

        public void Insert(int index, object item)
        {
            CheckSet(item);
            list.Insert(index, item);
        }

        public bool Remove(object item)
            => list.Remove(item);

        public void RemoveAt(int index)
            => list.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator()
            => list.GetEnumerator();

        int IList.Add(object value)
        {
            CheckSet(value);
            return ((IList)list).Add(value);
        }

        void IList.Remove(object value)
            => ((IList)list).Remove(value);

        public void CopyTo(Array array, int index)
            => ((IList)list).CopyTo(array, index);
        #endregion
    }
}
