namespace Example.FormsNavigation.Pages.Parameter
{
    using System.Threading.Tasks;

    using Smart.Forms.Input;
    using Smart.Forms.Navigation;

    public class DataListPageViewModel : AppViewModelBase
    {
        private readonly INavigator navigator;

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
    }
}
