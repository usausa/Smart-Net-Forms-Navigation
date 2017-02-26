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

    public class Navigator : NotificationObject, INavigator
    {
        private readonly IActivator activator;

        private readonly IPageResolver pageResolver;

        private readonly IPlugin[] plugins;

        public Navigator(IActivator activator = null, IPageResolver pageResolver = null, IPlugin[] plugins = null)
        {
            this.activator = activator ?? DefaultComponents.Activator;
            this.pageResolver = pageResolver ?? DefaultComponents.PageResolver;
            this.plugins = plugins ?? new IPlugin[0];
        }

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
            var context = new NavigationContext(
                parameters ?? new NavigationParameters(),
                false,
                normalizeName,
                previousName);

            // Confirm
            if (fromPage != null)
            {
                if (!await PageHelper.ProcessCanNavigateAsync(fromPage, context))
                {
                    return false;
                }
            }

            // Prepare
            var toPage = CreatePage(normalizeName);
            if (toPage == null)
            {
                throw new ArgumentException(
                    String.Format(CultureInfo.InvariantCulture, "Invalid name. [{0}]", name), nameof(name));
            }

            // From event
            ProcessNavigatedFrom(fromPage, context);

            // Replace new page
            await Application.Current.MainPage.Navigation.PushAsync(toPage);
            fromPage?.Apply(Application.Current.MainPage.Navigation.RemovePage);

            // To event
            ProcessNavigatedTo(toPage, context);

            // Remove old page
            ClosePage(fromPage);

            return true;
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
            var context = new NavigationContext(
                parameters ?? new NavigationParameters(),
                false,
                normalizeName,
                previousName);

            // Confirm
            if (fromPage != null)
            {
                if (!await PageHelper.ProcessCanNavigateAsync(fromPage, context))
                {
                    return false;
                }
            }

            // Prepare
            var toPage = CreatePage(normalizeName);
            if (toPage == null)
            {
                throw new ArgumentException(
                    String.Format(CultureInfo.InvariantCulture, "Invalid name. [{0}]", name), nameof(name));
            }

            // From event
            ProcessNavigatedFrom(fromPage, context);

            // Replace new page
            await Application.Current.MainPage.Navigation.PushModalAsync(toPage);

            // To event
            ProcessNavigatedTo(toPage, context);

            return true;
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
            var context = new NavigationContext(
                parameters ?? new NavigationParameters(),
                true,
                name,
                previousName);

            // Confirm
            if (!await PageHelper.ProcessCanNavigateAsync(fromPage, context))
            {
                return false;
            }

            // From event
            ProcessNavigatedFrom(fromPage, context);

            // Replace new page
            await Application.Current.MainPage.Navigation.PopModalAsync();

            // To event
            ProcessNavigatedTo(toPage, context);

            // Remove old page
            ClosePage(fromPage);

            return true;
        }

        private Page CreatePage(string name)
        {
            var type = pageResolver.ResolveType(name);
            if (type == null)
            {
                return null;
            }

            var page = (Page)activator.Get(type);

            foreach (var plugin in plugins)
            {
                plugin.OnCreate(page);
            }

            return page;
        }

        private void ClosePage(Page page)
        {
            if (page == null)
            {
                return;
            }

            foreach (var plugin in plugins)
            {
                plugin.OnClose(page);
            }

            PageHelper.DestroyPage(page);
        }

        private void ProcessNavigatedFrom(Page page, NavigationContext context)
        {
            if (page == null)
            {
                return;
            }

            PageHelper.ProcessNavigatedFrom(page, context);

            foreach (var plugin in plugins)
            {
                plugin.OnNavigatedFrom(page, context);
            }
        }

        private void ProcessNavigatedTo(Page page, NavigationContext context)
        {
            if (page == null)
            {
                return;
            }

            foreach (var plugin in plugins)
            {
                plugin.OnNavigatedTo(page, context);
            }

            PageHelper.ProcessNavigatedTo(page, context);
        }
    }
}
