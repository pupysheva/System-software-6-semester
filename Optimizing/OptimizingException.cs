namespace Optimizing
{
    [System.Serializable]
    public class OptimizingException : System.Exception
    {
        public OptimizingException() { }
        public OptimizingException(string message) : base(message) { }
        public OptimizingException(string message, System.Exception inner) : base(message, inner) { }
        protected OptimizingException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}