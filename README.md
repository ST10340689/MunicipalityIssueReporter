# Municipality Issue Reporter (Municipality Service Portal)

**Module:** AAPD7112w / PROG7312 – Portfolio of Evidence (Parts 1 & 2)  
**Student Number:** ST10340689

A C# Windows Forms (`.NET 10`) desktop application that enables South African residents to report municipal service delivery issues (**Part 1**) and browse, search, and receive personalised recommendations for local municipal events and announcements (**Part 2**).

---

## 1. Features

### Part 1: Report Issues
- **Main Menu Navigation (`MainMenuForm`)**: Clean portal presenting *Report Issues* (active), *Local Events and Announcements* (active), and *Service Request Status* (disabled for Part 3).
- **Issue Reporting Form (`ReportIssueForm`)**: Capture issue location, category (*Sanitation, Roads, Electricity, Water, Waste Management, Public Safety, Other*), rich-text description, and optional image/document attachment.
- **Real-Time Progress & Engagement**: Dynamic progress bar (`25%` per completed field) paired with encouraging status messages.
- **Validation & Reference Numbers**: Validates required inputs and assigns a formatted reference number (`#00001`, `#00002`, etc.) stored in memory (`List<IssueReport>`).
- **Submitted Reports Viewer (`SubmittedReportsForm`)**: Accessible via the **View Submitted Reports** button to inspect all reports logged during the session.

### Part 2: Local Events and Announcements
- **Prioritised Announcements Feed**: Displays 6 pre-seeded municipal notices (water outages, road closures, load shedding updates, etc.) ordered by urgency (`Urgent` → `High` → `Normal`) using a `PriorityQueue`, with soft colour highlights for urgent and high-priority items.
- **Chronological Events Feed**: Pre-seeded with 20 South African municipal community events across 7 categories (*Arts & Culture, Education, Environment, Health, Markets, Public Meeting, Sports*), automatically sorted by date using a `SortedDictionary`.
- **Multi-Criteria Search**: Filter events by keyword, category, and/or optional `From` and `To` date ranges, with date-range validation and a one-click **Clear** reset.
- **Personalised Recommendation Engine**: Dynamically scores and suggests 5 upcoming unviewed events in the **Recommended For You** panel based on the user's recent search history, category interest weights, keyword matches, and searched date ranges—displaying the reason for each suggestion in the **Why** column.
- **Recently Viewed History**: Tracks the last 5 inspected events in a LIFO `Stack` so residents can quickly revisit events they opened.

---

## 2. Part 2 Data Structures & Implementation (`Services/EventService.cs`)

| Category | C# Data Structure in `EventService` | How It Is Used |
| :--- | :--- | :--- |
| **Stacks, Queues, Priority Queues** | `PriorityQueue<Announcement, (int Level, long Newest)>`<br>`Stack<LocalEvent>`<br>`Queue<SearchRecord>` | • **`PriorityQueue`**: Orders announcements so `Urgent` notices always appear first, breaking ties by newest `DatePosted` without draining the queue on read.<br>• **`Stack`**: Stores the last 5 recently viewed events in LIFO order (most recent on top).<br>• **`Queue`**: Keeps a rolling FIFO history of the last 10 user searches to drive recommendations. |
| **Hash Tables, Dictionaries, Sorted Dictionaries** | `SortedDictionary<DateTime, List<LocalEvent>>`<br>`Dictionary<int, LocalEvent>`<br>`Dictionary<string, List<LocalEvent>>`<br>`Dictionary<string, int>` | • **`SortedDictionary`**: Groups and orders all 20 events chronologically by date and supports date-range filtering.<br>• **`Dictionary<int, LocalEvent>`**: Hash table providing $O(1)$ lookup by event ID.<br>• **`Dictionary<string, List<LocalEvent>>`**: Hash table indexing events by category (case-insensitive).<br>• **`Dictionary<string, int>`**: Hash table tracking user interest weights per category (`+2` per search, `+3` per event view). |
| **Sets** | `HashSet<string>`<br>`HashSet<DateTime>`<br>`HashSet<int>` | • **`HashSet<string>`**: Stores unique event categories to populate the Category dropdown without duplicates.<br>• **`HashSet<DateTime>`**: Tracks unique event dates.<br>• **`HashSet<int>`**: Excludes already-viewed event IDs during recommendation scoring. |
| **Recommendation Feature** | `EventService.GetRecommendations()` | Scores upcoming unviewed events using category interest (`Dictionary<string, int>`), recent search keywords and date ranges (`Queue<SearchRecord>`), and viewed-event exclusions (`HashSet<int>` from `Stack<LocalEvent>`), returning the top 5 recommendations with human-readable reasons. |

---

## 3. Requirements & How to Compile and Run

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

## 4. How to Use the Application

1. **Main Menu**: Click **Report Issues** (Part 1) or **Local Events and Announcements** (Part 2).
2. **Report Issues (Part 1)**:
   - Fill in **Location**, select a **Category**, enter a **Description**, and optionally click **Attach Image / Document**.
   - Watch the progress bar and engagement prompt update from `0%` to `100%`.
   - Click **Submit Issue** to receive a reference number (`#00001`), or click **View Submitted Reports** to inspect all logged reports.
3. **Local Events and Announcements (Part 2)**:
   - **Announcements**: View priority-ordered municipal notices at the top; double-click any announcement to read the full message.
   - **Search Events**: Enter a keyword (e.g., `market`, `soccer`, `clinic`), select a category, and/or tick the **From** / **To** date checkboxes, then click **Search** (or **Clear** to reset).
   - **Event Details & Recently Viewed**: Double-click any event in the main list or recommended list to view its full details and push it onto the **Recently Viewed** stack.
   - **Recommendations**: Observe the **Recommended For You** list automatically adapt after each search or event view, with the **Why** column explaining each recommendation.
