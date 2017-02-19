namespace Smart.Forms.Navigation
{
    using System.Collections.Generic;

    public class NavigationParameters
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public T GetValue<T>(string key)
        {
            return (T)values[key];
        }

        public T GetValue<T>()
        {
            return GetValue<T>(typeof(T).Name);
        }

        public T GetValueOrDefault<T>(string key)
        {
            object value;
            return values.TryGetValue(key, out value) ? (T)value : default(T);
        }

        public T GetValueOrDefault<T>()
        {
            return GetValueOrDefault<T>(typeof(T).Name);
        }

        public T GetValueOr<T>(string key, T defaultValue)
        {
            object value;
            return values.TryGetValue(key, out value) ? (T)value : defaultValue;
        }

        public T GetValueOr<T>(T defaultValue)
        {
            return GetValueOr(typeof(T).Name, defaultValue);
        }

        public NavigationParameters SetValue<T>(string key, T value)
        {
            values[key] = value;
            return this;
        }

        public NavigationParameters SetValue<T>(T value)
        {
            return SetValue(typeof(T).Name, value);
        }
    }
}
