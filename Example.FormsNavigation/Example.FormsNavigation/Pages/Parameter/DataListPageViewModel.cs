namespace Example.FormsNavigation.Pages.Parameter
{
    using System.Threading.Tasks;

    using Smart.Forms.Input;
    using Smart.Forms.Navigation;
    using Smart.Forms.Navigation.Plugins.Parameter;

    public class DataListPageViewModel : AppViewModelBase, INavigationAware
    {
        private readonly INavigator navigator;

        [Parameter]
        public int Parameter { get; set; }

        public AsyncCommand BackCommand { get; }

        public DataListPageViewModel(INavigator navigator)
        {
            this.navigator = navigator;

            BackCommand = MakeBusyCommand(Back);
        }

        private async Task Back()
        {
            await navigator.ForwardAsync("/MenuPage");
        }

        public void OnNavigatedFrom(NavigationContext context)
        {
        }

        public void OnNavigatingTo(NavigationContext context)
        {
        }

        public void OnNavigatedTo(NavigationContext context)
        {
        }
    }
}
