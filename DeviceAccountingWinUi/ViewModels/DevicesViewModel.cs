using CommunityToolkit.Mvvm.ComponentModel;
using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DeviceAccountingWinUi.ViewModels
{
    public partial class DevicesViewModel : ViewModelBase
    {
        private readonly OrganisationContext _context;
        [ObservableProperty]
        private ObservableCollection<Device> _devices = new();

        public DevicesViewModel(OrganisationContext context)
        {
            this._context = context;
            LoadDevicesAsync();
        }

        private async void LoadDevicesAsync()
        {
            if (_context is null)
                throw new InvalidOperationException("No Context was Passed");
            var devicesFromDb = await _context.Devices
                .FromSqlRaw("SELECT * FROM \"Devices\"")
                .AsNoTracking()
                .ToListAsync();

            Devices = new ObservableCollection<Device>(devicesFromDb);
        }

        public async Task AddDeviceAsync(Device device)
        {
            // Используем хранимую процедуру вместо _context.Devices.Add()
            await _context.Database.ExecuteSqlRawAsync(
                "CALL add_device({0}, {1}, {2}, {3}, {4}, {5})",
                device.InventoryId,
                device.Name,
                device.Model,
                device.DateOfObtaining,
                device.Cost,
                device.WorkerId
            );

            LoadDevicesAsync();
        }

        // Обновление через хранимую процедуру
        public async Task UpdateDeviceAsync(Device device)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL update_device({0}, {1}, {2}, {3}, {4}, {5})",
                device.InventoryId,
                device.Name,
                device.Model,
                device.DateOfObtaining,
                device.Cost,
                device.WorkerId
            );

            _context.ChangeTracker.Clear();
            LoadDevicesAsync();
        }

        // Удаление через хранимую процедуру
        public async Task DeleteDeviceAsync(string inventoryId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL delete_device({0})",
                inventoryId
            );

            LoadDevicesAsync();
        }
    }
}