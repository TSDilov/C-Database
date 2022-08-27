namespace Footballers.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ImportCoachModel[]), new XmlRootAttribute("Coaches"));

            var coaches = (ImportCoachModel[])serializer.Deserialize(new StringReader(xmlString));

            foreach (var coach in coaches)
            {
                if (!IsValid(coach))
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                var currentCoach = new Coach
                {
                    Name = coach.Name,
                    Nationality = coach.Nationality,
                };

                foreach (var footballer in coach.Footballers)
                {
                    if (!IsValid(footballer))
                    {
                        sb.AppendLine("Invalid data!");
                        continue;
                    }

                    object skillTypeObj;
                    bool isSkillnValid = Enum.TryParse(typeof(BestSkillType), footballer.BestSkillType, out skillTypeObj);

                    if (!isSkillnValid)
                    {
                        sb.AppendLine("Invalid data!");
                        continue;
                    }

                    object positionTypeObj;
                    bool isPositionValid = Enum.TryParse(typeof(PositionType), footballer.PositionType, out positionTypeObj);

                    if (!isPositionValid)
                    {
                        sb.AppendLine("Invalid data!");
                        continue;
                    }

                    DateTime contractStartDate;
                    bool iscontractStartDateValid = DateTime.TryParseExact(footballer.ContractStartDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out contractStartDate);

                    DateTime contractEndDate;
                    bool iscontractEndDateValid = DateTime.TryParseExact(footballer.ContractEndDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out contractEndDate);

                    if (contractStartDate > contractEndDate)
                    {
                        sb.AppendLine("Invalid data!");
                        continue;
                    }

                    currentCoach.Footballers.Add(new Footballer
                    {
                        Name = footballer.Name,
                        ContractStartDate = contractStartDate,
                        ContractEndDate = contractEndDate,
                        BestSkillType = (BestSkillType)skillTypeObj,
                        PositionType = (PositionType)positionTypeObj,
                        Coach = currentCoach,
                    });
                }

                context.Coaches.Add(currentCoach);
                context.SaveChanges();
                sb.AppendLine($"Successfully imported coach - {currentCoach.Name} with {currentCoach.Footballers.Count} footballers.");
            }

            return sb.ToString().TrimEnd();
        }
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var teams = JsonConvert.DeserializeObject<ImportTeamModel[]>(jsonString);

            foreach (var team in teams)
            {
                if (!IsValid(team) || team.Trophies <= 0)
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                var currentTeam = new Team 
                {
                    Name = team.Name,
                    Nationality = team.Nationality,
                    Trophies = team.Trophies
                };

                foreach (var footballerId in team.Footballers.Distinct())
                {
                    var currentFootballer = context.Footballers.FirstOrDefault(f => f.Id == footballerId);
                    if (currentFootballer == null)
                    {
                        sb.AppendLine("Invalid data!");
                        continue;
                    }

                    currentTeam.TeamsFootballers.Add(new TeamFootballer
                    {
                        FootballerId = footballerId,
                        Team = currentTeam,
                    });
                }

                context.Teams.Add(currentTeam);
                context.SaveChanges();
                sb.AppendLine($"Successfully imported team - {currentTeam.Name} with {currentTeam.TeamsFootballers.Count} footballers.");
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
