using SearchEngineApp.Models;

namespace SearchEngineApp.Interfaces
{
    public interface ISearchEngine
    {
        Task<SearchResult> SearchAsync(string keywords, string targetUrl);
    }
}
