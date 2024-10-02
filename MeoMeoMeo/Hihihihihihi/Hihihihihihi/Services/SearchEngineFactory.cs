using Microsoft.Extensions.DependencyInjection;
using SearchEngineApp.Interfaces;
using System;

namespace SearchEngineApp.Services
{
    public class SearchEngineFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SearchEngineFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISearchEngine GetSearchEngine(string engine)
        {
            if (!Enum.TryParse<SearchEngineType>(engine, true, out var engineType))
            {
                throw new NotSupportedException($"Search engine '{engine}' is not supported.");
            }

            return engineType switch
            {
                SearchEngineType.Google => _serviceProvider.GetRequiredService<GoogleSearchEngine>(),
                SearchEngineType.Bing => _serviceProvider.GetRequiredService<BingSearchEngine>(),
                _ => throw new NotSupportedException($"Search engine '{engineType}' is not supported.")
            };
        }

    }
}
