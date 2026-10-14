namespace MunicipalityIssueReporter
{
    partial class LocalEventsForm
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
            btnBack = new Button();
            lblTitle = new Label();
            grpAnnouncements = new GroupBox();
            lvAnnouncements = new ListView();
            colAnnPriority = new ColumnHeader();
            colAnnPosted = new ColumnHeader();
            colAnnTitle = new ColumnHeader();
            colAnnMessage = new ColumnHeader();
            grpSearch = new GroupBox();
            lblKeyword = new Label();
            txtKeyword = new TextBox();
            lblCategory = new Label();
            cmbCategory = new ComboBox();
            lblFrom = new Label();
            dtpFrom = new DateTimePicker();
            lblTo = new Label();
            dtpTo = new DateTimePicker();
            btnSearch = new Button();
            btnClear = new Button();
            lvEvents = new ListView();
            colEventDate = new ColumnHeader();
            colEventTitle = new ColumnHeader();
            colEventCategory = new ColumnHeader();
            colEventVenue = new ColumnHeader();
            lblResults = new Label();
            grpRecommended = new GroupBox();
            lvRecommended = new ListView();
            colRecEvent = new ColumnHeader();
            colRecReason = new ColumnHeader();
            grpRecent = new GroupBox();
            lstRecent = new ListBox();
            grpAnnouncements.SuspendLayout();
            grpSearch.SuspendLayout();
            grpRecommended.SuspendLayout();
            grpRecent.SuspendLayout();
            SuspendLayout();
            // 
            // btnBack
            // 
            btnBack.Font = new Font("Segoe UI", 9.75F);
            btnBack.Location = new Point(12, 12);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(134, 24);
            btnBack.TabIndex = 0;
            btnBack.Text = "Back to Menu";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTitle.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(200, 8);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(700, 40);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Local Events and Announcements";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpAnnouncements
            // 
            grpAnnouncements.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpAnnouncements.Controls.Add(lvAnnouncements);
            grpAnnouncements.Location = new Point(12, 56);
            grpAnnouncements.Name = "grpAnnouncements";
            grpAnnouncements.Padding = new Padding(8);
            grpAnnouncements.Size = new Size(1076, 154);
            grpAnnouncements.TabIndex = 2;
            grpAnnouncements.TabStop = false;
            grpAnnouncements.Text = "Announcements (most urgent first) - double-click to read";
            // 
            // lvAnnouncements
            // 
            lvAnnouncements.Columns.AddRange(new ColumnHeader[] { colAnnPriority, colAnnPosted, colAnnTitle, colAnnMessage });
            lvAnnouncements.Dock = DockStyle.Fill;
            lvAnnouncements.FullRowSelect = true;
            lvAnnouncements.MultiSelect = false;
            lvAnnouncements.Name = "lvAnnouncements";
            lvAnnouncements.TabIndex = 0;
            lvAnnouncements.UseCompatibleStateImageBehavior = false;
            lvAnnouncements.View = View.Details;
            lvAnnouncements.DoubleClick += lvAnnouncements_DoubleClick;
            // 
            // colAnnPriority
            // 
            colAnnPriority.Text = "Priority";
            colAnnPriority.Width = 75;
            // 
            // colAnnPosted
            // 
            colAnnPosted.Text = "Posted";
            colAnnPosted.Width = 115;
            // 
            // colAnnTitle
            // 
            colAnnTitle.Text = "Title";
            colAnnTitle.Width = 215;
            // 
            // colAnnMessage
            // 
            colAnnMessage.Text = "Message";
            colAnnMessage.Width = 615;
            // 
            // grpSearch
            // 
            grpSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpSearch.Controls.Add(lblKeyword);
            grpSearch.Controls.Add(txtKeyword);
            grpSearch.Controls.Add(lblCategory);
            grpSearch.Controls.Add(cmbCategory);
            grpSearch.Controls.Add(btnSearch);
            grpSearch.Controls.Add(lblFrom);
            grpSearch.Controls.Add(dtpFrom);
            grpSearch.Controls.Add(lblTo);
            grpSearch.Controls.Add(dtpTo);
            grpSearch.Controls.Add(btnClear);
            grpSearch.Location = new Point(12, 220);
            grpSearch.Name = "grpSearch";
            grpSearch.Size = new Size(700, 92);
            grpSearch.TabIndex = 3;
            grpSearch.TabStop = false;
            grpSearch.Text = "Search Events";
            // 
            // lblKeyword
            // 
            lblKeyword.AutoSize = true;
            lblKeyword.Location = new Point(12, 28);
            lblKeyword.Name = "lblKeyword";
            lblKeyword.Size = new Size(56, 15);
            lblKeyword.TabIndex = 0;
            lblKeyword.Text = "Keyword:";
            // 
            // txtKeyword
            // 
            txtKeyword.Location = new Point(80, 25);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.PlaceholderText = "e.g. market, soccer, clinic";
            txtKeyword.Size = new Size(200, 23);
            txtKeyword.TabIndex = 1;
            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.Location = new Point(300, 28);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(58, 15);
            lblCategory.TabIndex = 2;
            lblCategory.Text = "Category:";
            // 
            // cmbCategory
            // 
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Location = new Point(370, 25);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(180, 23);
            cmbCategory.TabIndex = 3;
            // 
            // btnSearch
            // 
            btnSearch.Font = new Font("Segoe UI", 9.75F);
            btnSearch.Location = new Point(570, 22);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(110, 28);
            btnSearch.TabIndex = 4;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // lblFrom
            // 
            lblFrom.AutoSize = true;
            lblFrom.Location = new Point(12, 61);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new Size(38, 15);
            lblFrom.TabIndex = 5;
            lblFrom.Text = "From:";
            // 
            // dtpFrom
            // 
            dtpFrom.Checked = false;
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(80, 57);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.ShowCheckBox = true;
            dtpFrom.Size = new Size(200, 23);
            dtpFrom.TabIndex = 6;
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Location = new Point(300, 61);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(23, 15);
            lblTo.TabIndex = 7;
            lblTo.Text = "To:";
            // 
            // dtpTo
            // 
            dtpTo.Checked = false;
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(370, 57);
            dtpTo.Name = "dtpTo";
            dtpTo.ShowCheckBox = true;
            dtpTo.Size = new Size(180, 23);
            dtpTo.TabIndex = 8;
            // 
            // btnClear
            // 
            btnClear.Font = new Font("Segoe UI", 9.75F);
            btnClear.Location = new Point(570, 54);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(110, 28);
            btnClear.TabIndex = 9;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // lvEvents
            // 
            lvEvents.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvEvents.Columns.AddRange(new ColumnHeader[] { colEventDate, colEventTitle, colEventCategory, colEventVenue });
            lvEvents.FullRowSelect = true;
            lvEvents.Location = new Point(12, 322);
            lvEvents.MultiSelect = false;
            lvEvents.Name = "lvEvents";
            lvEvents.Size = new Size(700, 308);
            lvEvents.TabIndex = 4;
            lvEvents.UseCompatibleStateImageBehavior = false;
            lvEvents.View = View.Details;
            lvEvents.DoubleClick += lvEvents_DoubleClick;
            // 
            // colEventDate
            // 
            colEventDate.Text = "Date";
            colEventDate.Width = 130;
            // 
            // colEventTitle
            // 
            colEventTitle.Text = "Event";
            colEventTitle.Width = 210;
            // 
            // colEventCategory
            // 
            colEventCategory.Text = "Category";
            colEventCategory.Width = 110;
            // 
            // colEventVenue
            // 
            colEventVenue.Text = "Venue";
            colEventVenue.Width = 225;
            // 
            // lblResults
            // 
            lblResults.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblResults.Location = new Point(12, 638);
            lblResults.Name = "lblResults";
            lblResults.Size = new Size(700, 23);
            lblResults.TabIndex = 5;
            lblResults.Text = "Double-click an event to view its details.";
            // 
            // grpRecommended
            // 
            grpRecommended.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            grpRecommended.Controls.Add(lvRecommended);
            grpRecommended.Location = new Point(725, 220);
            grpRecommended.Name = "grpRecommended";
            grpRecommended.Padding = new Padding(8);
            grpRecommended.Size = new Size(363, 240);
            grpRecommended.TabIndex = 6;
            grpRecommended.TabStop = false;
            grpRecommended.Text = "Recommended For You";
            // 
            // lvRecommended
            // 
            lvRecommended.Columns.AddRange(new ColumnHeader[] { colRecEvent, colRecReason });
            lvRecommended.Dock = DockStyle.Fill;
            lvRecommended.FullRowSelect = true;
            lvRecommended.MultiSelect = false;
            lvRecommended.Name = "lvRecommended";
            lvRecommended.ShowItemToolTips = true;
            lvRecommended.TabIndex = 0;
            lvRecommended.UseCompatibleStateImageBehavior = false;
            lvRecommended.View = View.Details;
            lvRecommended.DoubleClick += lvRecommended_DoubleClick;
            // 
            // colRecEvent
            // 
            colRecEvent.Text = "Event";
            colRecEvent.Width = 152;
            // 
            // colRecReason
            // 
            colRecReason.Text = "Why";
            colRecReason.Width = 160;
            // 
            // grpRecent
            // 
            grpRecent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            grpRecent.Controls.Add(lstRecent);
            grpRecent.Location = new Point(725, 468);
            grpRecent.Name = "grpRecent";
            grpRecent.Padding = new Padding(8);
            grpRecent.Size = new Size(363, 193);
            grpRecent.TabIndex = 7;
            grpRecent.TabStop = false;
            grpRecent.Text = "Recently Viewed";
            // 
            // lstRecent
            // 
            lstRecent.Dock = DockStyle.Fill;
            lstRecent.FormattingEnabled = true;
            lstRecent.IntegralHeight = false;
            lstRecent.Name = "lstRecent";
            lstRecent.TabIndex = 0;
            lstRecent.DoubleClick += lstRecent_DoubleClick;
            // 
            // LocalEventsForm
            // 
            AcceptButton = btnSearch;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 672);
            Controls.Add(grpRecent);
            Controls.Add(grpRecommended);
            Controls.Add(lblResults);
            Controls.Add(lvEvents);
            Controls.Add(grpSearch);
            Controls.Add(grpAnnouncements);
            Controls.Add(lblTitle);
            Controls.Add(btnBack);
            MinimumSize = new Size(1050, 650);
            Name = "LocalEventsForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Local Events and Announcements";
            Load += LocalEventsForm_Load;
            grpAnnouncements.ResumeLayout(false);
            grpSearch.ResumeLayout(false);
            grpSearch.PerformLayout();
            grpRecommended.ResumeLayout(false);
            grpRecent.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button btnBack;
        private Label lblTitle;
        private GroupBox grpAnnouncements;
        private ListView lvAnnouncements;
        private ColumnHeader colAnnPriority;
        private ColumnHeader colAnnPosted;
        private ColumnHeader colAnnTitle;
        private ColumnHeader colAnnMessage;
        private GroupBox grpSearch;
        private Label lblKeyword;
        private TextBox txtKeyword;
        private Label lblCategory;
        private ComboBox cmbCategory;
        private Label lblFrom;
        private DateTimePicker dtpFrom;
        private Label lblTo;
        private DateTimePicker dtpTo;
        private Button btnSearch;
        private Button btnClear;
        private ListView lvEvents;
        private ColumnHeader colEventDate;
        private ColumnHeader colEventTitle;
        private ColumnHeader colEventCategory;
        private ColumnHeader colEventVenue;
        private Label lblResults;
        private GroupBox grpRecommended;
        private ListView lvRecommended;
        private ColumnHeader colRecEvent;
        private ColumnHeader colRecReason;
        private GroupBox grpRecent;
        private ListBox lstRecent;
    }
}
