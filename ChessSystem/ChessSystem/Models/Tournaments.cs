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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Tournaments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tournaments()
        {
            this.Games = new HashSet<Games>();
            this.TournamentsParticipations = new HashSet<TournamentsParticipations>();
        }
    
        public int Id { get; set; }

        [Required]
        public int OrganizerId { get; set; }

        [Required]
        [DisplayName("Name *")]
        public string Name { get; set; }

        public Nullable<System.DateTime> Date { get; set; }

        public string Place { get; set; }

        [DisplayName("Is public?")]
        public bool IsPublic { get; set; }

        [DisplayName("Is finished?")]
        public bool IsFinished { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Games> Games { get; set; }
        public virtual Users Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TournamentsParticipations> TournamentsParticipations { get; set; }
    }
}
