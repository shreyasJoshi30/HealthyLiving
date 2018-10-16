using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HL_V1.Models
{
    public class Reservation
    {
        [Key]
        public Guid ReservationID { get; set; }

        [Required]
        
        public string NutritionistID { get; set; }


        [Required]
        [Display(Name = "Reservation Date-Time")]
        public DateTime ReservedDate { get; set; }

        
        public string ReservedBy { get; set; }

        public string Reservation_status { get; set; }


        public string ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }


        [ForeignKey("NutritionistID")]
        [InverseProperty("Reservations_NId")]
        public virtual ApplicationUser User1 { get; set; }

        [ForeignKey("ReservedBy")]
        [InverseProperty("Reservations_UId")]
        public virtual ApplicationUser User2 { get; set; }

    }


}