using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Globalization;
using Syncfusion.Blazor.Diagram;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace OrganizationalChart
{
    /// <summary>
    /// Partial class representing the DiagramToolBar component.
    /// </summary>
    public partial class DiagramToolBar
    {
        [Inject]
        protected IJSRuntime? jsRuntime { get; set; }
        #region events
        /// <summary>
        /// Event handler for the zoom change in the diagram.
        /// </summary>
        /// <param name="args">Arguments for the zoom change event.</param>
        private void DrawZoomChange(Syncfusion.Blazor.SplitButtons.MenuEventArgs args)
        {
            //How to resolve dereference warnings
            var diagram = Parent?.DiagramContent?.Diagram;// CS8602 on "Error"
            double currentZoom = Parent.DiagramContent.CurrentZoom;
            switch (args.Item.Text)
            {
                case "Zoom In":
                    Parent.DiagramContent.ZoomTo(new DiagramMainContent.ZoomOptions() { Type = "ZoomIn", ZoomFactor = 0.2 });
                    break;
                case "Zoom Out":
                    Parent.DiagramContent.ZoomTo(new DiagramMainContent.ZoomOptions() { Type = "ZoomOut", ZoomFactor = 0.2 });
                    break;
                case "Zoom to Fit":
                    FitOptions fitoption = new FitOptions()
                    {
                        Mode = FitMode.Both,
                        Region = DiagramRegion.Content,
                    };
                    diagram.FitToPage(fitoption);
                    break;
                case "Zoom to 50%":
                    Parent.DiagramContent.ZoomTo(new DiagramMainContent.ZoomOptions() { ZoomFactor = ((0.5 / currentZoom) - 1) });
                    break;
                case "Zoom to 100%":
                    Parent.DiagramContent.ZoomTo(new DiagramMainContent.ZoomOptions() { ZoomFactor = ((1 / currentZoom) - 1) });
                    break;
                case "Zoom to 200%":
                    Parent.DiagramContent.ZoomTo(new DiagramMainContent.ZoomOptions() { ZoomFactor = ((2 / currentZoom) - 1) });
                    break;
            }
            ZoomItemDropdownContent = FormattableString.Invariant($"{Math.Round(Parent.DiagramContent.CurrentZoom * 100)}") + "%";
        }
        /// <summary>
        /// Handles the delete action for selected diagram objects.
        /// </summary>
        public void DeleteData()
        {
            bool GroupAction = false;
            SfDiagramComponent diagram = Parent.DiagramContent.Diagram;
            if ((diagram.SelectionSettings.Nodes.Count > 1 || diagram.SelectionSettings.Connectors.Count > 1 || ((diagram.SelectionSettings.Nodes.Count + diagram.SelectionSettings.Connectors.Count) > 1)))
            {
                GroupAction = true;
            }
            if (GroupAction)
            {
                diagram.StartGroupAction();
            }
            if (diagram.SelectionSettings.Nodes.Count != 0)
            {
                for (var i = diagram.SelectionSettings.Nodes.Count - 1; i >= 0; i--)
                {
                    var item = diagram.SelectionSettings.Nodes[i];

                    diagram.Nodes.Remove(item);
                }
            }
            if (diagram.SelectionSettings.Connectors.Count != 0)
            {
                for (var i = diagram.SelectionSettings.Connectors.Count - 1; i >= 0; i--)
                {
                    var item1 = diagram.SelectionSettings.Connectors[i];

                    diagram.Connectors.Remove(item1);
                }
            }
            if (GroupAction)
                diagram.EndGroupAction();
        }
        #endregion
        /// <summary>
        /// Updates the toolbar items based on the current diagram selection.
        /// </summary>
        public void DiagramSelectionToolbarItems()
        {
            
        }
        /// <summary>
        /// Handles the change in the diagram's zoom value and updates the corresponding toolbar dropdown content.
        /// </summary>
        public void DiagramZoomValueChange()
        {
            ZoomItemDropdownContent = FormattableString.Invariant($"{Math.Round(Parent.DiagramContent.CurrentZoom * 100)}") + "%";
            StateHasChanged();
        }

        #region public methods
        /// <summary>
        /// Enables toolbar items based on the selected objects and event name.
        /// </summary>
        /// <typeparam name="T">Type of the selected objects.</typeparam>
        /// <param name="obj">Selected objects.</param>
        /// <param name="eventname">Name of the event.</param>
        public void EnableToolbarItems<T>(T obj, string eventname)
        {

            SfDiagramComponent diagram = Parent.DiagramContent.Diagram;
            ObservableCollection<NodeBase> collection = new ObservableCollection<NodeBase>();
            if (eventname == "selectionchange")
            {
                foreach (NodeBase node in obj as ObservableCollection<IDiagramObject>)
                {
                    Node val = node as Node;
                    collection.Add(node);
                }
                UtilityMethods_enableToolbarItems(collection);
            }

            if (eventname == "historychange")
            {
                RemoveUndo();
                RemoveRedo();
                if (diagram.HistoryManager.CanUndo)
                {
                    this.Parent.DiagramContent.IsUndo = diagram.HistoryManager.CanUndo;
                    this.Parent.DiagramContent.IsRedo = diagram.HistoryManager.CanRedo;
                    toolbarClassName += " db-undo";
                }
                if (diagram.HistoryManager.CanRedo)
                {
                    this.Parent.DiagramContent.IsRedo = diagram.HistoryManager.CanRedo;
                    this.Parent.DiagramContent.IsUndo = diagram.HistoryManager.CanUndo;
                    toolbarClassName += " db-redo";
                }
                StateHasChanged();
            }
        }
        /// <summary>
        /// Utility method to enable toolbar items based on the selected objects.
        /// </summary>
        /// <param name="SelectedObjects">Selected diagram objects.</param>
        public void UtilityMethods_enableToolbarItems(ObservableCollection<NodeBase> SelectedObjects)
        {
            SfDiagramComponent diagram = Parent.DiagramContent.Diagram;
            removeClassElement();
            if (this.Parent.DiagramContent.IsUndo)
            {
                toolbarClassName += " db-undo";
            }
            if (this.Parent.DiagramContent.IsRedo)
            {
                toolbarClassName += " db-redo";
            }
            if (SelectedObjects.Count > 0)
            {
                toolbarClassName = toolbarClassName + " db-child-sibling";
                var inedges = SelectedObjects[0] is Node ? (SelectedObjects[0] as Node).InEdges.Count : 0;
                changeChildParentCssName = SelectedObjects[0].ID == "rootNode" || inedges == 0 ? "tb-item-start tb-item-sibling" : "tb-item-start tb-item-child";
                addSiblingCssName = SelectedObjects[0].ID == "rootNode" || inedges == 0 ? "tb-item-start tb-item-sibling" : "tb-item-start tb-item-child";
            }
                StateHasChanged();
        }
        /// <summary>
        /// Removes the "Undo" class from the toolbar class name.
        /// </summary>
        public void RemoveUndo()
        {
            string undo = "undo";
            if (toolbarClassName.Contains(undo))
            {
                int first = toolbarClassName.IndexOf(undo);
                toolbarClassName = toolbarClassName.Remove(first - 4, 8);
            }
        }
        /// <summary>
        /// Event handler for the toolbar click event in the organization chart.
        /// </summary>
        /// <param name="args">Arguments for the click event.</param>
        private async Task ToolbarEditorClickInOrgChart(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            var diagram = Parent.DiagramContent.Diagram;
            var commandType = args.Item.TooltipText.ToLower(new CultureInfo("en-US"));
            switch (commandType)
            {
                case "undo":
                    diagram.Undo();
                     EnableToolbarItems(new object() { }, "historychange");
                    break;
                case "redo":
                    diagram.Redo();
                     EnableToolbarItems(new object() { }, "historychange");
                    break;
                case "pan tool":
                    Parent.DiagramContent.UpdateTool();
                    diagram.ClearSelection();
                    break;
                case "pointer":
                    Parent.DiagramContent.UpdatePointerTool();
                    break;
                case "add child":
                   
                    if (diagram.SelectionSettings.Nodes.Count > 0)
                    {
                        await Parent.OrgChartPropertyPanel.AddNode(diagram.SelectionSettings.Nodes[0].ID);
                    }
                   
                    break;
                case "add a child to the same level":
                    Parent.OrgChartPropertyPanel.ChildToSameLevel();
                    break;
                case "move the child parent to the next level":
                    diagram.StartGroupAction();
                    await Parent.OrgChartPropertyPanel.ChangeChildParent();
                    diagram.EndGroupAction();
                    break;
            }
            if (commandType == "pan tool" || commandType == "pointer")
            {
#pragma warning disable CA1307 // Specify StringComparison
                if (args.Item.CssClass.IndexOf("tb-item-selected") == -1)
#pragma warning restore CA1307 // Specify StringComparison
                {
                    if (commandType == "pan tool")
                        PanItemCssClass += " tb-item-selected";
                    if (commandType == "pointer")
                        PointerItemCssClass += " tb-item-selected";
                    await removeSelectedToolbarItem(commandType).ConfigureAwait(true);
                }
            }
           //await Parent.DiagramContent.Diagram.DoLayoutAsync();
            //Parent.DiagramPropertyPanel.PanelVisibility();
            //Parent.DiagramContent.StateChanged();
        }
        /// <summary>
        /// Removes the "tb-item-selected" CSS class from the toolbar item based on the specified tool.
        /// </summary>
        /// <param name="tool">The tool for which the "tb-item-selected" CSS class should be removed.</param>
        private async Task removeSelectedToolbarItem(string tool)
        {
#pragma warning disable CA1307 // Specify StringComparison

            if (tool != "pan tool" && PanItemCssClass.IndexOf("tb-item-selected") != -1)
            {
                PanItemCssClass = PanItemCssClass.Replace(" tb-item-selected", "");
            }
            if (tool != "pointer" && PointerItemCssClass.IndexOf("tb-item-selected") != -1)
            {
                PointerItemCssClass = PointerItemCssClass.Replace(" tb-item-selected", "");
            }
            StateHasChanged();
#pragma warning restore CA1307 // Specify StringComparison

        }
        /// <summary>
        /// Removes the "Redo" class from the toolbar class name.
        /// </summary>
        public void RemoveRedo()
        {
            string redo = "redo";
            if (toolbarClassName.Contains(redo))
            {
                int first = toolbarClassName.IndexOf(redo);
                toolbarClassName = toolbarClassName.Remove(first - 4, 8);
            }
        }
        /// <summary>
        /// Hides the property container in the diagram.
        /// </summary>
        public async Task HidePropertyContainer()
        {
            int index = Parent.MenuBar.WindowMenuItems.FindIndex(item => item.Text == "Show Properties");
            Parent.MenuBar.WindowMenuItems[index].IconCss = Parent.MenuBar.WindowMenuItems[index].IconCss == "sf-icon-Selection" ? "sf-icon-Remove" : "sf-icon-Selection";
            await this.HideElements("hide-properties", this.Parent.MenuBar.OpenClick);
            {
                await Task.Delay(800);
                object bounds = await jsRuntime.InvokeAsync<object>("getViewportBounds").ConfigureAwait(true);
                if (bounds != null)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    };
                    DiagramSize dataObj = System.Text.Json.JsonSerializer.Deserialize<DiagramSize>(bounds.ToString(), options);
                    if (dataObj != null)
                    {
                        Parent.DiagramContent.Diagram.BeginUpdate();
                        Parent.DiagramContent.Diagram.Width = dataObj.Width + "px";
                        Parent.DiagramContent.Diagram.Height = dataObj.Height + "px";
                        Parent.DiagramContent.Diagram.EndUpdateAsync();
                    }

                }
                await Parent.DiagramContent.Diagram.DoLayoutAsync();
            }
            Parent.MenuBar.StateChanged();
            Parent.DiagramContent.StateChanged();
            Parent.OrgChartPropertyPanel.statehaschanged();
        }
        /// <summary>
        /// Removes specific CSS class elements from the toolbar class name.
        /// </summary>
        public void removeClassElement()
        {
            string space = " ";
            if (toolbarClassName.Contains(space))
            {
                int first = toolbarClassName.IndexOf(space);
                if (first != 0)
                {
                    toolbarClassName = toolbarClassName.Remove(20);
                }
            }
            fill = "tb-item-start tb-item-fill";
            stroke = "tb-item-end tb-item-stroke";
        }
        /// <summary>
        /// Hides the toolbar.
        /// </summary>
        private async Task HideToolBar()
        {
#pragma warning disable CA1307 // Specify StringComparison
            if (MenuHideIconCss.Contains("sf-icon-Collapse"))
#pragma warning restore CA1307 // Specify StringComparison
            {
                MenuHideIconCss = "sf-icon-DownArrow tb-icons";
            }
            else
            {
                MenuHideIconCss = "sf-icon-Collapse tb-icons";
            }
            await jsRuntime.InvokeAsync<object>("hideMenubar").ConfigureAwait(true);
        }
        /// <summary>
        /// Hides specific elements in the diagram.
        /// </summary>
        /// <param name="eventname">Name of the event triggering the hiding.</param>
        public async Task HideElements(string eventname,bool isNewClick=false)
        {
            await jsRuntime.InvokeAsync<object>("UtilityMethods_hideElements", eventname, isNewClick).ConfigureAwait(true);
        }
        #endregion
    }
}
