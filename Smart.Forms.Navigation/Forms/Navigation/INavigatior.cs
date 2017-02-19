namespace Smart.Forms.Navigation
{
    using System.Threading.Tasks;

    public interface INavigatior
    {
        Task<bool> Forward(string name);

        Task<bool> Forward(string name, NavigationParameters parameters);

        Task<bool> PushModel(string name);

        Task<bool> PushModel(string name, NavigationParameters parameters);

        Task<bool> PopModal();

        Task<bool> PopModal(NavigationParameters parameters);
    }
}
