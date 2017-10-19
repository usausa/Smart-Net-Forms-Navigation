namespace Example.NavigationApp.Pages.Parameter
{
    public partial class ParameterDetailPage
    {
        public ParameterDetailPage(ParameterDetailPageViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;
        }
    }
}