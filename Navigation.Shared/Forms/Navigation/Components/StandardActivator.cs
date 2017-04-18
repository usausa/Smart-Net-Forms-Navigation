namespace Smart.Forms.Navigation.Components
{
    using System;

    public class StandardActivator : IActivator
    {
        public object Get(Type type)
        {
            // TODO
            return Activator.CreateInstance(type);
        }
    }
}
