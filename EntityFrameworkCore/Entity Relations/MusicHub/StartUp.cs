namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var sb = new StringBuilder();

            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .Select(a => new 
                {
                    a.Name,
                    a.ReleaseDate,
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs.Select(s => new 
                    {
                        SongName = s.Name,
                        Writer = s.Writer.Name,
                        s.Price
                    }),
                    a.Price
                })
                .ToList()
                .OrderByDescending(a => a.Price);

            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine($"-Songs:");
                var price = 0.0m;
                foreach (var song in album.Songs.OrderByDescending(s => s.SongName).ThenBy(s => s.Writer))
                {
                    var number = 1;
                    sb.AppendLine($"---#{number}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Writer: {song.Writer}");
                    sb.AppendLine($"---Price: {song.Price}");
                    number++;
                    price += song.Price;
                }
                sb.AppendLine($"-AlbumPrice: {album.Price:F2}");
                sb.AppendLine("---------------------------------------");
                price = 0.0m;
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var sb = new StringBuilder();

            var songs = context.Songs
                .ToList()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    s.Name,
                    Performer = s.SongPerformers
                        .Select(x => x.Performer.FirstName + " " + x.Performer.LastName)
                        .FirstOrDefault(),
                    Writer = s.Writer.Name,
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c")
                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.Writer)
                .ThenBy(s => s.Performer);

            foreach (var song in songs)
            {
                var number = 1;
                sb.AppendLine($"-Songs #{number++}");
                sb.AppendLine($"---SongName: {song.Name}");
                sb.AppendLine($"---Performer: {song.Performer}");
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
