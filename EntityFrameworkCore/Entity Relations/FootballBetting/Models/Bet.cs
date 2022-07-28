using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballBetting.Models
{
    public class Bet
    {
        public int BetId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int GameId { get; set; }
        public Game Game { get; set; }

        public string Prediction { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
