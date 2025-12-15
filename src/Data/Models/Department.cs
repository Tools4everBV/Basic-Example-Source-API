namespace EXAMPLE.SOURCE.API.Data.Models
{
    /// <summary>
    /// The Department model represents an organizational unit.
    /// Each department has an internal ID, name, code, external identifier, and a reference to the manager assigned to oversee it.
    /// </summary>
    public class Department
    {
        /// <summary>
        /// Internal/database ID. Assigned by the application.
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// External identifier used by external systems.
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// The full name of the department (e.g., "Human Resources").
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Short code representing the department (e.g., "HR", "ICT").
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// ExternalId of the employee assigned as department manager.
        /// </summary>
        public string ManagerExternalId { get; set; }
    }
}
