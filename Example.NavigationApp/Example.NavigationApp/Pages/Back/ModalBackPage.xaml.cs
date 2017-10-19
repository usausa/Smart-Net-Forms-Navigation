namespace Example.NavigationApp.Pages.Back
{
    public partial class ModalBackPage
    {
        public ModalBackPage(ModalBackPageViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;
        }
    }
}
