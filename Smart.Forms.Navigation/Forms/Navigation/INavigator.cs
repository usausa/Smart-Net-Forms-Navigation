namespace Smart.Forms.Navigation
{
    using System.Threading.Tasks;

    public interface INavigator
    {
        Task<bool> ForwardAsync(string name, NavigationParameters parameters = null);

        Task<bool> PushModelAsync(string name, NavigationParameters parameters = null);

        Task<bool> PopModalAsync(NavigationParameters parameters = null);
    }
}
