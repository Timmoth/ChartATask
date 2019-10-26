namespace ChartATask.Models
{
    public class KeyPressedEvent : IInteractionEvent
    {
        public readonly int KeyCode;

        public KeyPressedEvent(int _KeyCode)
        {
            KeyCode = _KeyCode;
        }

        public override string ToString()
        {
            return $@"KeyPressed {KeyCode}";
        }

        public override bool Equals(object obj)
        {
            return obj is KeyPressedEvent keyPressed && keyPressed.KeyCode == KeyCode;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }

    public interface IInteractionEvent
    {
    }
}