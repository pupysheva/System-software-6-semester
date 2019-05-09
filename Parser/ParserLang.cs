using System.Collections.Generic;
using Lexer;
using static Parser.RuleOperator;

namespace Parser
{
    /// <summary>
    /// Представляет собой класс, который реализует парсер.
    /// </summary>
    public class ParserLang
    {
        private readonly Nonterminal mainNonterminal;

        public ParserLang(Nonterminal mainNonterminal = null)
        {
            if(mainNonterminal == null)
            {
                // Переменная lang используется в while_body, поэтому её надо объявить раньше остальных.
                Nonterminal lang = new Nonterminal(ZERO_AND_MORE);

                Nonterminal while_body = new Nonterminal(AND, "L_QB", lang, "R_QB");
                Nonterminal value = new Nonterminal(OR, "VAR", "DIGIT");
                Nonterminal while_condition = new Nonterminal(AND, value, "LOGICAL_OP", value);
                Nonterminal while_expr = new Nonterminal(AND, "WHILE_KW", while_condition, while_body);
                Nonterminal stmt = new Nonterminal(AND, value, new Nonterminal(ZERO_AND_MORE, "OP", value));
                Nonterminal assign_expr = new Nonterminal(AND, "VAR", "ASSIGN_OP", stmt);
                Nonterminal expr = new Nonterminal(OR, assign_expr, while_expr, "PRINT_KW");
                lang.Add(expr);

                mainNonterminal = lang;
            }
            this.mainNonterminal = mainNonterminal;
        }

        /// <summary>
        /// Проверяет, соответсвует ли заданый язык программирования граматике.
        /// </summary>
        /// <param name="tokens">Список терминалов входного файла.</param>
        /// <returns>Отчёт об ошибках.</returns>
        public ReportParser Check(List<Token> tokens)
        {
            int begin = 0, end = tokens.Count - 1;
            ReportParser output = mainNonterminal.CheckRule(tokens, ref begin, ref end);
            if (output.IsSuccess && begin <= end)
                output.Add(new ParserException("Входной текст не полностью подходит к грамматике.", tokens[begin]));
            return output;
        }
    }
}
