namespace Smart.Forms.Navigation
{
    using Smart.Forms.Navigation.Components;

    public static class DefaultComponents
    {
        private static IFactory factory;

        private static IConverter converter;

        private static IPageResolver pageResolver;

        public static IFactory Factory => factory ?? (factory = new StandardFactory());

        public static IConverter Converter => converter ?? (converter = new StandardConverter());

        public static IPageResolver PageResolver => pageResolver ?? (pageResolver = new PathPageResolver());
    }
}
