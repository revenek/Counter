using System.Collections.ObjectModel;
using System.Text.Json;

namespace Counter.Views
{
    public partial class MainPage : ContentPage
    {
        private const string StorageKey = "saved_counters";
        public ObservableCollection<CounterModel> Counters { get; set; } = new();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;

            LoadCounters();
        }

        private void OnAddCounterClicked(object sender, EventArgs e)
        {
            string name = CounterNameEntry.Text;
            string initialValueText = InitialValueEntry.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                DisplayAlert("Blad", "Prosze podac nazwe licznika.", "OK");
                return;
            }

            if (!int.TryParse(initialValueText, out int initialValue))
            {
                initialValue = 0;
            }

            var newCounter = new CounterModel
            {
                Name = name,
                Value = initialValue
            };

            newCounter.ValueChanged += OnCounterValueChanged;

            Counters.Add(newCounter);
            SaveCounters();

            CounterNameEntry.Text = string.Empty;
            InitialValueEntry.Text = string.Empty;
        }

        private void OnIncrementClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is CounterModel counter)
            {
                counter.Value++;
            }
        }

        private void OnDecrementClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is CounterModel counter)
            {
                counter.Value--;
            }
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is CounterModel counter)
            {
                counter.ValueChanged -= OnCounterValueChanged;
                Counters.Remove(counter);
                SaveCounters();
            }
        }

        private void OnCounterValueChanged(object? sender, EventArgs e)
        {
            SaveCounters();
        }

        private void SaveCounters()
        {
            string json = JsonSerializer.Serialize(Counters);
            Preferences.Default.Set(StorageKey, json);
        }

        private void LoadCounters()
        {
            if (Preferences.Default.ContainsKey(StorageKey))
            {
                string json = Preferences.Default.Get(StorageKey, string.Empty);
                if (!string.IsNullOrEmpty(json))
                {
                    var loadedCounters = JsonSerializer.Deserialize<List<CounterModel>>(json);
                    if (loadedCounters != null)
                    {
                        Counters.Clear();
                        foreach (var counter in loadedCounters)
                        {
                            counter.ValueChanged += OnCounterValueChanged;
                            Counters.Add(counter);
                        }
                    }
                }
            }
        }
    }
}
