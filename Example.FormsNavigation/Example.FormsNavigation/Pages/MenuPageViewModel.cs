namespace Example.FormsNavigation.Pages
{
    using System.Threading.Tasks;

    using Smart.Forms.Input;
    using Smart.Forms.Navigation;
    using Smart.Forms.Navigation.Plugins.Parameter;

    public class MenuPageViewModel : AppViewModelBase
    {
        private readonly INavigator navigator;

        [Parameter]
        public int Parameter { get; set; }

        public AsyncCommand<string> NavigateCommand { get; }

        public MenuPageViewModel(INavigator navigator)
        {
            this.navigator = navigator;

            NavigateCommand = MakeBusyCommand<string>(Navigate);
        }

        private async Task Navigate(string page)
        {
            Parameter = 1;

            await navigator.ForwardAsync(page);
        }
    }
}
