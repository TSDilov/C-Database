namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var xmlSerializer = new XmlSerializer(typeof(ImportPlayModel[]), new XmlRootAttribute("Plays"));
            var plays = (ImportPlayModel[])xmlSerializer.Deserialize(new StringReader(xmlString));

            foreach (var play in plays)
            {
                if (!IsValid(play))
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                object genreObj;
                bool isGenreValid = Enum.TryParse(typeof(Genre), play.Genre, out genreObj);

                if (!isGenreValid)
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                TimeSpan duration;
                bool isDurationValid = TimeSpan.TryParseExact(play.Duration, "c",
                    CultureInfo.InvariantCulture, TimeSpanStyles.None, out duration);

                if (!isDurationValid || duration.Hours < 1)
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                var currentPlay = new Play
                {
                    Title = play.Title,
                    Duration = duration,
                    Rating = play.Rating,
                    Genre = (Genre)genreObj,
                    Description = play.Description,
                    Screenwriter = play.Screenwriter,
                };

                context.Plays.Add(currentPlay);
                context.SaveChanges();
                sb.AppendLine($"Successfully imported {play.Title} with genre {play.Genre} and a rating of {play.Rating}!");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var xmlSerializer = new XmlSerializer(typeof(ImportCastModel[]), new XmlRootAttribute("Casts"));
            var casts = (ImportCastModel[])xmlSerializer.Deserialize(new StringReader(xmlString));

            foreach (var cast in casts)
            {
                if (!IsValid(cast))
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                var currentCast = new Cast
                {
                    FullName = cast.FullName,
                    IsMainCharacter = cast.IsMainCharacter,
                    PhoneNumber = cast.PhoneNumber,
                    PlayId = cast.PlayId,
                };

                context.Casts.Add(currentCast);
                context.SaveChanges();
                sb.AppendLine(cast.IsMainCharacter ? 
                    $"Successfully imported actor {cast.FullName} as a main character!" 
                    : $"Successfully imported actor {cast.FullName} as a lesser character!");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var theaters = JsonConvert.DeserializeObject<ImportTheaterModel[]>(jsonString);
            foreach (var theater in theaters)
            {
                if (!IsValid(theater))
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                var currentTheater = new Theatre
                {
                    Name = theater.Name,
                    NumberOfHalls = theater.NumberOfHalls,
                    Director = theater.Director,
                };

                foreach (var ticket in theater.Tickets)
                {
                    if (!IsValid(ticket))
                    {
                        sb.AppendLine("Invalid data!");
                        continue;
                    }

                    currentTheater.Tickets.Add(new Ticket
                    {
                        Price = ticket.Price,
                        RowNumber = ticket.RowNumber,
                        PlayId = ticket.PlayId,
                        Theatre = currentTheater,
                    });
                }

                context.Theatres.Add(currentTheater);
                context.SaveChanges();
                sb.AppendLine($"Successfully imported theatre {currentTheater.Name} with #{currentTheater.Tickets.Count} tickets!");
            }

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
