namespace SearchEngineApp.Models
{
    public class SearchEngineConfig
    {
        public string BaseUrl { get; set; } = string.Empty;
    }

    public class SearchSettings
    {
        public int MaxResults { get; set; }
    }

    public class SearchEngineConfigurations
    {
        public SearchEngineConfig Google { get; set; } = new SearchEngineConfig();
        public SearchEngineConfig Bing { get; set; } = new SearchEngineConfig();
    }
}
