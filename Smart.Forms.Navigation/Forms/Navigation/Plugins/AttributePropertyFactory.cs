﻿namespace Smart.Forms.Navigation.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Smart.Reflection;

    public class AttributePropertyFactory<T>
        where T : Attribute
    {
        private readonly Dictionary<Type, AttributeProperty<T>[]> cache = new Dictionary<Type, AttributeProperty<T>[]>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public AttributeProperty<T>[] GetAttributeProperties(Type type)
        {
            if (cache.TryGetValue(type, out var properties))
            {
                return properties;
            }

            properties = type.GetTypeInfo().DeclaredProperties
                .SelectMany(
                    pi => pi.GetCustomAttributes(typeof(T)),
                    (pi, attr) => new AttributeProperty<T>((T)attr, TypeMetadataFactory.Default.CreateAccessor(pi)))
                .ToArray();
            cache[type] = properties;

            return properties;
        }
    }
}
