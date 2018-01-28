    public class NavigationBehavior : BindableBehavior<Page>, INavigationProxy, IPageStateListener
    {
        public static readonly BindableProperty NavigatorProperty =
            BindableProperty.Create(nameof(Navigator), typeof(INavigator), typeof(NavigationBehavior));

        public INavigator Navigator
            get => (INavigator)GetValue(NavigatorProperty);
            set => SetValue(NavigatorProperty, value);

        protected override void OnAttachedTo(Page bindable)
            base.OnAttachedTo(bindable);
            if (Navigator != null)
                Navigator.NavigationProxy = this;

        protected override void OnDetachingFrom(Page bindable)
            base.OnDetachingFrom(bindable);
            if (Navigator != null)
                Navigator.NavigationProxy = null;

----------
        public Task PushAsync<TViewModel>(NavigationParameters parameters, bool animated)
            return PushAsync<TViewModel>(parameters, animated, false, false);

        public Task PushModalAsync<TViewModel>(NavigationParameters parameters, bool animated, bool withNavigationPage)
            return PushAsync<TViewModel>(parameters, animated, withNavigationPage, true);

        private async Task PushAsync<TViewModel>(NavigationParameters parameters, bool animated, bool withNavigationPage, bool useModal)
        {
            var pageProvider = ServiceLocator.Resolve<IPageFactory>();
            var nextPage = pageProvider.CreatePage<TViewModel>(withNavigationPage);

            nextPage.OnActivating(parameters);
            AssociatedObject.OnDeactivating(parameters);

            if (useModal)
                await AssociatedObject.Navigation.PushModalAsync(nextPage, animated);
            else
                await AssociatedObject.Navigation.PushAsync(nextPage, animated);

            nextPage.OnActivated(parameters);
            AssociatedObject.OnDeactivated(parameters);
        }

        public Task PopAsync(bool animated)
            return AssociatedObject.Navigation.PopAsync(animated);

        public Task PopModalAsync(bool animated)
            return AssociatedObject.Navigation.PopModalAsync(animated);

        public async Task PopToRootAsync(bool animated)
            var pages = AssociatedObject.Navigation.NavigationStack.ToList();
            var navigationPage = AssociatedObject.Parent as NavigationPage;
            await AssociatedObject.Navigation.PopToRootAsync(animated);
            navigationPage?.CurrentPage.OnActivated(default(NavigationParameters));

            if (1 < pages.Count)
            {
                pages.RemoveAt(0);
                pages.Reverse();
                foreach (var page in pages)
                {
                    page.OnDisposed();
                }
            }
        }
----------
        public event EventHandler<NavigationEventArgs> Activating;
        public event EventHandler<NavigationEventArgs> Deactivating;
        public event EventHandler<NavigationEventArgs> Activated;
        public event EventHandler<NavigationEventArgs> Deactivated;
        public event EventHandler Disposed;

        public void OnActivating(NavigationParameters parameters)
            Activating?.Invoke(this, new NavigationEventArgs(parameters));

        public void OnDeactivating(NavigationParameters parameters)
            Deactivating?.Invoke(this, new NavigationEventArgs(parameters));

        public void OnActivated(NavigationParameters parameters)
            Activated?.Invoke(this, new NavigationEventArgs(parameters));

        public void OnDeactivated(NavigationParameters parameters)
            Deactivated?.Invoke(this, new NavigationEventArgs(parameters));

        public void OnDisposed()
            Disposed?.Invoke(this, EventArgs.Empty);
