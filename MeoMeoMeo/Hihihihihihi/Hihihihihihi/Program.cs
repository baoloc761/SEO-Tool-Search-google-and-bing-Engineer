using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SearchEngineApp.Services;
using SearchEngineApp.Models;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        var serviceProvider = new ServiceCollection()
            .Configure<SearchEngineConfigurations>(config.GetSection("SearchEngines"))
            .Configure<SearchSettings>(config.GetSection("SearchSettings"))
            .AddMemoryCache()
            .AddHttpClient()
            .AddTransient<GoogleSearchEngine>()
            .AddTransient<BingSearchEngine>()
            .AddTransient<SearchEngineFactory>()
            .AddSingleton<SearchCacheService>()
            .BuildServiceProvider();

        var searchEngineFactory = serviceProvider.GetService<SearchEngineFactory>();
        var cacheService = serviceProvider.GetService<SearchCacheService>();

        Console.WriteLine("Nhập từ khóa:");
        string keywords = Console.ReadLine();

        Console.WriteLine("Nhập URL để tìm:");
        string targetUrl = Console.ReadLine();

        Console.WriteLine("Chọn công cụ tìm kiếm (google hoặc bing):");
        string engineChoice = Console.ReadLine();

        var cachedResult = cacheService.GetCachedResult(keywords);
        if (cachedResult != null)
        {
            Console.WriteLine($"Kết quả từ bộ nhớ đệm: URL tìm thấy tại vị trí: {string.Join(", ", cachedResult.Positions)}");
        }
        else
        {
            var searchEngine = searchEngineFactory.GetSearchEngine(engineChoice);
            var result = await searchEngine.SearchAsync(keywords, targetUrl);

            if (result.Positions.Count > 0)
            {
                Console.WriteLine($"URL tìm thấy tại vị trí: {string.Join(", ", result.Positions)}");
            }
            else
            {
                Console.WriteLine("URL không xuất hiện trong 100 kết quả đầu tiên.");
            }

            cacheService.SetCachedResult(keywords, result);
        }
    }
}
