using System.Web.Mvc;
using System.Web.Security;
using WebSite.Models;
using WebSite.Providers;

namespace WebSite.Controllers
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
                    ModelState.AddModelError("", "Пользователь с таким e-mail уже существует");
                    return View();
                }

                FormsAuthentication.SetAuthCookie(signUpModel.Email, false);
                return RedirectToAction("Index", "Tew");
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
                    ModelState.AddModelError("", "Неверный e-mail или пароль");
                    return View();
                }

                FormsAuthentication.SetAuthCookie(signInModel.Email, signInModel.StayInLogin);

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Tew");
            }

            return View();
        }

        public ActionResult SignOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn", "Account");
        }
    }
}