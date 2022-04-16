using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace sh02Zahorulko
{
    public struct Address
    {
        private string _path;
        public string Path
        {
            get => _path;
            set
            {
                if (regex.IsMatch(value))
                    _path = value;
                else
                    throw new ArgumentException($"Illegal address {value}");
            }
        }

        public Address(string path)
        {
            if (regex.IsMatch(path))
                _path = path;
            else
                throw new ArgumentException($"Illegal address {path}");
        }

        private readonly static Regex regex = new("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");

        public static Task<Address> GetAsync(string path)
        {
            return Task.Run(() =>
            {
                return new Address(path);
            });
        }

        public override string ToString()
        {
            return Path;
        }

    }
}
