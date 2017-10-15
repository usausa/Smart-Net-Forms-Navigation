namespace Smart.Forms.Navigation.Plugins.Scope
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.Forms.Navigation.Components;

    using Xamarin.Forms;

    public class ScopePlugin : PluginBase
    {
        private class Reference
        {
            public object Instance { get; set; }

            public int Counter { get; set; }
        }

        private readonly AttributePropertyFactory<ScopeAttribute> factory = new AttributePropertyFactory<ScopeAttribute>();

        private readonly IActivator activator;

        private readonly Dictionary<string, Reference> store = new Dictionary<string, Reference>();

        public ScopePlugin(IActivator activator)
        {
            this.activator = activator ?? DefaultComponents.Activator;
        }

        public override void OnCreate(Page page)
        {
            ApplyScope(page);
            ApplyScope(page.BindingContext);
        }

        public override void OnClose(Page page)
        {
            ReleaseScopeWhenUnused(page);
            ReleaseScopeWhenUnused(page.BindingContext);

            var removes = store.Where(x => x.Value.Counter == 0).ToList();
            foreach (var remove in removes)
            {
                store.Remove(remove.Key);

                (remove.Value.Instance as IDisposable)?.Dispose();
            }
        }

        private void ApplyScope(object target)
        {
            if (target == null)
            {
                return;
            }

            foreach (var property in factory.GetAttributeProperties(target.GetType()))
            {
                var key = property.Attribute.Key ?? property.Accessor.Type.FullName;

                if (!store.TryGetValue(key, out var reference))
                {
                    reference = new Reference
                    {
                        Instance = activator.Get(property.Attribute.ConcreateType ?? property.Accessor.Type)
                    };

                    (reference.Instance as IInitializable)?.Initialize();

                    store[key] = reference;
                }

                reference.Counter++;

                property.Accessor.SetValue(target, reference.Instance);
            }
        }

        private void ReleaseScopeWhenUnused(object target)
        {
            if (target == null)
            {
                return;
            }

            foreach (var property in factory.GetAttributeProperties(target.GetType()))
            {
                var key = property.Attribute.Key ?? property.Accessor.Type.FullName;

                if (store.TryGetValue(key, out var reference))
                {
                    reference.Counter--;
                }
            }
        }
    }
}
