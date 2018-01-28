namespace Smart.Forms.Navigation.Plugins.Parameter
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ParameterAttribute : Attribute
    {
        public Directions Direction { get; }

        public string Name { get; }

        public ParameterAttribute()
            : this(Directions.Both, null)
        {
        }

        public ParameterAttribute(string name)
            : this(Directions.Both, name)
        {
        }

        public ParameterAttribute(Directions direction)
            : this(direction, null)
        {
        }

        public ParameterAttribute(Directions direction, string name)
        {
            Direction = direction;
            Name = name;
        }
    }
}
