namespace Example.NavigationApp.Pages.Parameter
{
    public partial class ParameterListPage
    {
        public ParameterListPage(ParameterListPageViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;
        }
    }
}