namespace Smart.Forms.Navigation.Components
{
    using System;

    using Smart.Resolver;

    public class SmartResolverActivator : IActivator
    {
        private readonly IResolver resolver;

        public SmartResolverActivator(IResolver resolver)
        {
            this.resolver = resolver;
        }

        public object Get(Type type)
        {
            return resolver.Get(type);
        }
    }
}
