namespace Smart.Forms.Navigation.Plugins
{
    using Xamarin.Forms;

    public abstract class PluginBase : IPlugin
    {
        public virtual void OnCreate(IPluginContext context, Page page)
        {
        }

        public virtual void OnClose(IPluginContext context, Page page)
        {
        }

        public virtual void OnNavigatedFrom(IPluginContext context, Page page)
        {
        }

        public virtual void OnNavigatingTo(IPluginContext context, Page page)
        {
        }

        public virtual void OnNavigatedTo(IPluginContext context, Page page)
        {
        }
    }
}
