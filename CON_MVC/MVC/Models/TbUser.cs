using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain;

public partial class TbUser
{
    public int UserId { get; set; }

    public int? TypeUser { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "email requires: @,.com,.co")]
    public string EmailId { get; set; } = null!;

    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character:")]

    public string Password { get; set; } = null!;

    [RegularExpression(@"^([0-9]{10})", ErrorMessage = "Please enter a 10 digit valid number")]
    public long PhoneNumber { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? Active { get; set; }

}
