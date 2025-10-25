using System;
using System.Collections.Generic;

namespace DeviceAccountingWinUi.Models;

public partial class Room
{
    public string Number { get; set; } = null!;

    public decimal Area { get; set; }

    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
