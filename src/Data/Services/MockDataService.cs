using Bogus;
using EXAMPLE.SOURCE.API.Data.Models;

namespace EXAMPLE.SOURCE.API.Data.Services
{
    public class MockDataService
    {
        private readonly Random _random = new();
        public Employer Employer { get; private set; }
        public List<JobTitle> JobTitles { get; private set; }
        public List<Department> Departments { get; private set; }
        public List<CostCenter> CostCenters { get; private set; }
        public List<Employee> Employees { get; private set; }
        public List<Contract> Contracts { get; private set; }

        public MockDataService()
        {
            Contracts = new List<Contract>();

            Employer = new Employer
            {
                Name = "Enyoi",
                Id = 1,
                Code = "ENI",
                ExternalId = "5000000"
            };

            CreateJobTitles();
            CreateDepartments();
            CreateCostCenters();
            GenerateEmployees(100);

            foreach (var department in Departments)
            {
                var manager = Employees[_random.Next(Employees.Count)];
                department.ManagerExternalId = manager.ExternalId;
            }

            foreach (var employee in Employees)
            {
                GenerateContract(employee, Employer, JobTitles);
                employee.Contracts = Contracts.Where(c => c.EmployeeObjectId == employee.Id).ToList();
            }
        }

        private void GenerateEmployees(int amount)
        {
            int id = _random.Next(1000000, 1999999);

            var faker = new Faker<Employee>("nl")

                .CustomInstantiator(f => new Employee(id++))
                .RuleFor(e => e.ExternalId, f => f.Random.Int(2000000, 2999999).ToString())
                .RuleFor(e => e.EmployeeId, f => f.Random.Int(3000000, 3999999).ToString())
                .RuleFor(e => e.GivenName, f => f.Name.FirstName())
                .RuleFor(e => e.FamilyName, f => f.Name.LastName())
                .RuleFor(e => e.NickName, (f, e) => e.GivenName)
                .RuleFor(e => e.Initials, (f, e) => $"{e.GivenName[0]}{e.FamilyName[0]}")
                .RuleFor(e => e.FamilyNamePrefix, f => f.PickRandom(new[] { "", "van", "de", "den", "ter", "van der" }))
                .RuleFor(e => e.UserName, (f, e) => $"{e.GivenName.Substring(0, 1)}.{e.FamilyName}@{Employer.Name}.nl".ToLower())
                .RuleFor(e => e.BusinessEmail, (f, e) => $"{e.GivenName}.{e.FamilyName}@{Employer.Name}.nl".ToLower().Replace(" ", ""))
                .RuleFor(e => e.BusinessPhoneFixed, f => f.Phone.PhoneNumber("+31 10 #######"))
                .RuleFor(e => e.BusinessPhoneMobile, f => f.Phone.PhoneNumber("+31 6 ########"))
                .FinishWith((f, e) => ApplyPartnerLogic(f, e));

            Employees = faker.Generate(amount);
        }

        private void ApplyPartnerLogic(Faker f, Employee e)
        {
            var prefixes = new[] { "", "van", "de", "den", "ter", "van der" };

            e.HasPartner = f.Random.Bool(0.4f);

            if (!e.HasPartner)
            {
                e.FamilyNamePartner = "";
                e.FamilyNamePartnerPrefix = "";
                e.Convention = "B";
                e.DisplayName = $"{e.FamilyNamePrefix} {e.FamilyName}".Trim();
                return;
            }

            e.FamilyNamePartner = f.Name.LastName();
            e.FamilyNamePartnerPrefix = f.PickRandom(prefixes);

            var picked = f.PickRandom(new[] { "E", "P", "C", "B", "D" });

            e.Convention = picked switch
            {
                "E" => "B",
                "P" => "P",
                "C" => "BP",
                "B" => "PB",
                "D" => "BP",
                _ => "B"
            };

            var birth = $"{e.FamilyNamePrefix} {e.FamilyName}".Trim();
            var partner = $"{e.FamilyNamePartnerPrefix} {e.FamilyNamePartner}".Trim();

            e.DisplayName = e.Convention switch
            {
                "P" => partner,
                "PB" => $"{partner}-{birth}",
                "BP" => $"{birth}-{partner}",
                _ => birth
            };
        }

