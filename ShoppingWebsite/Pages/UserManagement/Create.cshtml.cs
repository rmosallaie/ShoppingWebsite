using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingWebsite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWebsite.Models;

namespace ShoppingWebsite
{
    //[Authorize(Roles = "Admin")]
    [AllowAnonymous]
    public class CreateModel : PageModel
    {
        private readonly ShoppingWebsiteContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public CreateModel(ShoppingWebsiteContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User NewUser { get; set; }


        //[AcceptVerbs("Get", "Post")]
        //public async Task<IActionResult> IsUsernameInUse(string Username)
        //{
        //    var user = await userManager.FindByNameAsync(Username);

        //    if (user == null)
        //    {
        //        return Json(true);
        //    }
        //}

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new IdentityUser
            {
                UserName = NewUser.Email
            };
            var result = await userManager.CreateAsync(user, NewUser.Password);

            if (result.Succeeded)
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}