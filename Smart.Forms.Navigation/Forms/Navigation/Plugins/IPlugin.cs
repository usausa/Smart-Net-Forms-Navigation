namespace Smart.Forms.Navigation.Plugins
{
    using Xamarin.Forms;

    public interface IPlugin
    {
        void OnCreate(Page page);

        void OnNavigatedFrom(Page page, NavigationContext context);

        void OnNavigatedTo(Page page, NavigationContext context);

        void OnClose(Page page);
    }
}
