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


        private void LoadWorkersAsync()
        {
            if (_context is null)
                throw new InvalidOperationException("No Context was Passed");
            var workersFromDb = _context.Workers.ToList();
            Workers = new ObservableCollection<Worker>(workersFromDb);
        }


        public async Task AddWorkerAsync(Worker worker)
        {
            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();
            LoadWorkersAsync();
        }

        public async Task UpdateWorkerAsync(Worker worker)
        {
            var existing = await _context.Workers.FindAsync(worker.Id);
            if (existing != null)
            {
                existing.Name = worker.Name;
                existing.RoleId = worker.RoleId;
                existing.RoomNumber = worker.RoomNumber;
                await _context.SaveChangesAsync();
                LoadWorkersAsync();
            }
        }

        public async Task DeleteWorkerAsync(int id)
        {
            var existing = await _context.Workers.FindAsync(id);
            if (existing != null)
            {
                _context.Workers.Remove(existing);
                await _context.SaveChangesAsync();
                LoadWorkersAsync();
            }
        }
    }
}
