namespace Smart.Forms.Navigation
{
    public interface INavigationAware
    {
        void OnNavigatedFrom(NavigationContext context);

        void OnNavigatingTo(NavigationContext context);

        void OnNavigatedTo(NavigationContext context);
    }
}
