using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballBetting.Models
{
    public class Team
    {
        public Team()
        {
            this.Players = new HashSet<Player>();
            this.HomeGames = new HashSet<Game>();
            this.AwayGames = new HashSet<Game>();
        }

        public int TeamId { get; set; }

        public decimal Budget { get; set; }

        public string Initials { get; set; }

        [Column(TypeName = "varchar(2048)")]
        public string LogoUrl { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("Color")]
        public int? PrimaryKitColorId { get; set; }

        [InverseProperty(nameof(Color.HomeTeam))]
        public Color PrimaryKitColor { get; set; }

        [ForeignKey("Color")]       
        public int? SecondaryKitColorId { get; set; }

        [InverseProperty(nameof(Color.AwayTeam))]
        public Color SecondaryKitColor { get; set; }

        [Required]
        public int TownId { get; set; }
        public Town Town { get; set; }

        public ICollection<Player> Players { get; set; }
        public ICollection<Game> HomeGames { get; set; }
        public ICollection<Game> AwayGames { get; set; }
    }
}