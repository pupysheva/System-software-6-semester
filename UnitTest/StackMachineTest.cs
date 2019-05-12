using System;
using System.Collections.Generic;
using Lexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using static Parser.RuleOperator;

namespace UnitTest
{
    [TestClass]
    public class StackMachineTest
    {
        private readonly ParserLang EasyParserLang;
        private readonly LexerLang EasyLexerLang;

        public StackMachineTest()
        {
            EasyLexerLang = new LexerLang(new Terminal[]{
                new Terminal("ASSIGN_OP", "^=$"),
                new Terminal("VAR", "^[a-zA-Z]+$", uint.MaxValue),
                new Terminal("DIGIT", "^0|([1-9][0-9]*)$"),
                new Terminal("OP", "^\\+|-|\\*|/$"),
                new Terminal("WHILE_KW", "^while$", 0),
                new Terminal("PRINT_KW", "^print$", 0),
                new Terminal("L_QB", "^{$"),
                new Terminal("R_QB", "^}$"),
                new Terminal("L_B", "^\\($"),
                new Terminal("R_B", "^\\)$"),
                new Terminal("COM", "^,$"),

                new Terminal("CH_SPACE", "^ $"),
                new Terminal("CH_LEFTLINE", "^\r$"),
                new Terminal("CH_NEWLINE", "^\n$"),
                new Terminal("CH_TAB", "^\t$")
            });

            /*
             * Правила стековой машины:
             * Все команды выполняются в постфиксной записи.
             * 
             * {value logical, value addr, "if", com1}
             * Где:
             * logical - если 0, то переход к com1. Ложь - переход к адресу addr.
             * addr - адрес перехода.
             * 
             * {value addr, "goto"}
             * Переход к адресу addr.
             * 
             * {digit v, "push"}
             * Добавляет v в стек.
             * 
             * {var v, "pull"}
             * Извлекает из стека в v.
             */

            Nonterminal lang = new Nonterminal("lang",
                (List<string> commands, ActionInsert insert, int id) =>
                {
                    insert();
                }, ZERO_AND_MORE);
            Nonterminal value = new Nonterminal("value",
                (List<string> commands, ActionInsert insert, int id) =>
                {
                    insert();
                }, OR, "VAR", "DIGIT");
            Nonterminal stmt = new Nonterminal("stmt",
                (List<string> commands, ActionInsert insert, int id) =>
                {
                    insert(1);
                    insert(0);
                }, AND,
                value, new Nonterminal("(OP value)*", ZERO_AND_MORE, "OP", value));
            Nonterminal assign_expr = new Nonterminal("assign_expr",
                    (List<string> commands, ActionInsert insert, int id) =>
                    {
                        insert(0);
                        insert(2);
                        insert(1);
                    }, AND,
                    "VAR", "ASSIGN_OP", stmt);
            Nonterminal while_expr = new Nonterminal("while_expr",
                // Нужно преобразовать в стековый код.
                (List<string> commands, ActionInsert insert, int id) =>
                {
                    insert(1);
                    commands.Add("?"); // Адрес с истиной.
                    int indexAddrTrue = commands.Count - 1;
                    commands.Add("if");
                    commands.Add("?"); // Адрес с ложью.
                    int indexAdrrFalse = commands.Count - 1;
                    commands.Add("goto");
                    // Сюда надо попасть, если true. 
                    commands[indexAddrTrue] = commands.Count.ToString();
                    insert(4); // Тело while.
                    commands.Add(indexAddrTrue.ToString());
                    commands.Add("goto"); // Команда перехода в if к while.
                    // Надо выйти из цикла, если false:
                    commands[indexAdrrFalse] = commands.Count.ToString();
                }, AND,
                "L_B", stmt, "R_B", "L_QB", lang, "R_QB");
            Nonterminal expr = new Nonterminal("expr",
                // Нужно преобразовать в стековый код.
                (List<string> commands, ActionInsert insert, int id) =>
                {
                    switch(id)
                    {
                        case 0:
                            {
                                insert();
                            }
                            break;
                        case 1:
                            {
                                insert();
                            }
                            break;
                        case 2:
                            {
                                commands.Add("?");
                                int indexOfBack = commands.Count - 1;
                                commands.Add("push");
                                commands.Add("print");
                                commands.Add("goto");
                                commands[indexOfBack] = commands.Count.ToString();
                            }
                            break;
                    }
                }, OR,
                assign_expr, while_expr, "PRINT_KW");
            lang.Add(expr);

            EasyParserLang = new ParserLang(lang);
        }

        [TestMethod]
        public void TestMethod1()
        {
            
        }
    }
}
