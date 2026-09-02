using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalityIssueReporter.Models
{
    public class IssueReport
    {
        public int Id { get; set; }

        public string Location { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string AttachmentPath { get; set; } = string.Empty;

        public DateTime DateReported { get; set; }
    }
}