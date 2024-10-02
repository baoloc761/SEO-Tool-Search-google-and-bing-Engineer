using SearchEngineApp.Interfaces;
using SearchEngineApp.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace SearchEngineApp.Services
{
    public class GoogleSearchEngine : ISearchEngine
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public GoogleSearchEngine(HttpClient httpClient, IOptions<SearchEngineConfigurations> config)
        {
            _httpClient = httpClient;
            _baseUrl = config.Value.Google.BaseUrl;
        }

        public async Task<SearchResult> SearchAsync(string keywords, string targetUrl)
        {
            var positions = new List<int>();
            string query = $"{_baseUrl}{Uri.EscapeDataString(keywords)}";
            string htmlContent = await _httpClient.GetStringAsync(query);

            int position = 1;
            foreach (var url in ExtractUrls(htmlContent))
            {
                if (url.Contains(targetUrl))
                {
                    positions.Add(position);
                }
                if (position >= 100) break;
                position++;
            }

            return new SearchResult { Positions = positions };
        }

        private IEnumerable<string> ExtractUrls(string htmlContent)
        {
            var urls = new List<string>();
            var regex = new Regex(@"https?://[\w\.-]+");
            var matches = regex.Matches(htmlContent);
            foreach (Match match in matches)
            {
                urls.Add(match.Value);
            }
            return urls;
        }
    }
}
