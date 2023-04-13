using System;
using System.Collections.Generic;

namespace staff_register.Models;

public partial class Staff
{
    public int Id { get; set; }

    public DateTime Birthday { get; set; }

    public string Fio { get; set; } = null!;

    public int Department { get; set; }

    public string Number { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public string FamilyStatus { get; set; } = null!;

    public decimal Wage { get; set; }

    public byte[] Photo { get; set; } = null!;

    public virtual ICollection<Departmen> Departmen { get; set; } = new List<Departmen>();

    public virtual Departmen DepartmentNavigation { get; set; } = null!;

    public virtual User IdNavigation { get; set; } = null!;
}
