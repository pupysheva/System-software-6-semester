using Lexer;
using MyTypes;
using MyTypes.LinkedList;
using Parser;
using System;
using System.Collections.Generic;
using System.Text;
using static Parser.RuleOperator;

namespace StackMachine
{
    public static class Lang
    {

        private readonly static Terminal ASSIGN_OP   = new Terminal(nameof(ASSIGN_OP),        "^=$"),
                VAR              = new Terminal(nameof(VAR),              "^[a-zA-Z][a-zA-Z0-9]*$" , uint.MaxValue - 1),
                DIGIT            = new Terminal(nameof(DIGIT),            "^0|([1-9][0-9]*)$"      ),
                OP               = new Terminal(nameof(OP),               "^\\+|-|\\*|/$"          ),
                LOGICAL_OP       = new Terminal(nameof(LOGICAL_OP),       "^>|<|>=|<=|==$"         ),
                WHILE_KW         = new Terminal(nameof(WHILE_KW),         "^while$"                , 0),
                DO_KW            = new Terminal(nameof(DO_KW),            "^do$"                   , 0),
                PRINT_KW         = new Terminal(nameof(PRINT_KW),         "^print$"                , 0),
                FOR_KW           = new Terminal(nameof(FOR_KW),           "^for$"                  , 0),
                IF_KW            = new Terminal(nameof(IF_KW),            "^if$"                   , 0),
                ELSE_KW          = new Terminal(nameof(ELSE_KW),          "^else$"                 , 0),
                L_QB             = new Terminal(nameof(L_QB),             "^{$"                    ),
                R_QB             = new Terminal(nameof(R_QB),             "^}$"                    ),
                L_B              = new Terminal(nameof(L_B),              "^\\($"                  ),
                R_B              = new Terminal(nameof(R_B),              "^\\)$"                  ),
                COMMA            = new Terminal(nameof(COMMA),            "^;$"                    ),
                COM              = new Terminal(nameof(COM),              "^,$"                    ),
                HASHSET_ADD      = new Terminal(nameof(HASHSET_ADD),      "^HASHSET_ADD$"          ),
                HASHSET_CONTAINS = new Terminal(nameof(HASHSET_CONTAINS), "^HASHSET_CONTAINS$"     ),
                HASHSET_REMOVE   = new Terminal(nameof(HASHSET_REMOVE),   "^HASHSET_REMOVE$"       ),
                HASHSET_COUNT    = new Terminal(nameof(HASHSET_COUNT),    "^HASHSET_COUNT$"        ),
                LIST_ADD         = new Terminal(nameof(LIST_ADD),         "^LIST_ADD$"             ),
                LIST_CONTAINS    = new Terminal(nameof(LIST_CONTAINS),    "^LIST_CONTAINS$"        ),
                LIST_REMOVE      = new Terminal(nameof(LIST_REMOVE),      "^LIST_REMOVE$"          ),
                LIST_COUNT       = new Terminal(nameof(LIST_COUNT),       "^LIST_COUNT$"           ),
                /*                                                                                 
                 Те терминалы, которые ниже, по-сути нужны парсеру.                                
                 Для того, чтобы проанализировать выражение:                                       
                 a = nameof(Привет) мир!",                                                         
                 чтобы не было так:                                                                
                 a=nameof(Привет)мир!".                                                            
                 */                                                                                
                CH_LISTORSETMAYBE= new Terminal(nameof(CH_LISTORSETMAYBE), "^[a-zA-Z_]+$"          , uint.MaxValue),
                CH_COMMENT       = new Terminal(nameof(CH_COMMENT),       "^\\/\\/.*$"             ),
                CH_SPACE         = new Terminal(nameof(CH_SPACE),         "^ $"                    ),
                CH_LEFTLINE      = new Terminal(nameof(CH_LEFTLINE),      "^\r$"                   ),
                CH_NEWLINE       = new Terminal(nameof(CH_NEWLINE),       "^\n$"                   ),
                CH_TAB           = new Terminal(nameof(CH_TAB),           "^\t$"                   );


        public static readonly LexerLang lexerLang;
        public static readonly ParserLang parserLang;
        public static readonly MyStackLang stackMachine;

