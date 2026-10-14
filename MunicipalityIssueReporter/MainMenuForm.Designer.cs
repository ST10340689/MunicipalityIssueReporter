namespace MunicipalityIssueReporter
{
    partial class MainMenuForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitle = new Label();
            btnReportIssues = new Button();
            btnEvents = new Button();
            btnServiceStatus = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top;
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(155, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(476, 31);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "MUNICIPALITY SERVICE PORTAL";
            lblTitle.Click += label1_Click;
            // 
            // btnReportIssues
            // 
            btnReportIssues.Anchor = AnchorStyles.Top;
            btnReportIssues.Location = new Point(169, 106);
            btnReportIssues.Name = "btnReportIssues";
            btnReportIssues.Size = new Size(443, 39);
            btnReportIssues.TabIndex = 1;
            btnReportIssues.Text = "Report Issues";
            btnReportIssues.UseVisualStyleBackColor = true;
            btnReportIssues.Click += button1_Click;
            // 
            // btnEvents
            // 
            btnEvents.Anchor = AnchorStyles.Top;
            btnEvents.Location = new Point(169, 211);
            btnEvents.Name = "btnEvents";
            btnEvents.Size = new Size(443, 39);
            btnEvents.TabIndex = 4;
            btnEvents.Text = "Local Events and Announcements";
            btnEvents.UseVisualStyleBackColor = true;
            btnEvents.Click += btnEvents_Click;
            // 
            // btnServiceStatus
            // 
            btnServiceStatus.Anchor = AnchorStyles.Top;
            btnServiceStatus.Enabled = false;
            btnServiceStatus.Location = new Point(169, 307);
            btnServiceStatus.Name = "btnServiceStatus";
            btnServiceStatus.Size = new Size(443, 39);
            btnServiceStatus.TabIndex = 5;
            btnServiceStatus.Text = "Service Request Status";
            btnServiceStatus.UseVisualStyleBackColor = true;
            // 
            // MainMenuForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnServiceStatus);
            Controls.Add(btnEvents);
            Controls.Add(btnReportIssues);
            Controls.Add(lblTitle);
            MinimumSize = new Size(760, 440);
            Name = "MainMenuForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Municipality Service Portal";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Button btnReportIssues;
        private Button btnEvents;
        private Button btnServiceStatus;
    }
}
