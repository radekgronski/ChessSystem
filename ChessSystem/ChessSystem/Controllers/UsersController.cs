using System.Web.Mvc;
using System.Data.Entity;

using ChessSystem.Models;


namespace ChessSystem.Controllers
{
    public class UsersController : Controller
    {
        private ChessSystemDbEntities db = new ChessSystemDbEntities();


        ~UsersController()
        {
            try
            {
                db.Dispose();
            }
            finally
            {
                base.Dispose();
            }
        }


        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }


        public ActionResult UserProfile()
        {
            if (Session["UserId"] != null)
            {
                int userId = int.Parse(Session["UserId"].ToString());
                var user = db.Users.Find(userId);

                if (user == null)
                {
                    return new HttpStatusCodeResult(404);
                }

                return View(user);
            }

            return RedirectToAction("Index");
        }


        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(Users userData)
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Logout", "Home");
            }

            if (ModelState.IsValid)
            {
                bool isUserNameEmpty = string.IsNullOrEmpty(userData.Name);
                bool isUserPasswordNotValid = string.IsNullOrEmpty(userData.Password) || userData.Password.Trim().Length < 5;

                if (isUserNameEmpty || isUserPasswordNotValid)
                {
                    if (isUserNameEmpty)
                    {
                        ModelState.AddModelError("Name", "Name cannot be empty");
                    }

                    if (isUserPasswordNotValid)
                    {
                        ModelState.AddModelError("Password", "Password must have at least 5 characters.");
                    }

                    return View(userData);
                }

                userData.Name = userData.Name.Trim();
                userData.Password = userData.Password.Trim();

                var user = db.Users.Add(userData);
                db.SaveChanges();

                Session["UserId"] = user.Id.ToString();
                Session["Login"] = user.Login.ToString();
                Session["Name"] = user.Name.ToString();

                return RedirectToAction("Index");
            }

            return View(userData);
        }


        public ActionResult EditProfile()
        {
            if (Session["UserId"] != null)
            {
                int userId = int.Parse(Session["UserId"].ToString());
                var user = db.Users.Find(userId);

                return View(user);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult EditProfile(Users userData)
        {
            if (Session["UserId"] != null)
            {
                var user = db.Users.Find(userData.Id);

                bool isUserNameEmpty = string.IsNullOrEmpty(userData.Name);
                bool isUserPasswordNotValid = string.IsNullOrEmpty(userData.Password) || userData.Password.Trim().Length < 5;

                if (isUserNameEmpty || isUserPasswordNotValid)
                {
                    if (isUserNameEmpty)
                    {
                        ModelState.AddModelError("Name", "Name cannot be empty");
                    }

                    if (isUserPasswordNotValid)
                    {
                        ModelState.AddModelError("Password", "Password must have at least 5 characters.");
                    }

                    return View(user);
                }

                userData.Name = userData.Name.Trim();
                userData.Password = userData.Password.Trim();

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("UserProfile");
            }

            return RedirectToAction("Index");
        }


        public ActionResult DeleteProfile()
        {
            if (Session["UserId"] != null)
            {
                int userId = int.Parse(Session["UserId"].ToString());
                var user = db.Users.Find(userId);

                return View(user);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult DeleteProfile(Users userData)
        {
            if (Session["UserId"] != null)
            {
                int userId = int.Parse(Session["UserId"].ToString());

                if (userData.Id != userId)
                {
                    return new HttpStatusCodeResult(403);
                }

                var user = db.Users.Find(userId);

                db.Users.Remove(user);
                db.SaveChanges();

                return RedirectToAction("Logout", "Home");
            }

            return RedirectToAction("Index");
        }
    }
}