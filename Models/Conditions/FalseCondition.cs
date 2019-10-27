namespace ChartATask.Models.Conditions
{
    public class FalseCondition : ICondition
    {

        public FalseCondition()
        {
          
        }

        public bool Check(IEvaluator evaluator)
        {
            return false;
        }
    }
}