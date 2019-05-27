using System;

namespace Parser
{
    public class ReportParser
    {
        public ReportParser(ReportParserCompile Compile = null)
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
        public ReportParserCompile Compile { get; set; }

        /// <summary>
        /// Представляет информацию о ходе поиска ошибок в коде.
        /// </summary>
        public ReportParserInfo Info { get; } = new ReportParserInfo();

        /// <param name="id">Ид рассматриваемого элемента грамматики в нетерминале.</param>
        public void Merge(ReportParser reportParser, int id = int.MinValue)
        {
            if (this.Compile != null)
                if (id == int.MinValue)
                    throw new ArgumentNullException("Корень дерева уже занят, необходимо указать id.");
                else if(reportParser.Compile != null)
                    this.Compile.deepList[id] = reportParser.Compile;
            else
                this.Compile = reportParser.Compile;
            this.Info.AddRange(reportParser.Info);
        }

        internal void CompileCancel()
        {
            Compile = null;
        }
    }
}
