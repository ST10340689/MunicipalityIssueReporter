using MunicipalityIssueReporter.DataStructures;
using MunicipalityIssueReporter.Models;
using System;
using System.Collections.Generic;

namespace MunicipalityIssueReporter.Services
{
    /// <summary>
    /// Coordinates all Part 3 data structures for the Service Request Status feature:
    /// - Basic Tree (BasicTreeNode): Department -> Category -> ServiceRequest hierarchy
    /// - Binary Tree / BST / Self-Balancing AVL Tree (ServiceRequestAvlTree): O(log n) lookup by unique ID
    /// - Red-Black Tree (ServiceRequestRedBlackTree): Chronological ordering by DateSubmitted
    /// - Min-Heap (ServiceRequestMinHeap): Priority dispatch queue for active requests
    /// - Weighted Graph, BFS, DFS & Prim's MST (ServiceRequestGraph): Zone dispatch network & related requests
    /// </summary>
    public static class ServiceRequestService
    {
        private static readonly ServiceRequestAvlTree avlById = new ServiceRequestAvlTree();
        private static readonly ServiceRequestRedBlackTree redBlackByDate = new ServiceRequestRedBlackTree();
        private static readonly BasicTreeNode hierarchyRoot = new BasicTreeNode("All Municipal Requests", "Root");
        private static readonly ServiceRequestMinHeap dispatchMinHeap = new ServiceRequestMinHeap();
        private static readonly ServiceRequestGraph zoneGraph = new ServiceRequestGraph();

        private static int nextId = 1;

        static ServiceRequestService()
        {
            InitializeGraphTopology();
            SeedRequests();
        }

        public static BasicTreeNode GetHierarchyRoot() => hierarchyRoot;

        public static int TotalCount => avlById.Count;

        public static int AvlTreeHeight => avlById.TreeHeight;

        /// <summary>
        /// Returns all service requests sorted by unique ID using an In-Order traversal of the AVL Binary Search Tree.
        /// </summary>
        public static List<ServiceRequest> GetAllByIdOrder()
        {
            return avlById.InOrderTraversal();
        }

        /// <summary>
        /// Returns all service requests ordered newest-first using the Red-Black Tree.
        /// </summary>
        public static List<ServiceRequest> GetAllNewestFirst()
        {
            return redBlackByDate.GetNewestFirst();
        }

        /// <summary>
        /// Looks up a service request by its unique identifier (e.g. "5", "#00005", "SR-00005")
        /// using the self-balancing AVL Binary Search Tree in O(log n) time.
        /// </summary>
        public static ServiceRequest? FindById(int id, out List<int> visitedAvlPath)
        {
            return avlById.SearchById(id, out visitedAvlPath);
        }

        /// <summary>
        /// Parses a user-entered reference string (such as "5", "#00005", or "00005") into an integer ID.
        /// </summary>
        public static bool TryParseReferenceId(string input, out int id)
        {
            id = 0;
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            string cleaned = input.Trim();
            if (cleaned.StartsWith("SR-", StringComparison.OrdinalIgnoreCase))
            {
                cleaned = cleaned.Substring(3).Trim();
            }
            if (cleaned.StartsWith('#'))
            {
                cleaned = cleaned.Substring(1).Trim();
            }

            return int.TryParse(cleaned, out id) && id > 0;
        }

