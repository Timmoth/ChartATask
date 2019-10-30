namespace ChartATask.Core.Models.Acceptor
{
    public class Always : IAcceptor<object>
    {
        private readonly bool _value;
        public Always(bool value)
        {
            _value = value;
        }
        public bool Accepts(object input)
        {
            return _value;
        }
    }
}