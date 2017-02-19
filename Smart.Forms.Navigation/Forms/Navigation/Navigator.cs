namespace Smart.Forms.Navigation
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Smart.ComponentModel;
    using Smart.Forms.Navigation.Components;
    using Smart.Functional;

    using Xamarin.Forms;

    public class Navigator : NotificationObject, INavigator
    {
        private readonly IActivator activator;

        private readonly IPageResolver pageResolver;

        public Navigator()
            : this(new StandardActivator(), new DefaultPageResolver())
        {
        }

        public Navigator(IActivator activator)
            : this(activator, new DefaultPageResolver())
        {
        }

        public Navigator(IActivator activator, IPageResolver pageResolver)
        {
            this.activator = activator;
            this.pageResolver = pageResolver;
        }

        public Task<bool> Forward(string name)
        {
            return Forward(name, new NavigationParameters());
        }

        public async Task<bool> Forward(string name, NavigationParameters parameter)
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
                parameter,
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
            var toPage = ResolvePage(normalizeName);
            if (toPage == null)
            {
                throw new ArgumentException(
                    String.Format(CultureInfo.InvariantCulture, "Invalid name. [{0}]", name), nameof(name));
            }

            // From event
            PageHelper.ProcessNavigatedFrom(fromPage, context);

            // Replace new page
            await Application.Current.MainPage.Navigation.PushAsync(toPage);
            fromPage?.Apply(Application.Current.MainPage.Navigation.RemovePage);

            // To event
            PageHelper.ProcessNavigatedTo(toPage, context);

            // Remove old page
            fromPage?.Apply(PageHelper.DestroyPage);

            return true;
        }

        public Task<bool> PushModel(string name)
        {
            return PushModel(name, new NavigationParameters());
        }

        public async Task<bool> PushModel(string name, NavigationParameters parameter)
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
                parameter,
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
            var toPage = ResolvePage(normalizeName);
            if (toPage == null)
            {
                throw new ArgumentException(
                    String.Format(CultureInfo.InvariantCulture, "Invalid name. [{0}]", name), nameof(name));
            }

            // From event
            fromPage?.Apply(x => PageHelper.ProcessNavigatedFrom(x, context));

            // Replace new page
            await Application.Current.MainPage.Navigation.PushModalAsync(toPage);

            // To event
            PageHelper.ProcessNavigatedTo(toPage, context);

            return true;
        }

        public Task<bool> PopModal()
        {
            return PopModal(new NavigationParameters());
        }

        public async Task<bool> PopModal(NavigationParameters parameter)
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
                parameter,
                true,
                name,
                previousName);

            // Confirm
            if (!await PageHelper.ProcessCanNavigateAsync(fromPage, context))
            {
                return false;
            }

            // From event
            PageHelper.ProcessNavigatedFrom(fromPage, context);

            // Replace new page
            await Application.Current.MainPage.Navigation.PopModalAsync();

            // To event
            toPage?.Apply(x => PageHelper.ProcessNavigatedTo(x, context));

            // Remove old page
            PageHelper.DestroyPage(fromPage);

            return true;
        }

        private Page ResolvePage(string name)
        {
            var type = pageResolver.ResolveType(name);
            if (type == null)
            {
                return null;
            }

            return (Page)activator.Get(type);
        }
    }
}
