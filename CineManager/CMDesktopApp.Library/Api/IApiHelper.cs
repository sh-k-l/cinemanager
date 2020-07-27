using System.Net.Http;

namespace CMDesktopApp.Library.Api
{
    public interface IApiHelper
    {
        HttpClient ApiClient { get; }

        void AddAuthToken(string token);
        void ClearDefaultHeaders();
    }
}