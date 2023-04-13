using System;
using System.Collections.Generic;

namespace staff_register.Models;

public partial class Departmen
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int IdBoss { get; set; }

    public string About { get; set; } = null!;

    public virtual Staff IdBossNavigation { get; set; } = null!;

    public virtual Staff? Staff { get; set; }
}
