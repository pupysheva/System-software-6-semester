using System;
using System.Collections.Generic;
using Lexer;
using static Parser.RuleOperator;
using System.Linq;

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
            ReportParserCompile comp = report.Compile;
            comp.Source.TransferToStackCode(commands, (i) => Inserter(i, comp, commands), comp.Helper);
            return commands;
        }

        /// <summary>
        /// Занимается вставкой в стекый код.
        /// </summary>
        /// <param name="i">Какой объект из <see cref="Nonterminal.list"/> просит вставить.</param>
        /// <param name="comp">Информация о компиляции, которая запрашивает вставку.</param>
        /// <param name="commands">Список команд, куда надо вставить команду.</param>
        /// <returns></returns>
        private bool Inserter(int i, ReportParserCompile comp, List<string> commands)
        {
            if (i >= 0)
            {
                if (comp.CurrentRule == OR)
                    throw new NotSupportedException($"Возможно, неправильно настроены правила компиляции в нетерминале: {comp.Source}");
                if (comp.Tokens.ContainsKey(i))
                { // Это терминал.
                    commands.Add(comp.Tokens[i].Value);
                }
                else if (comp.deepList.ContainsKey(i))
                { // Это нетерминал.
                    comp.deepList[i].Source.TransferToStackCode(commands, (j) => Inserter(j, comp, commands), comp.Helper);
                }
                else
                    throw new ArgumentException($"Не получилось определить, терминал ли это, или нетерминал в списке {comp} с id {i}");
            }
            else // if(i < 0)
            {
                if (comp.CurrentRule != OR)
                    throw new NotSupportedException($"Возможно, неправильно настроены правила компиляции в нетерминале: {comp.Source}");
                if ((comp.Tokens.Count > 0 && comp.deepList.Count > 0)
                    || (comp.Tokens.Count > 1 || comp.deepList.Count > 1))
                    throw new ArgumentException($"Ошибка при настройке компиляции. Ожидался только один правильный элемент в нетерминале: {comp}");
                if(comp.Tokens.Count > 0)
                { // Это терминал.
                    commands.Add(comp.Tokens.First().Value.Value);
                }
                else
                { // Это нетерминал.
                    comp.deepList.First().Value.Source.TransferToStackCode(commands, (j) => Inserter(j, comp, commands), comp.Helper);
                }
            }
            return true;
        }

        public override string ToString()
            => $"{base.ToString()}: main nonterminal: {mainNonterminal}";
    }
}
