using System;
using System.Collections.Generic;
using Lexer;
using static Parser.RuleOperator;
using MyTypes.Tree;

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
                Nonterminal stmt =
                    new Nonterminal("stmt", OR, new Nonterminal("value (OP value)*", AND,
                    value,
                    new Nonterminal("(OP value)*", ZERO_AND_MORE,
                        new Nonterminal("OP & value", AND,
                            "OP",
                            value))),
                    func_expr);
                Nonterminal arguments_expr = new Nonterminal("arguments_expr", OR, new Nonterminal("(stmt COM)+", ONE_AND_MORE, new Nonterminal("stmt & COM", AND, stmt, "COM")), stmt);
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
            ReportParser output = mainNonterminal.CheckRule(500, tokens, ref begin, ref end);
            if (output.IsSuccess && begin <= end)
                output.Info.Add(new ReportParserInfoLine($"Входной текст не полностью подходит к грамматике.", null, tokens, tokens, begin));
            return output;
        }

        public List<string> Compile(List<Token> tokens, ReportParser report = null)
        {
            List<string> commands = new List<string>(tokens.Count);
            if (report == null)
                report = Check(tokens);
            if (!report.IsSuccess)
                return null;
            ITreeNode<object> compileTree = report.Compile;
            ReportParserCompile currentComp = (ReportParserCompile)compileTree.Current;
            currentComp.Source.TransferToStackCode(commands, (i) => Inserter(i, compileTree, commands), currentComp.Helper);
            return commands;
        }

        /// <summary>
        /// Занимается вставкой в стекый код.
        /// </summary>
        /// <param name="i">Какой объект из <see cref="Nonterminal.list"/> просит вставить.</param>
        /// <param name="compileTree">Информация о компиляции, которая запрашивает вставку.</param>
        /// <param name="commands">Список команд, куда надо вставить команду.</param>
        /// <returns></returns>
        private bool Inserter(int i, ITreeNode<object> compileTree, List<string> commands)
        {
            ReportParserCompile comp = (ReportParserCompile)compileTree.Current;
            if (i >= 0)
            { // AND, MORE
                if (comp.CurrentRule == OR)
                    throw new NotSupportedException($"Возможно, неправильно настроены правила компиляции в нетерминале: {comp.Source}");
                if (i >= compileTree.Count)
                    throw new IndexOutOfRangeException($"Был запрошен индекс вне границ. Индекс: {i}, коллекция: {compileTree}");
                if (compileTree[i].Current is Token token)
                { // Это терминал.
                    commands.Add(token.Value);
                }
                else if (compileTree[i].Current is ReportParserCompile deep)
                { // Это нетерминал.
                    deep.Source.TransferToStackCode(commands, (j) => Inserter(j, compileTree[i], commands), deep.Helper);
                }
                else
                    throw new ArgumentException($"Не получилось определить, терминал ли это, или нетерминал в списке {compileTree} с id {i}: {compileTree[i].GetType()}");
            }
            else // if(i < 0)
            { // OR
                if (comp.CurrentRule != OR)
                    throw new NotSupportedException($"Возможно, неправильно настроены правила компиляции в нетерминале: {comp.Source}");
                if (compileTree.Count != 1)
                    throw new ArgumentException($"Ошибка при настройке компиляции. Найдено: {compileTree.Count} Ожидался только один правильный элемент в нетерминале: {compileTree}\n");
                if (compileTree[0].Current is Token token)
                { // Это терминал.
                    commands.Add(token.Value);
                }
                else if (compileTree[0].Current is ReportParserCompile deep)
                { // Это нетерминал.
                    deep.Source.TransferToStackCode(commands, (j) => Inserter(j, compileTree[0], commands), deep.Helper);
                }
                else
                    throw new ArgumentException($"Не получилось определить, терминал ли это, или нетерминал в списке {compileTree} с id {0}: {compileTree[0].GetType()}");
            }
            return true;
        }

        public override string ToString()
            => $"{base.ToString()}: main nonterminal: {mainNonterminal}";
    }
}
