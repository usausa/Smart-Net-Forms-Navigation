namespace Smart.Forms.Navigation.Plugins.Parameter
{
    using System;
    using System.Collections.Generic;

    using Smart.Forms.Navigation.Components;

    using Xamarin.Forms;

    public class ParameterPlugin : PluginBase
    {
        private static readonly string ParameterName = typeof(ParameterPlugin).FullName;

        private readonly AttributePropertyFactory<ParameterAttribute> factory = new AttributePropertyFactory<ParameterAttribute>();

        private readonly IConverter converter;

        public ParameterPlugin(IConverter converter = null)
        {
            this.converter = converter ?? DefaultComponents.Converter;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnNavigatedFrom(Page page, NavigationContext context)
        {
            var parameters = default(Dictionary<string, object>);
            GatherExportParameters(page, ref parameters);
            GatherExportParameters(page.BindingContext, ref parameters);
            context.Parameters.SetValue(ParameterName, parameters);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnNavigatingTo(Page page, NavigationContext context)
        {
            var parameters = context.Parameters.GetValueOr(ParameterName, default(Dictionary<string, object>));
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

            foreach (var property in factory.GetAttributeProperties(target.GetType()))
            {
                if ((property.Attribute.Direction & Direction.Export) != 0)
                {
                    if (parameters == null)
                    {
                        parameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                    }

                    var name = property.Attribute.Name ?? property.Accessor.Name;
                    parameters.Add(name, property.Accessor.GetValue(target));
                }
            }
        }

        private void ApplyImportParameters(object target, IDictionary<string, object> parameters)
        {
            if (target == null)
            {
                return;
            }

            foreach (var property in factory.GetAttributeProperties(target.GetType()))
            {
                if ((property.Attribute.Direction & Direction.Import) != 0)
                {
                    var name = property.Attribute.Name ?? property.Accessor.Name;
                    if (parameters.TryGetValue(name, out var value))
                    {
                        property.Accessor.SetValue(target, converter.Convert(value, property.Accessor.Type));
                    }
                }
            }
        }
    }
}
