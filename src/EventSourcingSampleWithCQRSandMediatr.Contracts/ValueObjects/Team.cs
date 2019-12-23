using System;
using System.Collections.Generic;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects
{
    public class Team
    {
        public Guid Id { get; set; }
        public TeamType TeamType { get; set; }
        public string Name { get; set; }
        public List<Player> Players { get; set; }
        public string CoachName { get; set; }

    }
}
