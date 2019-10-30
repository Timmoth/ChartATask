namespace ChartATask.Core.Models.Acceptor
{
    public class StringAny : IAcceptor<string>
    {
        public bool Accepts(string value)
        {
            return true;
        }
    }
}