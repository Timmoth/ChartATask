using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WindowsWPF
{
    public abstract class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            this.RaisePropertyChanged(propertyName);
        }

        private void RaisePropertyChanged(string propertyName)
        {
            var propertyChanged = this.PropertyChanged;
            propertyChanged?.Invoke((object)this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (object.Equals((object)field, (object)value))
                return false;
            T obj = field;
            field = value;
            this.OnPropertyChanged(propertyName, (object)obj, (object)value);
            return true;
        }

        [Conditional("DEBUG")]
        private void VerifyProperty(string propertyName)
        {
            this.GetType().GetTypeInfo().GetDeclaredProperty(propertyName);
        }
    }
}