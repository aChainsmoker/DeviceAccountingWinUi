using CommunityToolkit.Mvvm.ComponentModel;
using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceAccountingWinUi.ViewModels;

public partial class DepartmentRoomsViewModel : ObservableObject
{
    private readonly OrganisationContext _context;

    [ObservableProperty]
    private string companyName = "";

    [ObservableProperty]
    private string departmentName = "";

    [ObservableProperty]
    private string currentTime = "";

    [ObservableProperty]
    private ObservableCollection<Room> rooms = new();

    public DepartmentRoomsViewModel(OrganisationContext context)
    {
        _context = context;
    }

    public async Task LoadDepartmentRoomsAsync(int departmentId)
    {
        // Получаем информацию о подразделении через Raw SQL
        var department = await _context.Departments
            .FromSqlRaw("SELECT * FROM \"Departments\" WHERE id = {0}", departmentId)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (department == null)
        {
            CompanyName = "Подразделение не найдено";
            DepartmentName = "";
            CurrentTime = "";
            Rooms = new ObservableCollection<Room>();
            return;
        }

        CompanyName = App.Services.GetRequiredService<MainWindowViewModel>().CompanyName;
        DepartmentName = department.FullName;
        CurrentTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

        // Используем Dapper для вызова функции get_rooms_by_department
        var roomsList = await GetRoomsByDepartmentAsync(departmentId);
        Rooms = new ObservableCollection<Room>(roomsList);
    }

    private async Task<System.Collections.Generic.List<Room>> GetRoomsByDepartmentAsync(int departmentId)
    {
        var connection = _context.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        try
        {
            // Используем Dapper для вызова функции get_rooms_by_department
            var result = await connection.QueryAsync<RoomResult>(
                "SELECT * FROM get_rooms_by_department(@DepartmentId)",
                new { DepartmentId = departmentId }
            );

            // Преобразуем результат в коллекцию Room
            return result.Select(r => new Room
            {
                Number = r.room_number,
                Area = r.area
            }).ToList();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                await connection.CloseAsync();
        }
    }

    // Класс для маппинга результата функции get_rooms_by_department
    private class RoomResult
    {
        public string department_name { get; set; } = "";
        public string room_number { get; set; } = "";
        public decimal area { get; set; }
    }
}