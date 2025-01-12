using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using tcsoft_pingpongclub.Models;
using System.Linq;
using System.Collections.Generic;

public class MenuActionFilter : ActionFilterAttribute
{
    private readonly ThuctapKtktcn2024Context _context;

    public MenuActionFilter(ThuctapKtktcn2024Context context)
    {
        _context = context;
    }
	private void SetMenuItemsToViewBag(ActionExecutingContext context, List<Tuple<string, string>> menuItems)
{
    if (context.Controller is Controller controller)
    {
        controller.ViewBag.menu = menuItems; 
    }
}
    public override void OnActionExecuting(ActionExecutingContext context)
{
    var resultList = new List<Tuple<string, string>>();
	resultList.Add(Tuple.Create("home", "Trang chủ"));
    var idRole = context.HttpContext.Session.GetInt32("IdRole");
    var idMember = context.HttpContext.Session.GetInt32("IdMember");

    if (idRole != null && idMember != null)
    {
        var user = _context.Members.FirstOrDefault(m => m.IdMember == idMember);
        if (user != null)
        {
            var urlsAndNames = (from r in _context.Roles
                                join pr in _context.PermissionRoles on r.IdRole equals pr.IdRole
                                join p in _context.Permissions on pr.IdPermission equals p.IdPermission
                                where r.IdRole == user.IdRole && r.Status == true
                                      && pr.Status == true && p.Status == true
                                select new
                                {
                                    Url = p.Url,
                                    Name = p.NamePermission
                                }).Distinct().ToList();

           resultList.AddRange(urlsAndNames.Select(item => Tuple.Create(item.Url, item.Name)));
        }
    }
    SetMenuItemsToViewBag(context, resultList);

    base.OnActionExecuting(context);
}

}
