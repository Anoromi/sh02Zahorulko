using sh01Zahorulko;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace sh02Zahorulko
{

    public class PersonRegisterViewModel : INotifyPropertyChanged
    {
        private Action<string> notify;

        private Dependency<bool> active;
        public bool Active { get => active.Value; set => active.Value = value; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string AddressPath { get; set; } = "";
        public DateTime? Birthday { get; set; } = null;

        private Dependency<string?> error;
        public string? Error
        {
            get => error.Value;
            set => error.Value = value;
        }


        private Dependency<Person?> person;
        public Person? Person { get => person.Value;
            set => person.Value = value; }


        public event PropertyChangedEventHandler? PropertyChanged;

        public Command<object> Click
        {
            get => new(async _ =>
            {
                active.Value = false;
                await Task.Delay(1000);
                ApplyValues();
                active.Value = true;
            }, canExecute: CheckAvailability);
        }



        public void ApplyValues()
        {
            Address address;
            try
            {
                address = new Address(AddressPath);
            }
            catch (ArgumentException)
            {
                Error = "Illegal address";
                return;
            }
            BirthdayDate birthdayDate;
            try
            {
                birthdayDate = BirthdayDate.Parse(Birthday!.Value);
            }
            catch (Exception e)
            {
                Error = e.Message;
                return;
            }

            Person = new Person(FirstName, LastName, address, birthdayDate);
        }

        public bool CheckAvailability(object? _) => !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName) && !string.IsNullOrWhiteSpace(AddressPath) && Birthday != null;

        public PersonRegisterViewModel()
        {
            notify = (s) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));
            active = new(true, nameof(Active), notify);
            person = new(null, nameof(Person), notify);
            error = new(null, nameof(Error), postAction: (s) => { if (s is not null) MessageBox.Show(s); });
        }
    }
}
