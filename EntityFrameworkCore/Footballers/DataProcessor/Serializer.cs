namespace Footballers.DataProcessor
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;
    using Microsoft.EntityFrameworkCore;
    using Footballers.DataProcessor.ExportDto;
    using System.Text;
    using System.IO;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coaches = context.Coaches
                .Include(c => c.Footballers)
                .ToArray()
                .Where(c => c.Footballers.Count >= 1)
                .Select(c => new ExportCoachModel
                {
                    FootballersCount = c.Footballers.Count,
                    CoachName = c.Name,
                    Footballers = c.Footballers.Select(f => new ExportFootballerModel
                    {
                        Name = f.Name,
                        Position = f.PositionType.ToString(),
                    }).OrderBy(f => f.Name).ToArray()
                })
                .OrderByDescending(c => c.FootballersCount)
                .ThenBy(c => c.CoachName)
                .ToArray();

            var sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);
            var serializer = new XmlSerializer(typeof(ExportCoachModel[]), new XmlRootAttribute("Coaches"));
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            serializer.Serialize(writer, coaches, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .ToArray()
                .Where(t => t.TeamsFootballers.Any(tf => tf.Footballer.ContractStartDate >= date))
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                    .Where(tf => tf.Footballer.ContractStartDate >= date)
                    .OrderByDescending(tf => tf.Footballer.ContractEndDate)
                    .ThenBy(tf => tf.Footballer.Name)
                    .Select(tf => new
                    {
                        FootballerName = tf.Footballer.Name,
                        ContractStartDate = tf.Footballer.ContractStartDate.ToString("d", provider: CultureInfo.InvariantCulture),
                        ContractEndDate = tf.Footballer.ContractEndDate.ToString("d", provider: CultureInfo.InvariantCulture),
                        BestSkillType = tf.Footballer.BestSkillType.ToString(),
                        PositionType = tf.Footballer.PositionType.ToString(),
                    })
                    .ToArray()
                })
                .OrderByDescending(t => t.Footballers.Count())
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
    }
}
