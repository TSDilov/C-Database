using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballBetting.Models
{
    public class User
    {
        public User()
        {
            this.Bets = new HashSet<Bet>();
        }
        public int UserId { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        [MaxLength(25)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        public string Password { get; set; }

        [Required]
        [MaxLength(30)]
        public string UserName { get; set; }

        public ICollection<Bet> Bets { get; set; }
    }
}
