namespace ChartATask.Core.Triggers.Acceptor
{
    public interface IAcceptor<in TValue>
    {
        bool Accepts(TValue input);
    }
}