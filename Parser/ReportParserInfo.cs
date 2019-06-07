using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser
{
    public class ReportParserInfo : IEnumerable<ReportParserInfoLine>, IList<ReportParserInfoLine>
    {
        /// <summary>
        /// Создание отчёта об ошибках в коде для парсера.
        /// </summary>
        /// <param name="error">Ошибка</param>
        /// <param name="previous">Предыдущий отчёт об ошибках.</param>
        public ReportParserInfo(ReportParserInfoLine error = null, ReportParserInfo previous = null)
        {
            errors = new List<ReportParserInfoLine>();
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
        public ReportParserInfo(IEnumerable<ReportParserInfoLine> errors, ReportParserInfo previous = null)
        {
            if (previous == null)
                if (errors == null)
                    this.errors = new List<ReportParserInfoLine>();
                else
                    this.errors = new List<ReportParserInfoLine>(errors);
            else
            {
                this.errors = new List<ReportParserInfoLine>(previous);
                if (errors != null)
                    AddRange(errors);
            }
        }

        /// <summary>
        /// Создание отчёта об ошибках в коде для парсера.
        /// </summary>
        /// <param name="previous">Предыдущий отчёт об ошибках.</param>
        /// <param name="errors">Список ошибок.</param>
        public ReportParserInfo(ReportParserInfo previous, params ReportParserInfoLine[] errors)
            : this(errors, previous) { }

        protected IList<ReportParserInfoLine> errors;
        private bool _isSuccess = true;

        /// <summary>
        /// Получение номера ошибки.
        /// </summary>
        /// <param name="index">Инджекс ошибки.</param>
        /// <returns>Вощвращает ошибку парсера.</returns>
        public ReportParserInfoLine this[int index] => errors[index];

        /// <summary>
        /// Добавить дополнительную информацию для пользователя в отчёт.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        public void AddInfo(string message)
        {
            errors.Add(new ReportParserInfoLine("info: " + message));
        }

        /// <summary>
        /// Присвоить удачу с некоторым сообщением.
        /// </summary>
        /// <param name="message">Сообщение, аргументирующее успех.</param>
        public void Success(string message)
        {
            Add(new ReportParserInfoLine("OK: " + message));
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
                    Add(new ReportParserInfoLine("OK"));
                _isSuccess = value;
            }
        }
        /// <summary>
        /// Возвращает отчёт с флагом SUCCESS.
        /// </summary>
        public static ReportParserReadOnly SUCCESS { get; } = new ReportParserReadOnly();

        public void AddRange(ReportParserInfo report)
        {
            if (IsReadOnly)
                throw new NotSupportedException("Read only report.");
            ((List<ReportParserInfoLine>)errors).AddRange(report);
            if (!report.IsSuccess)
                IsSuccess = false;
        }

        public void AddRange(IEnumerable<ReportParserInfoLine> toAdd)
        {
            if (IsReadOnly)
                throw new NotSupportedException("Read only report.");
            if (toAdd.Count() > 0)
                IsSuccess = false;
            ((List<ReportParserInfoLine>)errors).AddRange(toAdd);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ReportParserInfo))
                return false;
            if (((ReportParserInfo)obj).Count != this.Count)
                return false;
            return Enumerable.SequenceEqual(this, (ReportParserInfo)obj);
        }

        public override int GetHashCode()
        {
            // Число 993615222 сгенерировала VisualStudio
            return 993615222 + EqualityComparer<IList<ReportParserInfoLine>>.Default.GetHashCode(errors);
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
            foreach (ReportParserInfoLine o in this)
                sb.AppendLine(o.ToString());
            return sb.ToString();
        }

        #region IList

        public int Count => errors.Count;
        public bool IsReadOnly => errors.IsReadOnly;
        ReportParserInfoLine IList<ReportParserInfoLine>.this[int index] { get => errors[index]; set => errors[index] = value; }
        public IEnumerator<ReportParserInfoLine> GetEnumerator() => errors.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => errors.GetEnumerator();
        public int IndexOf(ReportParserInfoLine item) => errors.IndexOf(item);
        public void Insert(int index, ReportParserInfoLine item) => errors.Insert(index, item);
        public void RemoveAt(int index) => errors.RemoveAt(index);
        public void Add(ReportParserInfoLine item)
        {
            IsSuccess = false;
            errors.Add(item);
        }
        public void Clear() => errors.Clear();
        public bool Contains(ReportParserInfoLine item) => errors.Contains(item);
        public void CopyTo(ReportParserInfoLine[] array, int arrayIndex) => errors.CopyTo(array, arrayIndex);
        public bool Remove(ReportParserInfoLine item) => errors.Remove(item);

        #endregion
    }
}