using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChessSystem.Models
{
    public class TournamentsHelper
    {
        public static Players[] GetTournamentPlayers(Tournaments tournament)
        {
            ChessSystemDbEntities db = new ChessSystemDbEntities();

            var participations = tournament.TournamentsParticipations.Where(
                participation => participation.TournamentId == tournament.Id
            );
            var players = participations.Select(
                participation => db.Players.Where(
                    player => player.Id == participation.PlayerId
                ).First()
            ).ToArray();

            return players;
        }
    }
}