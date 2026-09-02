namespace MunicipalityIssueReporter
{
    partial class SubmittedReportsForm
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
            dgvReports = new DataGridView();
            ColID = new DataGridViewTextBoxColumn();
            ColLocation = new DataGridViewTextBoxColumn();
            colCategory = new DataGridViewTextBoxColumn();
            colDescription = new DataGridViewTextBoxColumn();
            colDateReported = new DataGridViewTextBoxColumn();
            colAttachment = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvReports).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(884, 60);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Submitted Reports";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // dgvReports
            // 
            dgvReports.AllowUserToAddRows = false;
            dgvReports.AllowUserToDeleteRows = false;
            dgvReports.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReports.Columns.AddRange(new DataGridViewColumn[] { ColID, ColLocation, colCategory, colDescription, colDateReported, colAttachment });
            dgvReports.Location = new Point(0, 63);
            dgvReports.MultiSelect = false;
            dgvReports.Name = "dgvReports";
            dgvReports.ReadOnly = true;
            dgvReports.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReports.Size = new Size(884, 383);
            dgvReports.TabIndex = 1;
            dgvReports.CellContentClick += dgvReports_CellContentClick;
            // 
            // ColID
            // 
            ColID.DataPropertyName = "Id";
            ColID.HeaderText = "ID";
            ColID.Name = "ColID";
            ColID.ReadOnly = true;
            // 
            // ColLocation
            // 
            ColLocation.DataPropertyName = "Location";
            ColLocation.HeaderText = "Location";
            ColLocation.Name = "ColLocation";
            ColLocation.ReadOnly = true;
            // 
            // colCategory
            // 
            colCategory.DataPropertyName = "Category";
            colCategory.HeaderText = "Category";
            colCategory.Name = "colCategory";
            colCategory.ReadOnly = true;
            // 
            // colDescription
            // 
            colDescription.DataPropertyName = "Description";
            colDescription.HeaderText = "Description";
            colDescription.Name = "colDescription";
            colDescription.ReadOnly = true;
            // 
            // colDateReported
            // 
            colDateReported.DataPropertyName = "DateReported";
            colDateReported.HeaderText = "Date Reported";
            colDateReported.Name = "colDateReported";
            colDateReported.ReadOnly = true;
            // 
            // colAttachment
            // 
            colAttachment.DataPropertyName = "AttachmentPath";
            colAttachment.HeaderText = "Attachment";
            colAttachment.Name = "colAttachment";
            colAttachment.ReadOnly = true;
            // 
            // SubmittedReportsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 561);
            Controls.Add(dgvReports);
            Controls.Add(lblTitle);
            MaximizeBox = false;
            Name = "SubmittedReportsForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Submitted Reports";
            ((System.ComponentModel.ISupportInitialize)dgvReports).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitle;
        private DataGridView dgvReports;
        private DataGridViewTextBoxColumn ColID;
        private DataGridViewTextBoxColumn ColLocation;
        private DataGridViewTextBoxColumn colCategory;
        private DataGridViewTextBoxColumn colDescription;
        private DataGridViewTextBoxColumn colDateReported;
        private DataGridViewTextBoxColumn colAttachment;
    }
}