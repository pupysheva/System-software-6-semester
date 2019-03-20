namespace Lexer
{
    /// <summary>
    /// Класс, представляющий токен.
    /// </summary>
    public class Token
    {
        public Token(TypeTranslator typeName, string value)
        {
            this.typeName = typeName;
            this.value = value;
        }

        /// <summary>
        /// Тип токена.
        /// </summary>
        public readonly TypeTranslator typeName;

        /// <summary>
        /// Значение токена.
        /// </summary>
        public readonly string value;

    }
}