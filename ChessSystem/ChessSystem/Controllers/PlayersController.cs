using System.Linq;
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
    }
}