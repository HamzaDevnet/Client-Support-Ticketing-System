# SolveIt - Client Support Ticketing System

## Project Overview
**SolveIt** is a client-support ticketing system developed to streamline customer support processes. It allows external clients to submit support tickets for various company products and enables support team members to manage and resolve them. Support managers can assign tickets, oversee operations, and monitor team performance. The system improves service quality by enhancing ticket resolution times and offering an intuitive experience for both clients and staff.

![1724320007270](https://github.com/user-attachments/assets/81407777-b47a-4885-8ba7-fdef829ae885)

## Main Actors
- **External Client**: Submits and tracks support tickets.
- **Company Support Team Member**: Works on assigned tickets and provides updates.
- **Company Support Manager**: Manages tickets, assigns them to team members, adds new support team members, and tracks performance.

## Key Features
- **Client Registration**: External clients can register via a registration link on the login page.
- **Profile Management**: Clients and support team members can manage their profiles, including updating personal information, changing passwords, and managing notification preferences.
- **Support Manager**: Pre-registered in the system through a database script, can add new support team members, manage employees, and external clients.
- **Ticket Management**: 
  - Clients can submit new tickets with product selection, problem descriptions, and attachments.
  - Support Managers can assign or reassign tickets to specific employees.
  - Employees can update, comment on, and close tickets upon confirmation from the client.
- **Dashboard**: Managers have access to a dashboard displaying ticket status, team productivity, and performance charts.

## Technologies Used
- **Frontend**: Angular for a responsive and dynamic user interface.
- **Backend**: ASP.NET Core (C#) for scalable and secure business logic.
- **Database**: MSSQL with Entity Framework for efficient data management.
- **Additional Features**: Logging, error handling, and secure coding practices for a robust and maintainable system.

## System Architecture
The **SolveIt** system uses a **3-Tier Architecture** to keep the application organized and efficient:

1. **Presentation Layer (UI)**:  
   - Built with **Angular**, this layer is responsible for the user interface and interacts with the backend to display data and process user actions.
2. **Business Logic Layer (API)**:  
   - The core logic of the system, developed with **ASP.NET Core**, processes requests from the UI and handles tasks like ticket management and user operations.
3. **Data Access Layer (DAL)**:  
   - Using **Entity Framework** and **MSSQL**, this layer manages all interactions with the database, performing operations like saving and retrieving data.
## File Structure Overview
```plaintext
/solveit
│
├── /UI                         # Angular Frontend
│   ├── /src                    # Application source code
│       ├── /app                # Core modules and components for UI
│           ├── /components     # Reusable UI components
│           ├── /services       # Services to interact with API
│           ├── /models         # Models used in UI
│           └── /pages          # Application pages (dashboard, ticket management, etc.)
│       ├── /assets             # Static assets (images, styles)
│       └── /environments       # Environment-specific configurations
│
├── /CSTS.API                   # ASP.NET Core API Backend
│   ├── /ApiServices            # Service classes for business logic
│   ├── /Controllers            # API Controllers to handle HTTP requests
│   ├── /Health                 # Health check for monitoring system status
│   ├── /wwwroot                # Static files served by the API
│   ├── appsettings.json        # Application settings and configurations
│   ├── nlog.config             # Logging configuration
│   └── Program.cs              # Application entry point
│
├── /CSTS.DAL                   # Data Access Layer
│   ├── /AutoMapper             # Configuration for object-object mapping
│   ├── /Enum                   # Enumerations used in the project
│   ├── /Migrations             # Database migrations handled by Entity Framework
│   ├── /Models                 # Entity models representing database tables
│   ├── /Repository             # Repository pattern for database operations
│   └── /Validation             # Input validation logic
└──
```
## Contributors
- [Ahmad Alkaraki](https://github.com/AlkarakiT2)  (Supervisor)
- [Alanoud Altuwaijri](https://github.com/alanoudtw) (Team Member)
- [Mustafa Sallat](https://github.com/msallat5)  (Team Member)
- [Hamzeh In'am](https://github.com/HamzaDevnet)  (Team Member)
- [Mohd Qusaireen](https://github.com/mohammed20021) (Team Member)
---
## License
This project is licensed under **[T2 - Business Research and Development Company](https://t2.sa/en) License**. 
