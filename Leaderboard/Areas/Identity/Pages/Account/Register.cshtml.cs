// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Leaderboard.Core.Contracts;
using Leaderboard.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using static Leaderboard.Core.Constants.MessagesConstants;
using static Leaderboard.Infrastructure.Constants.DataConstants;

namespace Leaderboard.Areas.Identity.Pages.Account
{
	public class RegisterModel : PageModel
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IUserStore<ApplicationUser> _userStore;
		private readonly IUserEmailStore<ApplicationUser> _emailStore;
		private readonly ILogger<RegisterModel> _logger;
		private readonly IOrganizationService _organizationService;

		public RegisterModel(
			UserManager<ApplicationUser> userManager,
			IUserStore<ApplicationUser> userStore,
			SignInManager<ApplicationUser> signInManager,
			ILogger<RegisterModel> logger,
			IOrganizationService organizationService)
		{
			_userManager = userManager;
			_userStore = userStore;
			_emailStore = GetEmailStore();
			_signInManager = signInManager;
			_logger = logger;
			_organizationService = organizationService;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		public class InputModel
		{
			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string Email { get; set; }

			[Required]
			[StringLength(OrganizationNameMaxLength,
				MinimumLength = OrganizatioNameMinLength,
				ErrorMessage = StringLengthBetweenValuesMessage)]
			[Display(Name = "Organization name")]
			public string OrganizationName { get; set; }

			[Required]
			[StringLength(UserPasswordMaxLength,
				MinimumLength = UserPasswordMinLength,
				ErrorMessage = StringLengthBetweenValuesMessage)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; }
		}


		public void OnGetAsync(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");

			if (ModelState.IsValid == false)
			{
				return Page();
			}

			else if (ModelState.IsValid)
			{
				var user = CreateUser();

				user.OrganizationId = await _organizationService.CreateOrganizationAsync(Input.OrganizationName);
				user.CanAddUsers = true;
				await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
				await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

				var result = await _userManager.CreateAsync(user, Input.Password);

				if (result.Succeeded)
				{
					_logger.LogInformation("User created a new account with password.");

					await _signInManager.SignInAsync(user, isPersistent: false);
					return LocalRedirect(returnUrl);
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}

		private ApplicationUser CreateUser()
		{
			try
			{
				return Activator.CreateInstance<ApplicationUser>();
			}
			catch
			{
				throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
					$"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
					$"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
			}
		}

		private IUserEmailStore<ApplicationUser> GetEmailStore()
		{
			if (!_userManager.SupportsUserEmail)
			{
				throw new NotSupportedException("The default UI requires a user store with email support.");
			}
			return (IUserEmailStore<ApplicationUser>)_userStore;
		}
	}
}
