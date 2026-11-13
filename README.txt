Municipality Service Portal (Municipality Issue Reporter)

----------------------------------------------------------------------
1. Overview
----------------------------------------------------------------------
The Municipality Issue Reporter (Municipality Service Portal) is a C# 
Windows Forms (.NET 10) desktop application designed to empower South 
African residents to report municipal service delivery issues (Part 1), 
stay informed about local community events and official announcements 
(Part 2), and track the resolution progress of submitted service 
requests using advanced tree, heap, and graph data structures (Part 3).

Current Implementation Status:
- Part 1: Report Issues (Active & Complete)
- Part 2: Local Events and Announcements (Active & Complete)
- Part 3: Service Request Status (Active & Complete)


----------------------------------------------------------------------
2. Features
----------------------------------------------------------------------
Part 1 – Report Issues:
- Main menu navigation across all three municipal service modules.
- Capture issue details: location, category, and rich-text description.
- Predefined categories: Sanitation, Roads, Electricity, Water, Waste 
  Management, Public Safety, Other.
- Attach supporting images or documents (.jpg, .png, .pdf, .docx).
- Input validation ensuring required fields are completed.
- Real-time progress bar (25% per completed field) with supportive 
  engagement messages.
- Immediate submission confirmation with a unique reference number 
  (e.g., #00016) that is automatically registered in the Part 3 
  Service Request Status tracker.
- View Submitted Reports table to inspect all reports created during 
  the current session.

Part 2 – Local Events and Announcements:
- Announcements panel automatically ordered by urgency (Urgent, High, 
  Normal) using a PriorityQueue, with soft colour highlights.
- Pre-seeded with 20 South African municipal events across 7 categories 
  and 6 official announcements.
- Events displayed chronologically using a SortedDictionary.
- Multi-criteria search by keyword, category, and From / To date range.
- Dynamic category dropdown populated from a HashSet with no duplicates.
- Smart "Recommended For You" panel scoring and suggesting 5 unviewed 
  events based on search history and category interest.
- "Recently Viewed" panel displaying the last 5 inspected events in 
  Last-In, First-Out (LIFO) order using a Stack.

Part 3 – Service Request Status:
- Displays a pre-seeded list of 15 realistic South African municipal 
  service requests plus any new issues reported in Part 1 during the 
  session, complete with status (Submitted, Assigned, In Progress, 
  Resolved) and completion percentage (25% to 100%).
- Track any service request by its unique Reference ID (e.g., #00005 
  or 5) in O(log n) time using a self-balancing AVL Binary Search Tree, 
  displaying the exact sequence of tree nodes visited during lookup.
- Toggle ordering between "By ID (AVL Tree In-Order)" and "Newest First 
  (Red-Black Tree)".
- Interactive "Department Tree (Basic Tree)" sidebar organising requests 
  hierarchically (All Municipal Requests -> Department -> Category).
- "Advance Status" button to progress a selected request through its 
  lifecycle (Submitted -> Assigned -> In Progress -> Resolved) and 
  observe live updates across the progress bar, Min-Heap, and Graph MST.
- "Priority Dispatch Queue (Min-Heap)" panel showing active unresolved 
  requests ordered by urgency priority and age.
- "Zone Dispatch Network (Graph, BFS & MST)" panel showing Prim's 
  Minimum Spanning Tree (MST) road connections from the Civic Depot to 
  all municipal wards, total minimum dispatch distance (km), and 
  Breadth-First Search (BFS) & Depth-First Search (DFS) zone traversals.
- Double-click any service request to inspect full details, AVL tree 
  lookup path, and nearby related requests discovered via Graph BFS.


----------------------------------------------------------------------
3. Part 2 Data Structures (Services\EventService.cs)
----------------------------------------------------------------------
- PriorityQueue<Announcement, (int Level, long Newest)>:
  Orders announcements so Urgent notices always appear first, breaking 
  ties by newest DatePosted without draining the queue on read.
- Stack<LocalEvent>:
  Maintains the "Recently Viewed" events list in LIFO order (last 5).
- Queue<SearchRecord>:
  Maintains a rolling FIFO history of the last 10 user searches to 
  drive event recommendations.
- SortedDictionary<DateTime, List<LocalEvent>>:
  Organises events chronologically by date and supports date-range 
  queries.
- Dictionary<int, LocalEvent>, Dictionary<string, List<LocalEvent>>, 
  and Dictionary<string, int>:
  Hash tables providing O(1) ID lookup, category indexing, and user 
  interest scoring.
- HashSet<string>, HashSet<DateTime>, and HashSet<int>:
  Sets storing unique event categories, unique event dates, and 
  viewed-event exclusions.


----------------------------------------------------------------------
4. Part 3 Data Structures & Efficiency Role
   (DataStructures\ServiceRequestTrees.cs,
    DataStructures\ServiceRequestHeapAndGraph.cs,
    Services\ServiceRequestService.cs)
----------------------------------------------------------------------
1. Basic Tree (BasicTreeNode):
   - Role: Models the multi-level organisational hierarchy of municipal 
     service requests:
     Root ("All Municipal Requests") -> Department -> Category -> Requests.
   - Contribution to Efficiency: Allows residents and dispatchers to 
     drill down into a specific department (e.g., "Water & Sanitation") 
     or sub-category (e.g., "Water") in the TreeView sidebar, collecting 
     only the subtree's requests via recursive tree traversal instead 
     of scanning unrelated departments.
   - Example: Clicking "Water & Sanitation (6)" in the Department Tree 
     immediately filters the table to the 6 water and sanitation 
     requests.

2. Binary Tree, Binary Search Tree (BST), and Self-Balancing AVL Tree
   (AvlTreeNode & ServiceRequestAvlTree):
   - Role: Indexes every ServiceRequest by its unique integer ID.
   - Contribution to Efficiency: Because service request IDs (#00001, 
     #00002, #00003, ...) are generated sequentially, a standard 
     unbalanced Binary Search Tree would degenerate into a linked list 
     with O(n) lookup time. The AVL Tree tracks node heights and 
     balance factors, performing Left, Right, Left-Right, and Right-Left 
     rotations on insertion. For 15 sequential requests, it maintains a 
     balanced height of 4 (instead of 15), guaranteeing O(log n) lookup 
     by unique ID and O(n) sorted retrieval via In-Order traversal.
   - Example: Searching for Reference ID "#00005" in the Track box 
     locates the request in 3 tree comparisons (#00008 -> #00004 -> 
     #00005) and displays that exact traversal path in the status bar.

3. Red-Black Tree (ServiceRequestRedBlackTree):
   - Role: A Left-Leaning Red-Black Binary Search Tree keyed by 
     (DateSubmitted, Id) using red/black colour links, left/right 
     rotations, and colour flips.
   - Contribution to Efficiency: Maintains service requests in 
     chronological submission order with O(log n) insertion and allows 
     instantaneous reverse in-order traversal when the user switches 
     "Tree Order" to "Newest First (Red-Black Tree)" without re-sorting.
   - Example: Selecting "Newest First (Red-Black Tree)" immediately 
     displays the most recently logged requests (such as #00015 or a 
     newly reported Part 1 issue) at the top of the table.

4. Min-Heap (ServiceRequestMinHeap):
   - Role: Array-backed binary Min-Heap (using BubbleUp and BubbleDown) 
     that prioritises active (non-resolved) service requests by urgency 
     (Urgent = 1, High = 2, Medium = 3, Low = 4) and breaks ties by 
     oldest DateSubmitted.
   - Contribution to Efficiency: Provides O(1) access to the highest-
     priority unresolved request and O(log n) insertion/extraction. 
     When a request is marked Resolved, it is removed from the dispatch 
     heap so dispatchers only focus on active work.
   - Example: Urgent requests (#00001 Burst Pipe on Oak Road, #00005 
     Blocked Sewer Manhole, #00008 Exposed Cable, #00012 Broken 
     Standpipe, #00015 Mini-Substation Trip) automatically bubble to 
     the top of the "Priority Dispatch Queue (Min-Heap)" panel.

5. Weighted Graph, Graph Traversal (BFS & DFS), and Minimum Spanning Tree
   (ServiceRequestGraph):
   - Role: Models the municipality's central depot ("Civic Depot") and 
     6 service wards (Ward 1 - CBD, Ward 4 - Oak Road, Ward 7 - 
     Riverside, Ward 8 - Greenfield, Ward 10 - Industrial, Ward 12 - 
     Township) as graph vertices connected by weighted road-distance 
     edges (in kilometres), with service requests mapped to their ward.
   - Contribution to Efficiency:
     * Breadth-First Search (BFS): Uses a FIFO Queue to traverse wards 
       level-by-level by hop count from a selected request's ward, 
       identifying nearby related service requests that a field crew 
       can inspect on the same trip.
     * Depth-First Search (DFS): Uses a LIFO Stack to explore deep 
       connected ward corridors across the municipal road network.
     * Minimum Spanning Tree (Prim's Algorithm): Computes the subset of 
       road connections that links the Civic Depot to all 6 municipal 
       wards with the minimum possible total road distance (18.7 km 
       across 6 links), optimising multi-ward maintenance dispatch.
   - Example: Double-clicking #00001 (Ward 4 - Oak Road) shows the BFS 
     zone traversal starting from Ward 4 and lists nearby active 
     requests in Ward 4, Ward 1, and Ward 8 so crews can batch repairs.


----------------------------------------------------------------------
5. System Requirements
----------------------------------------------------------------------
- Operating System: Windows 10 or Windows 11
- IDE: Visual Studio 2022 / 2026 (with ".NET desktop development" workload)
       OR Antigravity IDE / VS Code
- Framework: .NET 10 SDK (net10.0-windows)
- Language: C# 13


----------------------------------------------------------------------
6. How to Compile and Run
----------------------------------------------------------------------
Option A – Visual Studio:
1. Open Visual Studio.
2. Open the solution file:
   MunicipalityIssueReporter\MunicipalityIssueReporter.slnx
   (or open MunicipalityIssueReporter\MunicipalityIssueReporter\MunicipalityIssueReporter.csproj).
3. Select Build -> Build Solution (Ctrl+Shift+B) and confirm 0 errors.
4. Press F5 (or click "Start") to launch the Municipality Service Portal.

Option B – Antigravity IDE / VS Code:
1. Open the project root folder in Antigravity IDE.
2. In the terminal, run:
   .\run.bat
   (or press F5 to launch via launch.json).

Option C – .NET CLI (Command Prompt or PowerShell):
1. Navigate to the project directory and run:
   dotnet build MunicipalityIssueReporter\MunicipalityIssueReporter\MunicipalityIssueReporter.csproj
   dotnet run --project MunicipalityIssueReporter\MunicipalityIssueReporter\MunicipalityIssueReporter.csproj


----------------------------------------------------------------------
7. How to Use the Application
----------------------------------------------------------------------
Main Menu:
- Click "Report Issues" to log a new municipal issue (Part 1).
- Click "Local Events and Announcements" to browse and search community 
  events and urgent notices (Part 2).
- Click "Service Request Status" to track and manage service requests 
  (Part 3).

Tracking Service Requests (Part 3):
1. Click "Service Request Status" on the Main Menu.
2. Track by Unique ID (AVL Tree Lookup):
   - Type a Reference ID in "Ref ID / Keyword" (e.g., "#00005" or "5") 
     and click "Track" (or press Enter).
   - Observe the bottom status bar display the exact AVL Binary Search 
     Tree comparison path (e.g., #00008 -> #00004 -> #00005).
3. Filter by Department Hierarchy (Basic Tree):
   - Click any department or category node in the left "Department Tree 
     (Basic Tree)" panel to filter requests in that branch.
4. Filter & Change Tree Order:
   - Filter by Status (Submitted, Assigned, In Progress, Resolved) or 
     Priority (Urgent, High, Medium, Low).
   - Switch "Tree Order" between "By ID (AVL Tree In-Order)" and 
     "Newest First (Red-Black Tree)".
   - Click "Clear" to reset all filters.
5. Advance Request Progress:
   - Select any unresolved request and click "Advance Status" to move 
     it to the next stage (25% -> 50% -> 75% -> 100% Resolved). Watch 
     the progress bar, Min-Heap dispatch queue, and MST active counts 
     update immediately.
6. Inspect Details & Graph Traversal:
   - Double-click any request in the main table or in the Min-Heap 
     panel to open a popup showing full details, the AVL lookup path, 
     the BFS zone traversal order, and related requests in connected 
     municipal wards.


----------------------------------------------------------------------
8. Project Structure
----------------------------------------------------------------------
MunicipalityIssueReporter
|
|-- DataStructures
|   |-- ServiceRequestTrees.cs        (Basic Tree, AVL Tree, Red-Black Tree)
|   |-- ServiceRequestHeapAndGraph.cs (Min-Heap, Graph, BFS, DFS, Prim's MST)
|
|-- Models
|   |-- Announcement.cs               (Part 2 announcement model)
|   |-- IssueReport.cs                (Part 1 issue report model)
|   |-- LocalEvent.cs                 (Part 2 local event model)
|   |-- SearchRecord.cs               (Part 2 search & recommendation models)
|   |-- ServiceRequest.cs             (Part 3 service request & MST edge models)
|
|-- Services
|   |-- EventService.cs               (Part 2 events & recommendations service)
|   |-- IssueReportService.cs         (Part 1 issue storage service)
|   |-- ServiceRequestService.cs      (Part 3 service request coordinator)
|
|-- MainMenuForm.cs / .Designer.cs
|-- ReportIssueForm.cs / .Designer.cs
|-- SubmittedReportsForm.cs / .Designer.cs
|-- LocalEventsForm.cs / .Designer.cs
|-- ServiceRequestStatusForm.cs / .Designer.cs
|-- Program.cs
|-- README.txt
|-- README.md
