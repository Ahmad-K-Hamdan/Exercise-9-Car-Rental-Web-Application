using AutoMapper;
using CarRentalWebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CarRentalWebApplication.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required, Display(Name = "First Name")]
            public string? FirstName { get; set; }

            [Required, Display(Name = "Last Name")]
            public string? LastName { get; set; }

            [EmailAddress]
            [Display(Name = "Email")]
            public string? Email { get; set; }

            [Required, Display(Name = "Phone Number"), DataType(DataType.PhoneNumber)]
            [StringLength(10, ErrorMessage = "The {0} must be {1} numbers long.")]
            public string? PhoneNumber { get; set; }

            [Display(Name = "Date of Birth"), DataType(DataType.Date)]
            public DateTime? DateOfBirth { get; set; }

            [Required, Display(Name = "Address Line 1")]
            public string? AddressLine1 { get; set; }

            [Display(Name = "Address Line 2")]
            public string? AddressLine2 { get; set; }

            [Required, Display(Name = "City")]
            public string? City { get; set; }

            [Required, Display(Name = "Country")]
            public string? Country { get; set; }

            [Required, Display(Name = "Driver's License Number")]
            public string? DriverLicenseNumber { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Username = user.UserName;
            Input = _mapper.Map<InputModel>(user);
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

            _mapper.Map(Input, user);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated successfully.";
            return RedirectToPage();
        }
    }
}
