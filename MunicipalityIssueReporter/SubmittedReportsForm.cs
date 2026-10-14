using MunicipalityIssueReporter.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MunicipalityIssueReporter
{
    public partial class SubmittedReportsForm : Form
    {
        public SubmittedReportsForm()
        {
            InitializeComponent();

            LoadReports();
        }


        private void LoadReports()
        {
            dgvReports.DataSource = null;
            dgvReports.DataSource = IssueReportService.GetReports();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvReports_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
