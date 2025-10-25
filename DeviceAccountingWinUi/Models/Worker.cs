using System;
using System.Collections.Generic;

namespace DeviceAccountingWinUi.Models;

public partial class Worker
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int RoleId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public virtual ICollection<DeviceTransfer> DeviceTransferFromWorkers { get; set; } = new List<DeviceTransfer>();

    public virtual ICollection<DeviceTransfer> DeviceTransferToWorkers { get; set; } = new List<DeviceTransfer>();

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();

    public virtual Role Role { get; set; } = null!;

    public virtual Room RoomNumberNavigation { get; set; } = null!;
}
