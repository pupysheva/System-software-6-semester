﻿using Lexer;
using System;
using System.Collections;
using System.Collections.Generic;
using static Parser.RuleOperator;
using static Parser.ReportParser;

namespace Parser
{
    /// <summary>
    /// Хранит в себе одно правило грамматики.
    /// </summary>
    public class Nonterminal : IList, IList<object>
    {
        /// <summary>
        /// Лист содержит операторы нетерминалов или терминалы, образуя единый терминал.
        /// Тут должны быть типы:
        /// 1. <see cref="Terminal"/>;
        /// 2. <see cref="Nonterminal"/>.
        /// </summary>
        private readonly List<object> list = new List<object>();

        /// <summary>
        /// Указывает, какая реакция должна быть на истинность всех терминалов и нетерминалов.
        /// </summary>
        private readonly RuleOperator rule;

        /// <summary>
        /// Создание экземпляра нетерминала.
        /// </summary>
        /// <param name="rule">Указывает, какая реакция должна быть на истинность всех терминалов и нетерминалов.</param>
        /// <param name="terminalsOrNonterminals">Список терминалов и нетерминалов.</param>
        public Nonterminal(RuleOperator rule, params object[] terminalsOrNonterminals)
        {
            this.rule = rule;
            AddRange(terminalsOrNonterminals ?? throw new ArgumentNullException("Невероятная ошибка понимания ситаксиса C# достигнута."));
        }

        /// <summary>
        /// Это конструктор полностью повторяет аргументы <see cref="Nonterminal(RuleOperator, params object[])"/>.
        /// Аргумент RuleOperator rule включён в параметр OperatorAndterminalsOrNonterminals.
        /// </summary>
        public Nonterminal(params object[] OperatorAndterminalsOrNonterminals)
        {
            IEnumerator enu = OperatorAndterminalsOrNonterminals.GetEnumerator();
            enu.MoveNext();
            rule = (RuleOperator)enu.Current;
            while (enu.MoveNext())
                if (enu.Current is RuleOperator)
                {
                    List<object> toAdd = new List<object>
                    { enu.Current };
                    while (enu.MoveNext())
                        toAdd.Add(enu.Current);
                    Add(new Nonterminal(toAdd));
                    if (enu.MoveNext())
                        throw new Exception("Ожидался конец. А его нет. " + enu + ", " + OperatorAndterminalsOrNonterminals);
                }
                else
                    Add(enu.Current);
        }

        /// <summary>
        /// Проверяет, чтобы заданные токены соответсвовали нетерминалу.
        /// </summary>
        /// <param name="tokens">Список токенов, которые надо проверить.</param>
        /// <returns>True, если последовательность токенов подходит нетерминалу. Иначе - false.</returns>
        public ReportParser CheckRule(List<Token> tokens)
        {
            if (tokens == null)
                throw new ArgumentNullException("Список токенов должен был инициализирован.");
            switch(rule)
            {
                case AND:
                    return RuleAND(tokens);
                case ONE_AND_MORE:
                    return RuleONE_AND_MORE(tokens);
                case OR:
                    return RuleOR(tokens);
                case ZERO_AND_MORE:
                    return RuleZERO_AND_MORE(tokens);
                default:
                    throw new NotImplementedException($"Оператор {Enum.GetName(typeof(RuleOperator), rule)} не реализован.");
            }
        }

        private ReportParser RuleZERO_AND_MORE(List<Token> tokens)
        {
            throw new NotImplementedException();
        }

        private ReportParser RuleONE_AND_MORE(List<Token> tokens)
        {
            throw new NotImplementedException();
        }

        private ReportParser RuleAND(List<Token> tokens)
        {
            throw new NotImplementedException();
        }

        private ReportParser RuleOR(List<Token> tokens)
        {
            ReportParser output = new ReportParser();
            foreach(object o in this)
            {
                if (o is Terminal)
                {
                    if (tokens.Count != 1)
                        output.Add(new ParserException("Ожидался один терминал", 1, tokens.Count));
                    if (!tokens[0].Type.Equals(o))
                        output.Add(new ParserException(o, tokens[0]));
                    else
                        return SUCCESS;
                }
                else if (o is Nonterminal)
                {
                    ReportParser buffer = ((Nonterminal)o).CheckRule(tokens);
                    if (buffer.IsSuccess)
                        return buffer; // Да, этот нетерминал нам подходит.
                    else
                        output.AddRange(buffer);
                }
                else
                    throw new Exception($"Unexpected type {o.GetType()} of {o} in list");
            }
            if (output.Count == 0)
                output.Add(new ParserException("Для оператора OR не найдено ни одного истинного выражения.", list, tokens));
            return output;
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
            return value is string ? new Terminal((string)value)
                : value is object[] ? new Nonterminal((object[])value)
                : value;
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
         => value != null && 
            (value is Terminal
            || value is string
            || value is RuleOperator
            || value is Nonterminal
            || (value is object[] && IsCanAddRange((object[])value))
            );

        /// <summary>
        /// Вызывает <see cref="IsCanAdd(object)"/> для каждого элемента
        /// множества array.
        /// </summary>
        /// <returns>True, если все соответсуют <see cref="IsCanAdd(object)"/>. Иначе - false.</returns>
        public bool IsCanAddRange(IEnumerable array)
        {
            foreach (object o in array)
                if (!IsCanAdd(o))
                    return false;
            return true;
        }

        /// <summary>
        /// Добавляет множество операторов или терминалов в правило.
        /// </summary>
        /// <param name="operatorsWithTerminals">Перечень объектов,
        /// соответсвующих <see cref="IsCanAdd(object)"/>.</param>
        public void AddRange(IEnumerable<object> operatorsWithTerminals)
        { // Для безопасности необходимо добавлять каждый элемент последовательно.
            foreach (object opOrTer in operatorsWithTerminals)
                this.Add(opOrTer);
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
