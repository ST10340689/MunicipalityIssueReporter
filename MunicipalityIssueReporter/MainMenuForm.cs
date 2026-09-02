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
            ReportIssueForm reportIssueForm = new ReportIssueForm();

            reportIssueForm.Show();

            this.Hide();
        }
    }
}
