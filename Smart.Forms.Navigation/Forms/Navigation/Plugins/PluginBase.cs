namespace Smart.Forms.Navigation.Plugins
{
    using Xamarin.Forms;

    public abstract class PluginBase : IPlugin
    {
        public virtual void OnCreate(Page page)
        {
        }

        public virtual void OnNavigatedFrom(Page page, NavigationContext context)
        {
        }

        public virtual void OnNavigatedTo(Page page, NavigationContext context)
        {
        }

        public virtual void OnClose(Page page)
        {
        }
    }
}
