using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationalChart
{
    /// <summary>
    /// Represents a data structure for holding a value and its corresponding text for paper list fields.
    /// </summary>
    public class PaperListFields
    {
        /// <summary>
        /// Gets or sets the value of the paper list field.
        /// </summary>
        public string? Value { get; set; }
        /// <summary>
        /// Gets or sets the text associated with the paper list field value.
        /// </summary>
        public string? Text { get; set; }
    }
    /// <summary>
    /// Represents a paper size with width and height properties.
    /// </summary>
    public class PaperSize
    {
        /// <summary>
        /// Gets or sets the width of the page.
        /// </summary>
        public double PageWidth { get; set; }
        /// <summary>
        /// Gets or sets the height of the page.
        /// </summary>
        public double PageHeight { get; set; }
    }
    /// <summary>
    /// Specifies the region that has to be fit within the viewport.
    /// </summary>
    public enum Regions
    {
        /// <summary>
        /// Specifies the region within the given values(x,y, width and height) of page settings.
        /// </summary>
        PageSettings,
        /// <summary>
        /// Specifies the region holding content of the diagram without empty space around the content.
        /// </summary>
        Content,
    }
}
