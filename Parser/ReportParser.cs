using System;
using System.Collections;
using System.Collections.Generic;

namespace Parser
{
    public class ReportParser : IEnumerable<ParserException>
    {
        /// <summary>
        /// Создание отчёта об ошибках в коде для парсера.
        /// </summary>
        /// <param name="errors">Список ошибок.</param>
        public ReportParser(IEnumerable<ParserException> errors)
            => this.errors = new List<ParserException>(errors)
            ?? throw new ArgumentNullException();

        private List<ParserException> errors;

        /// <summary>
        /// Получение номера ошибки.
        /// </summary>
        /// <param name="index">Инджекс ошибки.</param>
        /// <returns>Вощвращает ошибку парсера.</returns>
        public ParserException this[int index] => errors[index];

        /// <summary>
        /// Возвращает true, если ошибки не найдены.
        /// </summary>
        public bool IsSuccess => errors.Count == 0;

        /// <summary>
        /// Возвращает количество ошибок.
        /// </summary>
        public int Length => errors.Count;

        public IEnumerator<ParserException> GetEnumerator()
            => errors.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => errors.GetEnumerator();
    }
}