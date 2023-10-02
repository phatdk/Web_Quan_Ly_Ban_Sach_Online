using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities.Identity
{
    public class Role: IdentityRole<int>
    {
        public override int Id { get => base.Id; set => base.Id = value; }
        public virtual ICollection<UserRoles>? _RoleUser { get; set; }
        public virtual ICollection<RoleClaims>? RoleClaims { get; set; }
    }
}
