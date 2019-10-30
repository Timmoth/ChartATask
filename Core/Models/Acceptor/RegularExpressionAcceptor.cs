using System.Text.RegularExpressions;

namespace ChartATask.Core.Models.Acceptor
{
    public class RegularExpressionAcceptor : IAcceptor<object>
    {
        private readonly Regex _regularExpression;

        public RegularExpressionAcceptor(string regularExpression)
        {
            _regularExpression = new Regex(regularExpression, RegexOptions.Compiled);
        }

        public bool Accepts(object input)
        {
            return _regularExpression.IsMatch(input.ToString());
        }
    }
}