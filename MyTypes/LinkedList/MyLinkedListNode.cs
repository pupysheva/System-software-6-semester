namespace MyTypes.LinkedList
{
    /// <summary>
    /// Представляет узел в <see cref="MyLinkedList{T}"/>.
    /// </summary>
    /// <typeparam name="T">Указывает тип элементов связанного списка.</typeparam>
    public class MyLinkedListNode<T>
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="MyLinkedListNode{T}"/> класс,
        /// содержащий указанное значение.
        /// </summary>
        /// <param name="value">Значение, которое должно содержаться в <see cref="MyLinkedListNode{T}"/>.</param>
        public MyLinkedListNode(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Инициализирует новый экзампляр <see cref="MyLinkedList{T}"/> класс,
        /// содержащий указанное значение и связанный с конкретным листом.
        /// </summary>
        /// <param name="list">Лист, в котором создаётся узел.</param>
        /// <param name="previous">Предыдущий узел по отношению к создаваемому узлу.</param>
        /// <param name="next">Следующий узел по отношению к создаваемому узлу.</param>
        /// <param name="value">Значение, которое должно содержаться в <see cref="MyLinkedListNode{T}"/>.</param>
        internal MyLinkedListNode(MyLinkedList<T> list, MyLinkedListNode<T> previous, MyLinkedListNode<T> next, T value = default(T)) : this(value)
        {
            List = list;
            Previous = previous;
            Next = next;
        }

        /// <summary>
        /// Возвращает <see cref="MyLinkedList{T}"/> которому принадлежит или null если <see cref="MyLinkedListNode{T}"/> не
        /// связан.
        /// </summary>
        public MyLinkedList<T> List { get; }

        /// <summary>
        /// Возвращает следующий узел <see cref="MyLinkedList{T}"/>.
        /// Или null если текущий узел является последним элементом (<see cref="MyLinkedList{T}.Last"/>)
        /// из <see cref="MyLinkedList{T}"/>.
        /// </summary>
        public MyLinkedListNode<T> Next { get; internal set; }

        /// <summary>
        /// Возвращает предыдущий узел <see cref="MyLinkedList{T}"/>.
        /// Ссылка на предыдущий узел в <see cref="MyLinkedList{T}"/>, или null
        /// если текущий узел является первым элементом (<see cref="MyLinkedList{T}.First"/>)
        /// из <see cref="MyLinkedList{T}"/>.
        /// </summary>
        public MyLinkedListNode<T> Previous { get; internal set; }

        /// <summary>
        /// Возвращает значение, содержащееся в узле.
        /// </summary>
        public T Value { get; set; }
    }
}
