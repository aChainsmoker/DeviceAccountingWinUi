using CommunityToolkit.Mvvm.ComponentModel;
using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DeviceAccountingWinUi.ViewModels
{
    public partial class RoomsViewModel : ViewModelBase
    {
        private readonly OrganisationContext _context;
        [ObservableProperty]
        private ObservableCollection<Room> _rooms = new();

        public RoomsViewModel(OrganisationContext context)
        {
            this._context = context;
            LoadRoomsAsync();
        }

        private async void LoadRoomsAsync()
        {
            if (_context is null)
                throw new InvalidOperationException("No Context was Passed");

            var roomsFromDb = await _context.Rooms
                .FromSqlRaw("SELECT * FROM \"Rooms\"")
                .AsNoTracking()
                .ToListAsync();

            Rooms = new ObservableCollection<Room>(roomsFromDb);
        }

        public async Task AddRoomAsync(Room room)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL add_room({0}, {1}, {2})",
                room.Number,
                room.Area,
                room.DepartmentId
            );

            LoadRoomsAsync();
        }

        public async Task UpdateRoomAsync(Room room)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL update_room({0}, {1}, {2})",
                room.Number,
                room.Area,
                room.DepartmentId
            );

            _context.ChangeTracker.Clear();
            LoadRoomsAsync();
        }

        public async Task DeleteRoomAsync(string number)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL delete_room({0})",
                number
            );

            LoadRoomsAsync();
        }
    }
}