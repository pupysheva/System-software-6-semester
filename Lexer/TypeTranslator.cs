using System.Collections.Generic;

namespace Lexer
{
    /// <summary>
    /// Представление транслятора.
    /// </summary>
    public abstract class Translator
    {
        /// <summary>
        /// Имя транслятора.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Отвечает на вопрос, является ли входная строка
        /// полходящей под данный транслятор.
        /// </summary>
        /// <param name="expression">Входная строка, которую
        /// необходимо проверить транслятором.</param>
        /// <returns><code>true</code>, если входная строка
        /// подходит к этому транслятору. Иначе - <code>false</code>.</returns>
        public abstract bool Match(string expression);

        /// <summary>
        /// Определяет, является ли текущий объект эквивалентным входому.
        /// </summary>
        /// <param name="translator">Объект, который сравнивается
        /// с текущим. Если отправить null, то функция вернёт
        /// <code>false</code>.</param>
        /// <returns><code>true</code>, если объекты эквивалентны.
        /// Иначе - <code>false</code>.</returns>
        public override bool Equals(object translator)
        {
            if (translator == null)
                return false;
            if (!(translator is Translator))
                return false;
            return Name.Equals(translator);
        }

        public override int GetHashCode()
        {
            // 539060726 - visual studio сгенерировала.
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}