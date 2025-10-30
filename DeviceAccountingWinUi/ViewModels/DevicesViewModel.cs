using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Dapper;
using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using DeviceAccountingWinUi.ViewModels.Messaging;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceAccountingWinUi.ViewModels;

public partial class DevicesViewModel : ViewModelBase
{
    private readonly OrganisationContext _context;
    [ObservableProperty]
    private ObservableCollection<DeviceInfo> _devices = new();

    public DevicesViewModel(OrganisationContext context)
    {
        this._context = context;
        LoadDevicesAsync();
    }

    private async void LoadDevicesAsync()
    {
        if (_context is null)
            throw new InvalidOperationException("No Context was Passed");

        // Используем Dapper для вызова функции get_device_info
        var devicesInfo = await GetDeviceInfoAsync();

        // Преобразуем результат в коллекцию Device

        Devices = new ObservableCollection<DeviceInfo>(devicesInfo);
    }

    private async Task<System.Collections.Generic.List<DeviceInfo>> GetDeviceInfoAsync()
    {
        var connectionString = _context.Database.GetConnectionString();

        // Создаем новое соединение для Dapper
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();


        // Используем Dapper для вызова функции get_device_info
        var result = await connection.QueryAsync<TempDeviceInfo>(
    @"SELECT 
        inventory_id AS InventoryId,
        device_name AS Name, 
        model AS Model,
        date_of_obtaining AS DateOfObtaining,
        cost AS Cost,
        worker_id AS WorkerId
        FROM get_device_info()"
);

        return result.Select(d => new DeviceInfo
        {
            InventoryId = d.InventoryId,
            Name = d.Name,
            Model = d.Model,
            DateOfObtaining = DateOnly.FromDateTime(d.DateOfObtaining),
            Cost = d.Cost,
            WorkerId = d.WorkerId
        }).ToList();
       
    }

    public async Task AddDeviceAsync(Device device, DateOnly date, int? workerId)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "CALL add_device({0}, {1}, {2}, {3}, {4}, {5})",
            device.InventoryId,
            device.Name,
            device.Model,
            device.Cost,
            date,
            workerId
        );

        LoadDevicesAsync();
        WeakReferenceMessenger.Default.Send(new DeviceChangedMessage());
    }

    public async Task UpdateDeviceAsync(Device device, DateOnly date, int? workerId)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "CALL update_device({0}, {1}, {2}, {3}, {4}, {5})",
            device.InventoryId,
            device.Name,
            device.Model,
            device.Cost,
            date,
            workerId
        );

        _context.ChangeTracker.Clear();
        LoadDevicesAsync();
        WeakReferenceMessenger.Default.Send(new DeviceChangedMessage());
    }

    public async Task DeleteDeviceAsync(string inventoryId)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "CALL delete_device({0})",
            inventoryId
        );

        LoadDevicesAsync();
        WeakReferenceMessenger.Default.Send(new DeviceChangedMessage());
    }

    private class TempDeviceInfo
    {
        public string InventoryId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Model { get; set; } = null!;
        public DateTime DateOfObtaining { get; set; }
        public decimal Cost { get; set; }
        public int? WorkerId { get; set; }
    }

}


public class DeviceInfo
{
    public string InventoryId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Model { get; set; } = null!;
    public DateOnly DateOfObtaining { get; set; }
    public decimal Cost { get; set; }
    public int? WorkerId { get; set; }
}

