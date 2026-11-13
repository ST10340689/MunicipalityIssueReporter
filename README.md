# Municipality Issue Reporter (Municipality Service Portal)

A C# Windows Forms (`.NET 10`) desktop application that enables South African residents to report municipal service delivery issues (**Part 1**), browse, search, and receive personalised recommendations for local municipal events and announcements (**Part 2**), and track the progress of submitted service requests using advanced tree, heap, and graph data structures (**Part 3**).

---

## 1. Features

### Part 1: Report Issues
- **Main Menu Navigation (`MainMenuForm`)**: Clean portal presenting *Report Issues*, *Local Events and Announcements*, and *Service Request Status* (all active).
- **Issue Reporting Form (`ReportIssueForm`)**: Capture issue location, category (*Sanitation, Roads, Electricity, Water, Waste Management, Public Safety, Other*), rich-text description, and optional image/document attachment.
- **Real-Time Progress & Engagement**: Dynamic progress bar (`25%` per completed field) paired with encouraging status messages.
- **Validation & Reference Numbers**: Validates required inputs, assigns a formatted reference number (`#00016`, `#00017`, etc.), stores the report in `List<IssueReport>`, and automatically registers it in the Part 3 **Service Request Status** tracker.
- **Submitted Reports Viewer (`SubmittedReportsForm`)**: Accessible via the **View Submitted Reports** button to inspect all reports logged during the session.

### Part 2: Local Events and Announcements
- **Prioritised Announcements Feed**: Displays 6 pre-seeded municipal notices ordered by urgency (`Urgent` → `High` → `Normal`) using a `PriorityQueue`, with soft colour highlights for urgent and high-priority items.
- **Chronological Events Feed**: Pre-seeded with 20 South African municipal community events across 7 categories, automatically sorted by date using a `SortedDictionary`.
- **Multi-Criteria Search**: Filter events by keyword, category, and/or optional `From` and `To` date ranges, with date-range validation and a one-click **Clear** reset.
- **Personalised Recommendation Engine**: Dynamically scores and suggests 5 upcoming unviewed events in the **Recommended For You** panel based on recent search history, category interest weights, keyword matches, and searched date ranges.
- **Recently Viewed History**: Tracks the last 5 inspected events in a LIFO `Stack`.

