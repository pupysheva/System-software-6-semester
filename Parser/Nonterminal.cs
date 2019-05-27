﻿using Lexer;
using System;
using System.Collections;
using System.Collections.Generic;
using static Parser.RuleOperator;
using System.Text;

namespace Parser
{

    /// <summary>
    /// Делегат хранит функцию, которая вставляет
    /// значение терминала или вызывает
    /// формирование стэк-кода у нетерминала.
    /// </summary>
    /// <param name="IndexToAdd">-1 используется для OR, так
    /// как он там единственный. Для остальных - порядковый номер
    /// начиная с 0 нужного нетерминала или терминала.</param>
    /// <returns>Возвращает true, если операция прошла успешна.
    /// Иначе - false.</returns>
    public delegate bool ActionInsert(int IndexToAdd = -1);

    /// <summary>
    /// Делегат, который представляет собой функцию, которая
    /// служит для преобразования токенов в стек-код.
    /// </summary>
    /// <param name="commands">Сюда вставляются команды
    /// для стековой машины.</param>
    /// <param name="insert">Функция вызывает вставку либо значение токена,
    /// либо вызывает функцию вставки кода входящего нетерминала.</param>
    /// <param name="id">Используется для OR. Символизирует номер,
    /// начиная с 0, какой терминал или нетерминал был выбран.</param>
    public delegate void TransferToStackCode(List<string> commands, ActionInsert insert, int id = 0);

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
        /// Имя нетерминала.
        /// </summary>
        public string Name { get; } = null;

        /// <summary>
        /// Функция преобразвания токенов в стек-код.
        /// </summary>
        public readonly TransferToStackCode TransferToStackCode = null;

        /// <summary>
        /// Создание экземпляра нетерминала.
        /// </summary>
        /// <param name="Name">Устанавливает имя терминала.</param>
        /// <param name="rule">Указывает, какая реакция должна быть на истинность всех терминалов и нетерминалов.</param>
        /// <param name="terminalsOrNonterminals">Список терминалов и нетерминалов.</param>
        public Nonterminal(string Name, TransferToStackCode TransferToStackCode, RuleOperator rule, params object[] terminalsOrNonterminals)
            : this(Name, rule, terminalsOrNonterminals)
        {
            this.TransferToStackCode = TransferToStackCode;
        }

        /// <summary>
        /// Создание экземпляра нетерминала.
        /// </summary>
        /// <param name="Name">Устанавливает имя терминала.</param>
        /// <param name="rule">Указывает, какая реакция должна быть на истинность всех терминалов и нетерминалов.</param>
        /// <param name="terminalsOrNonterminals">Список терминалов и нетерминалов.</param>
        public Nonterminal(string Name, RuleOperator rule, params object[] terminalsOrNonterminals)
            : this(rule, terminalsOrNonterminals)
        {
            this.Name = Name;
        }

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
        /// Проверяет, чтобы заданные токены соответсвовали нетерминалу.
        /// </summary>
        /// <param name="tokens">Список токенов, которые надо проверить.</param>
        /// <returns>True, если последовательность токенов подходит нетерминалу. Иначе - false.</returns>
        public ReportParser CheckRule(List<Token> tokens)
        {
            int a = 0, b = tokens.Count - 1;
            return CheckRule(tokens, ref a, ref b);
        }

        /// <summary>
        /// Проверяет, чтобы заданные токены соответсвовали нетерминалу.
        /// </summary>
        /// <param name="tokens">Список токенов, которые надо проверить.</param>
        /// <param name="begin">Первый доступный индекс в листе tokens.</param>
        /// <param name="end">Последний доступный индекс в листе tokens.</param>
        /// <returns>True, если последовательность токенов подходит нетерминалу. Иначе - false.</returns>
        public ReportParser CheckRule(List<Token> tokens, ref int begin, ref int end)
        {
            if (tokens == null)
                throw new ArgumentNullException("Список токенов должен был инициализирован.");
            ReportParser output = new ReportParser();
            output.Info.AddInfo("Зашёл в нетерминал: " + ToString());
            switch (rule)
            {
                case AND:
                    output.Merge(RuleAND(tokens, ref begin, ref end));
                    break;
                case ONE_AND_MORE:
                    output.Merge(RuleONE_AND_MORE(tokens, ref begin, ref end));
                    break;
                case OR:
                    output.Merge(RuleOR(tokens, ref begin, ref end));
                    break;
                case ZERO_AND_MORE:
                    output.Merge(RuleZERO_AND_MORE(tokens, ref begin, ref end));
                    break;
                default:
                    throw new NotImplementedException($"Оператор {Enum.GetName(typeof(RuleOperator), rule)} не реализован.");
            }
            output.Info.AddInfo("Cостояние отчёта: " + output.IsSuccess + ", выхожу из нетерминала: " + ToString());
            return output;
        }

