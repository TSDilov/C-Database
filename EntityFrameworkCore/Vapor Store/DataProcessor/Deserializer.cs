namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.Dto.Import;
	using System.Xml.Serialization;
    using System.IO;
    using System.Globalization;
    using Microsoft.EntityFrameworkCore;

    public static class Deserializer
	{
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
			var output = new StringBuilder();
			var games = JsonConvert.DeserializeObject<IEnumerable<ImportGameModel>>(jsonString);
            foreach (var game in games)
            {
                if (!IsValid(game) || game.Tags.Count() == 0)
                {
					output.AppendLine("Invalid Data");
					continue;
                }

				var genre = context.Genres.FirstOrDefault(g => g.Name == game.Genre);
                if (genre == null)
                {
					genre = new Genre { Name = game.Genre };
                }

				var developer = context.Developers.FirstOrDefault(d => d.Name == game.Developer);
				if (developer == null)
				{
					developer = new Developer { Name = game.Developer };
				}

				var currentGame = new Game
				{
					Name = game.Name,
					Price = game.Price,
					ReleaseDate = game.ReleaseDate.Value,
					Genre = genre,
					Developer = developer,
				};

                foreach (var tag in game.Tags)
                {
					var currentTag = context.Tags.FirstOrDefault(t => t.Name == tag)
						?? new Tag { Name = tag };
					currentGame.GameTags.Add(new GameTag { Tag = currentTag });
                }

				context.Games.Add(currentGame);
				context.SaveChanges();
				output.AppendLine($"Added {game.Name} ({game.Genre}) with {game.Tags.Count()} tags");

			}
			return output.ToString().TrimEnd();
		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
			var output = new StringBuilder();
			var users = JsonConvert.DeserializeObject<IEnumerable<ImportUserModel>>(jsonString);
            foreach (var user in users)
            {
                if (!IsValid(user))
                {
					output.AppendLine("Invalid Data");
					continue;
				}

				var currentUser = new User
				{
					FullName = user.FullName,
					Username = user.Username,
					Email = user.Email,
					Age = user.Age,
					Cards = user.Cards.Select(c => new Card 
					{
						Number = c.Number,
						Cvc = c.CVC,
						Type = c.Type.Value,
					}).ToList(),
				};

				context.Users.Add(currentUser);
				context.SaveChanges();
				output.AppendLine($"Imported {user.Username} with {user.Cards.Count()} cards"!);
            }

			return output.ToString().TrimEnd();
		}

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
			var output = new StringBuilder();

			var serializer = new XmlSerializer(
                typeof(ImportPurchaseModel[]),
				new XmlRootAttribute("Purchases"));

			var purchases = (ImportPurchaseModel[])serializer.Deserialize(new StringReader(xmlString));

            foreach (var purchase in purchases)
            {
                if (!IsValid(purchase))
                {
					output.AppendLine("Invalid Data");
					continue;
                }

				var parsedDate = DateTime.TryParseExact(purchase.Date, "dd/MM/yyyy HH:mm",
					CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);

                if (!parsedDate)
                {
					output.AppendLine("Invalid Data");
					continue;
				}

				var currentPurchase = new Purchase
				{
					Type = purchase.Type.Value,
					ProductKey = purchase.Key,
					Date = date,
				};

				currentPurchase.Game = context.Games.FirstOrDefault(g => g.Name == purchase.Title);
				currentPurchase.Card = context.Cards.FirstOrDefault(c => c.Number == purchase.Card);
				context.Purchases.Add(currentPurchase);
				context.SaveChanges();

				var username = context.Users
					.Where(u => u.Id == currentPurchase.Card.UserId)
					.Select(u => u.Username).FirstOrDefault();
				output.AppendLine($"Imported {purchase.Title} for {username}");

			}

			return output.ToString().TrimEnd();
		}

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}