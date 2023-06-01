using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace IAAITW
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                // �����o�ӨϥΪ̪� FormsIdentity
                FormsIdentity id = (FormsIdentity)User.Identity;

                // �A���X�ϥΪ̪� FormsAuthenticationTicket
                FormsAuthenticationTicket ticket = id.Ticket;

                // �N�x�s�b FormsAuthenticationTicket ��������w�q���X�A���ন�r��}�C
                string[] roles = ticket.UserData.Split(new char[] { ';' });

                // ���������ثe�o�� HttpContext �� User ����h
                Context.User = new GenericPrincipal(Context.User.Identity, roles);
            }
        }
    }
}
