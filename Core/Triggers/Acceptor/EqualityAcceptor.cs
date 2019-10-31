namespace ChartATask.Core.Triggers.Acceptor
{
    public class EqualityAcceptor<TValue> : IAcceptor<TValue>
    {
        private readonly TValue _value;

        public EqualityAcceptor(TValue value)
        {
            _value = value;
        }

        public bool Accepts(TValue input)
        {
            return _value.Equals(input);
        }
    }
}