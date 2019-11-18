using MyTypes;
using MyTypes.LinkedList;
using System;
using System.Collections.Generic;
using System.Text;
using static Lexer.ExampleLang;

namespace StackMachine
{
    public static class ExampleLang
    {
        public static readonly MyStackLang stackMachine = new MyStackLang();

        public class MyStackLang : AbstractStackExecuteLang
        {
            /// <summary>
            /// Создаёт новый экземпляр стековой машины.
            /// </summary>
            /// <param name="startVariables">Реализация таблицы переменных.</param>
            public MyStackLang(IDictionary<string, double> startVariables = null)
                : base(startVariables)
            { }

            private readonly Dictionary<string, Action<string, MyStackLang>> commands = new Dictionary<string, Action<string, MyStackLang>>()
            {
                ["print"] = (cmd, _) =>
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var pair in _.Variables)
                    {
                        sb.Append(pair.Key);
                        sb.Append(" = ");
                        sb.Append(pair.Value);
                        sb.AppendLine();
                    }
                    Console.Write(sb.ToString());
                },
                ["goto!"] = (cmd, _) =>
                {
                    _.InstructionPointer = (int)_.PopStk() - 1;
                },
                ["!f"] = (cmd, _) =>
                {
                    int addr = (int)_.PopStk();
                    int logical = (int)_.PopStk();
                    if (logical == 0) // Если ложь, то пропускаем body.
                        _.InstructionPointer = addr - 1;
                },
                ["="] = (cmd, _) =>
                {
                    double stmt = _.PopStk();
                    string var = _.Stack.Pop();
                    if (IsNumber(var))
                        throw new KeyNotFoundException();
                    _.Variables[var] = stmt;
                },
                ["+"] = (cmd, _) =>
                {
                    _.Stack.Push(
                        (_.PopStk() + _.PopStk())
                        .ToString());
                },
                ["-"] = (cmd, _) =>
                {
                    double b = _.PopStk();
                    double a = _.PopStk();
                    _.Stack.Push(
                        (a - b)
                        .ToString());
                },
                ["*"] = (cmd, _) =>
                {
                    _.Stack.Push(
                        (_.PopStk() * _.PopStk())
                        .ToString());
                },
                ["/"] = (cmd, _) =>
                {
                    double b = _.PopStk();
                    double a = _.PopStk();
                    _.Stack.Push(
                        (a / b)
                        .ToString());
                },
                [">"] = (cmd, _) =>
                {
                    double b = _.PopStk();
                    double a = _.PopStk();
                    _.Stack.Push(
                        (a > b)
                        ? "1" : "0");
                },
                ["<"] = (cmd, _) =>
                {
                    double b = _.PopStk();
                    double a = _.PopStk();
                    _.Stack.Push(
                        (a < b)
                        ? "1" : "0");
                },
                ["=="] = (cmd, _) =>
                {
                    _.Stack.Push(
                        (_.PopStk() == _.PopStk())
                        ? "1" : "0");
                },
                ["!="] = (cmd, _) =>
                {
                    _.Stack.Push(
                        (_.PopStk() != _.PopStk())
                        ? "1" : "0");
                },
                [HASHSET_ADD.Name] = (cmd, _) =>
                {
                    double buffer = _.PopStk();
                    _.Stack.Push(_.set.Add(buffer) ? "1" : "0");
                },
                [HASHSET_CONTAINS.Name] = (cmd, _) =>
                {
                    double buffer = _.PopStk();
                    _.Stack.Push(_.set.Contains(buffer) ? "1" : "0");
                },
                [HASHSET_COUNT.Name] = (cmd, _) =>
                {
                    _.Stack.Push(_.set.Count.ToString());
                },
                [HASHSET_REMOVE.Name] = (cmd, _) =>
                {
                    double buffer = _.PopStk();
                    _.Stack.Push(_.set.Remove(buffer) ? "1" : "0");
                },
                ["?"] = (cmd, _) =>
                {
                    throw new NotImplementedException();
                },
                [LIST_ADD.Name] = (cmd, _) =>
                {
                    double buffer = _.PopStk();
                    _.list.Add(buffer);
                    _.Stack.Push("1");
                },
                [LIST_CONTAINS.Name] = (cmd, _) =>
                {
                    double buffer = _.PopStk();
                    _.Stack.Push(_.list.Contains(buffer) ? "1" : "0");
                },
                [LIST_COUNT.Name] = (cmd, _) =>
                {
                    _.Stack.Push(_.list.Count.ToString());
                },
                [LIST_REMOVE.Name] = (cmd, _) =>
                {
                    double buffer = _.PopStk();
                    _.Stack.Push(_.list.Remove(buffer) ? "1" : "0");
                },
                [null] = (cmd, _) => // Объявлена новая переменная.
                {
                    if (!_.Variables.ContainsKey(cmd) && !IsNumber(cmd))
                        _.Variables[cmd] = 0;
                    _.Stack.Push(cmd);
                }
            };

            public readonly ICollection<double> list
                = new MyLinkedList<double>();
            public readonly ISet<double> set
                = new MyHashSet<double>();

            protected override void ExecuteCommand(string command)
            {
                commands.GetValueOrDefault(command, commands[null]).Invoke(command, this);
            }

            /// <summary>
            /// Получает с стека значение и вызывает <see cref="GetValueOfVarOrDigit(string)"/>.
            /// </summary>
            private double PopStk() => GetValueOfVarOrDigit(Stack.Pop());

            private static bool IsNumber(string str)
                => double.TryParse(str, out double _);

            private double GetValueOfVarOrDigit(string VarOrDigit)
            {
                if (double.TryParse(VarOrDigit, out double result))
                    return result;
                return Variables[VarOrDigit];
            }
        }
    }
}
