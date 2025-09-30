using System;
using System.Collections.Generic;

namespace DeviceAccountingWinUi.Models;

public partial class Department
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
