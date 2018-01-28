﻿namespace Smart.Forms.Navigation.Plugins
{
    using System;

    public interface IPluginContext
    {
        void Save<T>(Type type, T value);

        T Load<T>(Type type);

        T LoadOr<T>(Type type, T defaultValue);

        T LoadOr<T>(Type type, Func<T> defaultValueFactory);
    }
}
