namespace Parser
{
    public class ReportParserCompile
    {
        /// <summary>
        /// Кто добавил?
        /// </summary>
        public readonly Nonterminal Source;
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

        public override string ToString()
            => $"ReportParserCompile: {{{(Helper == int.MinValue ? "" : $"h: {Helper}, ")}rule: {CurrentRule}, src: {Source}}}";
    }
}
