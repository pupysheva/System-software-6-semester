using System.Collections.Generic;

namespace Parser
{
    public class ReportParserReadOnly : ReportParser, IReadOnlyList<ParserException>
    {
        internal ReportParserReadOnly(ReportParser parent = null)
            : base(parent)
        {
            errors = ((List<ParserException>)errors).AsReadOnly();
        }
    }
}