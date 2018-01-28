    public interface IPageStateListener
        void OnActivating(NavigationParameters parameters);
        void OnDeactivating(NavigationParameters parameters);
        void OnActivated(NavigationParameters parameters);
        void OnDeactivated(NavigationParameters parameters);
        void OnDisposed();
----------
    public class NavigationPageBehavior : BindableBehavior<NavigationPage>
    {
        protected override void OnAttachedTo(NavigationPage bindableObject)
            bindableObject.Popped += OnPopped;
            base.OnAttachedTo(bindableObject);

        protected override void OnDetachingFrom(NavigationPage bindableObject)
            bindableObject.Popped += OnPopped;
            base.OnDetachingFrom(bindableObject);

        private void OnPopped(object sender, Xamarin.Forms.NavigationEventArgs e)
        {
            AssociatedObject.OnActivated(default(NavigationParameters));
            e.Page.OnDisposed();
        }
----------
    public class PageFactory : IPageFactory
    {
        public Page CreatePage<TViewModel>(bool withNavigationPage)
...
            if (withNavigationPage)
                page = new NavigationPage(page);

            if (page is NavigationPage navigationPage)
                navigationPage.InjectionNavigationPageBehavior();

            return page;
        }
----------
    public interface INavigator : IPageStateListener
    {
        INavigationProxy NavigationProxy { get; set; }

        event EventHandler<NavigationEventArgs> Activating;

        event EventHandler<NavigationEventArgs> Deactivating;

        event EventHandler<NavigationEventArgs> Activated;

        event EventHandler<NavigationEventArgs> Deactivated;

        event EventHandler Disposed;

        void SetMainPage<TViewModel>(NavigationParameters parameters = default(NavigationParameters), bool animated = true, bool withNavigationPage = false);

        Task PushAsync<TViewModel>(NavigationParameters parameters = default(NavigationParameters), bool animated = true);

        Task PushModalAsync<TViewModel>(NavigationParameters parameters = default(NavigationParameters), bool animated = true, bool withNavigationPage = false);

        Task PopAsync(bool animated = true);
        Task PopModalAsync(bool animated = true);
        Task PopToRootAsync(bool animated = true);
    }
}
----------

namespace Mercury
{
    public class Navigator : INavigator
    {
        public INavigationProxy NavigationProxy { get; set; }

        public event EventHandler<NavigationEventArgs> Activating;
        public event EventHandler<NavigationEventArgs> Deactivating;
        public event EventHandler<NavigationEventArgs> Activated;
        public event EventHandler<NavigationEventArgs> Deactivated;
        public event EventHandler Disposed;

        private readonly IApplicationService _applicationService;

        public Navigator(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        public void SetMainPage<TViewModel>(NavigationParameters parameters = default(NavigationParameters), bool animated = true, bool withNavigationPage = false)
        {
            _applicationService.SetMainPage<TViewModel>();
        }

        public Task PushAsync<TViewModel>(NavigationParameters parameters = default(NavigationParameters), bool animated = true)
        {
            return NavigationProxy.PushAsync<TViewModel>(parameters, animated);
        }

        public Task PushModalAsync<TViewModel>(NavigationParameters parameters = default(NavigationParameters), bool animated = true, bool withNavigationPage = false)
        {
            return NavigationProxy.PushModalAsync<TViewModel>(parameters, animated, withNavigationPage);
        }

        public Task PopAsync(bool animated = true)
        {
            return NavigationProxy.PopAsync(animated);
        }

        public Task PopModalAsync(bool animated = true)
        {
            return NavigationProxy.PopModalAsync(animated);
        }

        public Task PopToRootAsync(bool animated = true)
        {
            return NavigationProxy.PopToRootAsync(animated);
        }

        public void OnActivating(NavigationParameters parameters)
        {
            Activating?.Invoke(this, new NavigationEventArgs(parameters));
        }

        public void OnDeactivating(NavigationParameters parameters)
        {
            Deactivating?.Invoke(this, new NavigationEventArgs(parameters));
        }

        public void OnActivated(NavigationParameters parameters)
        {
            Activated?.Invoke(this, new NavigationEventArgs(parameters));
        }

        public void OnDeactivated(NavigationParameters parameters)
        {
            Deactivated?.Invoke(this, new NavigationEventArgs(parameters));
        }

        public void OnDisposed()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
