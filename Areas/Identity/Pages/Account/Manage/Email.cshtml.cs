using System.ComponentModel.DataAnnotations;
using CarRentalWebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarRentalWebApplication.Areas.Identity.Pages.Account.Manage
{
    public class EmailModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public EmailModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; } = string.Empty;

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [Display(Name = "Current Email")]
        public string CurrentEmail { get; set; } = string.Empty;

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "New Email")]
            public string NewEmail { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            CurrentEmail = user.Email ?? string.Empty;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var existingUser = await _userManager.FindByEmailAsync(Input.NewEmail);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                ModelState.AddModelError("Input.NewEmail", "This email address is already in use by another account.");
                return Page();
            }

            if (Input.NewEmail.Equals(user.Email, StringComparison.OrdinalIgnoreCase))
            {
                StatusMessage = "Your email is unchanged.";
                return RedirectToPage();
            }

            user.Email = Input.NewEmail;
            user.UserName = Input.NewEmail;
            user.NormalizedEmail = Input.NewEmail.ToUpperInvariant();
            user.NormalizedUserName = Input.NewEmail.ToUpperInvariant();

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your email has been updated successfully.";
            return RedirectToPage();
        }
    }
}
