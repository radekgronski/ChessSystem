using System.Linq;
using System.Web.Mvc;

using ChessSystem.Models;


namespace ChessSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users userData)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ChessSystemDbEntities())
                {
                    var user = db.Users.Where(
                        u => u.Login.Equals(userData.Login) && u.Password.Equals(userData.Password)
                    ).FirstOrDefault();

                    if (user != null)
                    {
                        Session["UserId"] = user.Id.ToString();
                        Session["Login"] = user.Login.ToString();
                        Session["Name"] = user.Name.ToString();

                        return RedirectToAction("Index");
                    }
                }
            }

            return View(userData);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}