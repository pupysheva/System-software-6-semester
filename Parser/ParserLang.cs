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
                Nonterminal value = new Nonterminal(OR);

                Nonterminal func_expr = new Nonterminal(AND);
                Nonterminal stmt = new Nonterminal(OR, new Nonterminal(AND, value, new Nonterminal(ZERO_AND_MORE, "OP", value)), func_expr);
                Nonterminal arguments_expr = new Nonterminal(OR, new Nonterminal(ONE_AND_MORE,stmt,"COM"),stmt);
                Nonterminal b_val_expr = new Nonterminal(OR,stmt, new Nonterminal(AND,"L_B", stmt, "R_B"));
                Nonterminal body = new Nonterminal(AND, "L_QB", lang, "R_QB");
                Nonterminal condition = new Nonterminal(AND,"L_B", value, "LOGICAL_OP", value,"R_B");
                Nonterminal while_expr = new Nonterminal(AND, "WHILE_KW", condition, body);
                Nonterminal assign_expr = new Nonterminal(AND, "VAR", "ASSIGN_OP", b_val_expr);

                Nonterminal if_expr = new Nonterminal(AND, "IF_KW", condition, body, "ELSE_KW",body);
                Nonterminal for_expr = new Nonterminal(AND, "FOR_KW", "L_B", assign_expr, "COMMA", condition, "COMMA", assign_expr, "R_B", body);
                
                Nonterminal expr = new Nonterminal(OR, assign_expr, while_expr, "PRINT_KW", if_expr, for_expr, func_expr);

                lang.Add(expr);
                value.AddRange(new object[] { "VAR", "DIGIT", b_val_expr });
                func_expr.AddRange(new object[] { "VAR", "L_B", arguments_expr, "R_B" });

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
                output.Add(new ParserException("Входной текст не полностью подходит к грамматике.", null, tokens, tokens, begin));
            return output;
        }
    }
}
