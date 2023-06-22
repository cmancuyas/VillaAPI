using VillaUI.Models;

namespace VillaUI.Services.IServices
{
    public interface IBaseService
    {
        APIResponse responseModel { get; set; }
        Task<T> SendAsync<T>(APIRequest aPIRequest);
    }
}
