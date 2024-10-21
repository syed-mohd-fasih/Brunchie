using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Brunchie.Areas.Identity.Data;

namespace Brunchie.Areas.Identity.Pages.Account.Manage
{
    public class EditUsernameModel : PageModel
    {
        private readonly UserManager<BrunchieUser> _userManager;
        private readonly SignInManager<BrunchieUser> _signInManager;

        public EditUsernameModel(UserManager<BrunchieUser> userManager, SignInManager<BrunchieUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public string NewUsername { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            NewUsername = user.UserName;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            user.UserName = NewUsername;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            // Refresh sign-in to reflect the new username
            await _signInManager.RefreshSignInAsync(user);
            TempData["StatusMessage"] = "Your username has been updated";
            return RedirectToPage("./Index");
        }
    }
}
