using System;
using System.Collections.Generic;

namespace DeviceAccountingWinUi.Models;

public partial class Device
{
    public string InventoryId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Model { get; set; } = null!;

    public decimal Cost { get; set; }

    public virtual ICollection<DeviceTransfer> DeviceTransfers { get; set; } = new List<DeviceTransfer>();
}
