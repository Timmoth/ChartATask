namespace ChartATask.Core.Triggers.Events.Keyboard
{
    public class KeyPressedEvent : IEvent
    {
        public int KeyCode { get; }

        public KeyPressedEvent(int keyCode)
        {
            KeyCode = keyCode;
        }

        public override string ToString()
        {
            return $"KeyPressed {KeyCode}";
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
}