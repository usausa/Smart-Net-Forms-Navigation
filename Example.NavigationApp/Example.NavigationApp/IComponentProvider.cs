namespace Example.NavigationApp
{
    using Smart.Resolver;

    public interface IComponentProvider
    {
        void RegisterComponents(ResolverConfig config);
    }
}
