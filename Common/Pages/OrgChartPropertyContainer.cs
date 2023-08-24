using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationalChart
{
    public partial class OrgChartDetails
    {
        /// <summary>
        /// Gets or sets the name of the employee.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the employee ID.
        /// </summary>
        public string? EmployeeID { get; set; }

        /// <summary>
        /// Gets or sets the role of the employee.
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// Gets or sets the department of the employee.
        /// </summary>
        public string? Department { get; set; }

        /// <summary>
        /// Gets or sets the location of the employee.
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the employee.
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Gets or sets the email address of the employee.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the name of the employee's supervisor.
        /// </summary>
        public string? SupervisorName { get; set; }

        /// <summary>
        /// Gets or sets the ID of the employee's supervisor.
        /// </summary>
        public string? SupervisorID { get; set; }

        /// <summary>
        /// Gets or sets the URL of the employee's image.
        /// </summary>
        public string? ImageURL { get; set; }


    }
}
