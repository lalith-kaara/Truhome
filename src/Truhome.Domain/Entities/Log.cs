using System;
using System.Collections.Generic;

namespace Truhome.Domain.Entities;

public partial class Log
{
    public int Id { get; set; }

    public string Level { get; set; } = null!;

    public string Message { get; set; } = null!;

    public Guid? Correlationid { get; set; }

    public DateTime Loggedat { get; set; }
}
