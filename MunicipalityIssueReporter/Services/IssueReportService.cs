using MunicipalityIssueReporter.Models;
using System.Collections.Generic;

namespace MunicipalityIssueReporter.Services
{
    public static class IssueReportService
    {
        private static readonly List<IssueReport> reportedIssues =
            new List<IssueReport>();

        public static void AddReport(IssueReport report)
        {
            // Register in Part 3 ServiceRequestService as well so newly reported issues
            // can be tracked immediately by their unique Reference Number in Service Request Status.
            ServiceRequestService.RegisterFromIssueReport(report);
            reportedIssues.Add(report);
        }

        public static List<IssueReport> GetReports()
        {
            return reportedIssues;
        }

        public static int GetReportCount()
        {
            return reportedIssues.Count;
        }
    }
}
