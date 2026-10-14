Municipality Service Portal (Municipality Issue Reporter)

----------------------------------------------------------------------
1. Overview
----------------------------------------------------------------------
The Municipality Issue Reporter is a C# Windows Forms (.NET 10) desktop 
application designed to help South African residents report municipal 
service delivery issues and stay informed about local community events 
and official announcements.

Current Development Status:
- Part 1: Report Issues (Active)
- Part 2: Local Events and Announcements (Active)
- Part 3: Service Request Status (Disabled menu placeholder for Part 3)


----------------------------------------------------------------------
2. Features
----------------------------------------------------------------------
Part 1 – Report Issues:
- Main menu navigation.
- Capture issue details: location, category, and description.
- Predefined issue categories: Sanitation, Roads, Electricity, Water, 
  Waste Management, Public Safety, Other.
- Attach supporting images or documents (.jpg, .png, .pdf, .docx).
- Input validation to ensure required information is provided.
- Real-time progress bar (increments 25% per completed field) with 
  supportive engagement messages.
- Immediate submission confirmation with a unique reference number 
  (e.g., #00001).
- View Submitted Reports table to inspect all reports created during 
  the current session.
- In-memory storage using a generic List<IssueReport>.

Part 2 – Local Events and Announcements:
- Announcements panel automatically ordered by priority (Urgent notices 
  first) with soft color highlights (Urgent in red, High in yellow).
- Pre-seeded with 20 South African municipal events across 7 categories 
  and 6 municipal announcements.
- Events displayed chronologically using a sorted data structure.
- Multi-criteria search: keyword, category dropdown, and From / To dates.
- Category filter populated from unique event categories with no 
  duplicates.
- Smart "Recommended For You" panel that suggests 5 unviewed events 
  tailored to user search history and category interest, showing the 
  reason in a "Why" column.
- "Recently Viewed" panel displaying the last 5 events inspected in 
  Last-In, First-Out (LIFO) order.
- Double-click any announcement or event to open a full details dialog.


----------------------------------------------------------------------
3. Data Structures Used (Part 2)
----------------------------------------------------------------------
All Part 2 event management logic is implemented in Services\EventService.cs.

1. Stacks, Queues, and Priority Queues:
   - PriorityQueue<Announcement, (int Level, long Newest)>:
     Orders announcements by urgency (Urgent = 1, High = 2, Normal = 3) 
     and breaks ties by newest date posted. Reading uses a copy so the 
     queue is not drained.
   - Stack<LocalEvent>:
     Maintains the "Recently Viewed" panel in Last-In, First-Out (LIFO) 
     order, keeping the 5 most recent unique events viewed.
   - Queue<SearchRecord>:
     Maintains a rolling First-In, First-Out (FIFO) queue of the user's 
     last 10 searches to feed the recommendation engine.

2. Hash Tables, Dictionaries, and Sorted Dictionaries:
   - SortedDictionary<DateTime, List<LocalEvent>>:
     Groups events by date in ascending chronological order and provides 
     efficient date-range filtering.
   - Dictionary<int, LocalEvent>:
     Hash table providing O(1) lookup of any event by its unique ID.
   - Dictionary<string, List<LocalEvent>>:
     Case-insensitive hash table indexing events by category for fast 
     category filtering.
   - Dictionary<string, int>:
     Tracks category interest scores (+2 per search, +3 per event view) 
     to quantify user preferences.

3. Sets:
   - HashSet<string>:
     Stores distinct event categories to populate the category dropdown 
     without duplicate entries.
   - HashSet<DateTime>:
     Tracks unique dates on which events are scheduled.
   - HashSet<int>:
     Used inside the recommendation engine to perform O(1) exclusion of 
     already-viewed events so suggestions stay fresh.

4. Recommendation Engine:
   - Evaluates upcoming unviewed events against recent search history, 
     category interest weights, and searched date ranges.
   - Scores events using category affinity (+2 / +3), keyword matches 
     (+2), date-range matches (+1), and proximity (+1 if within 7 days).
   - Defaults to the soonest upcoming events ("Coming up soon") when no 
     searches have been made yet.


----------------------------------------------------------------------
4. System Requirements
----------------------------------------------------------------------
- Operating System: Windows 10 or Windows 11
- IDE: Visual Studio 2022 / 2026 (with ".NET desktop development" workload)
       OR Antigravity IDE / VS Code
- Framework: .NET 10 SDK (net10.0-windows)
- Language: C# 13


----------------------------------------------------------------------
5. How to Compile and Run
----------------------------------------------------------------------
Option A – Visual Studio:
1. Open Visual Studio.
2. Open the solution file:
   MunicipalityIssueReporter\MunicipalityIssueReporter.slnx
   (or open MunicipalityIssueReporter\MunicipalityIssueReporter\MunicipalityIssueReporter.csproj).
3. Select Build -> Build Solution (Ctrl+Shift+B).
4. Press F5 (or click "Start") to run the application.

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
6. How to Use the Application
----------------------------------------------------------------------
Main Menu:
- Click "Report Issues" to access the issue reporting form (Part 1).
- Click "Local Events and Announcements" to access events and notices (Part 2).
- "Service Request Status" is an inactive placeholder for Part 3.
- Closing any open child form returns you directly to the Main Menu.

Reporting an Issue (Part 1):
1. Enter the location of the issue.
2. Choose a category from the dropdown.
3. Enter a detailed description.
4. Optionally click "Attach Image / Document" to choose a file.
5. Notice the progress bar moving forward (25% per completed field).
6. Click "Submit Issue". A dialog confirms the submission and displays 
   a unique reference number (e.g. #00001).
7. Click "View Submitted Reports" (top left) to see all logged reports 
   in a table.

Browsing & Searching Local Events (Part 2):
1. Review the Announcements panel at the top. Urgent notices appear 
   first. Double-click any announcement row to read the full notice.
2. Browse upcoming events in the main table (sorted chronologically).
3. Search and filter:
   - Type a keyword (e.g. "market", "soccer", "clinic").
   - Select a category (e.g. "Sports", "Environment").
   - Tick "From" and/or "To" date checkboxes to filter by date range.
   - Click "Search" (or press Enter).
   - Click "Clear" to reset all filters.
4. Double-click any event to view full details (venue, time, description).
5. Viewing an event pushes it to the "Recently Viewed" panel on the bottom right.
6. The "Recommended For You" panel on the top right continuously adapts 
   to recommend events matching your searches and viewed categories, 
   showing the reason in the "Why" column.


----------------------------------------------------------------------
7. Project Structure
----------------------------------------------------------------------
MunicipalityIssueReporter
|
|-- Models
|   |-- Announcement.cs       (Announcement class & priority enum)
|   |-- IssueReport.cs        (Part 1 issue report model)
|   |-- LocalEvent.cs         (Local event model)
|   |-- SearchRecord.cs       (Search history & recommendation models)
|
|-- Services
|   |-- EventService.cs       (Part 2 collections, search & recommendation logic)
|   |-- IssueReportService.cs (Part 1 in-memory report list storage)
|
|-- MainMenuForm.cs / .Designer.cs
|-- ReportIssueForm.cs / .Designer.cs
|-- SubmittedReportsForm.cs / .Designer.cs
|-- LocalEventsForm.cs / .Designer.cs
|-- Program.cs
|-- README.txt