        static Lang()
        {
            lexerLang = new LexerLang(new List<Terminal>()
            {
                ASSIGN_OP, VAR, DIGIT, OP, LOGICAL_OP, WHILE_KW, DO_KW, PRINT_KW,
                FOR_KW, IF_KW, ELSE_KW, L_QB, R_QB, L_B, R_B, COMMA, COM,
                HASHSET_ADD, HASHSET_CONTAINS, HASHSET_REMOVE, HASHSET_COUNT, LIST_ADD, LIST_CONTAINS,
                LIST_REMOVE, LIST_COUNT, CH_LISTORSETMAYBE, CH_COMMENT, CH_SPACE, CH_LEFTLINE, CH_NEWLINE, CH_TAB
            });

            void OrInserter(List<string> commands, ActionInsert insert, int helper)
                => insert();

            TransferToStackCode AndInserter(params int[] order)
            {
                return (List<string> commands, ActionInsert insert, int helper) =>
                {
                    int a = 0;
                    while (a < order.Length)
                    {
                        insert(order[a++]);
                    }
                };
            }

            void MoreInserter(List<string> commands, ActionInsert insert, int helper)
            {
                for(int i = 0; i < helper; i++)
                    insert(i);
            }
            void WordAndValue(List<string> commands, ActionInsert insert, int helper)
            {
                insert(1);
                insert(0);
            }

            Nonterminal lang = new Nonterminal(nameof(lang), MoreInserter, ZERO_AND_MORE);
            Nonterminal value = new Nonterminal(nameof(value), OrInserter, OR);
            Nonterminal command_hash_expr = new Nonterminal(nameof(command_hash_expr), OrInserter, OR,
                new Nonterminal("HASHSET_ADD & value", WordAndValue, AND, HASHSET_ADD, value),
                new Nonterminal("HASHSET_CONTAINS & value", WordAndValue, AND, HASHSET_CONTAINS, value),
                new Nonterminal("HASHSET_REMOVE & value", WordAndValue, AND, HASHSET_REMOVE, value),
                new Nonterminal("HASHSET_COUNT", AndInserter(0), AND, HASHSET_COUNT));
            Nonterminal command_list_expr = new Nonterminal(nameof(command_list_expr), OrInserter, OR,
                new Nonterminal("LIST_ADD & value", WordAndValue, AND, LIST_ADD, value),
                new Nonterminal("LIST_CONTAINS & value", WordAndValue, AND, LIST_CONTAINS, value),
                new Nonterminal("LIST_REMOVE & value", WordAndValue, AND, LIST_REMOVE, value),
                new Nonterminal("LIST_COUNT", AndInserter(0), AND, LIST_COUNT));
            Nonterminal stmt =
                new Nonterminal(nameof(stmt), OrInserter, OR,
                    command_hash_expr,
                    command_list_expr,
                    new Nonterminal("value (OP value)*", AndInserter(0, 1), AND,
                        value,
                        new Nonterminal("(OP value)*", MoreInserter, ZERO_AND_MORE,
                            new Nonterminal("OP & value", AndInserter(1, 0), AND,
                                "OP",
                                value
                            )
                        )
                    )
                );
            Nonterminal b_val_expr = new Nonterminal(nameof(b_val_expr),
                OrInserter, OR, new Nonterminal("L_B stmt R_B", AndInserter(1), AND, L_B, stmt, R_B), stmt);
            Nonterminal body = new Nonterminal(nameof(body), AndInserter(1), AND, "L_QB", lang, "R_QB");
            Nonterminal condition = new Nonterminal(nameof(condition), AndInserter(1, 3, 2), AND, L_B, stmt, LOGICAL_OP, stmt, R_B);
            Nonterminal for_condition = new Nonterminal(nameof(condition), AndInserter(0, 2, 1), AND, value, LOGICAL_OP, value);
            Nonterminal while_expr = new Nonterminal(nameof(while_expr),
                (List<string> commands, ActionInsert insert, int helper) =>
                {
                    int beginWhile = commands.Count;
                    insert(1); // condition
                    int indexAddrFalse = commands.Count;
                    commands.Add("?"); // Адрес, который указывает на то место, куда надо перейти в случае лжи.
                    commands.Add("!f");
                    insert(2); // true body
                    commands.Add(beginWhile.ToString());
                    commands.Add("goto!");
                    commands[indexAddrFalse] = commands.Count.ToString();
                }, AND, "WHILE_KW", condition, body);
            Nonterminal do_while_expr = new Nonterminal(nameof(do_while_expr),
                (List<string> commands, ActionInsert insert, int helper) =>
                {
                    int beginDo = commands.Count;
                    insert(1);
                    insert(3);
                    int indexOfAddressExit = commands.Count;
                    commands.Add("?");
                    commands.Add("!f");
                    commands.Add(beginDo.ToString()); // Если попали сюда, значит истина. И надо повторить.
                    commands.Add("goto!");
                    commands[indexOfAddressExit] = commands.Count.ToString();
                }
                , AND, DO_KW, body, WHILE_KW, condition);
            Nonterminal assign_expr = new Nonterminal(nameof(assign_expr), AndInserter(0, 2, 1), AND, VAR, ASSIGN_OP, b_val_expr);
            Nonterminal ifelse_expr = new Nonterminal(nameof(ifelse_expr),
                (List<string> commands, ActionInsert insert, int helper) =>
                {
                    insert(1); // condition
                    int indexAddrFalse = commands.Count;
                    commands.Add("?"); // Адрес, который указывает на то место, куда надо перейти в случае лжи.
                    commands.Add("!f");
                    insert(2); // true body
                    int indexAddrWriteToEndElse = commands.Count;
                    commands.Add("?"); // Адрес, который указывает на конец body в else.
                    commands.Add("goto!");
                    commands[indexAddrFalse] = commands.Count.ToString();
                    insert(4);
                    commands[indexAddrWriteToEndElse] = commands.Count.ToString();
                }, AND, IF_KW, /*1*/ condition, /*2*/body, ELSE_KW, /*4*/body);
            Nonterminal if_expr = new Nonterminal(nameof(if_expr),
                (List<string> commands, ActionInsert insert, int helper) =>
                {
                    insert(1); // condition
                    int indexAddrFalse = commands.Count;
                    commands.Add("?"); // Адрес, который указывает на то место, куда надо перейти в случае лжи.
                    commands.Add("!f");
                    insert(2); // true body
                    commands[indexAddrFalse] = commands.Count.ToString();
                }, AND, IF_KW, /*1*/ condition, /*2*/body);
            Nonterminal if_expr_OR_ifelse_expr = new Nonterminal(nameof(if_expr_OR_ifelse_expr), OrInserter, OR, ifelse_expr, if_expr);
            Nonterminal for_expr = new Nonterminal(nameof(for_expr),
                (List<string> commands, ActionInsert insert, int helper) =>
                {
                    insert(2); // assign_expr
                    int indexCondition = commands.Count;
                    insert(4); // for_condition
                    int indexAddrFalse = commands.Count;
                    commands.Add("?"); // Адрес, который указывает на то место, куда надо перейти в случае лжи.
                    commands.Add("!f");
                    insert(8); // true body
                    insert(6); // assign_expr
                    commands.Add(indexCondition.ToString());
                    commands.Add("goto!");
                    commands[indexAddrFalse] = commands.Count.ToString();
                }, AND, "FOR_KW", "L_B", /*2*/assign_expr, "COMMA", /*4*/for_condition, "COMMA", /*6*/assign_expr, "R_B", /*8*/ body);
            Nonterminal cycle_expr = new Nonterminal(nameof(cycle_expr), OrInserter, OR, while_expr, do_while_expr, for_expr);
            Nonterminal expr = new Nonterminal(nameof(expr), OrInserter, OR, PRINT_KW, assign_expr, if_expr_OR_ifelse_expr, cycle_expr, command_hash_expr, command_list_expr);
            lang.Add(expr);
            value.AddRange(new object[] { "VAR", "DIGIT", b_val_expr });

            parserLang = new ParserLang(lang);
            stackMachine = new MyStackLang();
        }

