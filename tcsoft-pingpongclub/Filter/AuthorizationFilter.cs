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

            // Nếu không có IdRole trong session, chuyển hướng đến trang login
            if (idRole == null)
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
                return;
            }

            // Lấy tên controller và action hiện tại từ route data
            var controllerName = context.RouteData.Values["controller"].ToString();
            var actionName = context.RouteData.Values["action"].ToString();

            // Kiểm tra quyền truy cập của role đối với controller và action hiện tại
            var hasPermission = _authorizationService.hasPer(idRole.Value, controllerName);

            if (!hasPermission)
            {
                // Trả về HTTP 403 nếu không có quyền
                context.Result = new ForbidResult();
                return;
            }

            // Tiếp tục thực thi action nếu có quyền
            base.OnActionExecuting(context);
        }
    }
}
