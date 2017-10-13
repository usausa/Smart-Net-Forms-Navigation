namespace Example.NavigationApp.Pages
{
    public partial class MenuPage
    {
        public MenuPage(MenuPageViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;
        }
    }
}
