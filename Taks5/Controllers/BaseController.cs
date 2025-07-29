using Microsoft.AspNetCore.Mvc;

public class BaseController : Controller
{
    protected bool IsUserLoggedIn()
    {
        return !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));
    }
}
