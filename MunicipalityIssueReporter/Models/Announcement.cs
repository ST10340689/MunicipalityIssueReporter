namespace MunicipalityIssueReporter.Models
{
    /// <summary>
    /// Priority levels for announcements. Lower value = more important,
    /// which is what PriorityQueue uses to decide what comes out first.
    /// </summary>
    public enum AnnouncementPriority
    {
        Urgent = 1,
        High = 2,
        Normal = 3
    }

    /// <summary>
    /// A municipal announcement such as a water outage or service notice.
    /// </summary>
    public class Announcement
    {
        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime DatePosted { get; set; }

        public AnnouncementPriority Priority { get; set; } = AnnouncementPriority.Normal;
    }
}
