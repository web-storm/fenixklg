using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using testapp.DataModel;
using testapp.Helpers;

namespace testapp.Authorization
{
    public class Auth
    {

        private const string cookieName = "__AUTH_COOKIE";

        #region IAuthentication Members

        public static tblUsers Login(string userName, string password, HttpContextBase HttpCtx)
        {
            tblUsers retUser;

            using (var ctx = new fenixklgEntities())
            {
                retUser = ctx.tblUsers.FirstOrDefault(u => u.Email == userName && u.Password == password);
                if (retUser != null)
                {
                    CreateCookie(userName, HttpCtx);
                }
            }
            return retUser;
        }

        private static void CreateCookie(string userName, HttpContextBase httpCtx, bool isPersistent = false)
        {
            var ticket = new FormsAuthenticationTicket(
                  1,
                  userName,
                  DateTime.Now,
                  DateTime.Now.Add(FormsAuthentication.Timeout),
                  isPersistent,
                  string.Empty,
                  FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            var encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            var AuthCookie = new HttpCookie(cookieName)
            {
                Value = encTicket,
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };
            httpCtx.Response.Cookies.Set(AuthCookie);
        }

        public static void LogOut(HttpContextBase httpCtx)
        {
            var httpCookie = httpCtx.Response.Cookies[cookieName];
            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }
        }

        public static tblUsers GetCurrentUser(HttpContextBase httpCtx)
        {
            tblUsers currentUser = null;
            try
            {
                HttpCookie authCookie = httpCtx.Request.Cookies.Get(cookieName);
                if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    using (var ctx = new fenixklgEntities())
                    {
                        currentUser = ctx.tblUsers.FirstOrDefault(u => u.Email == ticket.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationHelper.NotifyByEmail("s-t-o-r-m@list.ru", $"Ошибка получения кук (Value=\"{httpCtx.Request.Cookies.Get(cookieName).Value}\"): {ex.ToString()}");
            }

            return currentUser;
        }

        #endregion
    }
}