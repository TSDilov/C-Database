using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballBetting.Models
{
    public class Player
    {
        public Player()
        {
            this.PlayerStatistics = new HashSet<PlayerStatistics>();
        }
        public int PlayerId { get; set; }

        public bool IsInjured { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int PositionId { get; set; }
        public Position Position { get; set; }

        [Required]
        public int SquadNumber { get; set; }

        public int TeamId { get; set; }

        public Team Team { get; set; }

        public ICollection<PlayerStatistics> PlayerStatistics { get; set; }
    }
}