        /// <summary>
        /// Filters service requests by optional category/department tree node, status, priority, and keyword/ID query.
        /// </summary>
        public static List<ServiceRequest> FilterRequests(
            BasicTreeNode? selectedTreeNode,
            string? statusFilter,
            string? priorityFilter,
            string? searchQuery,
            out List<int> visitedAvlPath)
        {
            visitedAvlPath = new List<int>();

            // If the user typed a valid numeric ID / Reference Number (e.g. "#00005" or "5"),
            // use the AVL Binary Search Tree directly for O(log n) lookup.
            if (!string.IsNullOrWhiteSpace(searchQuery) && TryParseReferenceId(searchQuery, out int targetId))
            {
                ServiceRequest? exact = avlById.SearchById(targetId, out visitedAvlPath);
                if (exact != null)
                {
                    return new List<ServiceRequest> { exact };
                }
                return new List<ServiceRequest>();
            }

            List<ServiceRequest> source = selectedTreeNode != null && selectedTreeNode != hierarchyRoot
                ? selectedTreeNode.CollectAllRequests()
                : avlById.InOrderTraversal();

            bool filterStatus = !string.IsNullOrWhiteSpace(statusFilter) &&
                                !string.Equals(statusFilter, "All Statuses", StringComparison.OrdinalIgnoreCase);

            bool filterPriority = !string.IsNullOrWhiteSpace(priorityFilter) &&
                                  !string.Equals(priorityFilter, "All Priorities", StringComparison.OrdinalIgnoreCase);

            string keyword = searchQuery?.Trim() ?? string.Empty;
            bool hasKeyword = keyword.Length > 0;

            List<ServiceRequest> results = new List<ServiceRequest>();
            foreach (ServiceRequest req in source)
            {
                if (filterStatus && !string.Equals(req.StatusDisplay, statusFilter, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (filterPriority && !string.Equals(req.Priority.ToString(), priorityFilter, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (hasKeyword)
                {
                    bool matches =
                        req.ReferenceNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                        req.Location.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                        req.Ward.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                        req.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                        req.Department.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                        req.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                        req.AssignedTeam.Contains(keyword, StringComparison.OrdinalIgnoreCase);

                    if (!matches)
                    {
                        continue;
                    }
                }

                results.Add(req);
            }

            results.Sort((a, b) => a.Id.CompareTo(b.Id));
            return results;
        }

        /// <summary>
        /// Returns active (non-resolved) service requests ordered by the Min-Heap dispatch priority queue.
        /// </summary>
        public static List<ServiceRequest> GetPriorityDispatchQueue()
        {
            return dispatchMinHeap.GetOrderedSnapshot();
        }

        /// <summary>
        /// Uses Breadth-First Search (BFS) on the zone graph to return related requests in the same or adjacent wards.
        /// </summary>
        public static List<ServiceRequest> GetRelatedRequestsBfs(ServiceRequest request, int maxCount = 4)
        {
            return zoneGraph.GetNearbyRelatedRequestsBfs(request, maxCount);
        }

        /// <summary>
        /// Returns the Breadth-First Search (BFS) zone visit order starting from a given ward/zone.
        /// </summary>
        public static List<string> GetBfsZoneTraversal(string startZone)
        {
            return zoneGraph.BreadthFirstSearch(startZone);
        }

        /// <summary>
        /// Returns the Depth-First Search (DFS) zone visit order starting from a given ward/zone.
        /// </summary>
        public static List<string> GetDfsZoneTraversal(string startZone)
        {
            return zoneGraph.DepthFirstSearch(startZone);
        }

        /// <summary>
        /// Computes the Minimum Spanning Tree (Prim's Algorithm) across all municipal zones from Civic Depot.
        /// </summary>
        public static List<MstEdge> GetMinimumSpanningTree(out double totalDistanceKm)
        {
            return zoneGraph.ComputeMinimumSpanningTree(out totalDistanceKm);
        }

        /// <summary>
        /// Advances the status of a service request to the next stage (Submitted -> Assigned -> In Progress -> Resolved)
        /// and rebuilds the Min-Heap and Graph active counts to reflect the update.
        /// </summary>
        public static bool AdvanceStatus(int requestId)
        {
            ServiceRequest? req = avlById.SearchById(requestId, out _);
            if (req == null || req.Status == ServiceRequestStatus.Resolved)
            {
                return false;
            }

            req.Status = req.Status switch
            {
                ServiceRequestStatus.Submitted => ServiceRequestStatus.Assigned,
                ServiceRequestStatus.Assigned => ServiceRequestStatus.InProgress,
                ServiceRequestStatus.InProgress => ServiceRequestStatus.Resolved,
                _ => ServiceRequestStatus.Resolved
            };

            req.LastUpdated = DateTime.Now;
            req.StatusNotes = req.Status switch
            {
                ServiceRequestStatus.Assigned => $"Assigned to {req.AssignedTeam} for field inspection.",
                ServiceRequestStatus.InProgress => $"{req.AssignedTeam} is on-site resolving the issue.",
                ServiceRequestStatus.Resolved => $"Completed and verified by {req.AssignedTeam}.",
                _ => req.StatusNotes
            };

            RebuildHeapAndGraphAssignments();
            return true;
        }

        /// <summary>
        /// Converts a newly submitted Part 1 IssueReport into a Part 3 ServiceRequest and indexes it
        /// across all trees, the Min-Heap, and the municipal zone graph.
        /// </summary>
        public static ServiceRequest RegisterFromIssueReport(IssueReport report)
        {
            string department = MapDepartment(report.Category);
            string ward = InferWardFromLocation(report.Location);
            ServiceRequestPriority priority = InferPriority(report.Category, report.Description);
            string team = MapAssignedTeam(department);

            ServiceRequest request = new ServiceRequest
            {
                Id = nextId++,
                Location = report.Location,
                Ward = ward,
                Category = report.Category,
                Department = department,
                Description = report.Description,
                AttachmentPath = report.AttachmentPath,
                DateSubmitted = report.DateReported == default ? DateTime.Now : report.DateReported,
                LastUpdated = DateTime.Now,
                Status = ServiceRequestStatus.Submitted,
                Priority = priority,
                AssignedTeam = team,
                StatusNotes = "Logged via Citizen Issue Reporter portal; awaiting dispatch."
            };

            report.Id = request.Id;
            AddInternal(request);
            return request;
        }

        public static int PeekNextId() => nextId;

        private static void AddInternal(ServiceRequest request)
        {
            // 1. Insert into Self-Balancing AVL Binary Search Tree (keyed by ID)
            avlById.Insert(request);

            // 2. Insert into Red-Black Tree (keyed by DateSubmitted, ID)
            redBlackByDate.Insert(request);

            // 3. Insert into Basic Tree hierarchy (Root -> Department -> Category -> Request)
            BasicTreeNode deptNode = hierarchyRoot.GetOrAddChild(request.Department, "Department");
            BasicTreeNode catNode = deptNode.GetOrAddChild(request.Category, "Category");
            catNode.Requests.Add(request);

            // 4. Insert into Min-Heap if still active (unresolved)
            if (request.Status != ServiceRequestStatus.Resolved)
            {
                dispatchMinHeap.Insert(request);
            }

            // 5. Assign to municipal zone vertex in the Graph
            zoneGraph.AssignRequestToZone(request);
        }

        private static void RebuildHeapAndGraphAssignments()
        {
            dispatchMinHeap.Clear();
            zoneGraph.ClearRequests();

            foreach (ServiceRequest req in avlById.InOrderTraversal())
            {
                if (req.Status != ServiceRequestStatus.Resolved)
                {
                    dispatchMinHeap.Insert(req);
                }
                zoneGraph.AssignRequestToZone(req);
            }
        }

        private static string MapDepartment(string category)
        {
            return category switch
            {
                "Water" or "Sanitation" => "Water & Sanitation",
                "Roads" => "Roads & Transport",
                "Electricity" => "Energy & Electricity",
                "Waste Management" or "Public Safety" or "Other" => "Waste & Public Safety",
                _ => "General Municipal Services"
            };
        }

        private static string MapAssignedTeam(string department)
        {
            return department switch
            {
                "Water & Sanitation" => "Rapid Plumbing & Water Unit",
                "Roads & Transport" => "Civil Roads Maintenance Crew",
                "Energy & Electricity" => "High-Voltage Electrical Team",
                "Waste & Public Safety" => "Environmental & By-Law Unit",
                _ => "General Field Response Team"
            };
        }

        private static string InferWardFromLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return "Ward 1 - CBD";
            }

            if (location.Contains("Oak", StringComparison.OrdinalIgnoreCase) ||
                location.Contains("Ward 4", StringComparison.OrdinalIgnoreCase))
            {
                return "Ward 4 - Oak Road";
            }

            if (location.Contains("River", StringComparison.OrdinalIgnoreCase) ||
                location.Contains("Ward 7", StringComparison.OrdinalIgnoreCase))
            {
                return "Ward 7 - Riverside";
            }

            if (location.Contains("Green", StringComparison.OrdinalIgnoreCase) ||
                location.Contains("Ward 8", StringComparison.OrdinalIgnoreCase))
            {
                return "Ward 8 - Greenfield";
            }

            if (location.Contains("Indust", StringComparison.OrdinalIgnoreCase) ||
                location.Contains("Ward 10", StringComparison.OrdinalIgnoreCase))
            {
                return "Ward 10 - Industrial";
            }

            if (location.Contains("Ward 12", StringComparison.OrdinalIgnoreCase) ||
                location.Contains("Township", StringComparison.OrdinalIgnoreCase))
            {
                return "Ward 12 - Township";
            }

            return "Ward 1 - CBD";
        }

        private static ServiceRequestPriority InferPriority(string category, string description)
        {
            if (description.Contains("burst", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("live wire", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("sewage", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("danger", StringComparison.OrdinalIgnoreCase))
            {
                return ServiceRequestPriority.Urgent;
            }

            return category switch
            {
                "Electricity" or "Water" => ServiceRequestPriority.High,
                "Sanitation" or "Roads" or "Public Safety" => ServiceRequestPriority.Medium,
                _ => ServiceRequestPriority.Low
            };
        }

        private static void InitializeGraphTopology()
        {
            // Weighted undirected graph representing road distances (km) between the Civic Depot and Municipal Wards
            zoneGraph.AddRoadConnection(ServiceRequestGraph.CentralDepot, "Ward 1 - CBD", 2.4);
            zoneGraph.AddRoadConnection(ServiceRequestGraph.CentralDepot, "Ward 4 - Oak Road", 4.1);
            zoneGraph.AddRoadConnection("Ward 1 - CBD", "Ward 4 - Oak Road", 2.8);
            zoneGraph.AddRoadConnection("Ward 1 - CBD", "Ward 7 - Riverside", 3.5);
            zoneGraph.AddRoadConnection("Ward 4 - Oak Road", "Ward 8 - Greenfield", 3.2);
            zoneGraph.AddRoadConnection("Ward 7 - Riverside", "Ward 8 - Greenfield", 4.6);
            zoneGraph.AddRoadConnection("Ward 7 - Riverside", "Ward 10 - Industrial", 3.9);
            zoneGraph.AddRoadConnection("Ward 8 - Greenfield", "Ward 12 - Township", 2.9);
            zoneGraph.AddRoadConnection("Ward 10 - Industrial", "Ward 12 - Township", 5.4);
        }

        private static void SeedRequests()
        {
            DateTime now = DateTime.Now;

            ServiceRequest[] initial =
            {
                new ServiceRequest
                {
                    Id = nextId++,
                    Location = "14 Oak Road, Near Primary School",
                    Ward = "Ward 4 - Oak Road",
                    Category = "Water",
                    Department = "Water & Sanitation",
                    Description = "Major burst main water pipe flooding the intersection and reducing pressure.",
                    DateSubmitted = now.AddDays(-5).AddHours(-3),
                    LastUpdated = now.AddHours(-2),
                    Status = ServiceRequestStatus.InProgress,
                    Priority = ServiceRequestPriority.Urgent,
                    AssignedTeam = "Rapid Plumbing & Water Unit",
                    StatusNotes = "Excavation complete; replacement pipe section being fitted."
                },
                new ServiceRequest
                {
                    Id = nextId++,
                    Location = "Corner Main & Church Street",
                    Ward = "Ward 1 - CBD",
                    Category = "Electricity",
                    Department = "Energy & Electricity",
                    Description = "Traffic light controller fault causing all-way flashing red at peak hours.",
                    DateSubmitted = now.AddDays(-6).AddHours(-1),
                    LastUpdated = now.AddDays(-1),
                    Status = ServiceRequestStatus.Resolved,
                    Priority = ServiceRequestPriority.High,
                    AssignedTeam = "High-Voltage Electrical Team",
                    StatusNotes = "Replaced blown relay board and synchronised signal timing."
                },
                new ServiceRequest
                {
                    Id = nextId++,
                    Location = "88 Riverside Drive",
                    Ward = "Ward 7 - Riverside",
                    Category = "Roads",
                    Department = "Roads & Transport",
                    Description = "Deep pothole cluster on the approach to the low-level bridge.",
                    DateSubmitted = now.AddDays(-4).AddHours(-5),
                    LastUpdated = now.AddDays(-1).AddHours(-4),
                    Status = ServiceRequestStatus.Assigned,
                    Priority = ServiceRequestPriority.High,
                    AssignedTeam = "Civil Roads Maintenance Crew",
                    StatusNotes = "Hot-mix asphalt scheduled for application on next dry shift."
                },
                new ServiceRequest
                {
                    Id = nextId++,
                    Location = "Greenfield Park Walkway",
                    Ward = "Ward 8 - Greenfield",
                    Category = "Public Safety",
                    Department = "Waste & Public Safety",
                    Description = "Four consecutive street lights out along pedestrian commuter path.",
                    DateSubmitted = now.AddDays(-4).AddHours(-2),
                    LastUpdated = now.AddHours(-6),
                    Status = ServiceRequestStatus.InProgress,
                    Priority = ServiceRequestPriority.High,
                    AssignedTeam = "High-Voltage Electrical Team",
                    StatusNotes = "Cherry-picker truck on site replacing LED luminaires."
                },
                new ServiceRequest
                {
                    Id = nextId++,
                    Location = "42 Foundry Avenue",
                    Ward = "Ward 10 - Industrial",
                    Category = "Sanitation",
                    Department = "Water & Sanitation",
                    Description = "Blocked municipal storm-water and sewer manhole overflowing onto verge.",
                    DateSubmitted = now.AddDays(-3).AddHours(-7),
                    LastUpdated = now.AddHours(-5),
                    Status = ServiceRequestStatus.Assigned,
                    Priority = ServiceRequestPriority.Urgent,
                    AssignedTeam = "Rapid Plumbing & Water Unit",
                    StatusNotes = "High-pressure jetting truck dispatched to clear blockage."
                },
                new ServiceRequest
                {
                    Id = nextId++,
                    Location = "19 Mandela Street",
                    Ward = "Ward 12 - Township",
                    Category = "Waste Management",
                    Department = "Waste & Public Safety",
                    Description = "Illegal dumping of building rubble and household refuse on open corner.",
                    DateSubmitted = now.AddDays(-3).AddHours(-4),
                    LastUpdated = now.AddDays(-2),
                    Status = ServiceRequestStatus.Submitted,
                    Priority = ServiceRequestPriority.Medium,
                    AssignedTeam = "Environmental & By-Law Unit",
                    StatusNotes = "Logged and queued for front-end loader and tipper truck clearance."
                },
                new ServiceRequest
                {
                    Id = nextId++,
                    Location = "7 Market Square",
                    Ward = "Ward 1 - CBD",
                    Category = "Water",
                    Department = "Water & Sanitation",
                    Description = "Leaking fire hydrant valve pooling water across pavement.",
                    DateSubmitted = now.AddDays(-3).AddHours(-1),
                    LastUpdated = now.AddDays(-1),
                    Status = ServiceRequestStatus.Resolved,
                    Priority = ServiceRequestPriority.Medium,
                    AssignedTeam = "Rapid Plumbing & Water Unit",
                    StatusNotes = "Reseated hydrant spindle and pressure-tested seal."
                },
                new ServiceRequest
                {
                    Id = nextId++,
                    Location = "Substation B, Acacia Lane",
                    Ward = "Ward 4 - Oak Road",
                    Category = "Electricity",
                    Department = "Energy & Electricity",
                    Description = "Exposed low-hanging overhead service cable after wind storm.",
                    DateSubmitted = now.AddDays(-2).AddHours(-6),
                    LastUpdated = now.AddHours(-3),
                    Status = ServiceRequestStatus.Submitted,
                    Priority = ServiceRequestPriority.Urgent,
                    AssignedTeam = "High-Voltage Electrical Team",
                    StatusNotes = "Emergency isolation requested; priority dispatch pending."
                },
                new ServiceRequest
                {
                    Id = nextId++,
                    Location = "Riverside Clinic Access Road",
                    Ward = "Ward 7 - Riverside",
                    Category = "Roads",
                    Department = "Roads & Transport",
                    Description = "Faded pedestrian crossing markings and missing stop sign outside clinic.",
                    DateSubmitted = now.AddDays(-2).AddHours(-3),
                    LastUpdated = now.AddDays(-1),
                    Status = ServiceRequestStatus.Assigned,
                    Priority = ServiceRequestPriority.Medium,
                    AssignedTeam = "Civil Roads Maintenance Crew",
                    StatusNotes = "Signpost and road-marking paint requisitioned from stores."
                },
                new ServiceRequest
                {
                    Id = nextId++,
                    Location = "105 Protea Crescent",
                    Ward = "Ward 8 - Greenfield",
                    Category = "Waste Management",
                    Department = "Waste & Public Safety",
                    Description = "Missed weekly wheelie-bin refuse collection for entire cul-de-sac.",
                    DateSubmitted = now.AddDays(-2),
                    LastUpdated = now.AddHours(-8),
                    Status = ServiceRequestStatus.InProgress,
                    Priority = ServiceRequestPriority.Low,
                    AssignedTeam = "Environmental & By-Law Unit",
                    StatusNotes = "Catch-up compactor truck currently servicing Ward 8."
                },
                new ServiceRequest
                {
                    Id = nextId++,
                    Location = "12 Steelworks Road",
                    Ward = "Ward 10 - Industrial",
                    Category = "Roads",
                    Department = "Roads & Transport",
                    Description = "Collapsed storm-water kerb inlet grate posing hazard to heavy vehicles.",
                    DateSubmitted = now.AddDays(-1).AddHours(-9),
                    LastUpdated = now.AddHours(-7),
                    Status = ServiceRequestStatus.Submitted,
                    Priority = ServiceRequestPriority.High,
                    AssignedTeam = "Civil Roads Maintenance Crew",
                    StatusNotes = "Barricaded by traffic wardens; concrete slab replacement queued."
                },
                new ServiceRequest
                {
                    Id = nextId++,
                    Location = "Ward 12 Community Hall",
                    Ward = "Ward 12 - Township",
                    Category = "Water",
                    Department = "Water & Sanitation",
                    Description = "Communal standpipe tap broken off, causing continuous clean water loss.",
                    DateSubmitted = now.AddDays(-1).AddHours(-5),
                    LastUpdated = now.AddHours(-4),
                    Status = ServiceRequestStatus.Assigned,
                    Priority = ServiceRequestPriority.Urgent,
                    AssignedTeam = "Rapid Plumbing & Water Unit",
                    StatusNotes = "Plumbing technician en route with heavy-duty brass standpipe valve."
                },
                new ServiceRequest
                {
                    Id = nextId++,
                    Location = "33 Jacaranda Avenue",
                    Ward = "Ward 4 - Oak Road",
                    Category = "Sanitation",
                    Department = "Water & Sanitation",
                    Description = "Slow drainage and odour reported along residential sewer line.",
                    DateSubmitted = now.AddDays(-1).AddHours(-2),
                    LastUpdated = now.AddHours(-3),
                    Status = ServiceRequestStatus.Submitted,
                    Priority = ServiceRequestPriority.Medium,
                    AssignedTeam = "Rapid Plumbing & Water Unit",
                    StatusNotes = "Scheduled for CCTV pipe inspection."
                },
                new ServiceRequest
                {
                    Id = nextId++,
                    Location = "Civic Theatre Parking Lot",
                    Ward = "Ward 1 - CBD",
                    Category = "Public Safety",
                    Department = "Waste & Public Safety",
                    Description = "Overgrown trees obstructing CCTV camera line of sight and street signage.",
                    DateSubmitted = now.AddHours(-18),
                    LastUpdated = now.AddHours(-10),
                    Status = ServiceRequestStatus.Submitted,
                    Priority = ServiceRequestPriority.Low,
                    AssignedTeam = "Environmental & By-Law Unit",
                    StatusNotes = "Parks & pruning team scheduled for upcoming maintenance cycle."
                },
                new ServiceRequest
                {
                    Id = nextId++,
                    Location = "64 Ubuntu Road",
                    Ward = "Ward 12 - Township",
                    Category = "Electricity",
                    Department = "Energy & Electricity",
                    Description = "Mini-substation feeder trip leaving 18 households without power.",
                    DateSubmitted = now.AddHours(-9),
                    LastUpdated = now.AddHours(-1),
                    Status = ServiceRequestStatus.InProgress,
                    Priority = ServiceRequestPriority.Urgent,
                    AssignedTeam = "High-Voltage Electrical Team",
                    StatusNotes = "Technicians testing underground feeder cable for fault location."
                }
            };

            foreach (ServiceRequest req in initial)
            {
                AddInternal(req);
            }
        }
    }
}
