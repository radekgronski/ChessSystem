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
                    participation => db.Players.Find(participation.PlayerId)
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

        public static Players[] GetTournamentPossiblePlayers(int tournamentId)
        {
            using (ChessSystemDbEntities db = new ChessSystemDbEntities())
            {
                var currentParticipations = db.TournamentsParticipations.Where(participation => participation.TournamentId == tournamentId);
                var currentPlayersIds = currentParticipations.Select(participation => participation.PlayerId);
                var possiblePlayers = db.Players.Where(player => !currentPlayersIds.Contains(player.Id));

                return possiblePlayers.ToArray();
            }
        }

        public static Players[] GetTournamentPlayers(int tournamentId)
        {
            using (ChessSystemDbEntities db = new ChessSystemDbEntities())
            {
                var currentParticipations = db.TournamentsParticipations.Where(participation => participation.TournamentId == tournamentId);
                var currentPlayersIds = currentParticipations.Select(participation => participation.PlayerId);
                var players = db.Players.Where(player => currentPlayersIds.Contains(player.Id));

                return players.ToArray();
            }
        }
    }
}