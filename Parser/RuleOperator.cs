namespace Parser
{
    /// <summary>
    /// Данный класс создан для сопоставления правил
    /// нетерминалов на бумаге с программным кодом.
    /// Чтобы посмотреть, как записываются правила нетерминалов
    /// в оригинале, посетите страницу: <see cref="https://docs.google.com/document/d/1h-KmbeFRwl8RrXxn11LIQvDpanU4vgixCs36xMXBNUU/edit#heading=h.apcvldjdfwkn"/>
    /// </summary>
    enum RuleOperator
    {
        /// <summary>
        /// Соотвествует открытой скобке.
        /// </summary>
        GROUP_START,
        /// <summary>
        /// Соответсвует закрытой скобки.
        /// </summary>
        GROUP_END,
        /// <summary>
        /// Соответсвует знаку +.
        /// </summary>
        ONE_AND_MORE,
        /// <summary>
        /// Соответсвует знаку *.
        /// </summary>
        ZERO_AND_MORE
    }
}