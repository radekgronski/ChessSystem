using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChessSystem.Models
{
    public class ParticipationsHelper
    {
        public static TournamentsParticipations GetParticipation(int tournamentId, int playerId)
        {
            using (ChessSystemDbEntities db = new ChessSystemDbEntities())
            {
                var participation = db.TournamentsParticipations.Where(
                    p => p.TournamentId == tournamentId && p.PlayerId == playerId
                ).FirstOrDefault();

                return participation;
            }
        }
    }
}