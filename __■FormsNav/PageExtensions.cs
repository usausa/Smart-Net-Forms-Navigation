using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mercury
{
    public static class PageExtensions
    {
        public static void InjectionNavigationPageBehavior(this NavigationPage navigationPage)
        {
            var navigationBehavior = navigationPage.GetBehavior<NavigationPageBehavior>();
            if (navigationBehavior == null)
            {
                navigationPage.Behaviors.Add(new NavigationPageBehavior());
            }
        }
        public static void OnActivating(this Page page, NavigationParameters parameters)
        {
            page.NotifyNavigation(
                parameters, 
                (listener, navigationParameters) => listener?.OnActivating(parameters));
        }

        public static void OnDeactivating(this Page page, NavigationParameters parameters)
        {
            page.NotifyNavigation(
                parameters,
                (listener, navigationParameters) => listener?.OnDeactivating(parameters));
        }

        public static void OnActivated(this Page page, NavigationParameters parameters)
        {
            page.NotifyReverseNavigation(
                parameters,
                (listener, navigationParameters) => listener?.OnActivated(parameters));
        }

        public static void OnDeactivated(this Page page, NavigationParameters parameters)
        {
            page.NotifyReverseNavigation(
                parameters,
                (listener, navigationParameters) => listener?.OnDeactivated(parameters));
        }

        public static void OnDisposed(this Page page)
        {
            page.NotifyReverseNavigation(
                null,
                (listener, navigationParameters) => listener?.OnDisposed());
        }

        private static void NotifyNavigation(this Page page, NavigationParameters parameters, Action<IPageStateListener, NavigationParameters> notifyAction)
        {
            var behavior = page.GetBehavior<NavigationBehavior>();
            notifyAction(behavior, parameters);
            notifyAction(behavior?.Navigator, parameters);
            switch (page)
            {
                case NavigationPage navigationPage:
                    navigationPage.Navigation.NavigationStack.LastOrDefault().NotifyNavigation(parameters, notifyAction);
                    break;
            }
        }

        private static void NotifyReverseNavigation(this Page page, NavigationParameters parameters, Action<IPageStateListener, NavigationParameters> notifyAction)
        {
            switch (page)
            {
                case NavigationPage navigationPage:
                    navigationPage.Navigation.NavigationStack.LastOrDefault().NotifyNavigation(parameters, notifyAction);
                    break;
            }
            var behavior = page.GetBehavior<NavigationBehavior>();
            notifyAction(behavior?.Navigator, parameters);
            notifyAction(behavior, parameters);
        }

        public static T GetBehavior<T>(this Page page) where T : Behavior
        {
            if (page == null) return null;

            foreach (var behavior in page.Behaviors)
            {
                if (behavior is T navigationBehavior)
                {
                    return navigationBehavior;
                }
            }
            return null;
        }
    }
}
