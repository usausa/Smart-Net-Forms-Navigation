namespace Smart.Forms.Navigation.Components
{
    using System;

    using Smart.Resolver;

    public class SmartResolverFactory : IFactory
    {
        private readonly IResolver resolver;

        public SmartResolverFactory(IResolver resolver)
        {
            this.resolver = resolver;
        }

        public object Create(Type type)
        {
            return resolver.Get(type);
        }
    }
}
