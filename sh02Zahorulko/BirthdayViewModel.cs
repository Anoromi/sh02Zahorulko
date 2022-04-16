using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace sh01Zahorulko
{
    public class BirthdayViewModel : INotifyPropertyChanged
    {
        private BirthdayDate? birthday = null;
        private DateTime? selectedDate = null;
        private string? error;

        public DateTime? SelectedDate
        {
            get => selectedDate; set { selectedDate = value; }
        }
        public BirthdayDate? Birthday
        {
            get => birthday;
            private set
            {
                birthday = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Birthday)));
            }
        }
        public string? Error
        {
            get => error; private set
            {
                error = value;
                if (value is not null)
                    MessageBox.Show(value);
                PropertyChanged?.Invoke(this, new(nameof(Error)));
            }
        }

        public string Years
        {
            get
            {
                if (Birthday is not null)
                    return $"You are {Birthday.Years} years old";
                return string.Empty;
            }
        }

        public string HappyBirthday
        {
            get
            {
                if (Birthday is not null && Birthday.IsBirthDay(DateTime.Now))
                    return "Happy Birthday!";
                return string.Empty;
            }
        }
        public string ChineseCalendar => Birthday is not null ? $"You were born in the year of {Birthday.ChineseZodiac}" : string.Empty;
        public string WesternCalendar => Birthday is not null ? $"You were born in the month of {Birthday.WesternZodiac}" : string.Empty;

        private void UpdateCalendars(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Birthday))
            {
                PropertyChanged?.Invoke(this, new(nameof(ChineseCalendar)));
                PropertyChanged?.Invoke(this, new(nameof(WesternCalendar)));
                PropertyChanged?.Invoke(this, new(nameof(HappyBirthday)));
                PropertyChanged?.Invoke(this, new(nameof(Years)));
            }
        }

        public BirthdayViewModel()
        {

            PropertyChanged += UpdateCalendars;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Command<object> Click { get => new(_ => ApplyDate(), _ => SelectedDate != null); }

        private async void ApplyDate()
        {
            await Task.Run(() =>
            {
                switch (BirthdayDate.TryParse(SelectedDate!.Value, out var b))
                {
                    case null:
                        Error = null;
                        Birthday = b;
                        break;
                    case string s:
                        Error = s;
                        break;
                }
            });
        }
    }
}