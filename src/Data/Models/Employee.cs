using System.Text.Json.Serialization;

namespace EXAMPLE.SOURCE.API.Data.Models
{
    /// <summary>
    /// The Employee model represents a person within the organization and contains both identity information (name, display rules, partner naming),
    /// business contact information, system identifiers, and a collection of related contracts. 
    /// </summary>
    public class Employee(int Id)
    {
        /// <summary>
        /// Internal/database ID. Assigned by the application.
        /// </summary>
        public int Id { get; internal set; } = Id;

        /// <summary>
        /// External identifier used by external systems.
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Personnel number used within the HR system.
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// The username for the employee, typically used for login or email prefixes. 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Employee’s given name used for display purposes.
        /// This may differ from the legal given name and is typically the preferred or commonly used name.
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// Employee’s first name. 
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Employee’s last name (birth name). 
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Fully calculated display name, based on naming conventions and partner naming rules.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Initials constructed from given and family name.
        /// </summary>
        public string Initials { get; set; }

        /// <summary>
        /// Dutch-style name prefix (e.g. *van*, *de*, *van der*)
        /// </summary>
        public string FamilyNamePrefix { get; set; }

        /// <summary>
        /// Last name of the employee’s partner (if applicable).
        /// </summary>
        public string FamilyNamePartner { get; set; }

        /// <summary>
        /// Name prefix belonging to the partner’s family name
        /// </summary>
        public string FamilyNamePartnerPrefix { get; set; }

        /// <summary>
        /// Specifies the naming rule used to construct the employee’s display name. The value defines which name parts are used (birth name, partner name) and in which order.
        /// </summary>
        public string Convention { get; set; }

        /// <summary>
        /// Employee’s business / corporate email address.
        /// <br>
        /// Can also be updated using: PATCH: api/employees/{id}/businessemail
        /// </br>
        /// </summary>
        public string BusinessEmail { get; set; }

        /// <summary>
        /// Employee’s fixed business phone number.
        /// </summary>
        public string BusinessPhoneFixed { get; set; }

        /// <summary>
        /// Employee’s mobile business phone number.  
        /// </summary>
        public string BusinessPhoneMobile { get; set; }

        /// <summary>
        /// List of employment contracts associated with the employee.
        /// </summary>
        public List<Contract> Contracts { get; set; }

        /// <summary>
        /// Internal flag indicating whether partner naming logic applies. Not returned via JSON.
        /// </summary>
        [JsonIgnore]
        public bool HasPartner { get; set; }
    }
}
