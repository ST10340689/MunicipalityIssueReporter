namespace MunicipalityIssueReporter
{
    partial class ReportIssueForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblLocation = new Label();
            txtLocation = new TextBox();
            lblCategory = new Label();
            cmbCategory = new ComboBox();
            lblDescription = new Label();
            rtbDescription = new RichTextBox();
            lblAttachment = new Label();
            btnAttach = new Button();
            lblSelectedFile = new Label();
            lblProgress = new Label();
            progressBarReport = new ProgressBar();
            btnSubmit = new Button();
            button1 = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(487, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(197, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Report an Issue";
            lblTitle.Click += lblTitle_Click;
            // 
            // lblLocation
            // 
            lblLocation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblLocation.AutoSize = true;
            lblLocation.Location = new Point(31, 83);
            lblLocation.Name = "lblLocation";
            lblLocation.Size = new Size(56, 15);
            lblLocation.TabIndex = 1;
            lblLocation.Text = "Location:";
            lblLocation.Click += lblLocation_Click;
            // 
            // txtLocation
            // 
            txtLocation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtLocation.Location = new Point(93, 80);
            txtLocation.Name = "txtLocation";
            txtLocation.Size = new Size(266, 23);
            txtLocation.TabIndex = 2;
            txtLocation.TextChanged += txtLocation_TextChanged;
            // 
            // lblCategory
            // 
            lblCategory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCategory.AutoSize = true;
            lblCategory.Location = new Point(31, 149);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(58, 15);
            lblCategory.TabIndex = 3;
            lblCategory.Text = "Category:";
            lblCategory.Click += label1_Click;
            // 
            // cmbCategory
            // 
            cmbCategory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Items.AddRange(new object[] { "Sanitation", "Roads", "Electricity", "Water", "Waste Management", "Public Safety", "Other" });
            cmbCategory.Location = new Point(95, 146);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(266, 23);
            cmbCategory.TabIndex = 4;
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            // 
            // lblDescription
            // 
            lblDescription.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(31, 202);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(70, 15);
            lblDescription.TabIndex = 5;
            lblDescription.Text = "Description:";
            lblDescription.Click += lblDescription_Click;
            // 
            // rtbDescription
            // 
            rtbDescription.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rtbDescription.DetectUrls = false;
            rtbDescription.Location = new Point(107, 202);
            rtbDescription.Name = "rtbDescription";
            rtbDescription.Size = new Size(600, 120);
            rtbDescription.TabIndex = 6;
            rtbDescription.Text = "";
            rtbDescription.TextChanged += rtbDescription_TextChanged;
            // 
            // lblAttachment
            // 
            lblAttachment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblAttachment.AutoSize = true;
            lblAttachment.Location = new Point(31, 344);
            lblAttachment.Name = "lblAttachment";
            lblAttachment.Size = new Size(73, 15);
            lblAttachment.TabIndex = 7;
            lblAttachment.Text = "Attachment:";
            lblAttachment.Click += label1_Click_1;
            // 
            // btnAttach
            // 
            btnAttach.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAttach.Font = new Font("Segoe UI", 9.75F);
            btnAttach.Location = new Point(107, 340);
            btnAttach.Name = "btnAttach";
            btnAttach.Size = new Size(200, 23);
            btnAttach.TabIndex = 8;
            btnAttach.Text = "Attach Image / Document";
            btnAttach.UseVisualStyleBackColor = true;
            btnAttach.Click += btnAttach_Click;
            // 
            // lblSelectedFile
            // 
            lblSelectedFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSelectedFile.Location = new Point(31, 380);
            lblSelectedFile.Name = "lblSelectedFile";
            lblSelectedFile.Size = new Size(500, 30);
            lblSelectedFile.TabIndex = 9;
            lblSelectedFile.Text = "No file selected";
            lblSelectedFile.Click += label1_Click_2;
            // 
            // lblProgress
            // 
            lblProgress.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblProgress.ImageAlign = ContentAlignment.MiddleLeft;
            lblProgress.Location = new Point(809, 562);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(218, 23);
            lblProgress.TabIndex = 10;
            lblProgress.Text = "Complete the form to report your issue";
            lblProgress.Click += lblProgress_Click;
            // 
            // progressBarReport
            // 
            progressBarReport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            progressBarReport.Location = new Point(393, 560);
            progressBarReport.Name = "progressBarReport";
            progressBarReport.Size = new Size(388, 25);
            progressBarReport.TabIndex = 11;
            progressBarReport.Click += progressBarReport_Click;
            // 
            // btnSubmit
            // 
            btnSubmit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSubmit.Font = new Font("Segoe UI", 9.75F);
            btnSubmit.Location = new Point(487, 499);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(206, 42);
            btnSubmit.TabIndex = 12;
            btnSubmit.Text = "Submit Issue";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Font = new Font("Segoe UI", 9.75F);
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(134, 24);
            button1.TabIndex = 13;
            button1.Text = "Back to Menu";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // ReportIssueForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1122, 611);
            Controls.Add(button1);
            Controls.Add(btnSubmit);
            Controls.Add(progressBarReport);
            Controls.Add(lblProgress);
            Controls.Add(lblSelectedFile);
            Controls.Add(btnAttach);
            Controls.Add(lblAttachment);
            Controls.Add(rtbDescription);
            Controls.Add(lblDescription);
            Controls.Add(cmbCategory);
            Controls.Add(lblCategory);
            Controls.Add(txtLocation);
            Controls.Add(lblLocation);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MinimumSize = new Size(700, 550);
            Name = "ReportIssueForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ReportIssueForm";
            Load += ReportIssueForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblLocation;
        private TextBox txtLocation;
        private Label lblCategory;
        private ComboBox cmbCategory;
        private Label lblDescription;
        private RichTextBox rtbDescription;
        private Label lblAttachment;
        private Button btnAttach;
        private Label lblSelectedFile;
        private Label lblProgress;
        private ProgressBar progressBarReport;
        private Button btnSubmit;
        private Button button1;
    }
}