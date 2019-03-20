namespace Lexer
{
    /// <summary>
    /// Класс, представляющий токен.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Создание экземпляра токена.
        /// </summary>
        /// <param name="type">Транслятор, с помощью которого
        /// был найден токен.</param>
        /// <param name="value">Подстрока, которая была найдена
        /// транслятором.</param>
        public Token(Translator type, string value)
        {
            this.type = type;
            this.value = value;
        }

        /// <summary>
        /// Тип токена.
        /// </summary>
        public readonly Translator type;

        /// <summary>
        /// Значение токена.
        /// </summary>
        public readonly string value;
    }
}