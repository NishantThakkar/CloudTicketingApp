using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingAppCloud.DBModels
{
    [Table("Ticket")]
    public class Ticket
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(64)]
        public string Summary { get; set; }
        public string Details { get; set; }
        [Required]
        [StringLength(16)]
        public string Priority { get; set; }
        [Required]
        [StringLength(16)]
        public string Status { get; set; }
        public string Notes { get; set; }
        [Required]
        [StringLength(16)]
        public string AssignedTo { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
