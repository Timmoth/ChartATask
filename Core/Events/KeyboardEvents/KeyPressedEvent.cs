namespace ChartATask.Core.Events.KeyboardEvents
{
    public class KeyPressedEvent : IEvent
    {
        public readonly int KeyCode;

        public KeyPressedEvent(int keyCode)
        {
            KeyCode = keyCode;
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
}