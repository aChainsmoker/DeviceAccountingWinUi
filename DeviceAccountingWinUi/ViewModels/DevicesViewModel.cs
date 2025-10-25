using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
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


        private void  LoadDevicesAsync()
        {
            if (_context is null)
                throw new InvalidOperationException("No Context was Passed");
            var devicesFromDb = _context.Devices.ToList();
            Devices = new ObservableCollection<Device>(devicesFromDb);
        }

        public async Task AddDeviceAsync(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            LoadDevicesAsync();
        }

        // Обновление
        public async Task UpdateDeviceAsync(Device device)
        {
            var existing = await _context.Devices.FindAsync(device.InventoryId);
            if (existing != null)
            {
                existing.Name = device.Name;
                existing.Model = device.Model;
                existing.DateOfObtaining = device.DateOfObtaining;
                existing.Cost = device.Cost;
                existing.WorkerId = device.WorkerId;

                await _context.SaveChangesAsync();
                LoadDevicesAsync();
            }
        }

        // Удаление
        public async Task DeleteDeviceAsync(string inventoryId)
        {
            var existing = await _context.Devices.FindAsync(inventoryId);
            if (existing != null)
            {
                _context.Devices.Remove(existing);
                await _context.SaveChangesAsync();
                LoadDevicesAsync();
            }
        }
    }
}
