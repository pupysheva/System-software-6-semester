using System;

namespace Parser
{
    public class ReportParser
    {
        /// <summary>
        /// Представляет информацию о ходе поиска ошибок в коде.
        /// </summary>
        public readonly ReportParserInfo Info
            = new ReportParserInfo();
        /// <summary>
        /// Представляет информацию о компиляции.
        /// </summary>
        public readonly ReportParserCompile Compile
            = new ReportParserCompile();

        /// <summary>
        /// Возвращает true, если ошибки не найдены.
        /// </summary>
        public bool IsSuccess =>
            Info.IsSuccess;

        public void Merge(ReportParser reportParser)
        {
            reportParser.Compile.AddRange(reportParser.Compile);
            reportParser.Info.AddRange(reportParser.Info);
        }
    }
}
