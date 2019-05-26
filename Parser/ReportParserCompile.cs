using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class ReportParserCompile : IList<ReportParserCompileLine>
    {
        private IList<ReportParserCompileLine> list;

        public ReportParserCompile(IList<ReportParserCompileLine> listManager)
        {
            list = listManager;
        }

        public ReportParserCompile(IEnumerable<ReportParserCompileLine> toAdd = null)
        {
            if (toAdd != null)
                list = new List<ReportParserCompileLine>(toAdd);
            else
                list = new List<ReportParserCompileLine>();
        }

        #region IList

        public ReportParserCompileLine this[int index] { get => list[index]; set => list[index] = value; }

        public int Count
            => list.Count;

        public bool IsReadOnly
            => list.IsReadOnly;

        public void Add(ReportParserCompileLine item)
            => list.Add(item);

        public void Clear()
            => list.Clear();

        public bool Contains(ReportParserCompileLine item)
            => list.Contains(item);

        public void CopyTo(ReportParserCompileLine[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ReportParserCompileLine> GetEnumerator()
            => list.GetEnumerator();

        public int IndexOf(ReportParserCompileLine item)
            => list.IndexOf(item);

        public void Insert(int index, ReportParserCompileLine item)
            => list.Insert(index, item);

        public bool Remove(ReportParserCompileLine item)
            => list.Remove(item);

        public void RemoveAt(int index)
            => list.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator()
            => list.GetEnumerator();

        #endregion IList
    }
}
