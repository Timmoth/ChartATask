namespace ChartATask.Core.Models.Acceptor
{
    public interface IAcceptor<in TValue>
    {
        bool Accepts(TValue value);
    }
}