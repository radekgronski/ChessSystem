using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

using ChessSystem.Models;


namespace ChessSystem.Controllers
{
    public class TournamentsController : Controller
    {
        private ChessSystemDbEntities db = new ChessSystemDbEntities();


        ~TournamentsController()
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
            // select public tournaments
            var tournaments = db.Tournaments.Where(tournament => tournament.IsPublic == true).ToList();

            // add also logged user's non-public tournaments
            if (Session["UserId"] != null)
            {
                int userId = int.Parse(Session["UserId"].ToString());
                var usersTournaments = db.Tournaments.Where(
                    tournament => tournament.OrganizerId == userId && tournament.IsPublic == false
                ).ToArray();

                tournaments.AddRange(usersTournaments);
            }

            return View(tournaments.ToArray());
        }


        public ActionResult Tournament(int id)
        {
            var tournament = db.Tournaments.Find(id);

            if (tournament == null)
            {
                return new HttpStatusCodeResult(404);
            }

            if (tournament.IsPublic == false)
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

        
        public ActionResult Add()
        {
            if (Session["UserId"] == null)
            {
                return new HttpStatusCodeResult(403);
            }

            return View();
        }


        [HttpPost]
        public ActionResult Add(Tournaments tournamentData)
        {
            if (ModelState.IsValid)
            {
                int userId = int.Parse(Session["UserId"].ToString());

                tournamentData.OrganizerId = userId;
                tournamentData.Users = db.Users.Find(userId);

                Tournaments tournament = db.Tournaments.Add(tournamentData);
                db.SaveChanges();

                return RedirectToAction("Tournament", new { id = tournament.Id });
            }

            return RedirectToAction("Add");
        }


        public ActionResult Edit(int id)
        {
            if (Session["UserId"] != null)
            {
                int userId = int.Parse(Session["UserId"].ToString());
                var tournamentToEdit = db.Tournaments.Find(id);

                if (tournamentToEdit == null || tournamentToEdit.OrganizerId != userId)
                {
                    return new HttpStatusCodeResult(403);
                }

                return View(tournamentToEdit);
            }

            return new HttpStatusCodeResult(403);
        }


        [HttpPost]
        public ActionResult Edit(Tournaments tournamentData)
        {
            var tournament = db.Tournaments.Find(tournamentData.Id);

            tournament.Name = tournamentData.Name;
            tournament.Date = tournamentData.Date;
            tournament.Place = tournamentData.Place;
            tournament.IsFinished = tournamentData.IsFinished;
            tournament.IsPublic = tournamentData.IsPublic;

            db.Entry(tournament).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Tournament", "Tournaments", new { id = tournamentData.Id });
        }


        public ActionResult Delete(int id)
        {
            if (Session["UserId"] != null)
            {
                int userId = int.Parse(Session["UserId"].ToString());
                var tournamentToRemove = db.Tournaments.Find(id);

                if (tournamentToRemove == null || tournamentToRemove.OrganizerId != userId)
                {
                    return new HttpStatusCodeResult(403);
                }

                return View(tournamentToRemove);
            }

            return new HttpStatusCodeResult(403);
        }


        [HttpPost]
        public ActionResult Delete(Tournaments tournamentData)
        {
            if (Session["UserId"] != null)
            {
                int userId = int.Parse(Session["UserId"].ToString());
                var tournamentToRemove = db.Tournaments.Find(tournamentData.Id);

                if (tournamentData == null || tournamentToRemove.OrganizerId != userId)
                {
                    return new HttpStatusCodeResult(403);
                }

                db.Tournaments.Remove(tournamentToRemove);
                db.SaveChanges();

                return RedirectToAction("Index", "Tournaments");
            }

            return new HttpStatusCodeResult(403);
        }


        public ActionResult AddParticipation(int id)
        {
            if (Session["UserId"] != null)
            {
                int userId = int.Parse(Session["UserId"].ToString());
                var tournament = db.Tournaments.Find(id);

                if (tournament == null || tournament.OrganizerId != userId)
                {
                    return new HttpStatusCodeResult(403);
                }

                var participation = new TournamentsParticipations()
                {
                    TournamentId = id,
                    Tournaments = tournament
                };

                return View(participation);
            }

            return new HttpStatusCodeResult(403);
        }


        [HttpPost]
        public ActionResult AddParticipation(TournamentsParticipations participationData)
        {
            if (Session["UserId"] != null)
            {
                int userId = int.Parse(Session["UserId"].ToString());
                var tournament = db.Tournaments.Find(participationData.TournamentId);
                var player = db.Players.Find(participationData.PlayerId);

                if (tournament == null || tournament.OrganizerId != userId || player == null)
                {
                    return new HttpStatusCodeResult(403);
                }

                var participationToInsert = new TournamentsParticipations()
                {
                    TournamentId = participationData.TournamentId,
                    PlayerId = participationData.PlayerId
                };

                var participation = db.TournamentsParticipations.Add(participationToInsert);
                db.SaveChanges();

                return RedirectToAction("Tournament", "Tournaments", new { id = participation.TournamentId });
            }

            return new HttpStatusCodeResult(403);
        }
    }
}