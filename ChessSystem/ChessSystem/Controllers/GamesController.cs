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


        public ActionResult Game(int id)
        {
            var game = db.Games.Find(id);

            if (game == null)
            {
                return new HttpStatusCodeResult(404);
            }

            if (!game.IsPublic)
            {
                if (Session["UserId"] == null)
                {
                    return new HttpStatusCodeResult(403);
                }

                int userId = int.Parse(Session["UserId"].ToString());

                if (game.OrganizerId.HasValue && game.OrganizerId.Value != userId)
                {
                    return new HttpStatusCodeResult(403);
                } 
                else
                {
                    var tournament = db.Tournaments.Find(game.TournamentId);

                    if (tournament == null || tournament.OrganizerId != userId)
                    {
                        return new HttpStatusCodeResult(403);
                    }
                }
            }

            return View(game);
        }


        public ActionResult Index()
        {
            // select public games
            var games = db.Games.Where(game => game.IsPublic).ToList();

            // add also logged user's non-public games
            if (Session["UserId"] != null)
            {
                int userId = int.Parse(Session["UserId"].ToString());

                var userPrivateGames = db.Games.Where(
                    game => game.OrganizerId.HasValue && game.OrganizerId.Value == userId && !game.IsPublic
                ).ToArray();

                games.AddRange(userPrivateGames);

                var userTournamentsIds = db.Tournaments.Where(
                    tournament => tournament.OrganizerId == userId
                ).Select(tournament => tournament.Id);

                var currentGamesIds = games.Select(game => game.Id);

                var userTournamentGames = db.Games.Where(
                    game => game.TournamentId.HasValue &&
                            !currentGamesIds.Contains(game.Id) &&
                            userTournamentsIds.Contains(game.TournamentId.Value)
                ).ToArray();

                games.AddRange(userTournamentGames);
            }

            return View(games.ToArray());
        }

        
        public ActionResult Add(int? id)
        {
            if (Session["UserId"] != null)
            {
                if (id != null)
                {
                    int userId = int.Parse(Session["UserId"].ToString());
                    var tournament = db.Tournaments.Find(id);

                    if (tournament == null || tournament.OrganizerId != userId)
                    {
                        return new HttpStatusCodeResult(403);
                    }

                    Games game = new Games() { TournamentId = id, OrganizerId = null };

                    return View(game);
                }

                return View();
            }

            return new HttpStatusCodeResult(403);
        }


        [HttpPost]
        public ActionResult Add(Games gameData)
        {
            if (ModelState.IsValid)
            {
                if (!gameData.TournamentId.HasValue)
                {
                    int userId = int.Parse(Session["UserId"].ToString());
                    gameData.OrganizerId = userId;
                }

                if (gameData.Player1Id == gameData.Player2Id)
                {
                    ModelState.AddModelError("Player1Id", "You must choose two different players.");
                    ModelState.AddModelError("Player2Id", "You must choose two different players.");

                    return View(gameData);
                }

                Games gameToAdd = new Games()
                {
                    TournamentId = gameData.TournamentId,
                    OrganizerId = gameData.OrganizerId,
                    Player1Id = gameData.Player1Id,
                    Player2Id = gameData.Player2Id,
                    Date = gameData.Date,
                    IsPublic = gameData.IsPublic,
                    WinnerId = null,
                    Moves = 0,
                    Duration = 0,
                    IsFinished = false
                };

                if (gameToAdd.TournamentId.HasValue)
                {
                    var tournament = db.Tournaments.Find(gameToAdd.TournamentId.Value);
                    gameToAdd.IsPublic = tournament.IsPublic;
                }

                Games game = db.Games.Add(gameToAdd);
                db.SaveChanges();

                return RedirectToAction("Game", "Games", new { id = game.Id });
            }

            return View(gameData);
        }
    }
}