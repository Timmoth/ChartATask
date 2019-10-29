namespace ChartATask.Core.Models.Acceptor
{
    public class StringContains : IAcceptor<string>
    {
        private readonly string _comparisonValue;

        public StringContains(string comparisonValue)
        {
            _comparisonValue = comparisonValue;
        }

        public bool Accepts(string value)
        {
            return value.Contains(_comparisonValue);
        }
    }
}