        private ReportParser RuleZERO_AND_MORE(List<Token> tokens, ref int begin, ref int end)
        {
            ReportParserCompile compile = new ReportParserCompile(this, ZERO_AND_MORE, -1);
            ReportParser output = new ReportParser(compile);
            ReportParser buffer;
            do
            {
                buffer = RuleAND(tokens, ref begin, ref end);
                output.Merge(buffer, ++compile.Helper);
            }
            while (buffer.IsSuccess) ;
            output.Info.Success("Нетерминалы ZERO_AND_MORE всегда успешны. Теущий: " + ToString());
            return output;
        }

        private ReportParser RuleONE_AND_MORE(List<Token> tokens, ref int begin, ref int end)
        {
            ReportParserCompile compile = new ReportParserCompile(this, ONE_AND_MORE, -1);
            ReportParser output = new ReportParser(compile);
            output.Merge(RuleAND(tokens, ref begin, ref end), ++compile.Helper);
            if (!output.IsSuccess)
            {
                output.CompileCancel();
                return output;
            }
            output.Merge(RuleZERO_AND_MORE(tokens, ref begin, ref end), ++compile.Helper);
            return output;
        }

        private ReportParser RuleAND(List<Token> tokens, ref int begin, ref int end)
        {
            ReportParserCompile compile = new ReportParserCompile(this, AND);
            ReportParser output = new ReportParser(compile);
            int b = begin;
            int e = end;
            int i = -1;
            foreach(object o in this)
            {
                i++;
                if(o is Terminal)
                {
                    if (begin > end)
                        output.Info.Add(new ReportParserInfoLine(
                            "Входные токены закончились", o, null, begin));
                    else if (!o.Equals(tokens[begin++].Type))
                        output.Info.Add(new ReportParserInfoLine(o, tokens[--begin], tokens, begin));
                    else
                        compile.Tokens[begin - b - 1] = tokens[begin - 1];
                }
                else if(o is Nonterminal)
                {
                    output.Merge(((Nonterminal)o).CheckRule(tokens, ref begin, ref end), i);
                    if(!output.IsSuccess)
                    {
                        output.Info.Add(new ReportParserInfoLine(o, null, tokens, begin));
                    }
                }
                if (!output.IsSuccess)
                {
                    begin = b;
                    end = e;
                    output.CompileCancel();
                    return output;
                }
            }
            if(!output.IsSuccess)
            {
                begin = b;
                end = e;
                output.CompileCancel();
            }
            return output;
        }

        private ReportParser RuleOR(List<Token> tokens, ref int begin, ref int end)
        {
            ReportParserCompile compile = new ReportParserCompile(this, OR, -1);
            ReportParser output = new ReportParser(compile);
            foreach (object o in this)
            {
                compile.Helper++;
                if (o is Terminal)
                {
                    if (begin > end)
                        output.Info.Add(new ReportParserInfoLine(
                            "Входные токены закончились", o, null, begin));
                    else if (!o.Equals(tokens[begin++].Type))
                        output.Info.Add(new ReportParserInfoLine(o, tokens[--begin], tokens, begin));
                    else
                    {
                        output.Info.Success(o.ToString());
                        compile.Tokens[compile.Helper] = tokens[begin - 1];
                        return output;
                    }
                }
                else if (o is Nonterminal)
                {
                    ReportParser buffer = ((Nonterminal)o).CheckRule(tokens, ref begin, ref end);
                    if (buffer.IsSuccess)
                    {
                        output.Merge(buffer, compile.Helper);
                        output.Info.Success(o.ToString());
                        return output; // Да, этот нетерминал нам подходит.
                    }
                    else
                        output.Merge(buffer, compile.Helper);
                }
                else
                    throw new Exception($"Unexpected type {o.GetType()} of {o} in list");
            }
            if(begin < tokens.Count)
                output.Info.Add(new ReportParserInfoLine("Для оператора OR не найдено ни одного истинного выражения.", this, tokens[begin], tokens, -1));
            output.CompileCancel();
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

        public override string ToString()
            => ToString(); // Как это работает?

        public string ToString(uint depth = 1)
        {
            if (Name != null)
                return $"name: {Name}";
            StringBuilder sb = new StringBuilder();
            sb.Append("rule: " + rule);
            sb.Append(", elements: ");
            if (list != null)
            {
                foreach (object o in this)
                {
                    if (o != null)
                    {
                        if (o is Nonterminal)
                            if (depth != 0)
                                sb.Append("{ " + ((Nonterminal)o).ToString(depth - 1) + " }");
                            else
                                sb.Append("{ ... }");
                        else
                            sb.Append("{ " + o.ToString() + " } ");
                    }
                    else
                        sb.Append("{ element is null }");
                }
            }
            else
            {
                sb.Append("list is null.");
            }
            return sb.ToString();
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
