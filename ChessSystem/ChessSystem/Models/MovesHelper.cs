using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChessSystem.Models
{
    public class MovesHelper
    {
        public static Moves[] GamesMoves(int gameId)
        {
            using (ChessSystemDbEntities db = new ChessSystemDbEntities())
            {
                var moves = db.Moves.Where(t => t.GameId == gameId).ToArray();
                return moves;
            }
        }
    }
}