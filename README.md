**AgendaPro** is a simple personal project developed as a learning exercise to practice ASP.NET Core using layered architecture and Domain-Driven Design (DDD). The idea was based on a fictional demand for a project that I prompted ChatGPT for, along with its business rules. We then elaborated together to refine the concept and create a structured implementation plan.

![image](https://github.com/user-attachments/assets/34790a26-94d4-4d5b-b773-bfe7620c97c4)

# Features
* CRUD operations for customers
* Calendar for scheduling a service with a customer
* Unit testing with xUnit
* User authentication and role-based authorization (in progress)
* Notifications via email for appointment confirmations, cancellations, and reminders (todo)
* RESTful API implementation (todo)
# Technologies Used
**Backend**: ASP.NET Core Web API

**Database**: Sqlite with Entity Framework Core

**Testing**: xUnit
# Installation & Running Locally
1. Clone the repository:

`git clone https://github.com/phenricor/AgendaPro.git
cd AgendaPro
`

2. Set up the database:

`
dotnet ef database update
`

3. Run the application:

`
dotnet run
`

Access at http://localhost:5278
