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
            get => @namespace ?? (@namespace = String.Concat(Application.Current.GetType().Namespace, ".Pages"));
            set => @namespace = value;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public string NormarizeName(string current, string name)
        {
            if (name.StartsWith(Separator, StringComparison.OrdinalIgnoreCase))
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public Type ResolveType(string name)
        {
            var typeName = String.Concat(Namespace, name.Replace(Separator, "."));
            return Application.Current.GetType().GetTypeInfo().Assembly.GetType(typeName);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public string ResolveName(Type type)
        {
            var name = type.FullName.Substring(Namespace.Length);
            return name.Replace(".", Separator);
        }
    }
}
