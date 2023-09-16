﻿using BookShop.DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
    public class User : IdentityUser<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birth { get; set; }
        public int? Gender { get; set; }
        //	public string? Email { get; set; }
        //public string? Phone { get; set; }
        //public string UserName { get; set; }
        //public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }

        //foreign key
        public virtual Cart Cart { get; set; }
        public virtual WalletPoint WalletPoint { get; set; }

        //reference
        public virtual ICollection<UserRoles>? _RoleUser { get; set; }
        public virtual ICollection<UserTokens>? UserRoles { get; set; }
        public virtual ICollection<UserLogins>? UserLogins { get; set; }
        public virtual List<WishList> WishLists { get; set; }
        public virtual List<Evaluate> Evaluates { get; set; }
        public virtual List<Order> Orders { get; set; }
        public virtual List<UserPromotion> UserPromotions { get; set; }
    }
}
