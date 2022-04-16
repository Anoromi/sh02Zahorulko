using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh02Zahorulko
{
    public struct Dependency<T>
    {
        private T value;
        public T Value
        {
            get => value;
            set
            {

                if ((canAssign?.Invoke(this.value) ?? true))
                {
                    this.value = value;
                    notification?.Invoke(name);
                    postAction?.Invoke(this.value);
                }
            }
        }
        private string name;
        private Action<string>? notification;
        private Predicate<T>? canAssign;
        private Action<T>? postAction;
        public Dependency(T value, string name, Action<string>? notification = null, Action<T>? postAction = null, Predicate<T>? canAssign = null) => (this.value, this.name, this.postAction, this.notification, this.canAssign) = (value, name, postAction, notification, canAssign);
    }
}
