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
        /// <param name="Type">Транслятор, с помощью которого
        /// был найден токен.</param>
        /// <param name="Value">Подстрока, которая была найдена
        /// транслятором.</param>
        public Token(Terminal Type, object Value)
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
        public virtual object Value { get; }
    }

    public class Token<T> : Token
    {
        public Token(Terminal Type, T Value) : base(Type, Value) { }

        /// <summary>
        /// Значение токена.
        /// </summary>
        public override T Value { get; }
    }
}