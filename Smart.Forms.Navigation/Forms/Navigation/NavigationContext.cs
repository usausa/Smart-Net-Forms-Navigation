namespace Smart.Forms.Navigation
{
    public class NavigationContext
    {
        public NavigationParameters Parameters { get; }

        public bool IsPopBack { get; }

        public string Name { get; }

        public string PreviousName { get; }

        public NavigationContext(NavigationParameters parameters, bool isPopBack, string name, string previousName)
        {
            Parameters = parameters;
            IsPopBack = isPopBack;
            Name = name;
            PreviousName = previousName;
        }
    }
}
