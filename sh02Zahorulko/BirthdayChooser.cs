using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh01Zahorulko
{
    public enum ChineseZodiac
    {
        Rat, Ox, Tiger, Rabbit, Dragon, Snake, Horse, Goat, Monkey, Rooster, Dog, Pig
    }
    public enum WesternZodiac
    {
        Water, Fish, Ram, Bull, Twins, Crab, Lion, Maiden, Scales, Scorpion, Archer, Goat
    }

    public class BirthdayDate
    {
        public DateTime Date { get; private set; }
        public WesternZodiac WesternZodiac { get; }
        public ChineseZodiac ChineseZodiac { get; }
        public int Years
        {
            get
            {
                var today = DateTime.Today;
                var partialYears = today.Year - Date.Year;
                var day = Date switch
                {
                    { Day: 29, Month: 2 } when !DateTime.IsLeapYear(today.Year) => 28,
                    _ => Date.Day
                };
                return partialYears + (new DateTime(today.Year, Date.Month, day) > today ? -1 : 0);
            }
        }

        private BirthdayDate(DateTime date)
        {
            Date = date;
            WesternZodiac = WesternFromDateTime(date);
            ChineseZodiac = ChineseFromDateTime(date);

        }

        public bool IsBirthDay(DateTime at)
        {
            return Date.Month == at.Month && Date.Day == at.Day;
        }

        static WesternZodiac WesternFromDateTime(in DateTime date)
        {
            var normalizedDate = new DateTime(2000, date.Month, date.Day);
            ReadOnlySpan<int> increments = stackalloc [] { 29, 30, 29, 30, 31, 30, 30, 30, 29, 30, 28 };
            var previous = new DateTime(2000, 1, 20);
            int? selected = null;
            for (int i = 0; i < increments.Length; i++)
            {
                var next = previous.AddDays(increments[i]).AddDays(1);
                if (normalizedDate < next && normalizedDate >= previous)
                    selected = i;
                previous = next;

            }
            selected ??= increments.Length;

            return (WesternZodiac)selected;
        }

        static ChineseZodiac ChineseFromDateTime(in DateTime date)
        {
            var nomalizedYear = (date.Year - 1924) % 12;
            if (nomalizedYear < 0) nomalizedYear = 12 + nomalizedYear;
            return (ChineseZodiac)nomalizedYear;
        }

        public static string? TryParse(DateTime date, out BirthdayDate res)
        {
            res = null!;
            var cur = DateTime.Now;
            if (cur < date)
                return "You shouldn't even exist";
            if (cur.Year - date.Year > 123)
                return "You should have died already";

            res = new(date);
            return null;
        }

        public static BirthdayDate Parse(DateTime date)
        {
            var error = TryParse(date, out var res);

            if (error == null)
                return res;
            else throw new Exception(error);
        }

        public override string ToString()
        {
            return Date.ToString();
        }
    }

}