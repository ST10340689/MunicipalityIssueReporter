using MunicipalityIssueReporter.Models;
using System;
using System.Collections.Generic;

namespace MunicipalityIssueReporter.DataStructures
{
    /// <summary>
    /// 4. MIN-HEAP (Binary Heap):
    /// Orders active (unresolved) municipal service requests by dispatch urgency:
    /// Priority (Urgent = 1, High = 2, Medium = 3, Low = 4), breaking ties by oldest DateSubmitted.
    /// Provides O(1) access to the most urgent request and O(log n) insertion/extraction.
    /// </summary>
    public class ServiceRequestMinHeap
    {
        private readonly List<ServiceRequest> heap = new List<ServiceRequest>();

        public int Count => heap.Count;

        public void Clear()
        {
            heap.Clear();
        }

        public void Insert(ServiceRequest request)
        {
            heap.Add(request);
            BubbleUp(heap.Count - 1);
        }

        public ServiceRequest? Peek()
        {
            return heap.Count > 0 ? heap[0] : null;
        }

        public ServiceRequest? ExtractMin()
        {
            if (heap.Count == 0)
            {
                return null;
            }

            ServiceRequest min = heap[0];
            int lastIndex = heap.Count - 1;
            heap[0] = heap[lastIndex];
            heap.RemoveAt(lastIndex);

            if (heap.Count > 0)
            {
                BubbleDown(0);
            }

            return min;
        }

        /// <summary>
        /// Returns all requests in the Min-Heap in priority dispatch order without draining the heap.
        /// </summary>
        public List<ServiceRequest> GetOrderedSnapshot()
        {
            ServiceRequestMinHeap clone = new ServiceRequestMinHeap();
            foreach (ServiceRequest req in heap)
            {
                clone.Insert(req);
            }

            List<ServiceRequest> ordered = new List<ServiceRequest>(heap.Count);
            while (clone.Count > 0)
            {
                ordered.Add(clone.ExtractMin()!);
            }

            return ordered;
        }

        /// <summary>
        /// Compares two service requests for Min-Heap ordering:
        /// 1. Lower Priority enum value (Urgent=1 before High=2, etc.)
        /// 2. Earlier DateSubmitted (older unresolved requests dispatched first)
        /// 3. Lower Id
        /// </summary>
        private static int Compare(ServiceRequest a, ServiceRequest b)
        {
            int priorityComparison = ((int)a.Priority).CompareTo((int)b.Priority);
            if (priorityComparison != 0)
            {
                return priorityComparison;
            }

            int dateComparison = a.DateSubmitted.CompareTo(b.DateSubmitted);
            if (dateComparison != 0)
            {
                return dateComparison;
            }

            return a.Id.CompareTo(b.Id);
        }

        private void BubbleUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (Compare(heap[index], heap[parent]) >= 0)
                {
                    break;
                }

