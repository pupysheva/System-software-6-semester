using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser
{
    public partial class ReportParser : IEnumerable<ParserLineReport>, IList<ParserLineReport>
    {
        /// <summary>
        /// Создание отчёта об ошибках в коде для парсера.
        /// </summary>
        /// <param name="error">Ошибка</param>
        /// <param name="previous">Предыдущий отчёт об ошибках.</param>
        public ReportParser(ParserLineReport error = null, ReportParser previous = null)
        {
            errors = new List<ParserLineReport>();
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
        public ReportParser(IEnumerable<ParserLineReport> errors, ReportParser previous = null)
        {
            if (previous == null)
                if (errors == null)
                    this.errors = new List<ParserLineReport>();
                else
                    this.errors = new List<ParserLineReport>(errors);
            else
            {
                this.errors = new List<ParserLineReport>(previous);
                if (errors != null)
                    AddRange(errors);
            }
        }

        /// <summary>
        /// Создание отчёта об ошибках в коде для парсера.
        /// </summary>
        /// <param name="previous">Предыдущий отчёт об ошибках.</param>
        /// <param name="errors">Список ошибок.</param>
        public ReportParser(ReportParser previous, params ParserLineReport[] errors)
            : this(errors, previous) { }

        protected IList<ParserLineReport> errors;
        private bool _isSuccess = true;

        /// <summary>
        /// Получение номера ошибки.
        /// </summary>
        /// <param name="index">Инджекс ошибки.</param>
        /// <returns>Вощвращает ошибку парсера.</returns>
        public ParserLineReport this[int index] => errors[index];

        /// <summary>
        /// Добавить дополнительную информацию для пользователя в отчёт.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        public void AddInfo(string message)
        {
            errors.Add(new ParserLineReport("info: " + message));
        }

        /// <summary>
        /// Присвоить удачу с некоторым сообщением.
        /// </summary>
        /// <param name="message">Сообщение, аргументирующее успех.</param>
        public void Success(string message)
        {
            Add(new ParserLineReport("OK: " + message));
            _isSuccess = true;
        }

        /// <summary>
        /// Возвращает true, если ошибки не найдены.
        /// </summary>
        public bool IsSuccess
        {
            get => _isSuccess;
            set
            {
                if (_isSuccess == false && value == true)
                    Add(new ParserLineReport("OK"));
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
            ((List<ParserLineReport>)errors).AddRange(report);
            if (!report.IsSuccess)
                IsSuccess = false;
        }

        public void AddRange(IEnumerable<ParserLineReport> toAdd)
        {
            if (IsReadOnly)
                throw new NotSupportedException("Read only report.");
            if (toAdd.Count() > 0)
                IsSuccess = false;
            ((List<ParserLineReport>)errors).AddRange(toAdd);
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
            return 993615222 + EqualityComparer<IList<ParserLineReport>>.Default.GetHashCode(errors);
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
            foreach (ParserLineReport o in this)
                sb.AppendLine(o.ToString());
            return sb.ToString();
        }

        #region IList

        public int Count => errors.Count;
        public bool IsReadOnly => errors.IsReadOnly;
        ParserLineReport IList<ParserLineReport>.this[int index] { get => errors[index]; set => errors[index] = value; }
        public IEnumerator<ParserLineReport> GetEnumerator() => errors.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => errors.GetEnumerator();
        public int IndexOf(ParserLineReport item) => errors.IndexOf(item);
        public void Insert(int index, ParserLineReport item) => errors.Insert(index, item);
        public void RemoveAt(int index) => errors.RemoveAt(index);
        public void Add(ParserLineReport item)
        {
            IsSuccess = false;
            errors.Add(item);
        }
        public void Clear() => errors.Clear();
        public bool Contains(ParserLineReport item) => errors.Contains(item);
        public void CopyTo(ParserLineReport[] array, int arrayIndex) => errors.CopyTo(array, arrayIndex);
        public bool Remove(ParserLineReport item) => errors.Remove(item);

        #endregion
    }
}