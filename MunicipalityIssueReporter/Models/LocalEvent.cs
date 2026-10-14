namespace MunicipalityIssueReporter.Models
{
    /// <summary>
    /// A local community event hosted or promoted by the municipality.
    /// </summary>
    public class LocalEvent
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public string Venue { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Date:dd MMM yyyy} - {Title}";
        }
    }
}
