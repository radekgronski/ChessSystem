using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChessSystem.Models
{
    public class UsersHelper
    {
        public static Users GetOrganizers(int organizerId)
        {
            using (ChessSystemDbEntities db = new ChessSystemDbEntities())
            {
                var users = db.Users.Where(user => user.Id == organizerId).First();
                return users;
            }
        }
    }
}