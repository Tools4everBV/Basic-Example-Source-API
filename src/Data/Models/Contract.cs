using System.Text.Json.Serialization;

namespace EXAMPLE.SOURCE.API.Data.Models
{
    /// <summary>
    /// The Contract model represents an employee’s employment contract within the system. 
    /// It describes when the contract is active, how many hours are worked, who the manager is, and how the contract relates to organizational structures(department, cost center, job title, employer, etc.).
    /// It also supports open-ended contracts.
    /// </summary>
    public class Contract(int Id)
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
        /// Id of the employee this contract belongs to. Links to the `Employee.Id`.
        /// </summary>
        public int EmployeeObjectId { get; set; }

        /// <summary>
        /// Date when the contract becomes active. 
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date when the contract ends. Also supports open-ended contracts.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Work allocation details: hours per week, FTE percentage, sequence.
        /// </summary>
        public ContractAllocation Allocation { get; set; }

        /// <summary>
        /// Contract type indicating whether the employee is internal or external.
        /// </summary>
        public ContractType ContractType { get; set; }

        /// <summary>
        /// Internal property describing the organization the contract is tied to. Not returned via JSON.
        /// </summary>
        [JsonIgnore]
        public Employer Employer { get; set; }

        /// <summary>
        /// Job title linked to the contract (e.g., Developer, HR advisor).
        /// </summary>
        public JobTitle JobTitle { get; set; }

        /// <summary>
        /// External Id of the manager responsible for this contract.
        /// </summary>
        public string ManagerExternalId { get; set; }

        /// <summary>
        /// Department object representing the organizational unit.
        /// </summary>
        public Department Department { get; set; }

        /// <summary>
        /// Cost center object associated with the contract.
        /// </summary>
        public CostCenter CostCenter { get; set; }
    }

    /// <summary>
    /// The ContractAllocation sub-model specifies the workload allocation: hours per week, percentage of FTE, and a sequence number (for systems supporting multiple allocations per contract).
    /// </summary>
    public class ContractAllocation
    {
        /// <summary>
        /// Number of hours per week the employee works under this contract (e.g., 36, 40).
        /// </summary>
        public int HoursPerWeek { get; set; }

        /// <summary>
        /// FTE percentage (0.0 – 1.0). Example: `0.8` for 80% FTE.
        /// </summary>
        public float Percentage { get; set; }

        /// <summary>
        /// Sequence number for ordering multiple allocations, if applicable.
        /// </summary>
        public int Sequence { get; set; }
    }

    public class ContractType
    {
        /// <summary>
        /// Short code representing the contract type (e.g., "INT" for internal, "EXT" for external).
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Descriptive name of the contract type indicating whether the employee is internal or external.
        /// </summary>
        public string Name { get; set; }
    }
}

