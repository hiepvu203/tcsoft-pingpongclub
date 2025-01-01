using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using tcsoft_pingpongclub.Service;

namespace tcsoft_pingpongclub.Filter
{
    public class AuthorizationFilter : ActionFilterAttribute
    {
        private readonly IsAuthorized _authorizationService;

        public AuthorizationFilter(IsAuthorized authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;

            // Lấy thông tin role từ session
            var idRole = httpContext.Session.GetInt32("IdRole");

            if (idRole == null)
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
                return;
            }
            var controllerName = context.RouteData.Values["controller"].ToString();
            var actionName = context.RouteData.Values["action"].ToString();
            var hasPermission = _authorizationService.hasPer(idRole.Value, controllerName);
            if (!hasPermission)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "login", null);
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
