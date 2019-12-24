using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventSourcingSampleWithCQRSandMediatr.Persistence.Entities
{
    public class Faul
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("GameId")]
        public Guid GameId { get; set; }
        public Game Game { get; set; }
        public TeamType Team { get; set; }
        public int PlayerNumber { get; set; }
        public DateTime FauledAt { get; set; } = DateTime.UtcNow;
    }
}
