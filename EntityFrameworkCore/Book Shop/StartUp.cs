namespace BookShop
{
    using Data;
    using Initializer;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            var command = Console.ReadLine().ToLower();
            Console.WriteLine(GetBooksByAgeRestriction(db, command)); 
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var sb = new StringBuilder();

            if (command == "minor")
            {
                var books = context.Books.Where(b => b.AgeRestriction == 0).OrderBy(b => b.Title).ToList();
                foreach (var book in books)
                {
                    sb.AppendLine($"{book.Title}");
                }
            }
            else if (command == "teen")
            {
                var books = context.Books.Where(b => b.AgeRestriction.Equals(1)).OrderBy(b => b.Title).ToList();
                foreach (var book in books)
                {
                    sb.AppendLine($"{book.Title}");
                }
            }
            else
            {
                var books = context.Books.Where(b => b.AgeRestriction.Equals(2)).OrderBy(b => b.Title).ToList();
                foreach (var book in books)
                {
                    sb.AppendLine($"{book.Title}");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
