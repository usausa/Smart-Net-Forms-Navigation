namespace Smart.Forms.Navigation
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Smart.ComponentModel;
    using Smart.Forms.Navigation.Components;
    using Smart.Forms.Navigation.Plugins;
    using Smart.Functional;

    using Xamarin.Forms;

    public sealed class Navigator : NotificationObject, INavigator, IDisposable
    {
        private readonly IFactory factory;

        private readonly IPageResolver pageResolver;

        private readonly IPlugin[] plugins;

        private bool navigating;

        public bool Navigating
        {
            get => navigating;
            set => SetProperty(ref navigating, value);
        }

        public Navigator(IFactory factory = null, IPageResolver pageResolver = null, IPlugin[] plugins = null)
        {
            this.factory = factory ?? DefaultComponents.Factory;
            this.pageResolver = pageResolver ?? DefaultComponents.PageResolver;
            this.plugins = plugins ?? new IPlugin[0];

            Application.Current.ModalPopping += OnModalPopping;
            Application.Current.ModalPopped += OnModalPopped;
        }

        public void Dispose()
        {
            Application.Current.ModalPopping -= OnModalPopping;
            Application.Current.ModalPopped -= OnModalPopped;
        }

        private void OnModalPopping(object sender, ModalPoppingEventArgs modalPoppingEventArgs)
        {
            if (!navigating)
            {
                // TODO
            }
        }

        private void OnModalPopped(object sender, ModalPoppedEventArgs modalPoppedEventArgs)
        {
            if (!navigating)
            {
                // TODO
            }
        }

        // TODO Handle ?

        public async Task<bool> ForwardAsync(string name, NavigationParameters parameters)
        {
            // Guard
            if (Application.Current.MainPage.Navigation.ModalStack.Count > 0)
            {
                return false;
            }

            // Stack
            var navigationStack = Application.Current.MainPage.Navigation.NavigationStack;

            var fromPage = navigationStack.Count > 0 ? navigationStack[navigationStack.Count - 1] : null;

            var previousName = fromPage != null ? pageResolver.ResolveName(fromPage.GetType()) : string.Empty;
            var normalizeName = pageResolver.NormarizeName(previousName, name);

            // Context
            var pluginContext = new PluginContext();
            var navigationContext = new NavigationContext(
                parameters ?? new NavigationParameters(),
                false,
                normalizeName,
                previousName);

            // Confirm
            if (fromPage != null)
            {
                if (!await PageHelper.ProcessCanNavigateAsync(fromPage, navigationContext))
                {
                    return false;
                }
            }

            try
            {
                Navigating = true;

                // Prepare
                var toPage = CreatePage(normalizeName, pluginContext);
                if (toPage == null)
                {
                    throw new ArgumentException(
                        String.Format(CultureInfo.InvariantCulture, "Invalid name. [{0}]", name), nameof(name));
                }

                // From event
                ProcessNavigatedFrom(fromPage, navigationContext, pluginContext);

                // To event
                ProcessNavigatingTo(toPage, navigationContext, pluginContext);

                // Replace new page
                await Application.Current.MainPage.Navigation.PushAsync(toPage);
                fromPage?.Apply(Application.Current.MainPage.Navigation.RemovePage);

                // To event
                ProcessNavigatedTo(toPage, navigationContext, pluginContext);

                // Remove old page
                ClosePage(fromPage, pluginContext);

                return true;
            }
            finally
            {
                Navigating = false;
            }
        }

        public async Task<bool> PushModelAsync(string name, NavigationParameters parameters)
        {
            // Stack
            var modalStack = Application.Current.MainPage.Navigation.ModalStack;
            var navigationStack = Application.Current.MainPage.Navigation.NavigationStack;

            var fromPage = modalStack.Count > 0
                ? modalStack[modalStack.Count - 1]
                : navigationStack.Count > 0 ? navigationStack[navigationStack.Count - 1] : null;

            var previousName = fromPage != null ? pageResolver.ResolveName(fromPage.GetType()) : string.Empty;
            var normalizeName = pageResolver.NormarizeName(previousName, name);

            // Context
            var pluginContext = new PluginContext();
            var navigationContext = new NavigationContext(
                parameters ?? new NavigationParameters(),
                false,
                normalizeName,
                previousName);

            // Confirm
            if (fromPage != null)
            {
                if (!await PageHelper.ProcessCanNavigateAsync(fromPage, navigationContext))
                {
                    return false;
                }
            }

            try
            {
                Navigating = true;

                // Prepare
                var toPage = CreatePage(normalizeName, pluginContext);
                if (toPage == null)
                {
                    throw new ArgumentException(
                        String.Format(CultureInfo.InvariantCulture, "Invalid name. [{0}]", name), nameof(name));
                }

                // From event
                ProcessNavigatedFrom(fromPage, navigationContext, pluginContext);

                // To event
                ProcessNavigatingTo(toPage, navigationContext, pluginContext);

                // Replace new page
                await Application.Current.MainPage.Navigation.PushModalAsync(toPage);

                // To event
                ProcessNavigatedTo(toPage, navigationContext, pluginContext);

                return true;
            }
            finally
            {
                Navigating = false;
            }
        }

        public async Task<bool> PopModalAsync(NavigationParameters parameters)
        {
            // Stack
            var modalStack = Application.Current.MainPage.Navigation.ModalStack;
            var navigationStack = Application.Current.MainPage.Navigation.NavigationStack;

            if (modalStack.Count == 0)
            {
                return false;
            }

            var fromPage = modalStack[modalStack.Count - 1];
            var toPage = modalStack.Count > 1
                ? modalStack[modalStack.Count - 2]
                : navigationStack.Count > 0 ? navigationStack[navigationStack.Count - 1] : null;

            var previousName = pageResolver.ResolveName(fromPage.GetType());
            var name = toPage != null ? pageResolver.ResolveName(toPage.GetType()) : string.Empty;

            // Context
            var pluginContext = new PluginContext();
            var navigationContext = new NavigationContext(
                parameters ?? new NavigationParameters(),
                true,
                name,
                previousName);

            // Confirm
            if (!await PageHelper.ProcessCanNavigateAsync(fromPage, navigationContext))
            {
                return false;
            }

            try
            {
                Navigating = true;

                // From event
                ProcessNavigatedFrom(fromPage, navigationContext, pluginContext);

                // To event
                ProcessNavigatingTo(toPage, navigationContext, pluginContext);

                // Replace new page
                await Application.Current.MainPage.Navigation.PopModalAsync();

                // To event
                ProcessNavigatedTo(toPage, navigationContext, pluginContext);

                // Remove old page
                ClosePage(fromPage, pluginContext);

                return true;
            }
            finally
            {
                Navigating = false;
            }
        }

        private Page CreatePage(string name, PluginContext pluginContext)
        {
            var type = pageResolver.ResolveType(name);
            if (type == null)
            {
                return null;
            }

            var page = (Page)factory.Create(type);

            foreach (var plugin in plugins)
            {
                plugin.OnCreate(pluginContext, page);
            }

            return page;
        }

        private void ClosePage(Page page, PluginContext pluginContext)
        {
            if (page == null)
            {
                return;
            }

            foreach (var plugin in plugins)
            {
                plugin.OnClose(pluginContext, page);
            }

            PageHelper.DestroyPage(page);
        }

        private void ProcessNavigatedFrom(Page page, NavigationContext navigationContext, PluginContext pluginContext)
        {
            if (page == null)
            {
                return;
            }

            PageHelper.ProcessNavigatedFrom(page, navigationContext);

            foreach (var plugin in plugins)
            {
                plugin.OnNavigatedFrom(pluginContext, page);
            }
        }

        private void ProcessNavigatingTo(Page page, NavigationContext navigationContext, PluginContext pluginContext)
        {
            if (page == null)
            {
                return;
            }

            foreach (var plugin in plugins)
            {
                plugin.OnNavigatingTo(pluginContext, page);
            }

            PageHelper.ProcessNavigatingTo(page, navigationContext);
        }

        private void ProcessNavigatedTo(Page page, NavigationContext navigationContext, PluginContext pluginContext)
        {
            if (page == null)
            {
                return;
            }

            foreach (var plugin in plugins)
            {
                plugin.OnNavigatedTo(pluginContext, page);
            }

            PageHelper.ProcessNavigatedTo(page, navigationContext);
        }
    }
}
