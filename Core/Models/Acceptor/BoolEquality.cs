namespace ChartATask.Core.Models.Acceptor
{
    public class BoolEquality : IAcceptor<bool>
    {
        private readonly bool _value;

        public BoolEquality(bool value)
        {
            _value = value;
        }

        public bool Accepts(bool input)
        {
            return _value.Equals(input);
        }
    }
}