        public class MyStackLang : AbstractStackExecuteLang
        {
            public readonly ICollection<double> list
                = new MyLinkedList<double>();
            public readonly ISet<double> set
                = new MyHashSet<double>();

            protected override void ExecuteCommand(string command)
            {
                switch (command)
                {
                    case "print":
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (var pair in variables)
                            {
                                sb.Append(pair.Key);
                                sb.Append(" = ");
                                sb.Append(pair.Value);
                                sb.AppendLine();
                            }
                            Console.Write(sb.ToString());
                        }
                        break;
                    case "goto!":
                        {
                            InstructionPointer =
                                (int)PopStk() - 1;
                        }
                        break;
                    case "!f":
                        {
                            int addr = (int)PopStk();
                            int logical = (int)PopStk();
                            if (logical == 0) // Если ложь, то пропускаем body.
                                InstructionPointer = addr - 1;
                        }
                        break;
                    case "=":
                        {
                            double stmt = PopStk();
                            string var = Stack.Pop();
                            if (IsNumber(var))
                                throw new KeyNotFoundException();
                            variables[var] = stmt;
                        }
                        break;
                    case "+":
                        {
                            Stack.Push(
                                (PopStk() + PopStk())
                                .ToString());
                        }
                        break;
                    case "-":
                        {
                            double b = PopStk();
                            double a = PopStk();
                            Stack.Push(
                                (a - b)
                                .ToString());
                        }
                        break;
                    case "*":
                        {
                            Stack.Push(
                                (PopStk() * PopStk())
                                .ToString());
                        }
                        break;
                    case "/":
                        {
                            double b = PopStk();
                            double a = PopStk();
                            Stack.Push(
                                (a / b)
                                .ToString());
                        }
                        break;
                    case ">":
                        {
                            double b = PopStk();
                            double a = PopStk();
                            Stack.Push(
                                (a > b)
                                ? "1" : "0");
                        }
                        break;
                    case "<":
                        {
                            double b = PopStk();
                            double a = PopStk();
                            Stack.Push(
                                (a < b)
                                ? "1" : "0");
                        }
                        break;
                    case "==":
                        {
                            Stack.Push(
                                (PopStk() == PopStk())
                                ? "1" : "0");
                        }
                        break;
                    case "!=":
                        {
                            Stack.Push(
                                (PopStk() != PopStk())
                                ? "1" : "0");
                        }
                        break;
                    case nameof(HASHSET_ADD):
                        {
                            double buffer = PopStk();
                            Stack.Push(set.Add(buffer) ? "1" : "0");
                        }
                        break;
                    case nameof(HASHSET_CONTAINS):
                        {
                            double buffer = PopStk();
                            Stack.Push(set.Contains(buffer) ? "1" : "0");
                        }
                        break;
                    case nameof(HASHSET_COUNT):
                        {
                            Stack.Push(set.Count.ToString());
                        }
                        break;
                    case nameof(HASHSET_REMOVE):
                        {
                            double buffer = PopStk();
                            Stack.Push(set.Remove(buffer) ? "1" : "0");
                        }
                        break;
                    case "?":
                        {
                            throw new NotImplementedException();
                        }
                    case nameof(LIST_ADD):
                        {
                            double buffer = PopStk();
                            list.Add(buffer);
                            Stack.Push("1");
                        }
                        break;
                    case nameof(LIST_CONTAINS):
                        {
                            double buffer = PopStk();
                            Stack.Push(list.Contains(buffer) ? "1" : "0");
                        }
                        break;
                    case nameof(LIST_COUNT):
                        {
                            Stack.Push(list.Count.ToString());
                        }
                        break;
                    case nameof(LIST_REMOVE):
                        {
                            double buffer = PopStk();
                            Stack.Push(list.Remove(buffer) ? "1" : "0");
                        }
                        break;
                    default:
                        {
                            if (!variables.ContainsKey(command) && !IsNumber(command))
                                variables[command] = 0;
                            Stack.Push(command);
                        }
                        break;
                }
            }

            /// <summary>
            /// Получает с стека значение и вызывает <see cref="GetValueOfVarOrDigit(string)"/>.
            /// </summary>
            private double PopStk() => GetValueOfVarOrDigit(Stack.Pop());

            private bool IsNumber(string str)
            {
                return double.TryParse(str, out double drop);
            }

            private double GetValueOfVarOrDigit(string VarOrDigit)
            {
                if (double.TryParse(VarOrDigit, out double result))
                    return result;
                return variables[VarOrDigit];
            }
        }
    }

    internal static class Writer
    {
        public static void WriteAll<T>(this IEnumerable<T> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var e in list)
                sb.AppendLine(e.ToString());
            if (sb.Length == 0)
                Console.WriteLine("length = 0");
            else
                Console.Write(sb.ToString());
        }
    }
}
