using CommunityToolkit.Mvvm.ComponentModel;
using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceAccountingWinUi.ViewModels;

public partial class DepartmentRoomsViewModel : ObservableObject
{
    private readonly OrganisationContext _context;

    [ObservableProperty]
    private string companyName = "";

    [ObservableProperty]
    private string departmentName = "";

    [ObservableProperty]
    private string currentTime = "";

    [ObservableProperty]
    private ObservableCollection<Room> rooms = new();

    public DepartmentRoomsViewModel(OrganisationContext context)
    {
        _context = context;
    }

    public async Task LoadDepartmentRoomsAsync(int departmentId)
    {
        // Получаем подразделение с комнатами
        var department = await _context.Departments
            .Include(d => d.Rooms)
            .FirstOrDefaultAsync(d => d.Id == departmentId);

        if (department == null)
        {
            CompanyName = "Подразделение не найдено";
            DepartmentName = "";
            CurrentTime = "";
            Rooms = new ObservableCollection<Room>();
            return;
        }

        CompanyName = App.Services.GetRequiredService<MainWindowViewModel>().CompanyName;
        DepartmentName = department.FullName;
        CurrentTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

        // Список комнат подразделения
        var roomList = department.Rooms
            .Select(r => new Room
            {
                Number = r.Number,
                Area = r.Area
            })
            .ToList();

        Rooms = new ObservableCollection<Room>(roomList);
    }
}
