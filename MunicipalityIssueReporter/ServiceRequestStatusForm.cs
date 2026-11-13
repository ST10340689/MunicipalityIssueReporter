using MunicipalityIssueReporter.DataStructures;
using MunicipalityIssueReporter.Models;
using MunicipalityIssueReporter.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MunicipalityIssueReporter
{
    public partial class ServiceRequestStatusForm : Form
    {
        private bool isInitializing = true;

        public ServiceRequestStatusForm()
        {
            InitializeComponent();
        }

        private void ServiceRequestStatusForm_Load(object sender, EventArgs e)
        {
            isInitializing = true;

            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new object[]
            {
                "All Statuses",
                "Submitted",
                "Assigned",
                "In Progress",
                "Resolved"
            });
            cmbStatus.SelectedIndex = 0;

            cmbPriority.Items.Clear();
            cmbPriority.Items.AddRange(new object[]
            {
                "All Priorities",
                "Urgent",
                "High",
                "Medium",
                "Low"
            });
            cmbPriority.SelectedIndex = 0;

            cmbSortOrder.Items.Clear();
            cmbSortOrder.Items.AddRange(new object[]
            {
                "By ID (AVL Tree In-Order)",
                "Newest First (Red-Black Tree)"
            });
            cmbSortOrder.SelectedIndex = 0;

            PopulateHierarchyTree();
            RefreshHeapPanel();
            RefreshMstAndGraphPanel(ServiceRequestGraph.CentralDepot);

            isInitializing = false;
            ApplyFilters(showNotFoundAlert: false);
        }

        private void PopulateHierarchyTree()
        {
            BasicTreeNode? previouslySelected = tvHierarchy.SelectedNode?.Tag as BasicTreeNode;

            tvHierarchy.BeginUpdate();
            tvHierarchy.Nodes.Clear();

            BasicTreeNode rootModel = ServiceRequestService.GetHierarchyRoot();
            TreeNode rootUi = CreateTreeNodeRecursive(rootModel);
            tvHierarchy.Nodes.Add(rootUi);
            tvHierarchy.ExpandAll();

            TreeNode? toSelect = FindTreeNodeByModel(rootUi, previouslySelected) ?? rootUi;
            tvHierarchy.SelectedNode = toSelect;

            tvHierarchy.EndUpdate();
        }

        private static TreeNode CreateTreeNodeRecursive(BasicTreeNode modelNode)
        {
            int count = modelNode.CollectAllRequests().Count;
            TreeNode uiNode = new TreeNode($"{modelNode.Name} ({count})")
            {
                Tag = modelNode
            };

            foreach (BasicTreeNode child in modelNode.Children)
            {
                uiNode.Nodes.Add(CreateTreeNodeRecursive(child));
            }

            return uiNode;
        }

        private static TreeNode? FindTreeNodeByModel(TreeNode current, BasicTreeNode? target)
        {
            if (target == null)
            {
                return null;
            }

            if (ReferenceEquals(current.Tag, target))
            {
                return current;
            }

            foreach (TreeNode child in current.Nodes)
            {
                TreeNode? match = FindTreeNodeByModel(child, target);
                if (match != null)
                {
                    return match;
                }
            }

            return null;
        }

        private void ApplyFilters(bool showNotFoundAlert, int? selectRequestId = null)
        {
            if (isInitializing)
            {
                return;
            }

            BasicTreeNode? selectedNode = tvHierarchy.SelectedNode?.Tag as BasicTreeNode;
            string statusFilter = cmbStatus.SelectedItem?.ToString() ?? "All Statuses";
            string priorityFilter = cmbPriority.SelectedItem?.ToString() ?? "All Priorities";
            string query = txtSearch.Text.Trim();

            List<ServiceRequest> filtered = ServiceRequestService.FilterRequests(
                selectedNode,
                statusFilter,
                priorityFilter,
                query,
                out List<int> visitedAvlPath);

            // Apply Red-Black Tree chronological order if selected and not doing a single-ID lookup
            if (cmbSortOrder.SelectedIndex == 1 && visitedAvlPath.Count == 0)
            {
                List<ServiceRequest> rbOrder = ServiceRequestService.GetAllNewestFirst();
                HashSet<int> allowedIds = new HashSet<int>();
                foreach (ServiceRequest r in filtered)
                {
                    allowedIds.Add(r.Id);
                }

                List<ServiceRequest> orderedByRb = new List<ServiceRequest>(filtered.Count);
                foreach (ServiceRequest r in rbOrder)
                {
                    if (allowedIds.Contains(r.Id))
                    {
                        orderedByRb.Add(r);
                    }
                }
                filtered = orderedByRb;
            }

            PopulateRequestsListView(filtered, selectRequestId);

            if (visitedAvlPath.Count > 0)
            {
                string pathText = FormatAvlPath(visitedAvlPath);
                if (filtered.Count > 0)
                {
                    ServiceRequest found = filtered[0];
                    lblStatusSummary.Text =
                        $"AVL Tree O(log n) Lookup ({found.ReferenceNumber}): found in {visitedAvlPath.Count} step(s) [{pathText}] " +
                        $"(Tree Height: {ServiceRequestService.AvlTreeHeight}).";

                    if (lvRequests.Items.Count > 0)
                    {
                        lvRequests.Items[0].Selected = true;
                    }
                }
                else
                {
                    lblStatusSummary.Text =
                        $"AVL Tree Lookup: Reference '{query}' not found after {visitedAvlPath.Count} step(s) [{pathText}].";

                    if (showNotFoundAlert)
                    {
                        MessageBox.Show(
                            $"No service request with Reference ID '{query}' was found in the AVL Binary Search Tree.\n\n" +
                            $"Visited AVL nodes: {pathText}",
                            "Service Request Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                string treeMode = cmbSortOrder.SelectedIndex == 1 ? "Red-Black Tree (Newest First)" : "AVL Tree (In-Order by ID)";
                lblStatusSummary.Text =
                    $"Showing {filtered.Count} of {ServiceRequestService.TotalCount} service request(s) via {treeMode}. " +
                    $"Double-click a row for full details & BFS graph analysis.";
            }
        }

        private void PopulateRequestsListView(List<ServiceRequest> requests, int? selectRequestId = null)
        {
            lvRequests.BeginUpdate();
            lvRequests.Items.Clear();

            ListViewItem? itemToSelect = null;

            foreach (ServiceRequest req in requests)
            {
                ListViewItem item = new ListViewItem(req.ReferenceNumber);
                item.SubItems.Add(req.Priority.ToString());
                item.SubItems.Add(req.StatusDisplay);
                item.SubItems.Add($"{req.ProgressPercentage}%");
                item.SubItems.Add(req.Category);
                item.SubItems.Add(req.Ward);
                item.SubItems.Add(req.Location);
                item.SubItems.Add(req.DateSubmitted.ToString("dd MMM HH:mm"));
                item.Tag = req;

                item.BackColor = GetRowHighlightColor(req);
                lvRequests.Items.Add(item);

                if (selectRequestId.HasValue && req.Id == selectRequestId.Value)
                {
                    itemToSelect = item;
                }
            }

            if (itemToSelect != null)
            {
                itemToSelect.Selected = true;
                itemToSelect.EnsureVisible();
            }
            else if (lvRequests.Items.Count > 0)
            {
                lvRequests.Items[0].Selected = true;
            }
            else
            {
                UpdateSelectedRequestControls(null);
            }

            lvRequests.EndUpdate();
        }

        private static Color GetRowHighlightColor(ServiceRequest req)
        {
            if (req.Status == ServiceRequestStatus.Resolved)
            {
                return Color.FromArgb(232, 248, 236); // Soft green for resolved
            }

            if (req.Priority == ServiceRequestPriority.Urgent)
            {
                return Color.FromArgb(255, 235, 235); // Soft red for urgent active
            }

            if (req.Priority == ServiceRequestPriority.High)
            {
                return Color.FromArgb(255, 249, 230); // Soft yellow for high priority active
            }

            if (req.Status == ServiceRequestStatus.InProgress)
            {
                return Color.FromArgb(232, 244, 253); // Soft blue for in-progress
            }

            return SystemColors.Window;
        }

        private void RefreshHeapPanel()
        {
            lvHeap.BeginUpdate();
            lvHeap.Items.Clear();

            List<ServiceRequest> heapQueue = ServiceRequestService.GetPriorityDispatchQueue();
            foreach (ServiceRequest req in heapQueue)
            {
                ListViewItem item = new ListViewItem(req.ReferenceNumber);
                item.SubItems.Add(req.Priority.ToString());
                item.SubItems.Add(req.StatusDisplay);
                item.SubItems.Add($"{req.Ward} ({req.Category})");
                item.ToolTipText = $"{req.ReferenceNumber}: {req.Description} [{req.AssignedTeam}]";
                item.Tag = req;

                if (req.Priority == ServiceRequestPriority.Urgent)
                {
                    item.BackColor = Color.FromArgb(255, 235, 235);
                }
                else if (req.Priority == ServiceRequestPriority.High)
                {
                    item.BackColor = Color.FromArgb(255, 249, 230);
                }

                lvHeap.Items.Add(item);
            }

            lvHeap.EndUpdate();
        }

        private void RefreshMstAndGraphPanel(string startZone)
        {
            lvMst.BeginUpdate();
            lvMst.Items.Clear();

            List<MstEdge> mstEdges = ServiceRequestService.GetMinimumSpanningTree(out double totalKm);
            foreach (MstEdge edge in mstEdges)
            {
                ListViewItem item = new ListViewItem(ShortenZoneName(edge.FromZone));
                item.SubItems.Add(ShortenZoneName(edge.ToZone));
                item.SubItems.Add($"{edge.DistanceKm:0.0}");
                item.SubItems.Add($"{edge.ActiveRequestsInTarget} req");
                lvMst.Items.Add(item);
            }

            lvMst.EndUpdate();

            List<string> bfsOrder = ServiceRequestService.GetBfsZoneTraversal(startZone);
            List<string> dfsOrder = ServiceRequestService.GetDfsZoneTraversal(startZone);

            lblGraphInfo.Text =
                $"Prim's MST Total Dispatch Distance: {totalKm:0.0} km ({mstEdges.Count} links)\r\n" +
                $"BFS from {ShortenZoneName(startZone)}: {FormatZoneList(bfsOrder)}\r\n" +
                $"DFS from {ShortenZoneName(startZone)}: {FormatZoneList(dfsOrder)}";
        }

        private static string ShortenZoneName(string zone)
        {
            int dash = zone.IndexOf(" - ", StringComparison.Ordinal);
            return dash > 0 ? zone.Substring(0, dash) : zone;
        }

        private static string FormatZoneList(List<string> zones)
        {
            List<string> shortNames = new List<string>(zones.Count);
            foreach (string z in zones)
            {
                shortNames.Add(ShortenZoneName(z));
            }
            return string.Join(" -> ", shortNames);
        }

        private static string FormatAvlPath(List<int> path)
        {
            List<string> refs = new List<string>(path.Count);
            foreach (int id in path)
            {
                refs.Add($"#{id:D5}");
            }
            return string.Join(" -> ", refs);
        }

        private void UpdateSelectedRequestControls(ServiceRequest? selected)
        {
            if (selected == null)
            {
                lblSelectedProgress.Text = "Progress: --";
                progressBarStatus.Value = 0;
                btnAdvanceStatus.Enabled = false;
                RefreshMstAndGraphPanel(ServiceRequestGraph.CentralDepot);
                return;
            }

            lblSelectedProgress.Text = $"{selected.ReferenceNumber}: {selected.ProgressPercentage}%";
            progressBarStatus.Value = selected.ProgressPercentage;
            btnAdvanceStatus.Enabled = selected.Status != ServiceRequestStatus.Resolved;
            RefreshMstAndGraphPanel(selected.Ward);
        }

        private void ShowServiceRequestDetails(ServiceRequest req)
        {
            ServiceRequestService.FindById(req.Id, out List<int> avlPath);
            List<ServiceRequest> nearbyRelated = ServiceRequestService.GetRelatedRequestsBfs(req, 4);
            List<string> bfsZones = ServiceRequestService.GetBfsZoneTraversal(req.Ward);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Reference ID: {req.ReferenceNumber}");
            sb.AppendLine($"Status: {req.StatusDisplay} ({req.ProgressPercentage}% Complete)");
            sb.AppendLine($"Priority: {req.Priority}");
            sb.AppendLine($"Department: {req.Department}");
            sb.AppendLine($"Category: {req.Category}");
            sb.AppendLine($"Ward / Zone: {req.Ward}");
            sb.AppendLine($"Location: {req.Location}");
            sb.AppendLine($"Assigned Team: {req.AssignedTeam}");
            sb.AppendLine($"Date Submitted: {req.DateSubmitted:dd MMM yyyy HH:mm}");
            sb.AppendLine($"Last Updated: {req.LastUpdated:dd MMM yyyy HH:mm}");
            if (!string.IsNullOrWhiteSpace(req.AttachmentPath))
            {
                sb.AppendLine($"Attachment: {req.AttachmentPath}");
            }
            sb.AppendLine();
            sb.AppendLine("Issue Description:");
            sb.AppendLine(req.Description);
            sb.AppendLine();
            sb.AppendLine("Latest Status Notes:");
            sb.AppendLine(req.StatusNotes);
            sb.AppendLine();
            sb.AppendLine($"--- Tree & Graph Diagnostics ---");
            sb.AppendLine($"AVL BST Lookup Path ({avlPath.Count} step(s)): {FormatAvlPath(avlPath)}");
            sb.AppendLine($"BFS Zone Traversal from {ShortenZoneName(req.Ward)}: {FormatZoneList(bfsZones)}");

            if (nearbyRelated.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Related Requests in Connected Zones (via Graph BFS):");
                foreach (ServiceRequest related in nearbyRelated)
                {
                    sb.AppendLine($"  • {related.ReferenceNumber} [{related.StatusDisplay}] {related.Category} - {related.Ward}");
                }
            }

            MessageBox.Show(
                sb.ToString(),
                $"Service Request {req.ReferenceNumber} - {req.StatusDisplay}",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilters(showNotFoundAlert: true);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            isInitializing = true;
            txtSearch.Clear();
            cmbStatus.SelectedIndex = 0;
            cmbPriority.SelectedIndex = 0;
            cmbSortOrder.SelectedIndex = 0;
            if (tvHierarchy.Nodes.Count > 0)
            {
                tvHierarchy.SelectedNode = tvHierarchy.Nodes[0];
            }
            isInitializing = false;

            ApplyFilters(showNotFoundAlert: false);
        }

        private void FilterControl_Changed(object sender, EventArgs e)
        {
            ApplyFilters(showNotFoundAlert: false);
        }

        private void tvHierarchy_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ApplyFilters(showNotFoundAlert: false);
        }

        private void lvRequests_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvRequests.SelectedItems.Count == 0)
            {
                UpdateSelectedRequestControls(null);
                return;
            }

            if (lvRequests.SelectedItems[0].Tag is ServiceRequest selected)
            {
                UpdateSelectedRequestControls(selected);
            }
        }

        private void lvRequests_DoubleClick(object sender, EventArgs e)
        {
            if (lvRequests.SelectedItems.Count > 0 &&
                lvRequests.SelectedItems[0].Tag is ServiceRequest selected)
            {
                ShowServiceRequestDetails(selected);
            }
        }

        private void lvHeap_DoubleClick(object sender, EventArgs e)
        {
            if (lvHeap.SelectedItems.Count > 0 &&
                lvHeap.SelectedItems[0].Tag is ServiceRequest selected)
            {
                ShowServiceRequestDetails(selected);
            }
        }

        private void btnAdvanceStatus_Click(object sender, EventArgs e)
        {
            if (lvRequests.SelectedItems.Count == 0 ||
                lvRequests.SelectedItems[0].Tag is not ServiceRequest selected)
            {
                MessageBox.Show(
                    "Please select a service request in the table first.",
                    "No Request Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (ServiceRequestService.AdvanceStatus(selected.Id))
            {
                RefreshHeapPanel();
                RefreshMstAndGraphPanel(selected.Ward);
                ApplyFilters(showNotFoundAlert: false, selectRequestId: selected.Id);
            }
        }
    }
}
