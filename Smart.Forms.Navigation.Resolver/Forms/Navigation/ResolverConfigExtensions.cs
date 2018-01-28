namespace Smart.Forms.Navigation
{
    using Smart.Forms.Navigation.Components;
    using Smart.Forms.Navigation.Plugins;
    using Smart.Forms.Navigation.Plugins.Parameter;
    using Smart.Resolver;
    using Smart.Resolver.Handlers;

    public static class ResolverConfigExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public static void UseNavigator(this ResolverConfig config)
        {
            config.Components.Add<DependencyServiceMissingHandler>();
            config.Bind<IFactory>().To<SmartResolverFactory>().InSingletonScope();
            config.Bind<IConverter>().To<StandardConverter>().InSingletonScope();
            config.Bind<IPlugin>().To<ParameterPlugin>().InSingletonScope();
            config.Bind<INavigator>().To<Navigator>().InSingletonScope();
        }
    }
}
