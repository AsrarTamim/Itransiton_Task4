using Microsoft.AspNetCore.Mvc;
using Taks5.Models;
using Taks5.Service;

namespace Taks5.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserService _userService;
        public AccountController(UserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _userService.Registration(model);
                    ViewBag.Massage = $"{model.Name} Registered successfully.";
                    return RedirectToAction("Login");
                }
                catch (ApplicationException ex)
                {
                    ModelState.AddModelError("Email", ex.Message);
                    return View(model); 
                }
            }

            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _userService.Login(model);

                    HttpContext.Session.SetString("UserId", user.ID.ToString());
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserName", user.Name);

                    return RedirectToAction("Dashboard");
                }
                catch (ApplicationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(model);
                }
            }

            return View(model);
        }
        public IActionResult Dashboard(string search)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }
            var users = _userService.GetAllUsers();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                users = users.Where(u =>
                    (!string.IsNullOrEmpty(u.Name) && u.Name.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(u.Email) && u.Email.ToLower().Contains(search))
                ).ToList();
            }

            return View(users);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        [HttpPost]
        public IActionResult Tools(string action, List<string> selectedEmails)
        {
            if (selectedEmails == null || !selectedEmails.Any())
            {
                TempData["Error"] = "No users selected.";
                return RedirectToAction("Dashboard");
            }

            foreach (var email in selectedEmails)
            {
                var user = _userService.GetUserByEmail(email);
                if (user == null) continue;

                switch (action.ToLower())
                {
                    case "block":
                        user.IsBlocked = true;
                        _userService.UpdateUser(user);
                        break;
                    case "unblock":
                        user.IsBlocked = false;
                        _userService.UpdateUser(user);
                        break;
                    case "delete":
                        _userService.DeleteUser(user.Email);
                        break;
                }

            }

            TempData["Success"] = $"Action '{action}' completed successfully.";
            return RedirectToAction("Dashboard");
        }




    }
}
