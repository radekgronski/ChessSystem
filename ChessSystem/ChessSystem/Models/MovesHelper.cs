using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChessSystem.Models
{
    public class MovesHelper
    {
        public static Moves[] GetMoves(Games game)
        {
            using (ChessSystemDbEntities db = new ChessSystemDbEntities())
            {
                var moves = db.Moves.Where(move => move.GameId == game.Id).ToArray();

                return moves;
            }
        }
    }
}