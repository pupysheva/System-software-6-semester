using Lexer;
using System;
using System.Collections;
using System.Collections.Generic;

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

        /// <summary>
        /// Проверяет, чтобы заданные токены соответсвовали нетерминалу.
        /// </summary>
        /// <param name="tokens">Список токенов, которые надо проверить.</param>
        /// <returns>True, если последовательность токенов подходит нетерминалу. Иначе - false.</returns>
        public bool CheckRule(List<Token> tokens)
        {
            int i = 0;
            List<object> lang = GetReversePolishNotationList();
            Stack<object> mem = new Stack<object>();
            foreach(object l in lang)
            {
                if(l is Terminal)
                {
                    if (((Terminal)l).Name.Equals(tokens[i].Type.Name))
                        mem.Push(l);
                    else
                        mem.Push(new ParserException(l, tokens[i].Type));
                    i++;
                }
            }
        }

        /// <summary>
        /// Проверяет, что элемент подходит для данного списка.
        /// Подходят объекты только Token и RuleOperator.
        /// </summary>
        /// <param name="value">Объект на проверку.</param>
        /// <returns>Обработанный объект.
        /// Например, преобразование <see cref="string"/> в <see cref="Terminal"/>.</returns>
        private object CheckSet(object value)
        {
            if (!IsCanAdd(value))
                throw new ArgumentException(
                    "Ожидался токен или оператор. Фактически: " + value.ToString());
            return value is string ? new Terminal((string)value) : value;
        }

        /// <summary>
        /// Определяет, может ли элемент быть добавлен в данное правило.
        /// </summary>
        /// <param name="value">Объект, рассматреть который необходимо.</param>
        /// <returns>True, если это одно из:
        /// 1. <see cref="RuleOperator"/>;
        /// 2. <see cref="Terminal"/>;
        /// 3. <see cref="Nonterminal"/>;
        /// 4. Представление названия <see cref="Terminal"/> в виде <see cref="string"/>.
        /// В противном случае, False. Если value = null, то возвращает False.</returns>
        public bool IsCanAdd(object value)
         => value != null && (value is Terminal || value is string || value is RuleOperator || value is Nonterminal);

        /// <summary>
        /// Добавляет множество операторов или терминалов в правило.
        /// </summary>
        /// <param name="operatorsWithTerminals">Перечень объектов,
        /// соответсвующих <see cref="IsCanAdd(object)"/>.</param>
        public void AddRange(IEnumerable<object> operatorsWithTerminals)
        {
            foreach (object opOrTer in operatorsWithTerminals)
                this.Add(opOrTer);
        }

        /// <summary>
        /// Возвращает лист в обратной польской последовательности текущего нетерминала.
        /// </summary>
        /// <returns>Лист в обратной польской последовательности.</returns>
        private List<object> GetReversePolishNotationList()
        {
            Stack<object> stack = new Stack<object>(list.Count);
            List<object> output = new List<object>(list.Count);

        }

        #region IList

        public object this[int index]
        {
            get => list[index];
            set
            {
                list[index] = CheckSet(value);
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
            list.Add(CheckSet(item));
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
            => list.Insert(index, CheckSet(item));

        public bool Remove(object item)
            => list.Remove(item);

        public void RemoveAt(int index)
            => list.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator()
            => list.GetEnumerator();

        int IList.Add(object value)
            => ((IList)list).Add(CheckSet(value));

        void IList.Remove(object value)
            => ((IList)list).Remove(value);

        public void CopyTo(Array array, int index)
            => ((IList)list).CopyTo(array, index);
        #endregion
    }
}
