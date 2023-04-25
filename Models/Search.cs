using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace staff_register.Models;


[NotMapped]
public partial class Search
{
    public string? Query { get; set; }
    public bool Fio { get; set; }
    public bool Adress { get; set; }
    public bool Number { get; set; }
}
