namespace Theatre.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            return "TODO";
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            var plays = context.Plays.ToArray()
                .Where(p => p.Rating <= rating)
                .Select(p => new ExportPlayModel
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c", CultureInfo.InvariantCulture),
                    Rating = p.Rating != 0 ? p.Rating.ToString() : "Premier",
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts.Where(c => c.IsMainCharacter).Select(c => new ExportActorModel
                    {
                        FullName = c.FullName,
                        MainCharacter = $"Plays main character in '{p.Title}'.",
                    })
                    .OrderByDescending(a => a.FullName)
                    .ToArray(),
                })
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToArray();

            var sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);
            var serializer = new XmlSerializer(typeof(ExportPlayModel[]), new XmlRootAttribute("Plays"));
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            serializer.Serialize(writer, plays, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
