/*using AppleStore.DataAcess;
using AppleStore.DataQuery;
using AppleStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace AppleStore.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext? _context;
        QueryData QD = new QueryData();

        // GET: User
        public ActionResult Index(string msg = null)
        {
            if(HttpContext.Session.GetString("user_id") == null)
            {
                return RedirectToAction("Login");
            }
            ViewData["user"] = QD.GetUser((int)HttpContext.Session.GetString("user_id"));
            ViewData["listInvoices"] = QD.GetInvoices((int)Session["user_id"]);
            ViewData["UserOrder"] = QD.GetUserOrder((int)Session["user_id"]);
            if (msg != null) ViewData["msg"] = msg;
            return View();
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            if (Session.["user_id"] == null)
            {
                return RedirectToAction("Login");
            }

            string msg;
            string password = HttpContext.Request.Form["password"];
            string new_password = HttpContext.Request.Form["new_password"];
            if (password != null && new_password != null)
            {
                if (new_password.Equals(HttpContext.Request.Form["re_new_password"]))
                {
                    if (QD.ChangePass((int)Session["user_id"], password, new_password, out msg))
                    {
                        msg = "Đổi mật khẩu thành công!";
                    }
                    //else msg = "Đổi mật khẩu thất bại. Vui lòng thử lại!";
                }
                else msg = "Mật khẩu mới không trùng nhau. Vui lòng thử lại!";
            }
            else
            {
                user.user_id = (int)Session["user_id"];
                if (QD.UpdateUser(user)) msg = "Cập nhật thành công!";
                else msg = "Không thể cập nhật. Vui lòng thử lại!";
            }
            ViewData["msg"] = msg;
            ViewData["user"] = QD.GetUser((int)Session["user_id"]);
            return RedirectToAction("Index");
        }


        public ActionResult Login()
        {
            if (Session["user_id"] != null)
            {
                return RedirectToAction("", "Home");
            }
            string token = Request.QueryString["access_token"];
            string from = Request.QueryString["from"];
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(from))
            {
                if (QD.LoginWith(token, from, out Users user) > 0)
                {
                    Session["user_id"] = user.user_id;
                    Session["user_fullname"] = user.user_fullname;
                    Session["user_email"] = user.user_email;
                    return RedirectToAction("", "Home");
                }
                else ViewData["msg"] = "Không thể đăng nhập. Vui lòng thử lại sau!";
            }
            return View();
        }


        [HttpPost]
        public ActionResult Login(User user)
        {
            int success = QD.Login(user.user_email, user.user_password, out user);
            string msg;
            if (success == -1) msg = "Tài khoản không tồn tại.";
            else if (success == 0) msg = "Tài khoản không chính xác.";
            else
            {
                Session["user_id"] = user.user_id;
                Session["user_fullname"] = user.user_fullname;
                Session["user_email"] = user.user_email;
                return RedirectToAction("", "Home");
            }
            ViewData["msg"] = msg;
            return View();
        }


        public ActionResult Register()
        {
            if (Session["user_id"] != null)
            {
                return RedirectToAction("", "Home");
            }
            string token = Request.QueryString["access_token"];
            string from = Request.QueryString["from"];
            if (token != null && from != null)
            {
                if (QD.LoginWith(token, from, out Users user) > 0)
                {
                    Session["user_id"] = user.user_id;
                    Session["user_fullname"] = user.user_fullname;
                    Session["user_email"] = user.user_email;
                    return RedirectToAction("", "Home");
                }
                else ViewData["msg"] = "Không thể đăng nhập. Vui lòng thử lại sau!";
            }
            return View();
        }

        [HttpPost]
        public ActionResult Register(Users user)
        {
            string msg;
            string password = HttpContext.Request.Form["user_password"];
            if (password.Equals(HttpContext.Request.Form["user_repassword"]))
            {
                int user_id = QD.Reg(HttpContext.Request.Form["user_fullname"], HttpContext.Request.Form["user_email"], password);
                if (user_id > 0)
                {
                    ViewData["msg"] = "Đăng ký thành công. Vui lòng đăng nhập!";
                    return RedirectToAction("Login");
                }
                else msg = "Đăng ký thất bại. Vui lòng thử lại!";
            }
            else msg = "Mật khẩu không trùng nhau. Vui lòng thử lại!";
            ViewData["msg"] = msg;
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("user_id");
            Session.Remove("user_fullname");
            Session.Remove("user_email");
            return RedirectToAction("", "Home");
        }   


    }
}
*/