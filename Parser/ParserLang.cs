using System;
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
                Nonterminal lang = new Nonterminal("lang", ZERO_AND_MORE);
                Nonterminal value = new Nonterminal("value", OR);

                Nonterminal func_expr = new Nonterminal("func_expr", AND);
                Nonterminal stmt = new Nonterminal("stmt", OR, new Nonterminal("value (OP value)*", AND, value, new Nonterminal("(OP value)*", ZERO_AND_MORE, "OP", value)), func_expr);
                Nonterminal arguments_expr = new Nonterminal("arguments_expr", OR, new Nonterminal("(stmt COM)+", ONE_AND_MORE, stmt, "COM"), stmt);
                Nonterminal b_val_expr = new Nonterminal("b_val_expr", OR, stmt, new Nonterminal("L_B stmt R_B", AND, "L_B", stmt, "R_B"));
                Nonterminal body = new Nonterminal("body", AND, "L_QB", lang, "R_QB");
                Nonterminal condition = new Nonterminal("condition", AND, "L_B", value, "LOGICAL_OP", value,"R_B");
                Nonterminal while_expr = new Nonterminal("while_expr", AND, "WHILE_KW", condition, body);
                Nonterminal assign_expr = new Nonterminal("assign_expr", AND, "VAR", "ASSIGN_OP", b_val_expr);

                Nonterminal if_expr = new Nonterminal("if_expr", AND, "IF_KW", condition, body, "ELSE_KW",body);
                Nonterminal for_expr = new Nonterminal("for_expr", AND, "FOR_KW", "L_B", assign_expr, "COMMA", condition, "COMMA", assign_expr, "R_B", body);
                
                Nonterminal expr = new Nonterminal("expr", OR, assign_expr, while_expr, "PRINT_KW", if_expr, for_expr, func_expr);

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
                output.Info.Add(new ReportParserInfoLine("Входной текст не полностью подходит к грамматике.", null, tokens, tokens, begin));
            return output;
        }

        public List<string> Compile(List<Token> tokens)
        {
            List<string> commands = new List<string>(tokens.Count);
            ReportParser report = Check(tokens);
            foreach (ReportParserCompileLine comp in report.Compile)
            {
                comp.Source.TransferToStackCode(commands, (i) => Catcher(i, comp, commands), comp.Helper);
            }
            return commands;
        }

        private bool Catcher(int i, ReportParserCompileLine comp, List<string> commands)
        {
            if (i == -1 && comp.CurrentRule == OR)
            {
                return Inserter(comp, comp.Helper, commands);
            }
            else if (i >= 0 && comp.CurrentRule != OR)
            {
                for (int repeat = comp.CurrentRule == ZERO_AND_MORE || comp.CurrentRule == ONE_AND_MORE ? comp.Helper : 1; repeat >= 1; repeat--)
                {
                    if (!Inserter(comp, i, commands))
                        return false;
                }
                return true;
            }
            else if (i == -1 && comp.CurrentRule != OR)
                throw new ArgumentOutOfRangeException("Поддержка insert по-умолчанию во время компиляции поддерживается только для правил нетерминалов OR. Пожалуйста, проверьте правила компиляции в:" + this);
            else
                throw new NotSupportedException();
        }

        private bool Inserter(ReportParserCompileLine context, int idCurrent, List<string> commands)
        {
            if (context.Source[idCurrent] is Terminal)
                commands.Add(context.Tokens[idCurrent].Value);
            else if (context.Source[idCurrent] is Nonterminal) ;
            //((Nonterminal)context.Source[idCurrent]).Compile(); Ай, ладно. Пусть foreach на compile разбирается.
            else
                throw new NotSupportedException($"Поддерживаются только {typeof(Terminal)} и {typeof(Nonterminal)}.");
            return true;
        }

        public override string ToString()
            => $"{base.ToString()}: main nonterminal: {mainNonterminal}";
    }
}
