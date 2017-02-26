namespace Example.FormsNavigation
{
    using Smart.Forms.Navigation;
    using Smart.Forms.Navigation.Components;
    using Smart.Forms.Navigation.Plugins;
    using Smart.Forms.Navigation.Plugins.Parameter;
    using Smart.Resolver;

    using Xamarin.Forms;

    public partial class App : Application
    {
        private IResolver Resolver { get; }

        public App()
        {
            InitializeComponent();

            var config = new ResolverConfig();
            RegisterComponents(config);
            Resolver = config.ToResolver();

            MainPage = Resolver.Get<MainPage>();

            var navigator = Resolver.Get<INavigator>();
            navigator.ForwardAsync("/MenuPage");
        }

        private void RegisterComponents(ResolverConfig config)
        {
            // TODO Xamarin DS
            config.Bind<IActivator>().To<SmartResolverActivator>().InSingletonScope();
            config.Bind<IConverter>().To<StandardConverter>().InSingletonScope();
            config.Bind<IPlugin>().To<ParameterPlugin>().InSingletonScope();
            config.Bind<INavigator>().To<Navigator>().InSingletonScope();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
