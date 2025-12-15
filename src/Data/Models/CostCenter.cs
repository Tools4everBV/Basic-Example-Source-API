namespace EXAMPLE.SOURCE.API.Data.Models
{
    /// <summary>
    /// The CostCenter object represents a financial or budgeting unit inside the organization. 
    /// Contracts reference a CostCenter to indicate where labor costs should be allocated.
    /// </summary>
    public class CostCenter
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
        /// The full name of the cost center (e.g., “Financiën”).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Short code representing the cost center (e.g., “FIN”).
        /// </summary>
        public string Code { get; set; }
    }
}
