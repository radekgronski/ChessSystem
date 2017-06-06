using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;


namespace ChessSystem.Models
{
    public class GamesHelper
    {
        public static Players GetPlayer1(Games game)
        {
            return GetPlayer(game.Player1Id);
        }

        public static Players GetPlayer2(Games game)
        {
            return GetPlayer(game.Player2Id);
        }

        public static Players GetWinner(Games game)
        {
            if (game.WinnerId.HasValue)
            {
                return GetPlayer(game.WinnerId.Value);
            }

            return null;
        }

        private static Players GetPlayer(int playerId)
        {
            using (ChessSystemDbEntities db = new ChessSystemDbEntities())
            {
                return db.Players.Find(playerId);
            }
        }

        public static Games[] GetPlayersGames(int playerId)
        {
            using (ChessSystemDbEntities db = new ChessSystemDbEntities())
            {
                var games = db.Games.Where(game => game.Player1Id == playerId || game.Player2Id == playerId).ToArray();
                
                return games;
            }
        }

        public static Tournaments GetTournament(Games game)
        {
            using (ChessSystemDbEntities db = new ChessSystemDbEntities())
            {
                if (game.TournamentId.HasValue)
                {
                    var tournament = db.Tournaments.Find(game.TournamentId.Value);
                    return tournament;
                }

                return null;
            }
        }

        public static Users GetOrganizer(Games game)
        {
            using (ChessSystemDbEntities db = new ChessSystemDbEntities())
            {
                if (game.OrganizerId.HasValue)
                {
                    var organizer = db.Users.Find(game.OrganizerId.Value);
                    return organizer;
                }

                return null;
            }
        }
    }
}