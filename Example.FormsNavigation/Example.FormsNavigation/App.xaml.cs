namespace Example.FormsNavigation
{
    using Smart.Forms.Navigation;
    using Smart.Forms.Navigation.Components;
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
            navigator.Forward("/MenuPage");
        }

        private void RegisterComponents(ResolverConfig config)
        {
            config.Bind<IActivator>().To<SmartResolverActivator>().InSingletonScope();
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
