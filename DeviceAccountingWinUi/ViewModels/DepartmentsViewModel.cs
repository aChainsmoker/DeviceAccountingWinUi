
using CommunityToolkit.Mvvm.ComponentModel;
using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceAccountingWinUi.ViewModels
{
    public partial class DepartmentsViewModel: ViewModelBase
    {
        private readonly OrganisationContext _context;
        [ObservableProperty]
        private ObservableCollection<Department> _departments = new();

        public DepartmentsViewModel(OrganisationContext context)
        {
            this._context = context;
            LoadDepartmentsAsync();
        }


        private void LoadDepartmentsAsync()
        {
            if (_context is null)
                throw new InvalidOperationException("No Context was Passed");
            var departmentsFromDb = _context.Departments.ToList();
            Departments = new ObservableCollection<Department>(departmentsFromDb);
        }

        public async Task AddDepartmentAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
           LoadDepartmentsAsync();
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            var existing = await _context.Departments.FindAsync(department.Id);
            if (existing != null)
            {
                existing.FullName = department.FullName;
                existing.ShortName = department.ShortName;
                await _context.SaveChangesAsync();
                 LoadDepartmentsAsync();
            }
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var existing = await _context.Departments.FindAsync(id);
            if (existing != null)
            {
                _context.Departments.Remove(existing);
                await _context.SaveChangesAsync();
                 LoadDepartmentsAsync();
            }
        }
    }
}
