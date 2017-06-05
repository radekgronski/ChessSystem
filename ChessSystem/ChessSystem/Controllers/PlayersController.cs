﻿using System.Linq;
using System.Web.Mvc;

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
            // select players
            var players = db.Players.ToArray();

            return View(players);
        }


        public ActionResult Player(int id)
        {
            var player = db.Players.Find(id);

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
    }
}