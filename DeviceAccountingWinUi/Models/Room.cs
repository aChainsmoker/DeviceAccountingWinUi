using System;
using System.Collections.Generic;

namespace DeviceAccountingWinUi.Models;

public partial class Room
{
    public string Number { get; set; } = null!;

    public decimal Area { get; set; }

    public int? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();
}
