namespace Smart.Forms.Navigation.Plugins.Parameter
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ParameterAttribute : Attribute
    {
        public Direction Direction { get; }

        public string Name { get; }

        public ParameterAttribute()
            : this(Direction.Both, null)
        {
        }

        public ParameterAttribute(string name)
            : this(Direction.Both, name)
        {
        }

        public ParameterAttribute(Direction direction)
            : this(direction, null)
        {
        }

        public ParameterAttribute(Direction direction, string name)
        {
            Direction = direction;
            Name = name;
        }
    }
}
