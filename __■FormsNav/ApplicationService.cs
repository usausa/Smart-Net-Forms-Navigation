    public interface IApplicationService
        void SetMainPage<TViewModel>(NavigationParameters parameters = default(NavigationParameters), bool animated = true, bool withNavigationPage = false);

    public class ApplicationService : IApplicationService
    {
        private readonly Application _application;

        private readonly IPageFactory _pageProvider;

        public ApplicationService(Application application, IPageFactory pageProvider)
        {
            _application = application ?? throw new ArgumentNullException(nameof(application));
            _pageProvider = pageProvider ?? throw new ArgumentNullException(nameof(pageProvider));

            _application.ModalPopping += OnModalPopping;
            _application.ModalPopped += OnModalPopped;
        }

        private Page _previousPage;

        private void OnModalPopping(object sender, ModalPoppingEventArgs e)
        {
            int currentIndex = _application.MainPage.Navigation.ModalStack.Count - 1;
            for (; 0 <= currentIndex; currentIndex--)
            {
                var page = _application.MainPage.Navigation.ModalStack[currentIndex];
                if (page == e.Modal)
                {
                    break;
                }
            }
            _previousPage = 0 < currentIndex
                ? _application.MainPage.Navigation.ModalStack[currentIndex - 1]
                : _application.MainPage;
            e.Modal.OnDisposed();
        }

        private void OnModalPopped(object sender, ModalPoppedEventArgs e)
        {
            //var poppedPage = e.Modal;
            var currentPage = _previousPage;
            var parameters = new NavigationParameters();
            currentPage.OnActivated(parameters);
            //poppedPage.OnDisposed(parameters);
        }

        public void SetMainPage<TViewModel>(NavigationParameters parameters = default(NavigationParameters), bool animated = true, bool withNavigationPage = false)
        {
            var currentPage = _application.MainPage;
            var nextPage = _pageProvider.CreatePage<TViewModel>(withNavigationPage);

            nextPage.OnActivating(parameters);
            currentPage.OnDeactivating(parameters);

            _application.MainPage = nextPage;

            nextPage.OnActivated(parameters);
            currentPage.OnDeactivated(parameters);
        }
    }
