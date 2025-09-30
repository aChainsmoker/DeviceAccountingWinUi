using CommunityToolkit.Mvvm.ComponentModel;
using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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


        private void LoadDeviceTransfersAsync()
        {
            if (_context is null)
                throw new InvalidOperationException("No Context was Passed");
            var deviceTransfersFromDb = _context.DeviceTransfers.ToList();
            DeviceTransfers = new ObservableCollection<DeviceTransfer>(deviceTransfersFromDb);
        }

        public async Task AddDeviceTransferAsync(DeviceTransfer transfer)
        {
            _context.DeviceTransfers.Add(transfer);
            await _context.SaveChangesAsync();
            LoadDeviceTransfersAsync();
        }

        public async Task UpdateDeviceTransferAsync(DeviceTransfer transfer)
        {
            var existing = await _context.DeviceTransfers.FindAsync(transfer.Id);
            if (existing != null)
            {
                existing.DeviceId = transfer.DeviceId;
                existing.DateOfTransfer = transfer.DateOfTransfer;
                existing.FromWorkerId = transfer.FromWorkerId;
                existing.ToWorkerId = transfer.ToWorkerId;
                await _context.SaveChangesAsync();
                LoadDeviceTransfersAsync();
            }
        }

        public async Task DeleteDeviceTransferAsync(int id)
        {
            var existing = await _context.DeviceTransfers.FindAsync(id);
            if (existing != null)
            {
                _context.DeviceTransfers.Remove(existing);
                await _context.SaveChangesAsync();
                LoadDeviceTransfersAsync();
            }
        }
    }
}
