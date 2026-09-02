using MunicipalityIssueReporter.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace MunicipalityIssueReporter.Services
{
    public static class IssueReportService
    {
        private static readonly List<IssueReport> reportedIssues =
            new List<IssueReport>();

        private static int nextId = 1;

        public static void AddReport(IssueReport report)
        {
            report.Id = nextId++;
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
