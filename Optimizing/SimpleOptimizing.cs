using Parser;
using Lexer;
using System.Linq;
using StackMachine;
using System.Collections.Generic;
using MyTypes.Tree;

namespace Optimizing
{
    public class SimpleOptimizing : IOptimizing
    {
        /// <summary>
        /// Получает готовый экземпляр <see cref="SimpleOptimizing"/> из кэша.
        /// </summary>
        public static readonly SimpleOptimizing Instance = new SimpleOptimizing();

        private SimpleOptimizing() {}

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
            ITreeNode<object> outputCompile = compiledCode.Compile.DeepClone(obj =>
            {
                switch(obj)
                {
                    case Token t:
                        return t;//new Token(t.Type, t.Value);
                    case ReportParserCompile rpc:
                        return rpc;//new ReportParserCompile(rpc.Source, rpc.CurrentRule, rpc.Helper);
                    default:
                        throw new OptimizingException($"В дереве компиляции встретился неизвестный тип: {obj.GetType()}");
                }
            });
            var assign_exprs = from a in outputCompile
                where a.Current is ReportParserCompile rpc && rpc.Source.Name == "assign_expr"
                select a;

            var VarsOnLeftOfAssign_expr = from a in assign_exprs select a[0];

            var allVars = from a in outputCompile
                where a.Current is Token t && t.Type.Name == "VAR"
                select a;
            
            var VarsOnRightOfAssign_expr = allVars.Except(VarsOnLeftOfAssign_expr);

            Dictionary<string, double> varsValues = new Dictionary<string, double>();

            foreach(var assign_expr in assign_exprs)
                if(tryCalculate(assign_expr, varsValues))
                    assign_expr[2].Current =
                        new Token(Lang.DIGIT, varsValues[((Token)assign_expr[0].Current).Value].ToString());
            return new ReportParser(outputCompile);
        }

        bool tryCalculate(ITreeNode<object> assign_expr, Dictionary<string, double> varsValues)
        {
            IEnumerable<string> commands = Lang.parserLang.Compile((from a in assign_expr where a.Current is Token select (Token)a.Current).ToList(),
                new ReportParser(assign_expr));
            Dictionary<string, double> toSend = new Dictionary<string, double>(varsValues);
            foreach(string varName in from a in assign_expr where a.Current is Token tk && tk.Type.Name == "VAR" && !toSend.ContainsKey(tk.Value) select ((Token)a.Current).Value)
            {
                toSend[varName] = double.NaN;
            }
            Lang.MyStackLang localStackMachine = new Lang.MyStackLang(toSend);
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