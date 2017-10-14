namespace Example.NavigationApp.Pages.Wizard
{
    public partial class ResultPage
    {
        public ResultPage(ResultPageViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;
        }
    }
}