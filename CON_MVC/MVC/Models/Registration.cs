using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Registration: TbUser
    {
        [Compare("Password",ErrorMessage ="Confirm Password is not matched")]
        public string confirmationPassword { get; set; }
    }
}
