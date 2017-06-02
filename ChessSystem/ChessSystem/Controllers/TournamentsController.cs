using System.Linq;
using System.Web.Mvc;

using ChessSystem.Models;


namespace ChessSystem.Controllers
{
    public class TournamentsController : Controller
    {
        public ActionResult Index()
        {
            var db = new ChessSystemDbEntities();

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
    }
}