namespace Lexer
{
    /// <summary>
    /// Класс, представляющий жетон.
    /// </summary>
    public class Token : Token<string>
    {
        public Token(Terminal Type, string Value) : base(Type, Value) { }
    }

    public class Token<T>
    {
        /// <summary>
        /// Создание экземпляра жетона.
        /// </summary>
        /// <param name="Type">Транслятор, с помощью которого
        /// был найден жетон.</param>
        /// <param name="Value">Подстрока, которая была найдена
        /// транслятором.</param>
        public Token(Terminal Type, T Value)
        {
            this.Type = Type;
            this.Value = Value;
        }

        /// <summary>
        /// Тип жетона.
        /// </summary>
        public Terminal Type { get; }

        /// <summary>
        /// Значение жетона.
        /// </summary>
        public T Value { get; }

        public override string ToString()
        {
            return $"{nameof(Token<T>)} {Type}: {Value}";
        }
    }
}