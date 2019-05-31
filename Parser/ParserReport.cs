using Parser.Tree;

namespace Parser
{
    public class ReportParser
    {
        public ReportParser(ReportParserCompile Compile = null)
        {
            this.Compile = new TreeNode<object>(Compile);
        }

        public ReportParser(ITreeNode<object> Compile)
        {
            this.Compile = Compile;
        }

        /// <summary>
        /// Возвращает true, если ошибки не найдены.
        /// </summary>
        public bool IsSuccess =>
            Info.IsSuccess;

        /// <summary>
        /// Представляет информацию о компиляции.
        /// </summary>
        public ITreeNode<object> Compile { get; protected set; }

        /// <summary>
        /// Представляет информацию о ходе поиска ошибок в коде.
        /// </summary>
        public ReportParserInfo Info { get; } = new ReportParserInfo();

        public void Merge(ReportParser reportParser)
        {
            if (Compile != null)
            {
                if (reportParser.Compile != null)
                    Compile.Add(reportParser.Compile);
                else
                    Info.AddInfo($"Не удалось добавить информацию компиляции к информации: {this.Compile}");
            }
            else
                Compile = reportParser.Compile;
            Info.AddRange(reportParser.Info);
        }

        /// <summary>
        /// Удаляет или отменяет все заметки о компиляции в данном отчёте.
        /// </summary>
        public void CompileCancel()
        {
            Compile = null;
        }
    }
}
