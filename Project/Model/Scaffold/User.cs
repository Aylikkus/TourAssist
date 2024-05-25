using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class User
{
    public int IdUser { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public DateOnly Birth { get; set; }

    public int UserRoleIdUserRole { get; set; }

    public string? Login { get; set; }

    public string? PasswordSha256 { get; set; }

    public virtual ICollection<Entry> Entries { get; set; } = new List<Entry>();

    public virtual Userrole UserRoleIdUserRoleNavigation { get; set; } = null!;
}
