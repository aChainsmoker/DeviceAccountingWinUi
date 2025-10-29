using CommunityToolkit.Mvvm.ComponentModel;
using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DeviceAccountingWinUi.ViewModels
{
    public partial class DepartmentsViewModel : ViewModelBase
    {
        private readonly OrganisationContext _context;
        [ObservableProperty]
        private ObservableCollection<Department> _departments = new();

        public DepartmentsViewModel(OrganisationContext context)
        {
            this._context = context;
            LoadDepartmentsAsync();
        }

        private async void LoadDepartmentsAsync()
        {
            if (_context is null)
                throw new InvalidOperationException("No Context was Passed");

            var departmentsFromDb = await _context.Departments
                .FromSqlRaw("SELECT * FROM \"Departments\"")
                .AsNoTracking()
                .ToListAsync();

            Departments = new ObservableCollection<Department>(departmentsFromDb);
        }

        public async Task AddDepartmentAsync(Department department)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL add_department({0}, {1})",
                department.FullName,
                department.ShortName
            );

            _context.ChangeTracker.Clear();
            LoadDepartmentsAsync();
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL update_department({0}, {1}, {2})",
                department.Id,
                department.FullName,
                department.ShortName
            );

            _context.ChangeTracker.Clear();
            LoadDepartmentsAsync();
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL delete_department({0})",
                id
            );

            _context.ChangeTracker.Clear();
            LoadDepartmentsAsync();
        }
    }
}