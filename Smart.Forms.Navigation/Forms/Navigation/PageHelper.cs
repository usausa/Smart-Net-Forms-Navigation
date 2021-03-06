﻿namespace Smart.Forms.Navigation
{
    using System;
    using System.Threading.Tasks;

    using Smart.Functional;

    using Xamarin.Forms;

    public static class PageHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Ignore")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public static void DestroyPage(Page page)
        {
            ProcessDispose(page);
            Cleanup(page);
        }

        public static void ProcessDispose(object page)
        {
            (page as IDisposable)?.Dispose();
            ((page as BindableObject)?.BindingContext as IDisposable)?.Dispose();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public static void Cleanup(VisualElement element)
        {
            element.Behaviors?.Clear();
            element.Effects?.Clear();
            element.BindingContext = null;

            if (element is Layout<View> layout)
            {
                foreach (var child in layout.Children)
                {
                    Cleanup(child);
                }
            }

            (element as ContentPage)?.Apply(x => Cleanup(x.Content));

            (element as ContentView)?.Apply(x => Cleanup(x.Content));
        }

        public static Task<bool> ProcessCanNavigateAsync(object page, NavigationContext context)
        {
            if (page is IConfirmNavigationAsync confirmNavigation)
            {
                return confirmNavigation.CanNavigateAsync(context);
            }

            confirmNavigation = (page as BindableObject)?.BindingContext as IConfirmNavigationAsync;
            if (confirmNavigation != null)
            {
                return confirmNavigation.CanNavigateAsync(context);
            }

            return Task.FromResult(ProcessCanNavigate(page, context));
        }

        public static bool ProcessCanNavigate(object page, NavigationContext context)
        {
            if (page is IConfirmNavigation confirmNavigation)
            {
                return confirmNavigation.CanNavigate(context);
            }

            confirmNavigation = (page as BindableObject)?.BindingContext as IConfirmNavigation;
            if (confirmNavigation != null)
            {
                return confirmNavigation.CanNavigate(context);
            }

            return true;
        }

        public static void ProcessNavigatedFrom(object page, NavigationContext context)
        {
            (page as INavigationAware)?.OnNavigatedFrom(context);
            ((page as BindableObject)?.BindingContext as INavigationAware)?.OnNavigatedFrom(context);
        }

        public static void ProcessNavigatingTo(object page, NavigationContext context)
        {
            (page as INavigationAware)?.OnNavigatingTo(context);
            ((page as BindableObject)?.BindingContext as INavigationAware)?.OnNavigatingTo(context);
        }

        public static void ProcessNavigatedTo(object page, NavigationContext context)
        {
            (page as INavigationAware)?.OnNavigatedTo(context);
            ((page as BindableObject)?.BindingContext as INavigationAware)?.OnNavigatedTo(context);
        }
    }
}
