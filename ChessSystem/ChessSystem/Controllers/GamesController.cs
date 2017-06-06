using System.Linq;
using System.Web.Mvc;

using ChessSystem.Models;

namespace ChessSystem.Controllers
{
    public class GamesController : Controller
    {
        private ChessSystemDbEntities db = new ChessSystemDbEntities();

        ~GamesController()
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

        public ActionResult Game(int Id)
        {
            var game = db.Games.Where(t => t.Id == Id).First();

            return View(game);
        }


        public ActionResult Index()
        {
            var games = db.Games.ToArray();

            return View(games);
        }
    }
}