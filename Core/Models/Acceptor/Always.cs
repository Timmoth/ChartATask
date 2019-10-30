namespace ChartATask.Core.Models.Acceptor
{
    public class Always<TValue> : IAcceptor<TValue>
    {
        private readonly bool _value;

        public Always(bool value)
        {
            _value = value;
        }

        public bool Accepts(TValue input)
        {
            return _value;
        }
    }
}