namespace Smart.Forms.Navigation
{
    using Smart.Forms.Navigation.Components;

    public static class DefaultComponents
    {
        private static IActivator activator;

        private static IConverter converter;

        private static IPageResolver pageResolver;

        public static IActivator Activator => activator ?? (activator = new StandardActivator());

        public static IConverter Converter => converter ?? (converter = new StandardConverter());

        public static IPageResolver PageResolver => pageResolver ?? (pageResolver = new PathPageResolver());
    }
}
