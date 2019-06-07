using Lexer;
using Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Parser.RuleOperator;

namespace StackMachine
{
    public static class Lang
    {
        public static readonly LexerLang lexerLang;
        public static readonly ParserLang parserLang;
        public static readonly AbstractStackExecuteLang stackMachine;

        static Lang()
        {
            Terminal ASSIGN_OP   = new Terminal(nameof(ASSIGN_OP),        "^=$"               ),
                VAR              = new Terminal(nameof(VAR),              "^[a-zA-Z]+$"       , uint.MaxValue),
                DIGIT            = new Terminal(nameof(DIGIT),            "^0|([1-9][0-9]*)$" ),
                OP               = new Terminal(nameof(OP),               "^\\+|-|\\*|/$"     ),
                LOGICAL_OP       = new Terminal(nameof(LOGICAL_OP),       "^>|<|>=|<=|==$"    ),
                WHILE_KW         = new Terminal(nameof(WHILE_KW),         "^while$"           , 0),
                DO_KW            = new Terminal(nameof(DO_KW),            "^do$"              , 0),
                PRINT_KW         = new Terminal(nameof(PRINT_KW),         "^print$"           , 0),
                FOR_KW           = new Terminal(nameof(FOR_KW),           "^for$"             , 0),
                IF_KW            = new Terminal(nameof(IF_KW),            "^if$"              , 0),
                ELSE_KW          = new Terminal(nameof(ELSE_KW),          "^else$"            , 0),
                L_QB             = new Terminal(nameof(L_QB),             "^{$"               ),
                R_QB             = new Terminal(nameof(R_QB),             "^}$"               ),
                L_B              = new Terminal(nameof(L_B),              "^\\($"             ),
                R_B              = new Terminal(nameof(R_B),              "^\\)$"             ),
                COMMA            = new Terminal(nameof(COMMA),            "^;$"               ),
                COM              = new Terminal(nameof(COM),              "^,$"               ),
                COMMENT          = new Terminal(nameof(COMMENT),          "^\\/\\/.+$"        ),
                HASHSET_ADD      = new Terminal(nameof(HASHSET_ADD),      "^HASHSET_ADD$"     ),
                HASHSET_CONTAINS = new Terminal(nameof(HASHSET_CONTAINS), "^HASHSET_CONTAINS$"),
                HASHSET_REMOVE   = new Terminal(nameof(HASHSET_REMOVE),   "^HASHSET_REMOVE$"  ),
                HASHSET_COUNT    = new Terminal(nameof(HASHSET_COUNT),    "^HASHSET_COUNT$"   ),
                LIST_ADD         = new Terminal(nameof(LIST_ADD),         "^LIST_ADD$"        ),
                LIST_CONTAINS    = new Terminal(nameof(LIST_CONTAINS),    "^LIST_CONTAINS$"   ),
                LIST_REMOVE      = new Terminal(nameof(LIST_REMOVE),      "^LIST_REMOVE$"     ),
                LIST_COUNT       = new Terminal(nameof(LIST_COUNT),       "^LIST_COUNT$"      ),
                /*                                                                            
                 Те терминалы, которые ниже, по-сути нужны парсеру.                           
                 Для того, чтобы проанализировать выражение:                                  
                 a = nameof(Привет) мир!",                                                    
                 чтобы не было так:                                                           
                 a=nameof(Привет)мир!".                                                       
                 */                                                                           
                CH_SPACE         = new Terminal(nameof(CH_SPACE),         "^ $"               ),
                CH_LEFTLINE      = new Terminal(nameof(CH_LEFTLINE),      "^\r$"              ),
                CH_NEWLINE       = new Terminal(nameof(CH_NEWLINE),       "^\n$"              ),
                CH_TAB           = new Terminal(nameof(CH_TAB),           "^\t$"              );
            lexerLang = new LexerLang(new List<Terminal>()
            {
                ASSIGN_OP, VAR, DIGIT, OP, LOGICAL_OP, WHILE_KW, DO_KW, PRINT_KW,
                FOR_KW, IF_KW, ELSE_KW, L_QB, R_QB, L_B, R_B, COMMA, COM, COMMENT,
                HASHSET_ADD, HASHSET_CONTAINS, HASHSET_REMOVE, HASHSET_COUNT, LIST_ADD, LIST_CONTAINS,
                LIST_REMOVE, LIST_COUNT, CH_SPACE, CH_LEFTLINE, CH_NEWLINE, CH_TAB
            });
            Nonterminal expr = new Nonterminal(nameof(expr),
                (List<string> command, ActionInsert insert, int helper) =>
                {
                    insert();
                }, OR, assign_expr, cycle_expr, command_expr);
            Nonterminal lang = new Nonterminal(nameof(lang),
                 (List<string> command, ActionInsert insert, int helper) =>
                 {
                     while (--helper != -1)
                         insert(helper);
                 }, ZERO_AND_MORE, expr);
            
        }
    }
}
