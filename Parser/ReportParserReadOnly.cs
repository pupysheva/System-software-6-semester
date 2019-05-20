using System.Collections.Generic;

namespace Parser
{
    public class ReportParserReadOnly : ReportParser, IReadOnlyList<ParserLineReport>
    {
        internal ReportParserReadOnly(ReportParser parent = null)
            : base(parent)
        {
            errors = ((List<ParserLineReport>)errors).AsReadOnly();
        }
    }
}