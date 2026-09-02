using MunicipalityIssueReporter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.IO;
using MunicipalityIssueReporter.Services;

namespace MunicipalityIssueReporter
{
    public partial class ReportIssueForm : Form
    {

        private string selectedFilePath = string.Empty;

        public ReportIssueForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void lblDescription_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainMenuForm mainmenu = new MainMenuForm();

            mainmenu.Show();

            this.Close();
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Images (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|" +
        "Documents (*.pdf;*.docx)|*.pdf;*.docx|" +
        "All Files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = dialog.FileName;
                lblSelectedFile.Text = $"Selected File: {Path.GetFileName(selectedFilePath)}";

                UpdateProgress();
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show(
                    "Please enter the location of the issue.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtLocation.Focus();
                return;
            }

            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select an issue category.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbCategory.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(rtbDescription.Text))
            {
                MessageBox.Show(
                    "Please provide a description of the issue.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                rtbDescription.Focus();
                return;
            }

            IssueReport report = new IssueReport
            {
                Location = txtLocation.Text.Trim(),
                Category = cmbCategory.SelectedItem.ToString()!,
                Description = rtbDescription.Text.Trim(),
                AttachmentPath = selectedFilePath,
                DateReported = DateTime.Now
            };

            IssueReportService.AddReport(report);

            MessageBox.Show(
                $"Your issue has been successfully reported.\n\n" +
                $"Reference Number: #{report.Id:D5}",
                "Report Submitted",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            ClearForm();
        }
        private void ClearForm()
        {
            txtLocation.Clear();

            cmbCategory.SelectedIndex = -1;

            rtbDescription.Clear();

            selectedFilePath = string.Empty;

            lblSelectedFile.Text = "No file selected";

            progressBarReport.Value = 0;

            lblProgress.Text = "Complete the form to report your issue.";
        }

        private void UpdateProgress()
        {
            int progress = 0;

            if (!string.IsNullOrWhiteSpace(txtLocation.Text))
                progress += 25;

            if (cmbCategory.SelectedIndex != -1)
                progress += 25;

            if (!string.IsNullOrWhiteSpace(rtbDescription.Text))
                progress += 25;

            if (!string.IsNullOrWhiteSpace(selectedFilePath))
                progress += 25;

            progressBarReport.Value = progress;

            lblProgress.Text = progress switch
            {
                0 => "Let's Begin.",
                25 => "Beginning Progress.",
                50 => "Halfway there.",
                75 => "Almost finished.",
                100 => "Submission Ready",
                _ => "Complete the form to report your issue."
            };
        }
        private void progressBarReport_Click(object sender, EventArgs e)
        {

        }

        private void lblLocation_Click(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void txtLocation_TextChanged(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void rtbDescription_TextChanged(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void lblProgress_Click(object sender, EventArgs e)
        {

        }

        private void ReportIssueForm_Load(object sender, EventArgs e)
        {

        }
    }
}
