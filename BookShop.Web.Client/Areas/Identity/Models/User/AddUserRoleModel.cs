using System.Collections.Generic;
using System.ComponentModel;
using BookShop.DAL.Entities;
using BookShop.DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace App.Areas.Identity.Models.UserViewModels
{
  public class AddUserRoleModel
  {
    public Userr user { get; set; }

    [DisplayName("Các role gán cho user")]
    public string[]? RoleNames { get; set; }

    public List<RoleClaims> claimsInRole { get; set; }
    public List<UserClaims> claimsInUserClaim { get; set; }

  }
}