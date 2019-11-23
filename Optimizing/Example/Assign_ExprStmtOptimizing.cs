using Parser;
using Lexer;
using System.Linq;
using System.Collections.Generic;
using MyTypes.Tree;
using System;

namespace Optimizing.Example
{
    /// <summary>
    /// Заранее вычисляет stmt и присваивает значения переменным.
    /// </summary>
    public class Assign_ExprStmtOptimizing : IOptimizing
    {
        /// <summary>
        /// Получает готовый экземпляр <see cref="Assign_ExprStmtOptimizing"/> из кэша.
        /// </summary>
        public static readonly Assign_ExprStmtOptimizing Instance = new Assign_ExprStmtOptimizing();
        private static readonly Random ran = new Random();

        private Assign_ExprStmtOptimizing() {}

        /// <summary>
        /// Оптимизирует входное дерево компиляции.
        /// </summary>
        /// <param name="input">Входное дерево компиляции.</param>
        /// <returns>Оптимизированное дерево компиляции.</returns>
        public ReportParser Optimize(ReportParser compiledCode)
        {
            if(!compiledCode.IsSuccess)
                throw new OptimizingException("Входное дерево компиляции построено не верно!");
            if(compiledCode.Compile == null)
                throw new OptimizingException("Вызовите compiledCode.Compile() перед началом.");
            ITreeNode<object> outputCompile = compiledCode.Compile.CloneCompileTree();

            var assign_exprs = from a in outputCompile
                where a.Current is ReportParserCompile rpc && rpc.Source == Parser.ExampleLang.assign_expr
                select a;

            Dictionary<string, double> varsValues = new Dictionary<string, double>();

            foreach(var assign_expr in assign_exprs)
                if(TryCalculate(assign_expr, varsValues))
                    assign_expr[2].Current =
                        new Token(Lexer.ExampleLang.DIGIT, varsValues[((Token)assign_expr[0].Current).Value].ToString(), ran.NextULong());
            return new ReportParser(outputCompile);
        }

        private bool TryCalculate(ITreeNode<object> assign_expr, Dictionary<string, double> varsValues)
        {
            IEnumerable<string> commands = Parser.ExampleLang.Lang.Compile((from a in assign_expr where a.Current is Token t select (Token)a.Current).ToList(),
                new ReportParser(assign_expr));
            Dictionary<string, double> toSend = new Dictionary<string, double>(varsValues);
            foreach(string varName in from a in assign_expr where a.Current is Token tk && tk.Type == Lexer.ExampleLang.VAR && !toSend.ContainsKey(tk.Value) select ((Token)a.Current).Value)
            {
                toSend[varName] = double.NaN;
            }
            StackMachine.ExampleLang.MyStackLang localStackMachine = new StackMachine.ExampleLang.MyStackLang(toSend);
            try
            {
                localStackMachine.Execute(commands);
            }
            catch { return false; }
            varsValues[((Token)assign_expr[0].Current).Value] = toSend.GetValueOrDefault(((Token)assign_expr[0].Current).Value, double.NaN);
            return true;
        }
    }
}