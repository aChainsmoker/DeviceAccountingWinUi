using CommunityToolkit.Mvvm.ComponentModel;
using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceAccountingWinUi.ViewModels;

public partial class WorkerDevicesViewModel : ObservableObject
{
    private readonly OrganisationContext _context;

    [ObservableProperty]
    private string workerName = "";

    [ObservableProperty]
    private string workerRole = "";

    [ObservableProperty]
    private string currentTime = "";

    [ObservableProperty]
    private ObservableCollection<Device> devices = new();

    public WorkerDevicesViewModel(OrganisationContext context)
    {
        _context = context;
    }

    public async Task LoadWorkerDevicesAsync(int workerId)
    {
        // Получаем информацию о работнике через Raw SQL
        var worker = await _context.Workers
            .FromSqlRaw("SELECT * FROM \"Workers\" WHERE id = {0}", workerId)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (worker == null)
        {
            WorkerName = "Работник не найден";
            WorkerRole = "";
            CurrentTime = "";
            Devices = new ObservableCollection<Device>();
            return;
        }

        WorkerName = worker.Name;

        // Получаем название роли работника
        WorkerRole = await GetWorkerRoleNameAsync(worker.RoleId);
        CurrentTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

        // Используем Dapper для вызова функции get_devices_by_worker
        var devicesList = await GetDevicesByWorkerAsync(workerId);
        Devices = new ObservableCollection<Device>(devicesList);
    }

    private async Task<string> GetWorkerRoleNameAsync(int? roleId)
    {
        if (roleId == null) return "Не указана";

        var role = await _context.Roles
            .FromSqlRaw("SELECT * FROM \"Roles\" WHERE id = {0}", roleId)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return role?.Name ?? "Не указана";
    }

    private async Task<System.Collections.Generic.List<Device>> GetDevicesByWorkerAsync(int workerId)
    {
        var connection = _context.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        try
        {
            // Используем Dapper для вызова функции get_devices_by_worker
            var result = await connection.QueryAsync<WorkerDeviceResult>(
                "SELECT * FROM get_devices_by_worker(@WorkerId)",
                new { WorkerId = workerId }
            );

            // Преобразуем результат в коллекцию Device
            return result.Select(d => new Device
            {
                InventoryId = d.inventory_id,
                Name = d.device_name,
                Model = d.model
            }).ToList();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                await connection.CloseAsync();
        }
    }

    // Класс для маппинга результата функции get_devices_by_worker
    private class WorkerDeviceResult
    {
        public string worker_name { get; set; } = "";
        public string worker_position { get; set; } = "";
        public string inventory_id { get; set; } = "";
        public string device_name { get; set; } = "";
        public string model { get; set; } = "";
    }
}