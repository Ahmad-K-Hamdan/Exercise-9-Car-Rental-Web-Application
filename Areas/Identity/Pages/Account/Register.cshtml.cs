using AutoMapper;
using CarRentalWebApplication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CarRentalWebApplication.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly IMapper _mapper;

        public RegisterModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _emailStore = GetEmailStore(userStore);
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required, Display(Name = "First Name")]
            public string FirstName { get; set; } = null!;

            [Required, Display(Name = "Last Name")]
            public string LastName { get; set; } = null!;

            [Required, EmailAddress, Display(Name = "Email")]
            public string Email { get; set; } = null!;

            [Required, DataType(DataType.Password), Display(Name = "Password")]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "The {0} must be at least {2} characters long.")]
            public string Password { get; set; } = null!;

            [Required, DataType(DataType.Password), Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "Passwords do not match.")]
            public string ConfirmPassword { get; set; } = null!;

            [Required, Display(Name = "Phone Number"), DataType(DataType.PhoneNumber)]
            [StringLength(10, ErrorMessage = "The {0} must be {1} numbers long.")]
            public string PhoneNumber { get; set; } = null!;

            [Display(Name = "Date of Birth"), DataType(DataType.Date)]
            public DateTime? DateOfBirth { get; set; }

            [Required, Display(Name = "Address Line 1")]
            public string AddressLine1 { get; set; } = null!;

            [Display(Name = "Address Line 2")]
            public string? AddressLine2 { get; set; }

            [Required, Display(Name = "City")]
            public string City { get; set; } = null!;

            [Required, Display(Name = "Country")]
            public string Country { get; set; } = null!;

            [Required, Display(Name = "Driver's License Number")]
            public string DriverLicenseNumber { get; set; } = null!;
        }

        public async Task OnGetAsync(string returnUrl = null!)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null!)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingUser = await _userManager.FindByEmailAsync(Input.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Input.Email", "An account with this email already exists.");
                return Page();
            }

            var user = _mapper.Map<User>(Input);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            await _userManager.SetUserNameAsync(user, Input.Email);

            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        private static IUserEmailStore<User> GetEmailStore(IUserStore<User> userStore)
        {
            if (userStore is not IUserEmailStore<User> emailStore)
            {
                throw new NotSupportedException("This user store does not support emails.");
            }

            return emailStore;
        }
    }
}
