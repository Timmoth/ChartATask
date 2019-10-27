namespace ChartATask.Models.Conditions
{
    public class TrueCondition : ICondition
    {

        public TrueCondition()
        {
          
        }

        public bool Check(IEvaluator evaluator)
        {
            return true;
        }
    }
}