using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballBetting.Models
{
    public class Game
    {
        public Game()
        {
            this.Bets = new HashSet<Bet>();
            this.PlayerStatistic = new HashSet<PlayerStatistics>();
        }
        public int GameId { get; set; }

        [Required]
        public decimal AwayTeamBetRate { get; set; }

        [Required]
        public int AwayTeamGoals { get; set; }
        
        [ForeignKey("Team")]
        public int? AwayTeamId { get; set; }

        [InverseProperty(nameof(Team.AwayGames))]
        public Team AwayTeam { get; set; }

        [Required]
        public decimal DrawBetRate { get; set; }

        [Required]
        public decimal HomeTeamBetRate { get; set; }

        [Required]
        public int HomeTeamGoals { get; set; }

        [ForeignKey("Team")]
        public int? HomeTeamId { get; set; }

        [InverseProperty(nameof(Team.HomeGames))]
        public Team HomeTeam { get; set; }

        [Required]
        public string Result { get; set; }
        
        [Required]
        public DateTime DateTime { get; set; }

        public ICollection<Bet> Bets { get; set; }
        public ICollection<PlayerStatistics> PlayerStatistic { get; set; }
    }
}