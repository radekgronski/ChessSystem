using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ChessSystem.Models
{
    public class TournamentNameHelper
    {
        public static Tournaments GetTournamentName(int tournamentId)
        {
            using (ChessSystemDbEntities db = new ChessSystemDbEntities())
            {
                return db.Tournaments.Where(tournament => tournament.Id == tournamentId).First();
            }
        }
    }
}
