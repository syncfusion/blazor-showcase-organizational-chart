using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationalChart
{
    /// <summary>
    /// Represents a context menu item for a menu bar option.
    /// </summary>
    public class ContextMenuItemModel
    {
        public List<ContextMenuItemModel> Items { get; set; }
        public string Text { get; set; }
        public string Id { get; set; }
        public string IconCss { get; set; }
        public Boolean Separator { get; set; }
        public Boolean Disabled { get; set; }
    }
}
