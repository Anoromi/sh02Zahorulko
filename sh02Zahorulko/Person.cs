using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sh01Zahorulko;

namespace sh02Zahorulko
{
    [Serializable]
    public class Person
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public Address? Address { get; init; }
        public BirthdayDate? Birthday { get; init; }

        public Person(string firstName, string lastName, Address? address = null, BirthdayDate? birthday = null)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            Birthday = birthday;

            _isAdult = birthday is not null ? birthday.Years >= 18 : null;
            _sunSign = birthday?.WesternZodiac;
            _chineseSign = birthday?.ChineseZodiac;
            _isBirthday = birthday?.IsBirthDay(DateTime.Now);
        }

        private readonly bool? _isAdult;
        public bool? IsAdult => _isAdult;

        private readonly WesternZodiac? _sunSign;
        public WesternZodiac? SunSign => _sunSign;

        private readonly ChineseZodiac? _chineseSign;
        public ChineseZodiac? ChineseSign => _chineseSign;

        private readonly bool? _isBirthday;
        public bool? IsBirthday => _isBirthday;
    }

}