        private void GenerateContract(Employee employee, Employer employer, List<JobTitle> jobTitles)
        {
            int id = _random.Next(1000, 9999);

            var faker = new Faker<Contract>("nl")
                .CustomInstantiator(f => new Contract(id++)
                {
                    Id = id,
                    Employer = employer,
                    EmployeeObjectId = employee.Id
                })
                .RuleFor(e => e.ExternalId, f => f.Random.Int(4000000, 4999999).ToString())
                .RuleFor(c => c.StartDate, f =>
                {
                    var yearsBack = f.Random.Int(1, 10);
                    return DateTime.UtcNow.AddYears(-yearsBack).AddDays(-f.Random.Int(0, 300));
                })
                .RuleFor(c => c.EndDate, (f, c) =>
                {
                    bool openEnded = f.Random.Bool(0.2f);
                    if (openEnded)
                        return DateTime.MaxValue;

                    int range = f.Random.Int(1, 5);
                    return c.StartDate.AddYears(range);
                })
                .RuleFor(c => c.Allocation, f => new ContractAllocation
                {
                    HoursPerWeek = f.Random.Int(8, 40),
                    Percentage = f.Random.Float(0.2f, 1.0f),
                    Sequence = f.Random.Int(1, 3)
                })
                .RuleFor(c => c.ContractType, f => new ContractType
                {
                    Code = f.PickRandom("INT", "EXT"),
                    Name = f.Random.Bool() ? "Internal" : "External"
                })
                .RuleFor(c => c.Department, f => f.PickRandom(Departments))
                .RuleFor(c => c.ManagerExternalId, (f, c) =>
                {
                    var managerId = c.Department.ManagerExternalId;
                    if (managerId == employee.ExternalId)
                    {
                        var others = Employees.Where(e => e.ExternalId != employee.ExternalId).ToList();
                        return f.PickRandom(others).ExternalId;
                    }

                    return managerId;
                })
                .RuleFor(c => c.CostCenter, f => f.PickRandom(CostCenters))
                .RuleFor(c => c.JobTitle, f => f.PickRandom(JobTitles));

            var contract = faker.Generate();
            Contracts.Add(contract);
        }

        private void CreateJobTitles()
        {
            JobTitles = new List<JobTitle>
            {
                new JobTitle { Id = 1, Code = "DEV", Name = "Softwareontwikkelaar" },
                new JobTitle { Id = 2, Code = "ADM", Name = "Administratief Medewerker" },
                new JobTitle { Id = 3, Code = "PM", Name = "Projectmanager" },
                new JobTitle { Id = 4, Code = "HR", Name = "HR-adviseur" },
                new JobTitle { Id = 5, Code = "FIN", Name = "Financieel Medewerker" },
                new JobTitle { Id = 6, Code = "ACC", Name = "Accountmanager" },
                new JobTitle { Id = 7, Code = "MK", Name = "Marketing Specialist" },
                new JobTitle { Id = 8, Code = "ICT", Name = "ICT-beheerder" },
                new JobTitle { Id = 9, Code = "OPS", Name = "Operationeel Manager" },
                new JobTitle { Id = 10, Code = "DS", Name = "Dataspecialist" },
                new JobTitle { Id = 11, Code = "TN", Name = "Teamleider" },
                new JobTitle { Id = 12, Code = "SECR", Name = "Secretaresse" },
                new JobTitle { Id = 13, Code = "TRN", Name = "Trainer / Opleider" },
                new JobTitle { Id = 14, Code = "QC", Name = "Kwaliteitsmedewerker" },
                new JobTitle { Id = 15, Code = "MM", Name = "Magazijnmedewerker" }
            };
        }

        private void CreateDepartments()
        {
            Departments = new List<Department>
            {
                new Department { Id = 1, ExternalId = "D001", Code = "ICT", Name = "ICT" },
                new Department { Id = 2, ExternalId = "D002", Code = "HR", Name = "Personeelszaken" },
                new Department { Id = 3, ExternalId = "D003", Code = "FIN", Name = "Financiën" },
                new Department { Id = 4, ExternalId = "D004", Code = "MKT", Name = "Marketing" },
                new Department { Id = 5, ExternalId = "D005", Code = "SAL", Name = "Sales" },
                new Department { Id = 6, ExternalId = "D006", Code = "OPS", Name = "Operations" },
                new Department { Id = 7, ExternalId = "D007", Code = "LOG", Name = "Logistiek" },
                new Department { Id = 8, ExternalId = "D008", Code = "QC", Name = "Kwaliteit" },
                new Department { Id = 9, ExternalId = "D009", Code = "RND", Name = "Onderzoek en Ontwikkeling" },
                new Department { Id = 10, ExternalId = "D010", Code = "CS", Name = "Klantenservice" }
            };
        }

        private void CreateCostCenters()
        {
            CostCenters = new List<CostCenter>
            {
                new CostCenter { Id = 1, ExternalId = "CC-1001", Name = "Financiën", Code = "FIN" },
                new CostCenter { Id = 2, ExternalId = "CC-1002", Name = "HRM", Code = "HRM" },
                new CostCenter { Id = 3, ExternalId = "CC-1003", Name = "ICT", Code = "ICT" },
                new CostCenter { Id = 4, ExternalId = "CC-1004", Name = "Inkoop", Code = "INK" },
                new CostCenter { Id = 5, ExternalId = "CC-1005", Name = "Marketing", Code = "MKT" },
                new CostCenter { Id = 6, ExternalId = "CC-1006", Name = "Sales", Code = "SAL" },
                new CostCenter { Id = 7, ExternalId = "CC-1007", Name = "Klantenservice", Code = "KLS" },
                new CostCenter { Id = 8, ExternalId = "CC-1008", Name = "Productie", Code = "PRD" },
                new CostCenter { Id = 9, ExternalId = "CC-1009", Name = "Logistiek", Code = "LOG" },
                new CostCenter { Id = 10, ExternalId = "CC-1010", Name = "Onderzoek & Ontwikkeling", Code = "RND" }
            };
        }
    }
}
