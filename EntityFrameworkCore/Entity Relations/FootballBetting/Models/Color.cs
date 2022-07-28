using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballBetting.Models
{
    public class Color
    {
        public Color()
        {
            this.HomeTeam = new HashSet<Team>();
            this.AwayTeam = new HashSet<Team>();
        }
        public int ColorId { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Team> HomeTeam { get; set; }

        public ICollection<Team> AwayTeam { get; set; }
    }
}
