using System;
using System.Collections.Generic;

namespace DeviceAccountingWinUi.Models;

public partial class DeviceTransfer
{
    public int Id { get; set; }

    public string DeviceId { get; set; } = null!;

    public DateOnly DateOfTransfer { get; set; }

    public int? FromWorkerId { get; set; }

    public int ToWorkerId { get; set; }

    public virtual Device Device { get; set; } = null!;

    public virtual Worker? FromWorker { get; set; }

    public virtual Worker ToWorker { get; set; } = null!;
}
