using MunicipalityIssueReporter.Models;
using MunicipalityIssueReporter.Services;

namespace MunicipalityIssueReporter
{
    public partial class LocalEventsForm : Form
    {
        private const string AllCategories = "All Categories";

        public LocalEventsForm()
        {
            InitializeComponent();
        }

        private void LocalEventsForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadAnnouncements();
            ShowEvents(EventService.GetUpcomingEvents(), "Showing all upcoming events.");
            RefreshSidePanels();
        }

        #region Loading

        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add(AllCategories);

            // Categories come from a HashSet, so there are no duplicates.
            foreach (string category in EventService.GetCategories())
                cmbCategory.Items.Add(category);

            cmbCategory.SelectedIndex = 0;
        }

        private void LoadAnnouncements()
        {
            lvAnnouncements.Items.Clear();

            // Comes out of the priority queue: urgent first, then high, then normal.
            foreach (Announcement announcement in EventService.GetAnnouncementsByPriority())
            {
                ListViewItem item = new ListViewItem(announcement.Priority.ToString());
                item.SubItems.Add(announcement.DatePosted.ToString("dd MMM HH:mm"));
                item.SubItems.Add(announcement.Title);
                item.SubItems.Add(announcement.Message);
                item.Tag = announcement;

                if (announcement.Priority == AnnouncementPriority.Urgent)
                    item.BackColor = Color.MistyRose;
                else if (announcement.Priority == AnnouncementPriority.High)
                    item.BackColor = Color.LightYellow;

                lvAnnouncements.Items.Add(item);
            }
        }

        private void ShowEvents(List<LocalEvent> events, string summary)
        {
            lvEvents.BeginUpdate();
            lvEvents.Items.Clear();

            foreach (LocalEvent localEvent in events)
            {
                ListViewItem item = new ListViewItem(localEvent.Date.ToString("ddd dd MMM, HH:mm"));
                item.SubItems.Add(localEvent.Title);
                item.SubItems.Add(localEvent.Category);
                item.SubItems.Add(localEvent.Venue);
                item.Tag = localEvent;

                lvEvents.Items.Add(item);
            }

            lvEvents.EndUpdate();

            lblResults.Text = events.Count == 0
                ? "No events match your search. Try a different keyword, category or date range."
                : $"{summary} {events.Count} event(s) found. Double-click an event to view its details.";
        }

        private void RefreshSidePanels()
        {
            lvRecommended.BeginUpdate();
            lvRecommended.Items.Clear();

            foreach (Recommendation recommendation in EventService.GetRecommendations())
            {
                ListViewItem item = new ListViewItem($"{recommendation.Event.Date:dd MMM} - {recommendation.Event.Title}");
                item.SubItems.Add(recommendation.Reason);
                item.Tag = recommendation.Event;
                item.ToolTipText = recommendation.Event.Description;

                lvRecommended.Items.Add(item);
            }

            lvRecommended.EndUpdate();

            lstRecent.Items.Clear();

            // Comes off the stack, so the most recently viewed is at the top.
            foreach (LocalEvent localEvent in EventService.GetRecentlyViewed())
                lstRecent.Items.Add(localEvent);

            if (lstRecent.Items.Count == 0)
                lstRecent.Items.Add("(Nothing viewed yet)");
        }

        #endregion

        #region Search

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime? fromDate = dtpFrom.Checked ? dtpFrom.Value.Date : null;
            DateTime? toDate = dtpTo.Checked ? dtpTo.Value.Date : null;

            if (fromDate.HasValue && toDate.HasValue && fromDate > toDate)
            {
                MessageBox.Show(
                    "The 'From' date cannot be after the 'To' date.",
                    "Invalid Date Range",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                dtpFrom.Focus();
                return;
            }

            string category = cmbCategory.SelectedItem as string ?? AllCategories;
            if (category == AllCategories)
                category = string.Empty;

            List<LocalEvent> results = EventService.Search(txtKeyword.Text, category, fromDate, toDate);

            ShowEvents(results, "Search complete.");

            // Searching changes what the user is interested in, so update suggestions.
            RefreshSidePanels();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtKeyword.Clear();
            cmbCategory.SelectedIndex = 0;
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today;
            dtpFrom.Checked = false;
            dtpTo.Checked = false;

            ShowEvents(EventService.GetUpcomingEvents(), "Showing all upcoming events.");
        }

        #endregion

        #region Viewing details

        private void ShowEventDetails(LocalEvent localEvent)
        {
            EventService.RecordView(localEvent);

            MessageBox.Show(
                $"{localEvent.Title}\n\n" +
                $"Category: {localEvent.Category}\n" +
                $"Date: {localEvent.Date:dddd, dd MMMM yyyy 'at' HH:mm}\n" +
                $"Venue: {localEvent.Venue}\n\n" +
                $"{localEvent.Description}",
                "Event Details",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            RefreshSidePanels();
        }

        private void lvEvents_DoubleClick(object sender, EventArgs e)
        {
            if (lvEvents.SelectedItems.Count > 0 && lvEvents.SelectedItems[0].Tag is LocalEvent localEvent)
                ShowEventDetails(localEvent);
        }

        private void lvRecommended_DoubleClick(object sender, EventArgs e)
        {
            if (lvRecommended.SelectedItems.Count > 0 && lvRecommended.SelectedItems[0].Tag is LocalEvent localEvent)
                ShowEventDetails(localEvent);
        }

        private void lstRecent_DoubleClick(object sender, EventArgs e)
        {
            if (lstRecent.SelectedItem is LocalEvent localEvent)
                ShowEventDetails(localEvent);
        }

        private void lvAnnouncements_DoubleClick(object sender, EventArgs e)
        {
            if (lvAnnouncements.SelectedItems.Count > 0 && lvAnnouncements.SelectedItems[0].Tag is Announcement announcement)
            {
                MessageBox.Show(
                    $"{announcement.Message}\n\nPosted: {announcement.DatePosted:dd MMMM yyyy HH:mm}",
                    $"[{announcement.Priority}] {announcement.Title}",
                    MessageBoxButtons.OK,
                    announcement.Priority == AnnouncementPriority.Urgent ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
            }
        }

        #endregion

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
