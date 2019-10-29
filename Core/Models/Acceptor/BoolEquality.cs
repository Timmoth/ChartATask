namespace ChartATask.Core.Models.Acceptor
{
    public class BoolEquality : IAcceptor<bool>
    {
        private readonly bool _comparisonValue;

        public BoolEquality(bool comparisonValue)
        {
            _comparisonValue = comparisonValue;
        }

        public bool Accepts(bool value)
        {
            return _comparisonValue.Equals(value);
        }
    }
}