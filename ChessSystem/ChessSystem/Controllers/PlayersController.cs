using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

using ChessSystem.Models;


namespace ChessSystem.Controllers
{
    public class PlayersController : Controller
    {
        private ChessSystemDbEntities db = new ChessSystemDbEntities();


        ~PlayersController()
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
            var players = db.Players.ToArray();

            return View(players);
        }


        public ActionResult Player(int id)
        {
            var player = db.Players.Find(id);

            if (player == null)
            {
                return new HttpStatusCodeResult(404);
            }

            return View(player);
        }


        public ActionResult Add()
        {
            if (Session["UserId"] == null)
            {
                return new HttpStatusCodeResult(403);
            }

            return View();
        }


        [HttpPost]
        public ActionResult Add(Players playerData)
        {
            if (ModelState.IsValid)
            {
                bool isTagnameAlreadyInUse = db.Players.Count(p => p.Tagname == playerData.Tagname) > 0;

                if (isTagnameAlreadyInUse)
                {
                    ModelState.AddModelError("Tagname", "This tagname already exists.");
                    return View(playerData);
                }

                Players player = db.Players.Add(playerData);
                db.SaveChanges();

                return RedirectToAction("Player", new { id = player.Id });
            }

            return RedirectToAction("Add");
        }


        public ActionResult Edit(int id)
        {
            if (Session["UserId"] != null)
            {
                var playerToEdit = db.Players.Find(id);

                if (playerToEdit == null)
                {
                    return new HttpStatusCodeResult(404);
                }

                return View(playerToEdit);
            }

            return new HttpStatusCodeResult(403);
        }


        [HttpPost]
        public ActionResult Edit(Players playerData)
        {
            var player = db.Players.Find(playerData.Id);

            player.FirstName = playerData.FirstName;
            player.LastName = playerData.LastName;

            db.Entry(player).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Player", "Players", new { id = player.Id });
        }


        public ActionResult Delete(int id)
        {
            if (Session["UserId"] != null)
            {
                var playerToRemove = db.Players.Find(id);

                if (playerToRemove == null)
                {
                    return new HttpStatusCodeResult(404);
                }

                return View(playerToRemove);
            }

            return new HttpStatusCodeResult(403);
        }


        [HttpPost]
        public ActionResult Delete(Players playerData)
        {
            if (Session["UserId"] != null)
            {
                var playerToRemove = db.Players.Find(playerData.Id);

                if (playerToRemove == null)
                {
                    return new HttpStatusCodeResult(404);
                }

                db.Players.Remove(playerToRemove);
                db.SaveChanges();

                return RedirectToAction("Index", "Players");
            }

            return new HttpStatusCodeResult(403);
        }
    }
}