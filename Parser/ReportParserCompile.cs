using Lexer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class ReportParserCompile
    {
        /// <summary>
        /// int - соответсвие int в <see cref="Nonterminal.list"/>.
        /// </summary>
        public readonly Dictionary<int, ReportParserCompile> deepList
            = new Dictionary<int, ReportParserCompile>();

        /// <summary>
        /// Создание нового эксемпляра инструкции компилятору.
        /// </summary>
        /// <param name="Source">Источник, кто добавил в компилятор запись.</param>
        /// <param name="Helper">Дополнительная информация о результатах парсера.</param>
        public ReportParserCompile(Nonterminal Source, RuleOperator CurrentRule, int Helper = int.MinValue)
        {
            this.Source = Source;
            this.CurrentRule = CurrentRule;
            this.Helper = Helper;
        }

        /// <summary>
        /// Кто добавил?
        /// </summary>
        public readonly Nonterminal Source;
        /// <summary>
        /// Список токенов, которые понадобятся при компиляции.
        /// Соответсвие id <see cref="Nonterminal.list"/> с <see cref="Token"/>.
        /// В случае, если <see cref="Nonterminal.list"/>[id] = null, то
        /// <see cref="Tokens"/>[id] не найдёт соответсвия.
        /// </summary>
        public readonly Dictionary<int, Token> Tokens
            = new Dictionary<int, Token>();

        /// <summary>
        /// Конкретное правило, которое использует нетерминал.
        /// Несмотря на то, что у нетерминала присваевается одно правило,
        /// использовать он может несколько.
        /// Так, при использовании <see cref="RuleOperator.ONE_AND_MORE"/> также используются правила:
        /// <see cref="RuleOperator.ZERO_AND_MORE"/> и <see cref="RuleOperator.AND"/>.
        /// </summary>
        public readonly RuleOperator CurrentRule;
        /// <summary>
        /// Для AND не нужен.
        /// Для OR - идентификатор следующего шага.
        /// Для MORE - количество повторов.
        /// </summary>
        public int Helper;

        public override string ToString()
            => $"{base.ToString()}: Source: {Source} Rule: {CurrentRule} Helper: {Helper} Tokens: [{string.Join(", ", Tokens)}] deepList: [{string.Join(", ", deepList)}]";
    }
}
