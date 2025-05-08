using Microsoft.AspNetCore.Mvc;

namespace BlogApp.ViewComponents
{
    public class TempMessage : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var alertTypes = new Dictionary<string, string>
            {
                 { "LoginErrorMessage", "alert-danger" },
                 { "RegisterErrorMessage", "alert-danger" },
                 { "RegisterMessage", "alert-success" },
                 { "PostAddMessage", "alert-success" },
                 { "PostEditMessage", "alert-success" },
                 { "UserNotFoundMessage", "alert-danger" },
                 { "TagExistsMessage", "alert-danger" },
            };

            var message = alertTypes.Keys.Select(key => TempData[key]?.ToString()).FirstOrDefault(val => val != null);
            var alertClass = alertTypes.FirstOrDefault(x => TempData[x.Key] != null).Value;

            return View("Default", (message, alertClass));
        }
    }
}