using System.Linq;
using System.Web.Mvc;

using ChessSystem.Models;


namespace ChessSystem.Controllers
{
    public class TournamentsController : Controller
    {
        private ChessSystemDbEntities db = new ChessSystemDbEntities();

        public ActionResult Index()
        {
            // select public tournaments
            var tournaments = db.Tournaments.Where(
                tournament => tournament.IsPublic.HasValue && tournament.IsPublic.Value == true
            ).ToList();

            // add also logged user's non-public tournaments
            if (Session["UserId"] != null)
            {
                int userId = int.Parse(Session["UserId"].ToString());
                var usersTournaments = db.Tournaments.Where(
                    tournament =>
                        tournament.OrganizerId == userId && 
                        (!tournament.IsPublic.HasValue ||
                            (tournament.IsPublic.HasValue && tournament.IsPublic.Value == false)
                        )
                ).ToArray();

                tournaments.AddRange(usersTournaments);
            }

            return View(tournaments.ToArray());
        }

        public ActionResult Tournament(int id)
        {
            var tournament = db.Tournaments.Where(t => t.Id == id).First();

            if (!tournament.IsPublic.HasValue && tournament.IsPublic.Value == false)
            {
                if (Session["UserId"] == null)
                {
                    return new HttpStatusCodeResult(403);
                }

                int userId = int.Parse(Session["UserId"].ToString());

                if (tournament.OrganizerId != userId)
                {
                    return new HttpStatusCodeResult(403);
                }
            }

            return View(tournament);
        }
    }
}