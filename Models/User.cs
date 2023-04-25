using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace staff_register.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Rank { get; set; } = null!;
    [NotMapped]
    public string url { get; set; } = null!;

    public virtual Staff? Staff { get; set; }
}
