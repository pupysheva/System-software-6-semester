namespace Lexer
{
    /// <summary>
    /// Класс, представляющий токен.
    /// </summary>
    public class Token : Token<string>
    {
        public Token(Terminal Type, string Value) : base(Type, Value) { }
    }

    public class Token<T>
    {
        /// <summary>
        /// Создание экземпляра токена.
        /// </summary>
        /// <param name="Type">Транслятор, с помощью которого
        /// был найден токен.</param>
        /// <param name="Value">Подстрока, которая была найдена
        /// транслятором.</param>
        public Token(Terminal Type, T Value)
        {
            this.Type = Type;
            this.Value = Value;
        }

        /// <summary>
        /// Тип токена.
        /// </summary>
        public Terminal Type { get; }

        /// <summary>
        /// Значение токена.
        /// </summary>
        public T Value { get; }

        public override string ToString()
        {
            return $"{Type}: {Value}";
        }
    }
}