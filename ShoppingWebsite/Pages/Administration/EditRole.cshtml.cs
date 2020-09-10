using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ShoppingWebsite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoppingWebsite
{
    [AllowAnonymous]
    //[Authorize(Roles = "Admin")]
    public class EditRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ShoppingWebsiteContext _context;

        public EditRoleModel(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager,
                            ShoppingWebsiteContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            _context = context;
        }

        public class EditRole
        {
            public EditRole()
            {
                Users = new List<string>();
            }
            public string Id { get; set; }

            [Required(ErrorMessage = "Role Name is required")]
            public string RoleName { get; set; }

            public List<string> Users { get; set; }

        }


        [BindProperty]
        public EditRole editRole { get; set; }


        public async Task<IActionResult> OnGet(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return LocalRedirect("/NotFound");
            }

            //editRole.Id = role.Id.ToString();
            //Globals.Idd = role.Id;
            //editRole.RoleName = role.Name;
            editRole = new EditRole
            {
                Id = role.Id,
                RoleName = role.Name
            };
            Globals.Idd = role.Id;
            foreach (var user in userManager.Users)
            {
                // If the user is in this role, add the username to
                // Users property of EditRoleViewModel. This model
                // object is then passed to the view for display
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    editRole.Users.Add(user.UserName);
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var role = await roleManager.FindByIdAsync(Globals.Idd);

            if (role == null)
            {
                return NotFound();
            }
            else
            {
                role.Name = editRole.RoleName;
                var result = roleManager.UpdateAsync(role).GetAwaiter().GetResult();
                await _context.SaveChangesAsync();

                if (result.Succeeded)
                {
                    return RedirectToPage("/Administration/ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return Page();
            }
        }
    }
}