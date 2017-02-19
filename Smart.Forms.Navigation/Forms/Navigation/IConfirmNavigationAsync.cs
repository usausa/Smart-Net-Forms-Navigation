namespace Smart.Forms.Navigation
{
    using System.Threading.Tasks;

    public interface IConfirmNavigationAsync
    {
        Task<bool> CanNavigateAsync(NavigationContext context);
    }
}
