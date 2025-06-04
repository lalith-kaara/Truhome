using System;
using System.Collections.Generic;

namespace Truhome.Domain.Entities;

public partial class Address
{
    public int Id { get; set; }

    public string? Addresstype { get; set; }

    public string? Unitno { get; set; }

    public string? Addline1 { get; set; }

    public string? Addline2 { get; set; }

    public string? Landmark { get; set; }

    public int? Pincode { get; set; }

    public string? AreaOfLocality { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public int? Customerid { get; set; }

    public virtual Customer? Customer { get; set; }
}
