namespace MunicipalityIssueReporter.Models
{
    /// <summary>
    /// One search the user ran on the Local Events page.
    /// Kept in a Queue so the recommendation engine can look at recent behaviour.
    /// </summary>
    public class SearchRecord
    {
        public string Keyword { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public DateTime SearchedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// A suggested event plus the reason it was suggested.
    /// </summary>
    public class Recommendation
    {
        public LocalEvent Event { get; set; } = new LocalEvent();

        public int Score { get; set; }

        public string Reason { get; set; } = string.Empty;
    }
}
