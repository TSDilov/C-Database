using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballBetting.Models
{
    public class PlayerStatistics
    {
        public int PlayertId { get; set; }
        public Player Player { get; set; }

        public int GameId { get; set; }
        public Game Course { get; set; }

        public int Assists { get; set; }

        public int MinutesPlayed { get; set; }
        public int ScoredGoals { get; set; }
    }
}
