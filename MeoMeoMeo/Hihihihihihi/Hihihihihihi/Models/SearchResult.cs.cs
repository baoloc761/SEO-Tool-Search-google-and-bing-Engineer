namespace SearchEngineApp.Models
{
    public class SearchResult
    {
        public List<int> Positions { get; set; } = new List<int>();
        public bool IsFromCache { get; set; }
    }
}
