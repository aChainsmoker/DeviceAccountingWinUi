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

public partial class DepartmentDevicesViewModel : ObservableObject
{
    private readonly OrganisationContext _context;

    [ObservableProperty]
    private string companyName = "";

    [ObservableProperty]
    private string departmentName = "";

    [ObservableProperty]
    private string currentTime = "";

    [ObservableProperty]
    private ObservableCollection<Device> devices = new();

    public DepartmentDevicesViewModel(OrganisationContext context)
    {
        _context = context;
    }

    public async Task LoadDepartmentDevicesAsync(int departmentId)
    {
        // Получаем информацию о подразделении через Raw SQL (можно оставить через EF)
        var department = await _context.Departments
            .FromSqlRaw("SELECT * FROM \"Departments\" WHERE id = {0}", departmentId)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (department == null)
        {
            CompanyName = "Подразделение не найдено";
            DepartmentName = "";
            CurrentTime = "";
            Devices = new ObservableCollection<Device>();
            return;
        }

        CompanyName = App.Services.GetRequiredService<MainWindowViewModel>().CompanyName;
        DepartmentName = department.FullName;
        CurrentTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

        // Используем Dapper для вызова функции
        var devicesList = await GetDevicesByDepartmentAsync(departmentId);
        Devices = new ObservableCollection<Device>(devicesList);
    }

    private async Task<System.Collections.Generic.List<Device>> GetDevicesByDepartmentAsync(int departmentId)
    {
        // Получаем connection string из контекста
        var connection = _context.Database.GetDbConnection();

        // Убедимся, что соединение открыто
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        try
        {
            // Используем Dapper для вызова функции
            var result = await connection.QueryAsync<DeviceResult>(
                "SELECT * FROM get_devices_by_department(@DepartmentId)",
                new { DepartmentId = departmentId }
            );

            // Преобразуем результат
            return result.Select(d => new Device
            {
                InventoryId = d.inventory_id,
                Name = d.device_name,
                Model = d.model
            }).ToList();
        }
        finally
        {
            // Закрываем соединение, если оно было открыто нами
            if (connection.State == ConnectionState.Open)
                await connection.CloseAsync();
        }
    }

    // Класс для маппинга результата функции
    private class DeviceResult
    {
        public string department_name { get; set; } = "";
        public string inventory_id { get; set; } = "";
        public string device_name { get; set; } = "";
        public string model { get; set; } = "";
    }
}