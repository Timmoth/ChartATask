using System;

namespace ChartATask.Models.Conditions
{
    public class TimeCondition : ICondition
    {
        private readonly ComparisonOperator _comparisonOperator;
        private readonly DateTime _dateTime;

        public TimeCondition(DateTime dateTime, ComparisonOperator comparisonOperator)
        {
            _dateTime = dateTime;
            _comparisonOperator = comparisonOperator;
        }

        public bool Check(IEvaluator evaluator)
        {
            switch (_comparisonOperator)
            {
                case ComparisonOperator.GreaterThen:
                    return _dateTime > DateTime.Now;
                case ComparisonOperator.GreaterThenEqualTo:
                    return _dateTime >= DateTime.Now;
                case ComparisonOperator.EqualTo:
                    return _dateTime == DateTime.Now;
                case ComparisonOperator.LessThenEqualTo:
                    return _dateTime <= DateTime.Now;
                case ComparisonOperator.LessThen:
                    return _dateTime < DateTime.Now;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}