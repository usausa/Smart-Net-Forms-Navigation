namespace Example.FormsNavigation.Pages
{
    using System.Threading.Tasks;

    using Smart.Forms.Input;
    using Smart.Forms.Navigation;

    public class MenuPageViewModel : AppViewModelBase
    {
        private readonly INavigator navigator;

        public AsyncCommand<string> NavigateCommand { get; }

        public MenuPageViewModel(INavigator navigator)
        {
            this.navigator = navigator;

            NavigateCommand = MakeBusyCommand<string>(Navigate);
        }

        private async Task Navigate(string page)
        {
            await navigator.Forward(page);
        }
    }
}
