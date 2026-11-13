namespace MunicipalityIssueReporter
{
    partial class ServiceRequestStatusForm
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
            grpSearch = new GroupBox();
            lblSearchId = new Label();
            txtSearch = new TextBox();
            lblStatus = new Label();
            cmbStatus = new ComboBox();
            lblPriority = new Label();
            cmbPriority = new ComboBox();
            btnSearch = new Button();
            lblSortOrder = new Label();
            cmbSortOrder = new ComboBox();
            lblSelectedProgress = new Label();
            progressBarStatus = new ProgressBar();
            btnAdvanceStatus = new Button();
            btnClear = new Button();
            grpHierarchy = new GroupBox();
            tvHierarchy = new TreeView();
            lvRequests = new ListView();
            colReqId = new ColumnHeader();
            colReqPriority = new ColumnHeader();
            colReqStatus = new ColumnHeader();
            colReqProgress = new ColumnHeader();
            colReqCategory = new ColumnHeader();
            colReqWard = new ColumnHeader();
            colReqLocation = new ColumnHeader();
            colReqSubmitted = new ColumnHeader();
            lblStatusSummary = new Label();
            grpHeap = new GroupBox();
            lvHeap = new ListView();
            colHeapId = new ColumnHeader();
            colHeapPriority = new ColumnHeader();
            colHeapStatus = new ColumnHeader();
            colHeapWard = new ColumnHeader();
            grpGraphMst = new GroupBox();
            lvMst = new ListView();
            colMstFrom = new ColumnHeader();
            colMstTo = new ColumnHeader();
            colMstDist = new ColumnHeader();
            colMstActive = new ColumnHeader();
            lblGraphInfo = new Label();
            grpSearch.SuspendLayout();
            grpHierarchy.SuspendLayout();
            grpHeap.SuspendLayout();
            grpGraphMst.SuspendLayout();
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
            lblTitle.Size = new Size(720, 40);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Service Request Status";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpSearch
            // 
            grpSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpSearch.Controls.Add(lblSearchId);
            grpSearch.Controls.Add(txtSearch);
            grpSearch.Controls.Add(lblStatus);
            grpSearch.Controls.Add(cmbStatus);
            grpSearch.Controls.Add(lblPriority);
            grpSearch.Controls.Add(cmbPriority);
            grpSearch.Controls.Add(btnSearch);
            grpSearch.Controls.Add(lblSortOrder);
            grpSearch.Controls.Add(cmbSortOrder);
            grpSearch.Controls.Add(lblSelectedProgress);
            grpSearch.Controls.Add(progressBarStatus);
            grpSearch.Controls.Add(btnAdvanceStatus);
            grpSearch.Controls.Add(btnClear);
            grpSearch.Location = new Point(12, 54);
            grpSearch.Name = "grpSearch";
            grpSearch.Size = new Size(725, 92);
            grpSearch.TabIndex = 2;
            grpSearch.TabStop = false;
            grpSearch.Text = "Track && Filter Service Requests (AVL Binary Search Tree ID Lookup)";
            // 
            // lblSearchId
            // 
            lblSearchId.AutoSize = true;
            lblSearchId.Location = new Point(12, 28);
            lblSearchId.Name = "lblSearchId";
            lblSearchId.Size = new Size(99, 15);
            lblSearchId.TabIndex = 0;
            lblSearchId.Text = "Ref ID / Keyword:";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(116, 25);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "e.g. #00001, 5, Oak Road";
            txtSearch.Size = new Size(175, 23);
            txtSearch.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(305, 28);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(42, 15);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Status:";
            // 
            // cmbStatus
            // 
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Location = new Point(352, 25);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(125, 23);
            cmbStatus.TabIndex = 3;
            cmbStatus.SelectedIndexChanged += FilterControl_Changed;
            // 
            // lblPriority
            // 
            lblPriority.AutoSize = true;
            lblPriority.Location = new Point(488, 28);
            lblPriority.Name = "lblPriority";
            lblPriority.Size = new Size(48, 15);
            lblPriority.TabIndex = 4;
            lblPriority.Text = "Priority:";
            // 
            // cmbPriority
            // 
            cmbPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPriority.FormattingEnabled = true;
            cmbPriority.Location = new Point(540, 25);
            cmbPriority.Name = "cmbPriority";
            cmbPriority.Size = new Size(100, 23);
            cmbPriority.TabIndex = 5;
            cmbPriority.SelectedIndexChanged += FilterControl_Changed;
            // 
            // btnSearch
            // 
            btnSearch.Font = new Font("Segoe UI", 9.75F);
            btnSearch.Location = new Point(648, 23);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(66, 27);
            btnSearch.TabIndex = 6;
            btnSearch.Text = "Track";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // lblSortOrder
            // 
            lblSortOrder.AutoSize = true;
            lblSortOrder.Location = new Point(12, 60);
            lblSortOrder.Name = "lblSortOrder";
            lblSortOrder.Size = new Size(65, 15);
            lblSortOrder.TabIndex = 7;
            lblSortOrder.Text = "Tree Order:";
            // 
            // cmbSortOrder
            // 
            cmbSortOrder.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSortOrder.FormattingEnabled = true;
            cmbSortOrder.Location = new Point(116, 57);
            cmbSortOrder.Name = "cmbSortOrder";
            cmbSortOrder.Size = new Size(175, 23);
            cmbSortOrder.TabIndex = 8;
            cmbSortOrder.SelectedIndexChanged += FilterControl_Changed;
            // 
            // lblSelectedProgress
            // 
            lblSelectedProgress.AutoSize = true;
            lblSelectedProgress.Location = new Point(305, 60);
            lblSelectedProgress.Name = "lblSelectedProgress";
            lblSelectedProgress.Size = new Size(103, 15);
            lblSelectedProgress.TabIndex = 9;
            lblSelectedProgress.Text = "Progress: --";
            // 
            // progressBarStatus
            // 
            progressBarStatus.Location = new Point(415, 57);
            progressBarStatus.Name = "progressBarStatus";
            progressBarStatus.Size = new Size(106, 22);
            progressBarStatus.TabIndex = 10;
            // 
            // btnAdvanceStatus
            // 
            btnAdvanceStatus.Font = new Font("Segoe UI", 9F);
            btnAdvanceStatus.Location = new Point(529, 55);
            btnAdvanceStatus.Name = "btnAdvanceStatus";
            btnAdvanceStatus.Size = new Size(112, 26);
            btnAdvanceStatus.TabIndex = 11;
            btnAdvanceStatus.Text = "Advance Status";
            btnAdvanceStatus.UseVisualStyleBackColor = true;
            btnAdvanceStatus.Click += btnAdvanceStatus_Click;
            // 
            // btnClear
            // 
            btnClear.Font = new Font("Segoe UI", 9.75F);
            btnClear.Location = new Point(648, 55);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(66, 26);
            btnClear.TabIndex = 12;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // grpHierarchy
            // 
            grpHierarchy.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            grpHierarchy.Controls.Add(tvHierarchy);
            grpHierarchy.Location = new Point(12, 154);
            grpHierarchy.Name = "grpHierarchy";
            grpHierarchy.Padding = new Padding(6);
            grpHierarchy.Size = new Size(195, 486);
            grpHierarchy.TabIndex = 3;
            grpHierarchy.TabStop = false;
            grpHierarchy.Text = "Department Tree (Basic Tree)";
            // 
            // tvHierarchy
            // 
            tvHierarchy.Dock = DockStyle.Fill;
            tvHierarchy.HideSelection = false;
            tvHierarchy.Location = new Point(6, 22);
            tvHierarchy.Name = "tvHierarchy";
            tvHierarchy.Size = new Size(183, 458);
            tvHierarchy.TabIndex = 0;
            tvHierarchy.AfterSelect += tvHierarchy_AfterSelect;
            // 
            // lvRequests
            // 
            lvRequests.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvRequests.Columns.AddRange(new ColumnHeader[]
            {
                colReqId,
                colReqPriority,
                colReqStatus,
                colReqProgress,
                colReqCategory,
                colReqWard,
                colReqLocation,
                colReqSubmitted
            });
            lvRequests.FullRowSelect = true;
            lvRequests.Location = new Point(215, 154);
            lvRequests.MultiSelect = false;
            lvRequests.Name = "lvRequests";
            lvRequests.Size = new Size(522, 486);
            lvRequests.TabIndex = 4;
            lvRequests.UseCompatibleStateImageBehavior = false;
            lvRequests.View = View.Details;
            lvRequests.SelectedIndexChanged += lvRequests_SelectedIndexChanged;
            lvRequests.DoubleClick += lvRequests_DoubleClick;
            // 
            // colReqId
            // 
            colReqId.Text = "Ref ID";
            colReqId.Width = 54;
            // 
            // colReqPriority
            // 
            colReqPriority.Text = "Priority";
            colReqPriority.Width = 54;
            // 
            // colReqStatus
            // 
            colReqStatus.Text = "Status";
            colReqStatus.Width = 68;
            // 
            // colReqProgress
            // 
            colReqProgress.Text = "Progress";
            colReqProgress.Width = 54;
            // 
            // colReqCategory
            // 
            colReqCategory.Text = "Category";
            colReqCategory.Width = 76;
            // 
            // colReqWard
            // 
            colReqWard.Text = "Ward / Zone";
            colReqWard.Width = 92;
            // 
            // colReqLocation
            // 
            colReqLocation.Text = "Location";
            colReqLocation.Width = 104;
            // 
            // colReqSubmitted
            // 
            colReqSubmitted.Text = "Submitted";
            colReqSubmitted.Width = 82;
            // 
            // lblStatusSummary
            // 
            lblStatusSummary.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblStatusSummary.Location = new Point(12, 647);
            lblStatusSummary.Name = "lblStatusSummary";
            lblStatusSummary.Size = new Size(725, 23);
            lblStatusSummary.TabIndex = 5;
            lblStatusSummary.Text = "Double-click any service request to inspect its tree lookup path and BFS zone connections.";
            lblStatusSummary.UseMnemonic = false;
            // 
            // grpHeap
            // 
            grpHeap.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            grpHeap.Controls.Add(lvHeap);
            grpHeap.Location = new Point(747, 54);
            grpHeap.Name = "grpHeap";
            grpHeap.Padding = new Padding(8);
            grpHeap.Size = new Size(361, 286);
            grpHeap.TabIndex = 6;
            grpHeap.TabStop = false;
            grpHeap.Text = "Priority Dispatch Queue (Min-Heap)";
            // 
            // lvHeap
            // 
            lvHeap.Columns.AddRange(new ColumnHeader[] { colHeapId, colHeapPriority, colHeapStatus, colHeapWard });
            lvHeap.Dock = DockStyle.Fill;
            lvHeap.FullRowSelect = true;
            lvHeap.Location = new Point(8, 24);
            lvHeap.MultiSelect = false;
            lvHeap.Name = "lvHeap";
            lvHeap.ShowItemToolTips = true;
            lvHeap.Size = new Size(345, 254);
            lvHeap.TabIndex = 0;
            lvHeap.UseCompatibleStateImageBehavior = false;
            lvHeap.View = View.Details;
            lvHeap.DoubleClick += lvHeap_DoubleClick;
            // 
            // colHeapId
            // 
            colHeapId.Text = "Ref ID";
            colHeapId.Width = 58;
            // 
            // colHeapPriority
            // 
            colHeapPriority.Text = "Priority";
            colHeapPriority.Width = 58;
            // 
            // colHeapStatus
            // 
            colHeapStatus.Text = "Status";
            colHeapStatus.Width = 72;
            // 
            // colHeapWard
            // 
            colHeapWard.Text = "Ward & Category";
            colHeapWard.Width = 135;
            // 
            // grpGraphMst
            // 
            grpGraphMst.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            grpGraphMst.Controls.Add(lblGraphInfo);
            grpGraphMst.Controls.Add(lvMst);
            grpGraphMst.Location = new Point(747, 348);
            grpGraphMst.Name = "grpGraphMst";
            grpGraphMst.Padding = new Padding(8);
            grpGraphMst.Size = new Size(361, 318);
            grpGraphMst.TabIndex = 7;
            grpGraphMst.TabStop = false;
            grpGraphMst.Text = "Zone Dispatch Network (Graph, BFS && MST)";
            // 
            // lvMst
            // 
            lvMst.Columns.AddRange(new ColumnHeader[] { colMstFrom, colMstTo, colMstDist, colMstActive });
            lvMst.Dock = DockStyle.Top;
            lvMst.FullRowSelect = true;
            lvMst.Location = new Point(8, 24);
            lvMst.MultiSelect = false;
            lvMst.Name = "lvMst";
            lvMst.Size = new Size(345, 178);
            lvMst.TabIndex = 0;
            lvMst.UseCompatibleStateImageBehavior = false;
            lvMst.View = View.Details;
            // 
            // colMstFrom
            // 
            colMstFrom.Text = "From Zone";
            colMstFrom.Width = 108;
            // 
            // colMstTo
            // 
            colMstTo.Text = "To Zone";
            colMstTo.Width = 114;
            // 
            // colMstDist
            // 
            colMstDist.Text = "Km";
            colMstDist.Width = 45;
            // 
            // colMstActive
            // 
            colMstActive.Text = "Active";
            colMstActive.Width = 50;
            // 
            // lblGraphInfo
            // 
            lblGraphInfo.Dock = DockStyle.Fill;
            lblGraphInfo.Font = new Font("Segoe UI", 8.5F);
            lblGraphInfo.Location = new Point(8, 202);
            lblGraphInfo.Name = "lblGraphInfo";
            lblGraphInfo.Padding = new Padding(2, 6, 2, 2);
            lblGraphInfo.Size = new Size(345, 108);
            lblGraphInfo.TabIndex = 1;
            lblGraphInfo.Text = "MST Route Distance: --";
            lblGraphInfo.UseMnemonic = false;
            // 
            // ServiceRequestStatusForm
            // 
            AcceptButton = btnSearch;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1120, 680);
            Controls.Add(grpGraphMst);
            Controls.Add(grpHeap);
            Controls.Add(lblStatusSummary);
            Controls.Add(lvRequests);
            Controls.Add(grpHierarchy);
            Controls.Add(grpSearch);
            Controls.Add(lblTitle);
            Controls.Add(btnBack);
            MinimumSize = new Size(1060, 660);
            Name = "ServiceRequestStatusForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Service Request Status";
            Load += ServiceRequestStatusForm_Load;
            grpSearch.ResumeLayout(false);
            grpSearch.PerformLayout();
            grpHierarchy.ResumeLayout(false);
            grpHeap.ResumeLayout(false);
            grpGraphMst.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button btnBack;
        private Label lblTitle;
        private GroupBox grpSearch;
        private Label lblSearchId;
        private TextBox txtSearch;
        private Label lblStatus;
        private ComboBox cmbStatus;
        private Label lblPriority;
        private ComboBox cmbPriority;
        private Button btnSearch;
        private Label lblSortOrder;
        private ComboBox cmbSortOrder;
        private Label lblSelectedProgress;
        private ProgressBar progressBarStatus;
        private Button btnAdvanceStatus;
        private Button btnClear;
        private GroupBox grpHierarchy;
        private TreeView tvHierarchy;
        private ListView lvRequests;
        private ColumnHeader colReqId;
        private ColumnHeader colReqPriority;
        private ColumnHeader colReqStatus;
        private ColumnHeader colReqProgress;
        private ColumnHeader colReqCategory;
        private ColumnHeader colReqWard;
        private ColumnHeader colReqLocation;
        private ColumnHeader colReqSubmitted;
        private Label lblStatusSummary;
        private GroupBox grpHeap;
        private ListView lvHeap;
        private ColumnHeader colHeapId;
        private ColumnHeader colHeapPriority;
        private ColumnHeader colHeapStatus;
        private ColumnHeader colHeapWard;
        private GroupBox grpGraphMst;
        private ListView lvMst;
        private ColumnHeader colMstFrom;
        private ColumnHeader colMstTo;
        private ColumnHeader colMstDist;
        private ColumnHeader colMstActive;
        private Label lblGraphInfo;
    }
}
