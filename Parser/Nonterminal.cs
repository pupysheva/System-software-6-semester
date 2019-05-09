using Lexer;
using System;
using System.Collections;
using System.Collections.Generic;
using static Parser.RuleOperator;
using static Parser.ReportParser;
using System.Text;

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
        private Nonterminal(params object[] OperatorAndterminalsOrNonterminals)
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
            switch (rule)
            {
                case AND:
                    return RuleAND(tokens, ref begin, ref end);
                case ONE_AND_MORE:
                    return RuleONE_AND_MORE(tokens, ref begin, ref end);
                case OR:
                    return RuleOR(tokens, ref begin, ref end);
                case ZERO_AND_MORE:
                    return RuleZERO_AND_MORE(tokens, ref begin, ref end);
                default:
                    throw new NotImplementedException($"Оператор {Enum.GetName(typeof(RuleOperator), rule)} не реализован.");
            }
        }

        private ReportParser RuleZERO_AND_MORE(List<Token> tokens, ref int begin, ref int end)
        {
            ReportParser output = new ReportParser();
            ReportParser buffer;
            do
            {
                buffer = RuleAND(tokens, ref begin, ref end);
                output.AddRange(buffer);
            }
            while (buffer.IsSuccess) ;
            output.IsSuccess = true;
            return output;
        }

        private ReportParser RuleONE_AND_MORE(List<Token> tokens, ref int begin, ref int end)
        {
            ReportParser output = new ReportParser();
            output.AddRange(RuleAND(tokens, ref begin, ref end));
            if (!output.IsSuccess)
                return output;
            output.AddRange(RuleZERO_AND_MORE(tokens, ref begin, ref end));
            return output;
        }

        private ReportParser RuleAND(List<Token> tokens, ref int begin, ref int end)
        {
            ReportParser output = new ReportParser();
            int b = begin;
            int e = end;
            foreach(object o in this)
            {
                if(o is Terminal)
                {
                    if (begin > end)
                        output.Add(new ParserException(
                            "Входные токены закончились", o, null, begin));
                    else if (!o.Equals(tokens[begin++].Type))
                        output.Add(new ParserException(o, tokens[--begin], tokens, begin));
                }
                else if(o is Nonterminal)
                {
                    output.AddRange(((Nonterminal)o).CheckRule(tokens, ref begin, ref end));
                    if(!output.IsSuccess)
                    {
                        output.Add(new ParserException(o, null, tokens, begin));
                    }
                }
                if (!output.IsSuccess)
                {
                    begin = b;
                    end = e;
                    return output;
                }
            }
            if(!output.IsSuccess)
            {
                begin = b;
                end = e;
            }
            return output;
        }

        private ReportParser RuleOR(List<Token> tokens, ref int begin, ref int end)
        {
            ReportParser output = new ReportParser();
            foreach(object o in this)
            {
                if (o is Terminal)
                {
                    if (begin > end)
                        output.Add(new ParserException(
                            "Входные токены закончились", o, null, begin));
                    else if (!o.Equals(tokens[begin++].Type))
                        output.Add(new ParserException(o, tokens[--begin], tokens, begin));
                    else
                    {
                        output.IsSuccess = true;
                        return output;
                    }
                }
                else if (o is Nonterminal)
                {
                    ReportParser buffer = ((Nonterminal)o).CheckRule(tokens, ref begin, ref end);
                    if (buffer.IsSuccess)
                    {
                        output.AddRange(buffer);
                        output.IsSuccess = true;
                        return output; // Да, этот нетерминал нам подходит.
                    }
                    else
                        output.AddRange(buffer);
                }
                else
                    throw new Exception($"Unexpected type {o.GetType()} of {o} in list");
            }
            if (output.Count == 0)
                output.Add(new ParserException("Для оператора OR не найдено ни одного истинного выражения.", this, tokens[begin], tokens, -1));
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

        public override string ToString()
            => ToString(); // Как это работает?

        public string ToString(uint depth = 1)
        {
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
