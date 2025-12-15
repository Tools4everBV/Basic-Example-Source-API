# Basic-Example-Source-API

This example API specifies the minimal requirements for developing a new API that will be used as an HR source system for HelloID provisioning.

> [!NOTE]
> The swagger interface can be found on:

## About

First and foremost, this API is merely an example. Your API (or the API you need to build) probably differs in more than one way. For example: Your actions might have different names, methods or inputs. And that's okay. We understand that no two APIs are alike.

Hopefully, the example API and documentation will provide some insight on what we expect and what we need in order to build a solid connector that will interact with your API.

If you have any questions or concerns, feel free to contact us. We are always happy to explain things more in depth.

## Table of contents

- [Basic-Example-Source-API](#basic-example-source-api)
  - [About](#about)
  - [Table of contents](#table-of-contents)
  - [What's in the repo?](#whats-in-the-repo)
  - [Prerequisites](#prerequisites)
  - [Running the project](#running-the-project)
    - [macOS](#macos)
    - [Swagger interface](#swagger-interface)
  - [API information](#api-information)
    - [Inner workings if the API](#inner-workings-if-the-api)
      - [Features](#features)
    - [Schemas](#schemas)
      - [Employee](#employee)
        - [Convention](#convention)
        - [Contract](#contract)
        - [ContractAllocation](#contractallocation)
      - [ContractType](#contracttype)
      - [Department](#department)
      - [CostCenter](#costcenter)
      - [JobTitle](#jobtitle)
    - [Available API calls](#available-api-calls)
      - [Employee related actions](#employee-related-actions)
      - [Department related actions](#department-related-actions)
    - [Undocumenated API calls](#undocumenated-api-calls)
      - [Employee related actions](#employee-related-actions-1)
      - [Department related actions](#department-related-actions-1)
      - [Contract related actions](#contract-related-actions)
      - [CostCenter related actions](#costcenter-related-actions)
      - [JobTitle related actions](#jobtitle-related-actions)
    - [Example data](#example-data)

## What's in the repo?

This repo contains the following:
- Source code for the `Basic.Example.Source.API` API in the `src` folder.
- A swagger.yaml file with the api definitions that can be loaded in a swagger editor, so the definitions can be viewed without running the sample api.
- Postman collection with API calls.

## Prerequisites
- The .NET 8.0 SDK is required in order to use the API. Download from: https://dotnet.microsoft.com/en-us/download

## Running the project
Download the content of this repo directly using the zip file.

Or, from your favorite terminal:

1. Clone the repo. `gh repo clone Tools4everBV/Basic-Example-Source-API`.
2. Go to the `./src` folder.
3. Type: `dotnet build` or, to directly run the project: `dotnet run --urls https://localhost:{portNumber}`

> [!NOTE]
> Make sure to change the URL and portnumber according to your environment.

### macOS
When you are using macOS, you might run into problems regarding keyChain.
To bypass this type: `dotnet run --urls https://localhost:{portNumber} -p:UseAppHost=false`
see also: https://github.com/dotnet/sdk/issues/22544

### Swagger interface
The API comes with a swagger interface located at: `{url}/swagger/index.html`

## API information

### Inner workings if the API

This API provides a simple in-memory mock data service for generating realistic sample data. It creates a set of Employees, Departments, CostCenters, JobTitles and Contracts.

#### Features

| Feature                        | Description                                                                                                                                                                   |
| ------------------------------ | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Employer info**              | Single `Employer` object with basic metadata (Name, Code, ExternalId).                                                                                                        |
| **Job titles**                 | Predefined list of job titles (e.g. Developer, HR, Financieel Medewerker, etc.).                                                                                              |
| **Departments & Cost Centers** | Predefined departments and cost centers lists.                                                                                                                                |
| **Employees**                  | Arbitrary number (configurable) of employees — randomized first name, last name, initials, e-mail, phone numbers, partner logic (optional partner names), display name logic. |
| **Contracts**                  | For each employee: 1 contract is generated (can be extended) — randomized start date, optional open end date, job title, allocation, assigned department and cost center.     |
| **Manager assignment**         | Each department is assigned a random manager. Contracts inherit the department manager unless it equals the employee, then another random manager is chosen.                  |
| **Relational data integrity**  | Employees contain their contracts, contracts link to departments/cost centers/job titles, departments link to managers, etc.                                                  |

### Schemas

#### Employee

The Employee model represents a person within the organization and contains both identity information (name, display rules, partner naming), business contact information, system identifiers, and a collection of related contracts. Certain fields are generated automatically (like Id) and others are used purely internally (like HasPartner).

| Property                    | Type                | Description                                                                                                                                                                                            |
| --------------------------- | ------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **Id**                      | `int`               | Internal/database ID. Assigned by the application.                                                                                                                                                     |
| **ExternalId**              | `string`            | External identifier used by external systems.                                                                                                                                                          |
| **EmployeeId**              | `string`            | Personnel number used within the HR system.                                                                                                                                                            |
| **UserName**                | `string`            | The username for the employee, typically used for login or email prefixes.                                                                                                                             |
| **NickName**                | `string`            | Employee’s given name used for display purposes. <br> This may differ from the legal given name and is typically the preferred or commonly used name.                                                  |
| **GivenName**               | `string`            | Employee’s first name.                                                                                                                                                                                 |
| **FamilyName**              | `string`            | Employee’s last name (birth name).                                                                                                                                                                     |
| **DisplayName**             | `string`            | Fully calculated display name, based on naming conventions and partner naming rules.                                                                                                                   |
| **Initials**                | `string`            | Initials constructed from given and family name.                                                                                                                                                       |
| **FamilyNamePrefix**        | `string`            | Dutch-style name prefix (e.g. *van*, *de*, *van der*).                                                                                                                                                 |
| **FamilyNamePartner**       | `string`            | Last name of the employee’s partner (if applicable).                                                                                                                                                   |
| **FamilyNamePartnerPrefix** | `string`            | Name prefix belonging to the partner’s family name.                                                                                                                                                    |
| **Convention**              | `string`            | Specifies the naming rule used to construct the employee’s display name. The value defines which name parts are used (birth name, partner name) and in which order. See also [convention](#convention) |
| **BusinessEmail**           | `string`            | Employee’s business / corporate email address. (Can also be updated using: PATCH: api/employees/{id}/businessemail)                                                                                    |
| **BusinessPhoneFixed**      | `string`            | Employee’s fixed business landline.                                                                                                                                                                    |
| **BusinessPhoneMobile**     | `string`            | Employee’s mobile business phone number.                                                                                                                                                               |
| **Contracts**               | `List<Contract>`    | List of employment contracts associated with the employee.                                                                                                                                             |
| **HasPartner**              | `bool` *(internal)* | Internal flag indicating whether partner naming logic applies. Not returned via JSON.                                                                                                                  |

> [!NOTE]
> The `EmployeeId` is also the _id_ that will be used by target connectors for correlation.

##### Convention

Specifies the naming rule used to construct the employee’s display name. The value defines which name parts are used (birth name, partner name) and in which order.

| Code   | Meaning                    | Description                                                              |
| ------ | -------------------------- | ------------------------------------------------------------------------ |
| **B**  | BirthName                  | Display name is based only on the employee’s birth name.                 |
| **P**  | PartnerName                | Display name is based only on the partner’s family name.                 |
| **PB** | PartnerNameBeforeBirthName | Display name shows the partner’s name first, followed by the birth name. |
| **BP** | BirthNameBeforePartnerName | Display name shows the birth name first, followed by the partner’s name. |

##### Contract

The Contract model represents an employee’s employment contract within the system.
It describes when the contract is active, how many hours are worked, who the manager is, and how the contract relates to organizational structures (department, cost center, job title, employer, etc.).

It also supports open-ended contracts.

The ContractAllocation sub-model specifies the workload allocation: hours per week, percentage of FTE, and a sequence number (for systems supporting multiple allocations per contract).

| Property              | Type                 | Description                                                                                   |
| --------------------- | -------------------- | --------------------------------------------------------------------------------------------- |
| **Id**                | `int`                | Internal/database ID. Assigned by the application.                                            |
| **ExternalId**        | `string`             | External identifier used by external systems.                                                 |
| **EmployeeObjectId**  | `int`                | Id of the employee this contract belongs to. Links to the `Employee.Id`.                      |
| **StartDate**         | `DateTime`           | Date when the contract becomes active.                                                        |
| **EndDate**           | `DateTime`           | Date when the contract ends. Also supports open-ended contracts.                              |
| **Allocation**        | `ContractAllocation` | Work allocation details: hours per week, FTE percentage, sequence.                            |
| **ContractType**      | `ContractType`       | Contract type indicating whether the employee is internal or external.                        |
| **Employer**          | `Employer`           | Internal property describing the organization the contract is tied to. Not returned via JSON. |
| **JobTitle**          | `JobTitle`           | Job title linked to the contract (e.g., Developer, HR advisor).                               |
| **ManagerExternalId** | `string`             | External Id of the manager responsible for this contract.                                     |
| **Department**        | `Department`         | Department object representing the organizational unit.                                       |
| **CostCenter**        | `CostCenter`         | Cost center object associated with the contract.                                              |

##### ContractAllocation

The ContractAllocation sub-model specifies the workload allocation: hours per week, percentage of FTE, and a sequence number (for systems supporting multiple allocations per contract).

| Property         | Type    | Description                                                                     |
| ---------------- | ------- | ------------------------------------------------------------------------------- |
| **HoursPerWeek** | `int`   | Number of hours per week the employee works under this contract (e.g., 36, 40). |
| **Percentage**   | `float` | FTE percentage (0.0 – 1.0). Example: `0.8` for 80% FTE.                         |
| **Sequence**     | `int`   | Sequence number for ordering multiple allocations, if applicable.               |

#### ContractType

The ContractType sub-model specifies whether the employee is internal or external.

#### Department

The Department model represents an organizational unit.
Each department has an internal ID, name, code, external identifier, and a reference to the manager assigned to oversee it.

| Property              | Type     | Description                                                 |
| --------------------- | -------- | ----------------------------------------------------------- |
| **Id**                | `int`    | Internal/database ID. Assigned by the application.          |
| **ExternalId**        | `string` | External identifier used by external systems.               |
| **Name**              | `string` | The full name of the department (e.g., "Human Resources").  |
| **Code**              | `string` | Short code representing the department (e.g., "HR", "ICT"). |
| **ManagerExternalId** | `string` | ExternalId of the employee assigned as department manager.  |

#### CostCenter

The CostCenter object represents a financial or budgeting unit inside the organization.
Contracts reference a CostCenter to indicate where labor costs should be allocated.

| Property       | Type     | Description                                            |
| -------------- | -------- | ------------------------------------------------------ |
| **Id**         | `int`    | Internal/database ID. Assigned by the application.     |
| **ExternalId** | `string` | External identifier used by external systems.          |
| **Name**       | `string` | The full name of the cost center (e.g., “Financiën”).  |
| **Code**       | `string` | Short code representing the cost center (e.g., “FIN”). |

#### JobTitle

The JobTitle entity represents a job title within the target system. It contains both system-generated identifiers and business-defined attributes.

| Property       | Type     | Description                                        |
| -------------- | -------- | -------------------------------------------------- |
| **Id**         | `int`    | Internal/database ID. Assigned by the application. |
| **ExternalId** | `string` | External identifier used by external systems.      |
| **Name**       | `string` | The full name of the job title.                    |
| **Code**       | `string` | Short code representing the job title.             |

### Available API calls

#### Employee related actions

An employee represents a person within the organization and contains both identity information (name, display rules, partner naming), business contact information, system identifiers, and a collection of related contracts.

In addition to retrieving all employees, we also need an API call to update the `businessEmail` address of an employee. Email addresses are generally provisioned by identity platforms such as Active Directory or Entra ID. Therefore, the HR system must be able to receive and store externally generated email addresses through a dedicated update operation.

| HTTP Method | Endpoint                                   | Description                               |
| ----------- | ------------------------------------------ | ----------------------------------------- |
| GET         | `/api/employees`                           | Get all employees                         |
| PATCH       | `/api/employees/:employeeId/businessemail` | Update the `businessEmail` of an employee |

#### Department related actions

A department represents an organizational unit.
Each department has an internal ID, name, code, external identifier, and a reference to the manager assigned to oversee it.

> [!NOTE]
> Departmental information is essential because many business rules -and ultimately- the entitlements a person receives, are determined by attributes related to the individual, their contract, or the department they belong to.

| HTTP Method | Endpoint           | Description         |
| ----------- | ------------------ | ------------------- |
| GET         | `/api/departments` | Get all departments |

### Undocumenated API calls

The API calls below are not documented in the swagger interface but are still available for development purposes.

#### Employee related actions

| HTTP Method | Endpoint             | Description        |
| ----------- | -------------------- | ------------------ |
| GET         | `/api/employees/:id` | Get employee by Id |

#### Department related actions

| HTTP Method | Endpoint               | Description          |
| ----------- | ---------------------- | -------------------- |
| GET         | `/api/departments/:id` | Get department by Id |

#### Contract related actions

| HTTP Method | Endpoint             | Description        |
| ----------- | -------------------- | ------------------ |
| GET         | `/api/contracts`     | Get all contracts  |
| GET         | `/api/contracts/:id` | Get contract by Id |

#### CostCenter related actions

| HTTP Method | Endpoint               | Description          |
| ----------- | ---------------------- | -------------------- |
| GET         | `/api/costcenters`     | Get all costcenters  |
| GET         | `/api/costcenters/:id` | Get costcenter by Id |

#### JobTitle related actions

| HTTP Method | Endpoint             | Description        |
| ----------- | -------------------- | ------------------ |
| GET         | `/api/jobtitles`     | Get all jobtitle   |
| GET         | `/api/jobtitles/:id` | Get jobtitle by Id |

### Example data

```json
[
  {
    "id": 1875256,
    "externalId": "2477250",
    "employeeId": "3835469",
    "userName": "e.vries@enyoi.nl",
    "nickName": "Eva",
    "givenName": "Eva",
    "familyName": "Vries",
    "displayName": "van der Bos",
    "initials": "EV",
    "familyNamePrefix": "van",
    "familyNamePartner": "Bos",
    "familyNamePartnerPrefix": "van der",
    "convention": "P",
    "businessEmail": "eva.vries@enyoi.nl",
    "businessPhoneFixed": "+31 10 2317972",
    "businessPhoneMobile": "+31 6 35861697",
    "contracts": [
      {
        "id": 5765,
        "externalId": "4815618",
        "employeeObjectId": 1875256,
        "startDate": "2021-05-26T15:17:10.2176983Z",
        "endDate": "2024-05-26T15:17:10.2176983Z",
        "allocation": {
          "hoursPerWeek": 26,
          "percentage": 0.6608925,
          "sequence": 2
        },
        "contractType": {
          "code": "EXT",
          "name": "Internal"
        },
        "jobTitle": {
          "id": 12,
          "externalId": "001",
          "name": "Secretaresse",
          "code": "SECR"
        },
        "managerExternalId": "2316262",
        "department": {
          "id": 8,
          "externalId": "D008",
          "name": "Kwaliteit",
          "code": "QC",
          "managerExternalId": "2316262"
        },
        "costCenter": {
          "id": 1,
          "externalId": "CC-1001",
          "name": "Financiën",
          "code": "FIN"
        }
      }
    ]
  },
  {
    "id": 1875257,
    "externalId": "2015207",
    "employeeId": "3938549",
    "userName": "l.jacobs@enyoi.nl",
    "nickName": "Lars",
    "givenName": "Lars",
    "familyName": "Jacobs",
    "displayName": "Jacobs",
    "initials": "LJ",
    "familyNamePrefix": "",
    "familyNamePartner": "",
    "familyNamePartnerPrefix": "",
    "convention": "B",
    "businessEmail": "lars.jacobs@enyoi.nl",
    "businessPhoneFixed": "+31 10 2487606",
    "businessPhoneMobile": "+31 6 56355761",
    "contracts": [
      {
        "id": 7592,
        "externalId": "4452008",
        "employeeObjectId": 1875257,
        "startDate": "2018-03-05T15:17:10.2213226Z",
        "endDate": "2021-03-05T15:17:10.2213226Z",
        "allocation": {
          "hoursPerWeek": 13,
          "percentage": 0.4912822,
          "sequence": 3
        },
        "contractType": {
          "code": "EXT",
          "name": "External"
        },
        "jobTitle": {
          "id": 4,
          "externalId": "002",
          "name": "HR-adviseur",
          "code": "HR"
        },
        "managerExternalId": "2720635",
        "department": {
          "id": 6,
          "externalId": "D006",
          "name": "Operations",
          "code": "OPS",
          "managerExternalId": "2720635"
        },
        "costCenter": {
          "id": 3,
          "externalId": "CC-1003",
          "name": "ICT",
          "code": "ICT"
        }
      }
    ]
  },
  {
    "id": 1875258,
    "externalId": "2190415",
    "employeeId": "3660419",
    "userName": "t.vermeulen@enyoi.nl",
    "nickName": "Thijs",
    "givenName": "Thijs",
    "familyName": "Vermeulen",
    "displayName": "den Vermeulen",
    "initials": "TV",
    "familyNamePrefix": "den",
    "familyNamePartner": "",
    "familyNamePartnerPrefix": "",
    "convention": "B",
    "businessEmail": "thijs.vermeulen@enyoi.nl",
    "businessPhoneFixed": "+31 10 9201454",
    "businessPhoneMobile": "+31 6 42039848",
    "contracts": [
      {
        "id": 6541,
        "externalId": "4774214",
        "employeeObjectId": 1875258,
        "startDate": "2017-06-10T15:17:10.2236787Z",
        "endDate": "9999-12-31T23:59:59.9999999",
        "allocation": {
          "hoursPerWeek": 20,
          "percentage": 0.4168368,
          "sequence": 1
        },
        "contractType": {
          "code": "EXT",
          "name": "External"
        },
        "jobTitle": {
          "id": 8,
          "externalId": "003",
          "name": "ICT-beheerder",
          "code": "ICT"
        },
        "managerExternalId": "2863928",
        "department": {
          "id": 3,
          "externalId": "D003",
          "name": "Financiën",
          "code": "FIN",
          "managerExternalId": "2863928"
        },
        "costCenter": {
          "id": 2,
          "externalId": "CC-1002",
          "name": "HRM",
          "code": "HRM"
        }
      }
    ]
  }
]
```