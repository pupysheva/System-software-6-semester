using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Lexer
{
    /// <summary>
    /// Представление терминала.
    /// </summary>
    public class Terminal
    {
        /// <summary>
        /// Создание экземпляра терминала.
        /// </summary>
        /// <param name="Name">Имя терминала.</param>
        /// <param name="RegularExpression">Регулярное
        /// выражение терминала.</param>
        public Terminal(string Name, string RegularExpression)
            : this(Name, new Regex(RegularExpression)) { }

        /// <summary>
        /// Создание экземпляра терминала.
        /// </summary>
        /// <param name="Name">Имя терминала.</param>
        /// <param name="RegularExpression">Регулярное
        /// выражение терминала.</param>
        public Terminal(string Name, Regex RegularExpression)
        {
            this.Name = Name;
            this.RegularExpression = RegularExpression;
        }

        /// <summary>
        /// Имя терминала.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Регулярное выражение, которое соответсвует данному
        /// терминалу.
        /// </summary>
        public readonly Regex RegularExpression;

        /// <summary>
        /// Определяет, является ли текущий объект эквивалентным входому.
        /// Стоит отметить, что идёт сравнение только по именам, так как
        /// предполагается, что имена уникальны.
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
            if (!(translator is Terminal))
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