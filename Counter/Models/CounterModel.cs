using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Counter
{
    public class CounterModel : INotifyPropertyChanged
    {
        private int _value;

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        public int Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged();
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler? ValueChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}