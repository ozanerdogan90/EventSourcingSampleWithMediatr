using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventSourcingSampleWithCQRSandMediatr.Persistence.Entities
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid HomeTeamId { get; set; }
        public Guid AwayTeamId { get; set; }
        public string StadiumName { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.Now;
        public DateTime EndedAt { get; set; } = DateTime.Now;
    }
}
