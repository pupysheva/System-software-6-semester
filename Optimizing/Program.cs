namespace Optimizing
{
    class Program
    {
        static void Main(string[] args)
        {
            var tokensReport = StackMachine.Program.GetParserReportFromUser(args);
            tokensReport.Item2 = Example.AllOptimizing.Instance.Optimize(tokensReport.Item2);
            StackMachine.Program.CompileAndExecute(tokensReport);
        }
    }
}
