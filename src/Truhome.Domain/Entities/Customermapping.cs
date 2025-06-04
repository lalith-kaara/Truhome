using System;
using System.Collections.Generic;

namespace Truhome.Domain.Entities;

public partial class Customermapping
{
    public int Id { get; set; }

    public int? Customerid { get; set; }

    public string? Externalcustomerid { get; set; }
}
