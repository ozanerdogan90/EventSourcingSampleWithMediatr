using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects
{
    public class Statistics
    {
        public string Type { get; set; }
        public TeamType Team { get; set; }
        public string ActionBy { get; set; }
        public DateTime ActionAt { get; set; }
    }

    public class GameDetails
    {
        public Guid GameId { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public List<Statistics> Statistics { get; set; }
    }
}
