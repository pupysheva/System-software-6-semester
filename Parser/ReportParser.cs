using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser
{
    public partial class ReportParser : IEnumerable<ParserException>, IList<ParserException>
    {
        /// <summary>
        /// Создание отчёта об ошибках в коде для парсера.
        /// </summary>
        /// <param name="error">Ошибка</param>
        /// <param name="previous">Предыдущий отчёт об ошибках.</param>
        public ReportParser(ParserException error = null, ReportParser previous = null)
        {
            errors = new List<ParserException>();
            if (previous != null)
                AddRange(previous);
            if (error != null)
                Add(error);
        }

        /// <summary>
        /// Создание отчёта об ошибках в коде для парсера.
        /// </summary>
        /// <param name="errors">Список ошибок.</param>
        /// <param name="previous">Предыдущий отчёт об ошибках.</param>
        public ReportParser(IEnumerable<ParserException> errors, ReportParser previous = null)
        {
            if (previous == null)
                if (errors == null)
                    this.errors = new List<ParserException>();
                else
                    this.errors = new List<ParserException>(errors);
            else
            {
                this.errors = new List<ParserException>(previous);
                if (errors != null)
                    AddRange(errors);
            }
        }

        /// <summary>
        /// Создание отчёта об ошибках в коде для парсера.
        /// </summary>
        /// <param name="previous">Предыдущий отчёт об ошибках.</param>
        /// <param name="errors">Список ошибок.</param>
        public ReportParser(ReportParser previous, params ParserException[] errors)
            : this(errors, previous) { }

        protected IList<ParserException> errors;
        private bool _isSuccess = true;

        /// <summary>
        /// Получение номера ошибки.
        /// </summary>
        /// <param name="index">Инджекс ошибки.</param>
        /// <returns>Вощвращает ошибку парсера.</returns>
        public ParserException this[int index] => errors[index];

        /// <summary>
        /// Возвращает true, если ошибки не найдены.
        /// </summary>
        public bool IsSuccess
        {
            get => _isSuccess;
            set
            {
                if (_isSuccess == false && value == true)
                    Add(new ParserException("OK"));
                _isSuccess = value;
            }
        }
        /// <summary>
        /// Возвращает отчёт с флагом SUCCESS.
        /// </summary>
        public static ReportParserReadOnly SUCCESS { get; } = new ReportParserReadOnly();

        public void AddRange(ReportParser report)
        {
            if (IsReadOnly)
                throw new NotSupportedException("Read only report.");
            ((List<ParserException>)errors).AddRange(report);
            if (!report.IsSuccess)
                IsSuccess = false;
        }

        public void AddRange(IEnumerable<ParserException> toAdd)
        {
            if (IsReadOnly)
                throw new NotSupportedException("Read only report.");
            if (toAdd.Count() > 0)
                IsSuccess = false;
            ((List<ParserException>)errors).AddRange(toAdd);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ReportParser))
                return false;
            if (((ReportParser)obj).Count != this.Count)
                return false;
            return Enumerable.SequenceEqual(this, (ReportParser)obj);
        }

        public override int GetHashCode()
        {
            // Число 993615222 сгенерировала VisualStudio
            return 993615222 + EqualityComparer<IList<ParserException>>.Default.GetHashCode(errors);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (IsSuccess)
            {
                sb.Append("Success");
                if (Count > 0)
                    sb.AppendLine(", но в отчёте есть информация:");
            }
            foreach (ParserException o in this)
                sb.AppendLine(o.ToString());
            return sb.ToString();
        }

        #region IList

        public int Count => errors.Count;
        public bool IsReadOnly => errors.IsReadOnly;
        ParserException IList<ParserException>.this[int index] { get => errors[index]; set => errors[index] = value; }
        public IEnumerator<ParserException> GetEnumerator() => errors.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => errors.GetEnumerator();
        public int IndexOf(ParserException item) => errors.IndexOf(item);
        public void Insert(int index, ParserException item) => errors.Insert(index, item);
        public void RemoveAt(int index) => errors.RemoveAt(index);
        public void Add(ParserException item)
        {
            IsSuccess = false;
            errors.Add(item);
        }
        public void Clear() => errors.Clear();
        public bool Contains(ParserException item) => errors.Contains(item);
        public void CopyTo(ParserException[] array, int arrayIndex) => errors.CopyTo(array, arrayIndex);
        public bool Remove(ParserException item) => errors.Remove(item);

        #endregion
    }
}