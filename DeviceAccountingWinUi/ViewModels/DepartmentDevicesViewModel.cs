using CommunityToolkit.Mvvm.ComponentModel;
using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceAccountingWinUi.ViewModels;

public partial class DepartmentDevicesViewModel : ObservableObject
{
    private readonly OrganisationContext _context;

    [ObservableProperty]
    private string companyName = "";

    [ObservableProperty]
    private string departmentName = "";

    [ObservableProperty]
    private string currentTime = "";

    [ObservableProperty]
    private ObservableCollection<Device> devices = new();

    public DepartmentDevicesViewModel(OrganisationContext context)
    {
        _context = context;
        
    }

    public async Task LoadDepartmentDevicesAsync(int departmentId)
    {
        // Получаем подразделение с названием предприятия
        var department = await _context.Departments
            .Include(d => d.Rooms)
            .ThenInclude(r => r.Devices)
            .FirstOrDefaultAsync(d => d.Id == departmentId);

        if (department == null)
        {
            CompanyName = "Подразделение не найдено";
            DepartmentName = "";
            CurrentTime = "";
            Devices = new ObservableCollection<Device>();
            return;
        }

        CompanyName = App.Services.GetRequiredService<MainWindowViewModel>().CompanyName;
        DepartmentName = department.FullName;
        CurrentTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

        // Получаем все устройства из всех комнат подразделения
        var devicesList = department.Rooms
            .SelectMany(r => r.Devices)
            .Select(d => new Device
            {
                InventoryId = d.InventoryId,
                Name = d.Name,
                Model = d.Model
            })
            .ToList();

        Devices = new ObservableCollection<Device>(devicesList);
    }
}
