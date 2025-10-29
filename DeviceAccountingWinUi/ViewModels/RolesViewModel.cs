using CommunityToolkit.Mvvm.ComponentModel;
using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;

namespace DeviceAccountingWinUi.ViewModels
{
    public partial class RolesViewModel : ViewModelBase
    {
        private readonly OrganisationContext _context;
        [ObservableProperty]
        private ObservableCollection<Role> _roles = new();

        public RolesViewModel(OrganisationContext context)
        {
            this._context = context;
            LoadRolesAsync();
        }

        private async void LoadRolesAsync()
        {
            if (_context is null)
                throw new InvalidOperationException("No Context was Passed");

            var rolesFromDb = await _context.Roles
                .FromSqlRaw("SELECT * FROM \"Roles\"")
                .AsNoTracking()
                .ToListAsync();

            Roles = new ObservableCollection<Role>(rolesFromDb);
        }

        public async Task AddRoleAsync(Role role)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL add_role({0})",
                role.Name
            );

            LoadRolesAsync();
        }

        public async Task UpdateRoleAsync(Role role)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL update_role({0}, {1})",
                role.Id,
                role.Name
            );

            _context.ChangeTracker.Clear();
            LoadRolesAsync();
        }

        public async Task DeleteRoleAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL delete_role({0})",
                id
            );

            LoadRolesAsync();
        }
    }
}