                (heap[index], heap[parent]) = (heap[parent], heap[index]);
                index = parent;
            }
        }

        private void BubbleDown(int index)
        {
            int count = heap.Count;
            while (true)
            {
                int left = 2 * index + 1;
                int right = 2 * index + 2;
                int smallest = index;

                if (left < count && Compare(heap[left], heap[smallest]) < 0)
                {
                    smallest = left;
                }

                if (right < count && Compare(heap[right], heap[smallest]) < 0)
                {
                    smallest = right;
                }

                if (smallest == index)
                {
                    break;
                }

                (heap[index], heap[smallest]) = (heap[smallest], heap[index]);
                index = smallest;
            }
        }
    }

    /// <summary>
    /// 5. WEIGHTED UNDIRECTED GRAPH, GRAPH TRAVERSAL (BFS & DFS), AND MINIMUM SPANNING TREE (MST):
    /// Models the municipal service zones/wards as vertices and road routes (in km) as weighted edges.
    /// Maps each service request to its municipal zone to:
    /// - Traverse connected zones via Breadth-First Search (BFS) and Depth-First Search (DFS)
    /// - Discover related service requests in adjacent zones
    /// - Compute the Minimum Spanning Tree (Prim's Algorithm) for optimal dispatch routing across active wards.
    /// </summary>
    public class ServiceRequestGraph
    {
        public const string CentralDepot = "Civic Depot";

        private readonly Dictionary<string, List<(string Neighbor, double DistanceKm)>> adjacencyList =
            new Dictionary<string, List<(string Neighbor, double DistanceKm)>>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, List<ServiceRequest>> zoneRequests =
            new Dictionary<string, List<ServiceRequest>>(StringComparer.OrdinalIgnoreCase);

        public void ClearRequests()
        {
            foreach (List<ServiceRequest> list in zoneRequests.Values)
            {
                list.Clear();
            }
        }

        public void AddZone(string zone)
        {
            if (!adjacencyList.ContainsKey(zone))
            {
                adjacencyList[zone] = new List<(string Neighbor, double DistanceKm)>();
            }

            if (!zoneRequests.ContainsKey(zone))
            {
                zoneRequests[zone] = new List<ServiceRequest>();
            }
        }

        public void AddRoadConnection(string zoneA, string zoneB, double distanceKm)
        {
            AddZone(zoneA);
            AddZone(zoneB);

            adjacencyList[zoneA].Add((zoneB, distanceKm));
            adjacencyList[zoneB].Add((zoneA, distanceKm));
        }

        public void AssignRequestToZone(ServiceRequest request)
        {
            string zone = string.IsNullOrWhiteSpace(request.Ward) ? "Ward 1 - CBD" : request.Ward;
            AddZone(zone);
            zoneRequests[zone].Add(request);
        }

        public int GetActiveRequestCount(string zone)
        {
            if (!zoneRequests.TryGetValue(zone, out List<ServiceRequest>? list))
            {
                return 0;
            }

            int active = 0;
            foreach (ServiceRequest req in list)
            {
                if (req.Status != ServiceRequestStatus.Resolved)
                {
                    active++;
                }
            }
            return active;
        }

        /// <summary>
        /// Breadth-First Search (BFS) traversal starting from a given municipal zone.
        /// Uses a FIFO Queue and a visited HashSet to visit zones level-by-level by hop distance.
        /// </summary>
        public List<string> BreadthFirstSearch(string startZone)
        {
            List<string> order = new List<string>();
            if (!adjacencyList.ContainsKey(startZone))
            {
                startZone = CentralDepot;
            }

            if (!adjacencyList.ContainsKey(startZone))
            {
                return order;
            }

            HashSet<string> visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { startZone };
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(startZone);

            while (queue.Count > 0)
            {
                string current = queue.Dequeue();
                order.Add(current);

                foreach ((string neighbor, _) in adjacencyList[current])
                {
                    if (visited.Add(neighbor))
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return order;
        }

        /// <summary>
        /// Depth-First Search (DFS) traversal starting from a given municipal zone.
        /// Uses a LIFO Stack and a visited HashSet to explore each branch deeply before backtracking.
        /// </summary>
        public List<string> DepthFirstSearch(string startZone)
        {
            List<string> order = new List<string>();
            if (!adjacencyList.ContainsKey(startZone))
            {
                startZone = CentralDepot;
            }

            if (!adjacencyList.ContainsKey(startZone))
            {
                return order;
            }

            HashSet<string> visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            Stack<string> stack = new Stack<string>();
            stack.Push(startZone);

            while (stack.Count > 0)
            {
                string current = stack.Pop();
                if (!visited.Add(current))
                {
                    continue;
                }

                order.Add(current);

                List<(string Neighbor, double DistanceKm)> neighbors = adjacencyList[current];
                for (int i = neighbors.Count - 1; i >= 0; i--)
                {
                    string neighbor = neighbors[i].Neighbor;
                    if (!visited.Contains(neighbor))
                    {
                        stack.Push(neighbor);
                    }
                }
            }

            return order;
        }

        /// <summary>
        /// Uses BFS graph traversal from a service request's zone to find related active requests
        /// in the same or immediately adjacent municipal zones.
        /// </summary>
        public List<ServiceRequest> GetNearbyRelatedRequestsBfs(ServiceRequest source, int maxResults = 4)
        {
            List<ServiceRequest> related = new List<ServiceRequest>();
            string startZone = string.IsNullOrWhiteSpace(source.Ward) ? CentralDepot : source.Ward;

            List<string> bfsZones = BreadthFirstSearch(startZone);
            foreach (string zone in bfsZones)
            {
                if (!zoneRequests.TryGetValue(zone, out List<ServiceRequest>? requestsInZone))
                {
                    continue;
                }

                foreach (ServiceRequest candidate in requestsInZone)
                {
                    if (candidate.Id == source.Id)
                    {
                        continue;
                    }

                    // Prioritise requests in the same/adjacent zones that are still active or share the department
                    if (candidate.Status != ServiceRequestStatus.Resolved ||
                        string.Equals(candidate.Department, source.Department, StringComparison.OrdinalIgnoreCase))
                    {
                        related.Add(candidate);
                        if (related.Count >= maxResults)
                        {
                            return related;
                        }
                    }
                }
            }

            return related;
        }

        /// <summary>
        /// Computes the Minimum Spanning Tree (MST) using Prim's Algorithm starting from the Civic Depot,
        /// connecting all municipal zones so dispatch teams can service all wards with minimum total road distance.
        /// </summary>
        public List<MstEdge> ComputeMinimumSpanningTree(out double totalDistanceKm)
        {
            List<MstEdge> mstEdges = new List<MstEdge>();
            totalDistanceKm = 0.0;

            if (!adjacencyList.ContainsKey(CentralDepot))
            {
                return mstEdges;
            }

            HashSet<string> inTree = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { CentralDepot };

            while (inTree.Count < adjacencyList.Count)
            {
                string? bestFrom = null;
                string? bestTo = null;
                double bestDistance = double.MaxValue;

                foreach (string u in inTree)
                {
                    foreach ((string v, double weight) in adjacencyList[u])
                    {
                        if (!inTree.Contains(v) && weight < bestDistance)
                        {
                            bestDistance = weight;
                            bestFrom = u;
                            bestTo = v;
                        }
                    }
                }

                if (bestFrom == null || bestTo == null)
                {
                    break; // Disconnected component guard
                }

                inTree.Add(bestTo);
                totalDistanceKm += bestDistance;
                mstEdges.Add(new MstEdge
                {
                    FromZone = bestFrom,
                    ToZone = bestTo,
                    DistanceKm = bestDistance,
                    ActiveRequestsInTarget = GetActiveRequestCount(bestTo)
                });
            }

            return mstEdges;
        }
    }
}
