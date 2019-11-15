using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WindowsWPF
{
    public abstract class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) => RaisePropertyChanged(propertyName);

        private void RaisePropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value))
            {
                return false;
            }

            var obj = field;
            field = value;
            OnPropertyChanged(propertyName, obj, value);
            return true;
        }

        [Conditional("DEBUG")]
        private void VerifyProperty(string propertyName) => GetType().GetTypeInfo().GetDeclaredProperty(propertyName);
    }
}