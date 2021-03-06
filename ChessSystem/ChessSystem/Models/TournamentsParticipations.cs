//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChessSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TournamentsParticipations
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int PlayerId { get; set; }
        public Nullable<int> Place { get; set; }
        public int Won { get; set; }
        public int Lost { get; set; }
        public int Drawn { get; set; }
    
        public virtual Players Players { get; set; }
        public virtual Tournaments Tournaments { get; set; }
    }
}
