namespace Example.FormsNavigation.Pages.Parameter
{
    using Xamarin.Forms;

    public partial class DataListPage : ContentPage
    {
        public DataListPage(DataListPageViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
