namespace Smart.Forms.Navigation
{
    using System;
    using System.Reflection;

    using Xamarin.Forms;

    public class PathPageResolver : IPageResolver
    {
        private const string Separator = "/";

        private string @namespace;

        public string Namespace
        {
            get { return @namespace ?? (@namespace = String.Concat(Application.Current.GetType().Namespace, ".Pages")); }
            set { @namespace = value; }
        }

        public string NormarizeName(string current, string name)
        {
            if (name.StartsWith(Separator))
            {
                return name;
            }

            if (!String.IsNullOrEmpty(current))
            {
                var index = current.LastIndexOf(Separator, StringComparison.OrdinalIgnoreCase);
                return String.Concat(current.Substring(0, index + 1), name);
            }

            return String.Concat(Separator, name);
        }

        public Type ResolveType(string name)
        {
            var typeName = String.Concat(Namespace, name.Replace(Separator, "."));
            return Application.Current.GetType().GetTypeInfo().Assembly.GetType(typeName);
        }

        public string ResolveName(Type type)
        {
            var name = type.FullName.Substring(Namespace.Length);
            return name.Replace(".", Separator);
        }
    }
}
