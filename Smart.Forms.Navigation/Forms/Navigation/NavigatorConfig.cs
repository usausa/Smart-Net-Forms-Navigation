namespace Smart.Forms.Navigation
{
    using Smart.Forms.Navigation.Components;

    public class NavigatorConfig
    {
        public IActivator Activator { get; set; }

        public IPageResolver PageResolver { get; set; }
    }
}