### Part 3: Service Request Status
- **Organised Service Request Tracker (`ServiceRequestStatusForm`)**: Pre-seeded with 15 realistic South African municipal service requests across 6 wards and 4 departments—plus any new issues submitted in Part 1 during the session—displaying status (*Submitted, Assigned, In Progress, Resolved*) and progress (`25%` to `100%`).
- **$O(\log n)$ Unique Identifier Lookup (AVL Binary Search Tree)**: Track any request by its unique Reference ID (e.g. `#00005` or `5`) and view the exact sequence of AVL tree nodes visited during the search.
- **Department & Category Hierarchy (`TreeView` backed by a Basic Tree)**: Drill down by department (*Water & Sanitation, Roads & Transport, Energy & Electricity, Waste & Public Safety*) or category.
- **Chronological & ID Tree Ordering**: Switch between **By ID (AVL Tree In-Order)** and **Newest First (Red-Black Tree)**.
- **Live Status Progression**: Click **Advance Status** on any active request to advance its progress (`Submitted 25%` → `Assigned 50%` → `In Progress 75%` → `Resolved 100%`) and observe live updates across the progress bar, Min-Heap, and Graph MST.
- **Priority Dispatch Queue (Min-Heap)**: Displays active unresolved requests in urgency dispatch order (`Urgent` → `High` → `Medium` → `Low`, breaking ties by oldest submission date).
- **Zone Dispatch Network (Graph, BFS, DFS & Prim's MST)**: Models the central `Civic Depot` and 6 municipal wards as a weighted road-distance graph, computing the **Minimum Spanning Tree (MST)** dispatch route (`18.7 km`), **Breadth-First Search (BFS)** and **Depth-First Search (DFS)** zone traversals, and nearby related service requests.

---

## 2. Part 2 Data Structures (`Services/EventService.cs`)

| Category | C# Data Structure in `EventService` | Role & Efficiency Contribution |
| :--- | :--- | :--- |
| **Stacks, Queues, Priority Queues** | `PriorityQueue<Announcement, (int Level, long Newest)>`<br>`Stack<LocalEvent>`<br>`Queue<SearchRecord>` | • **`PriorityQueue`**: Dequeues `Urgent` announcements first, breaking ties by newest `DatePosted`.<br>• **`Stack`**: Stores the last 5 recently viewed events in LIFO order.<br>• **`Queue`**: Keeps a rolling FIFO history of the last 10 user searches to drive recommendations. |
| **Hash Tables, Dictionaries, Sorted Dictionaries** | `SortedDictionary<DateTime, List<LocalEvent>>`<br>`Dictionary<int, LocalEvent>`<br>`Dictionary<string, List<LocalEvent>>`<br>`Dictionary<string, int>` | • **`SortedDictionary`**: Groups and orders all 20 events chronologically by date and supports date-range filtering.<br>• **`Dictionary<int, LocalEvent>`**: Hash table providing $O(1)$ lookup by event ID.<br>• **`Dictionary<string, List<LocalEvent>>`**: Case-insensitive hash table indexing events by category.<br>• **`Dictionary<string, int>`**: Tracks user interest weights per category (`+2` per search, `+3` per event view). |
| **Sets** | `HashSet<string>`<br>`HashSet<DateTime>`<br>`HashSet<int>` | • **`HashSet<string>`**: Stores unique event categories for the filter dropdown.<br>• **`HashSet<DateTime>`**: Tracks unique event dates.<br>• **`HashSet<int>`**: Excludes already-viewed event IDs during recommendation scoring. |

---

## 3. Part 3 Data Structures & In-Depth Efficiency Explanations (`DataStructures/` & `Services/ServiceRequestService.cs`)

| Data Structure | Implementation Class | Role, Efficiency Contribution & Example |
| :--- | :--- | :--- |
| **Basic Tree ($N$-ary Tree)** | `BasicTreeNode` (`ServiceRequestTrees.cs`) | • **Role**: Organises service requests into a 3-tier hierarchy: `Root ("All Municipal Requests")` → `Department` → `Category` → `Requests`.<br>• **Efficiency**: Recursive subtree traversal (`CollectAllRequests()`) retrieves only requests belonging to a selected department or category without scanning unrelated branches.<br>• **Example**: Selecting `Water & Sanitation (6)` in the left `TreeView` filters the main list to the 6 water and sanitation requests. |
| **Binary Tree, Binary Search Tree (BST) & Self-Balancing AVL Tree** | `AvlTreeNode` & `ServiceRequestAvlTree` (`ServiceRequestTrees.cs`) | • **Role**: Indexes every `ServiceRequest` by its unique integer `Id`.<br>• **Efficiency**: Because reference IDs (`#00001`..`#00015`) are assigned sequentially, a plain BST would degenerate into an $O(n)$ linked list. `ServiceRequestAvlTree` tracks node heights and balance factors, applying Left, Right, Left-Right, and Right-Left rotations to keep tree height at $4$ for $15$ nodes—guaranteeing $O(\log n)$ ID searches and $O(n)$ sorted In-Order traversal.<br>• **Example**: Tracking `#00005` locates the request in just $3$ comparisons (`#00008 -> #00004 -> #00005`) and displays the visited path in the status bar. |
| **Red-Black Tree** | `ServiceRequestRedBlackTree` (`ServiceRequestTrees.cs`) | • **Role**: A Left-Leaning Red-Black BST keyed by `(DateSubmitted, Id)` using red/black node links, rotations, and colour flips.<br>• **Efficiency**: Guarantees $O(\log n)$ insertion and enables $O(n)$ reverse in-order traversal to list requests from newest to oldest without sorting overhead.<br>• **Example**: Switching **Tree Order** to *Newest First (Red-Black Tree)* immediately places `#00015` (and any newly reported Part 1 issues) at the top. |
| **Min-Heap (Binary Heap)** | `ServiceRequestMinHeap` (`ServiceRequestHeapAndGraph.cs`) | • **Role**: Array-backed binary Min-Heap (`BubbleUp` / `BubbleDown`) ordering active unresolved requests by `Priority` (`Urgent = 1`, `High = 2`, `Medium = 3`, `Low = 4`) and oldest `DateSubmitted`.<br>• **Efficiency**: Provides $O(1)$ peek at the most urgent request and $O(\log n)$ insertion/extraction (`GetOrderedSnapshot()` clones the heap so reading does not drain it).<br>• **Example**: Urgent requests (`#00001`, `#00005`, `#00008`, `#00012`, `#00015`) automatically appear at the top of the **Priority Dispatch Queue (Min-Heap)** panel. |
| **Weighted Graph, Graph Traversal (BFS & DFS) & Minimum Spanning Tree (MST)** | `ServiceRequestGraph` (`ServiceRequestHeapAndGraph.cs`) | • **Role**: Models `Civic Depot` and 6 municipal wards as vertices connected by weighted road-distance edges (km).<br>• **Efficiency**: **BFS** (FIFO `Queue`) traverses zones level-by-level by hop count to find nearby related requests (`GetNearbyRelatedRequestsBfs`); **DFS** (LIFO `Stack`) explores deep connected ward corridors; **Prim's MST** (`ComputeMinimumSpanningTree`) finds the minimum-distance dispatch tree (`18.7 km` across 6 road links) connecting `Civic Depot` to all wards.<br>• **Example**: Double-clicking `#00001` (`Ward 4 - Oak Road`) displays the BFS zone order (`Ward 4 -> Civic Depot -> Ward 1 -> Ward 8 -> ...`) and lists related active requests in adjacent wards so field crews can batch inspections. |

---

## 4. Requirements & How to Compile and Run

### Prerequisites
- Windows 10 or Windows 11
- Visual Studio 2022 / 2026 with the **.NET desktop development** workload, **or** Antigravity IDE / VS Code with the **.NET 10 SDK** (`net10.0-windows`)

### Option A: Visual Studio
1. Open `MunicipalityIssueReporter/MunicipalityIssueReporter.slnx` (or `MunicipalityIssueReporter/MunicipalityIssueReporter/MunicipalityIssueReporter.csproj`).
2. Select **Build → Build Solution** (`Ctrl+Shift+B`).
3. Press **`F5`** (or **Debug → Start Debugging**) to launch the **Municipality Service Portal**.

### Option B: Antigravity IDE / VS Code
1. Open the workspace root folder (`MunicipalityIssueReporter2`).
2. In the Integrated Terminal (`` Ctrl+` ``), run:
   ```powershell
   .\run.bat
   ```
   Or press **`F5`** to build and launch via `.vscode/launch.json`.

### Option C: .NET CLI
```powershell
dotnet build MunicipalityIssueReporter/MunicipalityIssueReporter/MunicipalityIssueReporter.csproj
dotnet run --project MunicipalityIssueReporter/MunicipalityIssueReporter/MunicipalityIssueReporter.csproj
```

---

## 5. How to Use the Application

1. **Main Menu**: Choose **Report Issues** (Part 1), **Local Events and Announcements** (Part 2), or **Service Request Status** (Part 3).
2. **Report Issues (Part 1)**:
   - Enter **Location**, select **Category**, enter **Description**, and optionally click **Attach Image / Document**.
   - Click **Submit Issue** to receive a unique reference number (e.g. `#00016`), which is also immediately available for tracking in **Service Request Status**.
3. **Local Events and Announcements (Part 2)**:
   - View priority-ordered announcements at the top, search/filter the 20 upcoming events by keyword, category, or date range, and inspect the live **Recommended For You** and **Recently Viewed** panels.
4. **Service Request Status (Part 3)**:
   - **Track by Unique ID**: Enter an ID (e.g. `#00005` or `5`) in **Ref ID / Keyword** and click **Track** to search the self-balancing **AVL Tree** and view the visited node path in the bottom status bar.
   - **Filter by Department Tree**: Click any department or category in the left **Department Tree (Basic Tree)** to filter the list.
   - **Switch Tree Ordering**: Use **Tree Order** to toggle between *By ID (AVL Tree In-Order)* and *Newest First (Red-Black Tree)*.
   - **Advance Status**: Select an active request and click **Advance Status** to progress it (`Submitted` → `Assigned` → `In Progress` → `Resolved`) and watch the progress bar, **Min-Heap** queue, and **MST** active counts update live.
   - **Inspect Details & Graph BFS**: Double-click any request in the main table or Min-Heap list to view its full details, AVL lookup path, BFS zone traversal, and related requests in connected wards.
