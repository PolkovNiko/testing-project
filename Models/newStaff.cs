using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace staff_register.Models;
[NotMapped]
public partial class newStaff
{
    public int Id { get; set; }

    public DateTime Birthday { get; set; }

    public string Fio { get; set; } = null!;

    public int Department { get; set; }

    public string Number { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public string FamilyStatus { get; set; } = null!;

    public decimal Wage { get; set; }

    public IFormFile Photo { get; set; } = null!;

    public virtual ICollection<Departmen> Departmen { get; set; } = new List<Departmen>();
    public virtual Departmen DepartmentNavigation { get; set; } = null!;
}
