using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Identity;    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities.Identity
{


    public class UserRoles : IdentityUserRole<int>
    {
        //public virtual User? _User { get; set; }
        //public virtual Role? _Role { get; set; }
    }
}
