using System.Collections.Generic;

namespace Parser
{
    public class ReportParserReadOnly : ReportParserInfo, IReadOnlyList<ReportParserInfoLine>
    {
        internal ReportParserReadOnly(ReportParserInfo parent = null)
            : base(parent)
        {
            errors = ((List<ReportParserInfoLine>)errors).AsReadOnly();
        }
    }
}