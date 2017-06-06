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
                return db.Players.Where(player => player.Id == playerId).First();
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
    }
}