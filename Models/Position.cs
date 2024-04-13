using System;
using System.Collections.Generic;

namespace testapi.Models;

public partial class Position
{
    public string Name { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longtitude { get; set; }
}
