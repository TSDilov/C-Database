using FootballBetting.Data;
using System;

namespace FootballBetting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new FootballBettingContext();
            context.Database.EnsureCreated();
        }
    }
}
