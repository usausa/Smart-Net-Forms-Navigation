namespace Smart.Forms.Navigation.Components
{
    using System;

    public class StandardFactory : IFactory
    {
        public object Create(Type type)
        {
            // TODO
            return Activator.CreateInstance(type);
        }
    }
}
