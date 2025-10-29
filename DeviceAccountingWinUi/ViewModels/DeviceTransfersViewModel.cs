using CommunityToolkit.Mvvm.ComponentModel;
using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DeviceAccountingWinUi.ViewModels
{
    public partial class DeviceTransfersViewModel : ViewModelBase
    {
        private readonly OrganisationContext _context;
        [ObservableProperty]
        private ObservableCollection<DeviceTransfer> _deviceTransfers = new();

        public DeviceTransfersViewModel(OrganisationContext context)
        {
            this._context = context;
            LoadDeviceTransfersAsync();
        }

        private async void LoadDeviceTransfersAsync()
        {
            if (_context is null)
                throw new InvalidOperationException("No Context was Passed");

            var deviceTransfersFromDb = await _context.DeviceTransfers
                .FromSqlRaw("SELECT * FROM \"DeviceTransfers\"")
                .AsNoTracking()
                .ToListAsync();

            DeviceTransfers = new ObservableCollection<DeviceTransfer>(deviceTransfersFromDb);
        }

        public async Task AddDeviceTransferAsync(DeviceTransfer transfer)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL add_device_transfer({0}, {1}, {2}, {3})",
                transfer.DeviceId,
                transfer.DateOfTransfer,
                transfer.FromWorkerId,
                transfer.ToWorkerId
            );

            _context.ChangeTracker.Clear();
            LoadDeviceTransfersAsync();
        }

        public async Task UpdateDeviceTransferAsync(DeviceTransfer transfer)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL update_device_transfer({0}, {1}, {2}, {3})",
                transfer.Id,
                transfer.DateOfTransfer,
                transfer.FromWorkerId,
                transfer.ToWorkerId
            );

            _context.ChangeTracker.Clear();
            LoadDeviceTransfersAsync();
        }

        public async Task DeleteDeviceTransferAsync(int id)
        {
            // Создадим процедуру удаления, если её нет
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM \"DeviceTransfers\" WHERE id = {0}",
                id
            );

            _context.ChangeTracker.Clear();
            LoadDeviceTransfersAsync();
        }
    }
}