namespace EXAMPLE.SOURCE.API.Data.Models
{
    public class JobTitle
    {
        /// <summary>
        /// Internal/database ID. Assigned by the application.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// External identifier used by external systems.
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// The full name of the job title.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Short code representing the job title. 
        /// </summary>
        public string Code { get; set; }
    }
}
