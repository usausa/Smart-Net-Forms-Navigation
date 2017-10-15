namespace Smart.Forms.Navigation.Plugins.Scope
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ScopeAttribute : Attribute
    {
        public string Key { get; }

        public Type ConcreateType { get; }

        public ScopeAttribute()
        {
        }

        public ScopeAttribute(string key)
            : this(key, null)
        {
        }

        public ScopeAttribute(Type concreateType)
            : this(null, concreateType)
        {
        }

        public ScopeAttribute(string key, Type concreateType)
        {
            Key = key;
            ConcreateType = concreateType;
        }
    }
}
