using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TewCloud.Models;
using TewCloud.Providers;

namespace TewCloud.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		[AllowAnonymous]
		public ActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult SignUp(SignUpModel signUpModel)
		{
			if (ModelState.IsValid)
			{
				var isCreated = ((TewMembershipProvider)Membership.Provider).CreateUser(signUpModel.Email, signUpModel.Password);

				if (isCreated == false)
				{
					ModelState.AddModelError("", "User with that email already exists.");
					return View();
				}

				FormsAuthentication.SetAuthCookie(signUpModel.Email, false);
				return RedirectToAction("Learning", "Tew");
			}

			return View();
		}

		[AllowAnonymous]
		public ActionResult SignIn(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult SignIn(SignInModel signInModel, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				var isValidated = Membership.Provider.ValidateUser(signInModel.Email, signInModel.Password);

				if (isValidated == false)
				{
					ModelState.AddModelError("", "Email or password is incorrect.");
					return View();
				}

				FormsAuthentication.SetAuthCookie(signInModel.Email, signInModel.StayInLogin);

				if (Url.IsLocalUrl(returnUrl))
				{
					return Redirect(returnUrl);
				}

				return RedirectToAction("Learning", "Tew");
			}

			return View();
		}
	}
}