The Municipality Issue Reporter is a C# Windows Forms application designed to allow residents to report municipal issues in a simple and accessible way.
The current phase of the application focuses on the Report Issues functionality. The Local Events and Announcements and Service Request Status modules are displayed on the main menu but are disabled for future development.

Features
- Main menu navigation
- Report municipal issues
- Select an issue category
- Enter the location of an issue
- Provide a detailed description
- Attach images or documents
- Input validation
- Progress indicator and user engagement messages
- Submission confirmation
- Unique issue reference numbers
- Dynamic in-memory storage using a generic List<IssueReport>
- Multiple issue reports can be stored during an application session

Issue Categories
The following categories are available:
- Sanitation
- Roads
- Electricity
- Water
- Waste Management
- Public Safety
- Other

Requirements
To compile and run the application, the following are required:
- Windows operating system
- Visual Studio
- .NET 10
- C# support
- Windows Forms development workload
How to Compile
1. Open Visual Studio.
2. Open the MunicipalityIssueReporter solution.
3. Allow Visual Studio to restore any required project dependencies.
4. Select Build → Build Solution.
5. If the build completes successfully, the application is ready to run.

How to Run
1. Open the project in Visual Studio.
2. Press F5 or select Debug → Start Debugging.
3. The Municipality Service Portal main menu will appear.
How to Use the Application
Main Menu
When the application starts, three service options are displayed:
- Report Issues – available for use.
- Local Events and Announcements – disabled in the current phase.
- Service Request Status – disabled in the current phase.
Select Report Issues to begin submitting an issue.
Reporting an Issue
1. Enter the location of the issue.
2. Select an issue category.
3. Enter a detailed description.
4. Optionally attach an image or document using Attach Image / Document.
5. Complete the required information.
6. Select Submit Issue.
The application validates the required fields before accepting the report.
Progress Indicator
The progress indicator provides feedback as the report is completed.
The progress is based on:
- Location entered – 25%
- Category selected – 50%
- Description entered – 75%
- Attachment selected – 100%
The application displays encouraging messages as progress is made.
Successful Submission
After a valid report is submitted, the application displays a confirmation message containing a unique reference number.
For example:
Your issue has been successfully reported.

Reference Number: #00001
The form is then cleared so another issue can be reported.
Data Storage
Reported issues are stored in memory using a generic C# list:
List<IssueReport>
Each issue contains:
- ID
- Location
- Category
- Description
- Attachment path
- Date reported
A unique ID is assigned to each report when it is submitted.
The data is stored only while the application is running. Closing the application clears the in-memory collection.
Project Structure
MunicipalityIssueReporter
│
├── Models
│   └── IssueReport.cs
│
├── Services
│   └── IssueReportService.cs
│
├── MainMenuForm.cs
├── MainMenuForm.Designer.cs
│
├── ReportIssueForm.cs
├── ReportIssueForm.Designer.cs
│
├── SubmittedReportsForm.cs
├── SubmittedReportsForm.Designer.cs
│
├── Program.cs
└── README.md
Error Handling and Validation
The application checks that:
- A location has been entered.
- An issue category has been selected.
- A description has been provided.
If required information is missing, the application displays a warning message and prevents submission until the required information has been provided.
Current Development Scope
The current version focuses on municipal issue reporting.
The following modules are reserved for future development:
- Local Events and Announcements
- Service Request Status
These options remain disabled in accordance with the current project requirements.
Testing
The following functionality has been tested:
- Application startup
- Main menu navigation
- Disabled menu options
- Issue location validation
- Category validation
- Description validation
- Issue submission
- File attachment
- Progress indicator
- Submission confirmation
- Unique issue reference numbers
- Form reset after submission
- Multiple issue submissions

