namespace ChartATask.Core.Acceptor
{
    public interface IAcceptor<in TValue>
    {
        bool Accepts(TValue input);
    }
}