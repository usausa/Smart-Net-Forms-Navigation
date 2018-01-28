namespace Smart.Forms.Navigation.Plugins
{
    using Xamarin.Forms;

    public interface IPlugin
    {
        void OnCreate(IPluginContext context, Page page);

        void OnClose(IPluginContext context, Page page);

        void OnNavigatedFrom(IPluginContext context, Page page);

        void OnNavigatingTo(IPluginContext context, Page page);

        void OnNavigatedTo(IPluginContext context, Page page);
    }
}
