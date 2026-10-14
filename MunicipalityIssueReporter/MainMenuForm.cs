namespace MunicipalityIssueReporter
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ReportIssueForm());
        }

        private void btnEvents_Click(object sender, EventArgs e)
        {
            OpenChildForm(new LocalEventsForm());
        }

        /// <summary>
        /// Hides the menu while a page is open and shows it again when that page closes.
        /// This keeps a single menu instance alive, so closing the menu exits the app.
        /// </summary>
        private void OpenChildForm(Form child)
        {
            child.FormClosed += (_, _) => Show();
            Hide();
            child.Show();
        }
    }
}
