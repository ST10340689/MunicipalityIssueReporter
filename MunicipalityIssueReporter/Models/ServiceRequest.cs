using System;

namespace MunicipalityIssueReporter.Models
{
    /// <summary>
    /// Lifecycle stages of a municipal service request.
    /// </summary>
    public enum ServiceRequestStatus
    {
        Submitted = 1,
        Assigned = 2,
        InProgress = 3,
        Resolved = 4
    }

    /// <summary>
    /// Dispatch urgency level for a service request (lower number = higher urgency in Min-Heap).
    /// </summary>
    public enum ServiceRequestPriority
    {
        Urgent = 1,
        High = 2,
        Medium = 3,
        Low = 4
    }

    /// <summary>
    /// Represents a trackable municipal service request (Part 3).
    /// </summary>
    public class ServiceRequest
    {
        public int Id { get; set; }

        public string ReferenceNumber => $"#{Id:D5}";

        public string Location { get; set; } = string.Empty;

        public string Ward { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string AttachmentPath { get; set; } = string.Empty;

        public DateTime DateSubmitted { get; set; }

        public DateTime LastUpdated { get; set; }

        public ServiceRequestStatus Status { get; set; } = ServiceRequestStatus.Submitted;

        public ServiceRequestPriority Priority { get; set; } = ServiceRequestPriority.Medium;

        public string AssignedTeam { get; set; } = string.Empty;

        public string StatusNotes { get; set; } = string.Empty;

        public int ProgressPercentage => Status switch
        {
            ServiceRequestStatus.Submitted => 25,
            ServiceRequestStatus.Assigned => 50,
            ServiceRequestStatus.InProgress => 75,
            ServiceRequestStatus.Resolved => 100,
            _ => 0
        };

        public string StatusDisplay => Status switch
        {
            ServiceRequestStatus.Submitted => "Submitted",
            ServiceRequestStatus.Assigned => "Assigned",
            ServiceRequestStatus.InProgress => "In Progress",
            ServiceRequestStatus.Resolved => "Resolved",
            _ => Status.ToString()
        };

        public override string ToString()
        {
            return $"{ReferenceNumber} [{StatusDisplay}] {Category} - {Location} ({Ward})";
        }
    }

    /// <summary>
    /// Represents an edge in the Minimum Spanning Tree (MST) connecting municipal dispatch zones.
    /// </summary>
    public class MstEdge
    {
        public string FromZone { get; set; } = string.Empty;
        public string ToZone { get; set; } = string.Empty;
        public double DistanceKm { get; set; }
        public int ActiveRequestsInTarget { get; set; }

        public override string ToString()
        {
            return $"{FromZone} -> {ToZone} ({DistanceKm:0.0} km)";
        }
    }
}
