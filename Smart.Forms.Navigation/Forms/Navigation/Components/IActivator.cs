namespace Smart.Forms.Navigation.Components
{
    using System;

    public interface IActivator
    {
        object Get(Type type);
    }
}
