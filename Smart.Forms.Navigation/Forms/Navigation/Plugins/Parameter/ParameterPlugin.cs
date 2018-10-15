namespace Smart.Forms.Navigation.Plugins.Parameter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Smart.Forms.Navigation.Components;
    using Smart.Reflection;

    using Xamarin.Forms;

    public class ParameterPlugin : PluginBase
    {
        private readonly Dictionary<Type, ParameterProperty[]> typeProperties = new Dictionary<Type, ParameterProperty[]>();

        private readonly IDelegateFactory delegateFactory;

        private readonly IConverter converter;

        public ParameterPlugin(IDelegateFactory delegateFactory, IConverter converter)
        {
            this.delegateFactory = delegateFactory;
            this.converter = converter;
        }

        private ParameterProperty[] GetTypeProperties(Type type)
        {
            if (!typeProperties.TryGetValue(type, out var properties))
            {
                properties = type.GetProperties()
                    .Select(x => new
                    {
                        Property = x,
                        Attribute = (ParameterAttribute)x.GetCustomAttribute(typeof(ParameterAttribute))
                    })
                    .Where(x => x.Attribute != null)
                    .Select(x => new ParameterProperty(
                        x.Attribute.Name ?? x.Property.Name,
                        delegateFactory.GetExtendedPropertyType(x.Property),
                        (x.Attribute.Direction & Directions.Export) != 0 ? delegateFactory.CreateGetter(x.Property, true) : null,
                        (x.Attribute.Direction & Directions.Import) != 0 ? delegateFactory.CreateSetter(x.Property, true) : null))
                    .ToArray();
                typeProperties[type] = properties;
            }

            return properties;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnNavigatedFrom(IPluginContext context, Page page)
        {
            var parameters = default(Dictionary<string, object>);
            GatherExportParameters(page, ref parameters);
            GatherExportParameters(page.BindingContext, ref parameters);
            context.Save(GetType(), parameters);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnNavigatingTo(IPluginContext context, Page page)
        {
            var parameters = context.LoadOr(GetType(), default(Dictionary<string, object>));
            if (parameters != null)
            {
                ApplyImportParameters(page, parameters);
                ApplyImportParameters(page.BindingContext, parameters);
            }
        }

        private void GatherExportParameters(object target, ref Dictionary<string, object> parameters)
        {
            if (target == null)
            {
                return;
            }

            foreach (var property in GetTypeProperties(target.GetType()))
            {
                if (property.Getter != null)
                {
                    if (parameters == null)
                    {
                        parameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                    }

                    parameters.Add(property.Name, property.Getter(target));
                }
            }
        }

        private void ApplyImportParameters(object target, IDictionary<string, object> parameters)
        {
            if (target == null)
            {
                return;
            }

            foreach (var property in GetTypeProperties(target.GetType()))
            {
                if ((property.Setter != null) &&
                    parameters.TryGetValue(property.Name, out var value))
                {
                    property.Setter(target, converter.Convert(value, property.PropertyType));
                }
            }
        }
    }
}
