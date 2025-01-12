using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
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

    private void SetMenuItemsToViewBag(ActionExecutingContext context, List<Permission> menuItems)
    {
        if (context.Controller is Controller controller)
        {
            controller.ViewBag.Menu = menuItems;
        }
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var menuList = new List<Permission>();

        var idRole = context.HttpContext.Session.GetInt32("IdRole");
        var idMember = context.HttpContext.Session.GetInt32("IdMember");

        if (idRole.HasValue && idMember.HasValue)
        {
            var parentPermissions = (from p in _context.Permissions
                                     join pr in _context.PermissionRoles on p.IdPermission equals pr.IdPermission
                                     where pr.IdRole == idRole && p.Status == true && p.IdPerParent == null
                                     select p).ToList().Distinct();

            var childPermissions = (from p in _context.Permissions
                                    join pr in _context.PermissionRoles on p.IdPermission equals pr.IdPermission
                                    where pr.IdRole == idRole && p.Status == true && p.IdPerParent != null
                                    select p).ToList().Distinct();

            foreach (var parent in parentPermissions)
            {
                var subPermissions = childPermissions
                    .Where(c => c.IdPerParent == parent.IdPermission)
                    .ToList();

                parent.SubPermissions = subPermissions;

                menuList.Add(parent);
            }
        }

        SetMenuItemsToViewBag(context, menuList);
        base.OnActionExecuting(context);
    }
}