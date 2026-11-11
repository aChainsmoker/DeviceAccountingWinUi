# Device Accounting

Device Accounting is a Windows desktop application for managing an organisation's devices, employees, departments, rooms, roles, and device transfer history.

## Technology stack

- **C# and .NET 8** (`net8.0-windows10.0.19041.0`)
- **WinUI 3** with **Windows App SDK 1.8** for the desktop interface
- **XAML** for page and control layouts
- **CommunityToolkit.Mvvm** for observable view models
- **Entity Framework Core 9** with the **Npgsql** provider for PostgreSQL access
- **Dapper** for selected database functions and query results
- **Microsoft.Extensions.DependencyInjection** for application services

The application uses a Model–View–ViewModel style structure, with views in `Views`, view models in `ViewModels`, data models in `Models`, and the EF Core context in `DatabaseControl`.

## Implemented features

- Browse and manage devices, including inventory ID, name, model, acquisition date, cost, and assigned employee.
- Browse and manage employees, with role and room assignments.
- Browse and manage departments and their rooms.
- Browse and manage roles and rooms.
- Record, edit, and delete device transfers between employees.
- Look up devices assigned to a selected employee.
- Look up devices and rooms associated with a selected department.
- Enter an organisation name on the home page.
- Display tabular data using WinUI DataGrid controls.

The management screens provide add, edit, and delete modes. Most writes call PostgreSQL stored procedures, while device transfer deletion issues a SQL `DELETE` directly. The employee and department lookup screens use database functions for their result lists.

## Requirements

- Windows 10 version 1809 (build 17763) or later
- .NET 8 SDK
- Visual Studio 2022 with the **WinUI application development** workload, or the .NET CLI with Windows App SDK dependencies available
- PostgreSQL server with the `organisation` database and the schema, stored procedures, and functions expected by the application

## Database configuration

The PostgreSQL connection string is loaded from `DeviceAccountingWinUi/appsettings.json` under `ConnectionStrings:Organisation`. This local file is ignored by Git. Create it from the checked-in template:

```powershell
Copy-Item .\DeviceAccountingWinUi\appsettings.example.json .\DeviceAccountingWinUi\appsettings.json
```

Then replace `YOUR_PASSWORD` with the password for your PostgreSQL user. The project copies `appsettings.json` to the build output when present. Do not commit the local file or real credentials.

The repository includes `roles.sql` and `rooms.sql`, but the application also expects tables and database routines for devices, employees, departments, and transfers. Ensure the full database schema and routines exist before using those screens.

## Build and run

Open `DeviceAccountingWinUi.sln` in Visual Studio, restore NuGet packages, select a Windows target such as `x64`, and run the `DeviceAccountingWinUi` project. The PostgreSQL database must be reachable using the configured connection settings. From PowerShell, run `dotnet run --project .\DeviceAccountingWinUi\ -r win-x64`.

## Project layout

```text
DeviceAccountingWinUi/
  DatabaseControl/  EF Core PostgreSQL context
  Models/           Organisation and inventory entities
  UserControls/     Shared UI controls
  ViewModels/       Observable presentation logic and database operations
  Views/            WinUI pages for management and lookups
```
