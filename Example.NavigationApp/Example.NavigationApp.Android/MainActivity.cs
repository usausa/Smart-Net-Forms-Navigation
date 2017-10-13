namespace Example.NavigationApp.Droid
{
    using Android.App;
    using Android.Content.PM;
    using Android.OS;

    using Smart.Resolver;

    [Activity(MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new ComponentProvider()));
        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }

        private class ComponentProvider : IComponentProvider
        {
            public void RegisterComponents(ResolverConfig config)
            {
            }
        }
    }
}
