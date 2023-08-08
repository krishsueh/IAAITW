using IAAITW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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

            #region Menu
            List<Permission> permissions = db.Permissions.ToList();
            var roots = permissions.Where(p => p.ParentId == null);
            StringBuilder sbTree = new StringBuilder();
            foreach (var root in roots)
            {
                sbTree.Append(GetNode(root, userPermissions));
            }
            filterContext.Controller.ViewBag.menu = sbTree.ToString();
            #endregion

            #region 防止手動輸入網址進入無權限頁面
            string websiteUrl = filterContext.HttpContext.Request.Url.AbsolutePath;

            // 分割URL並只取前四個部分
            string[] segments = websiteUrl.Split('/');
            websiteUrl = string.Join("/", segments.Take(4));

            // 檢查特定網址，如果是"/CMS/Home"則直接返回
            if (websiteUrl == "/CMS/Home") return;

            // 檢查用戶的所有權限，看是否有一個匹配當前網址
            bool hasPermission = userPermissions.Any(userPermission =>
            {
                // 從數據庫中查找匹配的URL
                var permissionUrl = db.Permissions.Where(x => x.Code == userPermission)
                                                  .Select(x => x.Url)
                                                  .FirstOrDefault();

                // 如果權限URL與websiteUrl完全匹配，或者權限URL包含websiteUrl
                return websiteUrl == permissionUrl || permissionUrl.Contains(websiteUrl);
            });

            // 如果用戶沒有權限，則重定向至登錄頁面
            if (!hasPermission)
            {
                filterContext.Result = new RedirectResult("/CMS/Home/Login");
                return;
            }
            #endregion

            #region 用權限資料表處理 Breadcrum
            //string path = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            //StringBuilder sbBreadcrum = new StringBuilder();
            //foreach (var root in roots)
            //{
            //    if (root.Permissions.Count() == 0 && root.Url == path)
            //    {
            //        sbBreadcrum.Append($"<li class='breadcrumb-item'><a href='{root.Url}'>{root.Subject}</a></li>");
            //    }
            //    else
            //    {
            //        sbBreadcrum.Append(GetBreadcrum(root, path));
            //    }
            //}
            //filterContext.Controller.ViewBag.breadcrum = sbBreadcrum.ToString();
            #endregion

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

        #region 用權限資料表處理 Breadcrum
        //private string GetBreadcrum(Permission root, string path)
        //{
        //    StringBuilder breadcrum = new StringBuilder();
        //    if (root.Permissions.Count > 0)
        //    {
        //        foreach (var item in root.Permissions)
        //        {
        //            if (item.Url == path)
        //            {
        //                breadcrum.Append($"<li class='breadcrumb-item'>{root.Subject}</li>");
        //                breadcrum.Append($"<li class='breadcrumb-item'><a href='{item.Url}'>{item.Subject}</a></li>");
        //            }
        //            else
        //            {
        //                if (item.Permissions.Count > 0)
        //                {
        //                    foreach (var items in item.Permissions)
        //                    {
        //                        if (items.Url == path)
        //                        {
        //                            breadcrum.Append($"<li class='breadcrumb-item'>{root.Subject}</li>");
        //                            breadcrum.Append($"<li class='breadcrumb-item'><a href='{item.Url}'>{item.Subject}</a></li>");
        //                            breadcrum.Append($"<li class='breadcrumb-item'><a href='{items.Url}'>{items.Subject}</a></li>");
        //                        }
        //                    }
        //                }

        //            }

        //        }
        //    }
        //    return breadcrum.ToString();
        //}
        #endregion
    }
}