namespace ChartATask.Core.Models.Acceptor
{
    public class StringEquality : IAcceptor<string>
    {
        private readonly string _comparisonValue;

        public StringEquality(string comparisonValue)
        {
            _comparisonValue = comparisonValue;
        }

        public bool Accepts(string value)
        {
            return _comparisonValue.Equals(value);
        }
    }
}