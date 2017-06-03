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
            using (ChessSystemDbEntities db = new ChessSystemDbEntities())
            {
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

        public static int GetPlayersPlace(Tournaments tournament, Players player)
        {
            using (ChessSystemDbEntities db = new ChessSystemDbEntities())
            {
                var playerParticipation = db.TournamentsParticipations.Where(
                    participation => participation.TournamentId == tournament.Id && participation.PlayerId == player.Id
                ).First();

                if (!playerParticipation.Place.HasValue)
                {
                    return 0;
                }

                return playerParticipation.Place.Value;
            }
        }
    }
}