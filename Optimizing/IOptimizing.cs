using Parser;

namespace Optimizing
{
    /// <summary>
    /// Интерфейс говорит, что умеет оптимизировать лексическое дерево.
    /// </summary>
    public interface IOptimizing
    {
        /// <summary>
        /// Оптимизирует входное дерево компиляции.
        /// </summary>
        /// <param name="input">Входное дерево компиляции.</param>
        /// <returns>Оптимизированное дерево компиляции.</returns>
        ReportParser Optimize(ReportParser compiledCode);
    }
}