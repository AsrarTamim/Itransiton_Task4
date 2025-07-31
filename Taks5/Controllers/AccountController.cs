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
        public IActionResult Dashboard(string search, int page = 1, int pageSize = 5)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }
            var users = _userService.GetAllUsers();
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var user = _userService.GetUserByEmail(userEmail);

            if (user == null )
            {
                HttpContext.Session.Clear();
                TempData["Error"] = "You have been deleted.";
                return RedirectToAction("Login");
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                users = users.Where(u =>
                    (!string.IsNullOrEmpty(u.Name) && u.Name.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(u.Email) && u.Email.ToLower().Contains(search))
                ).ToList();
            }
            var totalItems = users.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var Allusers = users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Search = search;

            return View(Allusers);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        [HttpPost]
        public IActionResult Tools(string action, List<string> selectedEmails)
        {
            string currentUserEmail = HttpContext.Session.GetString("UserEmail");
            var current_user = _userService.GetUserByEmail(currentUserEmail);

            if (current_user == null)
            {
                HttpContext.Session.Clear();
                TempData["Error"] = "Your account has been deleted.";
                return RedirectToAction("Login");
            }

            if (current_user.IsBlocked)
            {
                HttpContext.Session.Clear();
                TempData["Error"] = "You are blocked and cannot perform this action.";
                return RedirectToAction("Login");
            }

            if (selectedEmails == null || !selectedEmails.Any())
            {
                TempData["Error"] = "No users selected.";
                return RedirectToAction("Dashboard");
            }

            bool currentUser = false;

            foreach (var email in selectedEmails)
            {
                var user = _userService.GetUserByEmail(email);
                if (user == null) continue;

                if (email == currentUserEmail) currentUser = true;

                if (action.ToLower() == "block")
                {
                    user.IsBlocked = true;
                    _userService.UpdateUser(user);
                }
                else if (action.ToLower() == "unblock")
                {
                    user.IsBlocked = false;
                    _userService.UpdateUser(user);
                }
                else if (action.ToLower() == "delete")
                {
                    _userService.DeleteUser(email);
                }
            }

            if (currentUser && action.ToLower() == "delete")
            {
                HttpContext.Session.Clear();
                TempData["Error"] = "Your account has been deleted.";
                return RedirectToAction("Login");
            }

            TempData["Success"] = $"Action '{action}' completed successfully.";
            return RedirectToAction("Dashboard");
        }

    }
}
