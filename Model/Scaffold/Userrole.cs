﻿using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class Userrole
{
    public int IdUserRole { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
