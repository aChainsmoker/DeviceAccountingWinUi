using CommunityToolkit.Mvvm.ComponentModel;
using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DeviceAccountingWinUi.ViewModels
{
    public partial class WorkersViewModel : ViewModelBase
    {
        private readonly OrganisationContext _context;
        [ObservableProperty]
        private ObservableCollection<Worker> _workers = new();

        public WorkersViewModel(OrganisationContext context)
        {
            this._context = context;
            LoadWorkersAsync();
        }

        private async void LoadWorkersAsync()
        {
            if (_context is null)
                throw new InvalidOperationException("No Context was Passed");

            var workersFromDb = await _context.Workers
                .FromSqlRaw("SELECT * FROM \"Workers\"")
                .AsNoTracking()
                .ToListAsync();

            Workers = new ObservableCollection<Worker>(workersFromDb);
        }

        public async Task AddWorkerAsync(Worker worker)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL add_worker({0}, {1}, {2})",
                worker.Name,
                worker.RoleId,
                worker.RoomNumber
            );

            _context.ChangeTracker.Clear();
            LoadWorkersAsync();
        }

        public async Task UpdateWorkerAsync(Worker worker)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL update_worker({0}, {1}, {2}, {3})",
                worker.Id,
                worker.Name,
                worker.RoleId,
                worker.RoomNumber
            );

            _context.ChangeTracker.Clear();
            LoadWorkersAsync();
        }

        public async Task DeleteWorkerAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL delete_worker({0})",
                id
            );

            _context.ChangeTracker.Clear();
            LoadWorkersAsync();
        }
    }
}