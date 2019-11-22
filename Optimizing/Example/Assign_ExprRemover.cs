using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MyTypes.Tree;
using Parser;
using Lexer;
using System.Linq;

namespace Optimizing.Example
{
    /// <summary>
    /// Реализация оптимизации удаления лишних присваиваний.
    /// </summary>
    public class Assign_ExprRemover : IOptimizing
    {
        public static readonly Assign_ExprRemover Instance = new Assign_ExprRemover();

        private Assign_ExprRemover() {}

        public ReportParser Optimize(ReportParser compiledCode)
        {
            if(!compiledCode.IsSuccess)
                throw new OptimizingException("Входное дерево компиляции построено не верно!");
            if(compiledCode.Compile == null)
                throw new OptimizingException("Вызовите compiledCode.Compile() перед началом.");
            ITreeNode<object> outputCompile = compiledCode.Compile.CloneCompileTree();

            var assignOpTokens = from a in outputCompile
            where a.Current is Token token && token.Type == Lexer.ExampleLang.OP && token.Value == "="
            select (Token)a.Current;

            foreach(Token t in assignOpTokens)
            {
                t.Value = t.Id + " =";
            }

            IList<string> commands = string.Join(' ', Parser.ExampleLang.Lang.Compile((from a in outputCompile where a.Current is Token t select (Token)a.Current).ToList())).Split(' ');
            HashSet<ulong> TokensToRemove = new OptimizingStackMachine().MyExecute(commands);

            var RPCToRemove = from a in outputCompile where a.Current is ReportParserCompile && a.Count == 3 && a[1].Current is Token && TokensToRemove.Contains(((Token)a[1].Current).Id)
            select a;
            foreach(var rpc in RPCToRemove)
            {
                rpc.Clear();
            }

            return new ReportParser(outputCompile);
        }

        class OptimizingStackMachine : StackMachine.ExampleLang.MyStackLang
        {
            readonly DictionaryHooker myDic;
            readonly HashSet<ulong> ToRemove = new HashSet<ulong>();
            public OptimizingStackMachine(IDictionary<string, double> startVariables = null) : base(null)
            {
                myDic = new DictionaryHooker(startVariables);
                base.Variables = myDic;
                base.commands["="] = _ =>
                {
                    ulong id = ulong.Parse(Stack.Pop());
                    double stmt = PopStk();
                    string var = Stack.Pop();
                    if(!myDic.IsLastSetIsUsed)
                    {
                        ToRemove.Add(id);
                    }
                    if (IsNumber(var))
                        throw new KeyNotFoundException();
                    Variables[var] = stmt;
                };
            }

            public HashSet<ulong> MyExecute(IList<string> code)
            {
                base.Execute(code);
                ToRemove.UnionWith(GetIdsOfLastSet(code));
                return ToRemove;
            }

            public IEnumerable<ulong> GetIdsOfLastSet(IList<string> code)
            {
                IEnumerable<string> ECode = code.Reverse();
                foreach(string var in myDic.IsNotUsed)
                {
                    using IEnumerator<string> EnumCode = ECode.GetEnumerator();
                    while(EnumCode.MoveNext())
                    {
                        if(EnumCode.Current == var)
                        {
                            EnumCode.MoveNext();
                            yield return ulong.Parse(EnumCode.Current);
                            break;
                        }
                    }
                }
            }

            class DictionaryHooker : IDictionary<string, double>
            {
                public DictionaryHooker(IDictionary<string, double> source = null)
                {
                    if(source != null)
                        Source = source;
                    else
                        Source = new Dictionary<string, double>();
                }

                public IDictionary<string, double> Source;
                /// <summary>
                /// Переменные, которые использовались для чтения.
                /// </summary>
                public HashSet<string> IsNotUsed;

                public bool IsLastSetIsUsed { get; private set; } 

                public double this[string key]
                {
                    get
                    {
                        IsNotUsed.Remove(key);
                        return Source[key];
                    }
                    set
                    {
                        IsLastSetIsUsed = !IsNotUsed.Contains(key);
                        IsNotUsed.Add(key);
                        Source[key] = value;
                    }
                }

                public ICollection<string> Keys => Source.Keys;

                public ICollection<double> Values => Source.Values;

                public int Count => Source.Count;

                public bool IsReadOnly => Source.IsReadOnly;

                public void Add(string key, double value)
                {
                    Source.Add(key, value);
                }

                public void Add(KeyValuePair<string, double> item)
                {
                    IsNotUsed.Add(item.Key);
                    Source.Add(item);
                }

                public void Clear()
                {
                    Source.Clear();
                }

                public bool Contains(KeyValuePair<string, double> item)
                {
                    IsNotUsed.Remove(item.Key);
                    return Source.Contains(item);
                }

                public bool ContainsKey(string key)
                {
                    IsNotUsed.Remove(key);
                    return Source.ContainsKey(key);
                }

                public void CopyTo(KeyValuePair<string, double>[] array, int arrayIndex)
                {
                    Source.CopyTo(array, arrayIndex);
                }

                public IEnumerator<KeyValuePair<string, double>> GetEnumerator()
                {
                    return Source.GetEnumerator();
                }

                public bool Remove(string key)
                {
                    return Source.Remove(key);
                }

                public bool Remove(KeyValuePair<string, double> item)
                {
                    return Source.Remove(item);
                }

                public bool TryGetValue(string key, [MaybeNullWhen(false)] out double value)
                {
                    return Source.TryGetValue(key, out value);
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return Source.GetEnumerator();
                }
            }
        }
    }
}