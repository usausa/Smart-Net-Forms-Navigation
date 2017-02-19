namespace Example.FormsNavigation.Pages
{
    using Xamarin.Forms;

    public partial class MenuPage : ContentPage
    {
        public MenuPage(MenuPageViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
