using CommunityToolkit.Mvvm.ComponentModel;
using DeviceAccountingWinUi.DatabaseControl;
using DeviceAccountingWinUi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceAccountingWinUi.ViewModels;

public partial class WorkerDevicesViewModel : ObservableObject
{
    private readonly OrganisationContext _context;

    [ObservableProperty]
    private string workerName = "";

    [ObservableProperty]
    private string workerRole = "";

    [ObservableProperty]
    private string currentTime = "";

    [ObservableProperty]
    private ObservableCollection<Device> devices = new();

    public WorkerDevicesViewModel(OrganisationContext context)
    {
        _context = context;
    }

    public async Task LoadWorkerDevicesAsync(int workerId)
    {
        var worker = await _context.Workers
            .Include(w => w.Role)
            .Include(w => w.Devices)
            .FirstOrDefaultAsync(w => w.Id == workerId);

        if (worker == null)
        {
            WorkerName = "Работник не найден";
            WorkerRole = "";
            CurrentTime = "";
            Devices = new ObservableCollection<Device>();
            return;
        }

        WorkerName = worker.Name;
        WorkerRole = worker.Role?.Name ?? "Не указана";
        CurrentTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

        var deviceList = worker.Devices
            .Select(d => new Device
            {
                InventoryId = d.InventoryId,
                Name = d.Name,
                Model = d.Model
            })
            .ToList();

        Devices = new ObservableCollection<Device>(deviceList);
    }
}
