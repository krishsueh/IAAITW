using IAAITW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IAAITW.Areas.CMS.Security
{
    public class PermissionFilters : ActionFilterAttribute
    {
        private IaaiTwDb db = new IaaiTwDb();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //取得使用者 permission
            FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
            FormsAuthenticationTicket ticket = identity.Ticket;
            string userData = ticket.UserData;
            string[] userDataParts = userData.Split(';');
            string permissionInToken = userDataParts[5];
            List<string> userPermissions = permissionInToken.Split(',').ToList();

            List<Permission> permissions = db.Permissions.ToList();
            var roots = permissions.Where(p => p.ParentId == null);

            StringBuilder sbTree = new StringBuilder();
            foreach (var root in roots)
            {
                sbTree.Append(GetNode(root, userPermissions));
            }

            //todo:遞迴組字串
            filterContext.Controller.ViewBag.menu = sbTree.ToString();
        }

        private string GetNode(Permission root, List<string> userPermissions)
        {
            StringBuilder sbNode = new StringBuilder();

            if (!userPermissions.Any(p => p.StartsWith(root.Code)))
            {
                // Skip this menu item if the user doesn't have permission
                return "";
            }

            if (root.Permissions.Count() > 0)
            {
                sbNode.Append($"<li class='nav-item pcoded-hasmenu'>");
                sbNode.Append($"<a href='javascript:' class='nav-link '><span class='pcoded-micon'><i class='feather icon-box'></i></span><span class='pcoded-mtext'>{root.Subject}</span></a>");
                sbNode.Append("<ul class='pcoded-submenu'>");
                foreach (var items in root.Permissions)
                {
                    sbNode.Append(GetSubNode(items, userPermissions));
                }
                sbNode.Append("</ul>");
            }
            else
            {
                sbNode.Append($"<li class='nav-item'>");
                sbNode.Append($"<a href='{root.Url}'><span class='pcoded-micon'><i class='feather icon-file-text'></i></span><span class='pcoded-mtext'>{root.Subject}</span></a>");
            }
            sbNode.Append("</li>");
            return sbNode.ToString();
        }

        private string GetSubNode(Permission items, List<string> userPermissions)
        {
            StringBuilder sbSubNode = new StringBuilder();

            if (!userPermissions.Any(p => p.StartsWith(items.Code)))
            {
                // Skip this menu item if the user doesn't have permission
                return "";
            }

            sbSubNode.Append($"<li><a href='{items.Url}'>{items.Subject}</a></li>");
            return sbSubNode.ToString();
        }


    }
}