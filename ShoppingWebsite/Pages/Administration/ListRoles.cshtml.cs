using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoppingWebsite
{
    //[Authorize(Roles = "Admin")]
    [AllowAnonymous]
    public class ListRolesModel : PageModel
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public ListRolesModel(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public IEnumerable<IdentityRole> roles { get; set; }
        //List<IdentityRole> roles { get; set; } = new List<IdentityRole>();

        public IActionResult OnGet()
        {
            
            roles = roleManager.Roles;

            return Page();
        }
    }
}