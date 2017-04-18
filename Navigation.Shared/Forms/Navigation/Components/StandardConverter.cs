namespace Smart.Forms.Navigation.Components
{
    using System;
    using Smart.Converter;

    public class StandardConverter : IConverter
    {
        private readonly IObjectConverter converter;

        public StandardConverter()
            : this(ObjectConverter.Default)
        {
        }

        public StandardConverter(IObjectConverter converter)
        {
            this.converter = converter;
        }

        public object Convert(object value, Type type)
        {
            return converter.Convert(value, type);
        }
    }
}
