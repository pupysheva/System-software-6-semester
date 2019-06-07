namespace MyTypes.Tree
{
    public enum StringFormat
    {
        /// <summary>
        /// Печатать дерево по-умолчанию, в одну строку.
        /// Поддеревья помечаются квадратными скобками.
        /// </summary>
        Default,
        /// <summary>
        /// Печатать дерево в красивом виде с новыми строками и табуляцией.
        /// </summary>
        NewLine,
        /// <summary>
        /// Вызывает <code>base.ToString();</code>
        /// </summary>
        Base
    }
}
