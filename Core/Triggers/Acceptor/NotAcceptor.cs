namespace ChartATask.Core.Triggers.Acceptor
{
    public class NotAcceptor<TValue> : IAcceptor<TValue>
    {
        private readonly IAcceptor<TValue> _acceptor;

        public NotAcceptor(IAcceptor<TValue> acceptor)
        {
            _acceptor = acceptor;
        }

        public bool Accepts(TValue input)
        {
            return !_acceptor.Accepts(input);
        }
    }
}