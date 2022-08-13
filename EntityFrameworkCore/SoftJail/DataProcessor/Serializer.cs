namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners.ToList()
                .Where(p => ids.Contains(p.Id))
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers.Select(po => new
                    {
                        OfficerName = po.Officer.FullName,
                        Department = po.Officer.Department.Name,
                    }).OrderBy(o => o.OfficerName),
                    TotalOfficerSalary = p.PrisonerOfficers.Sum(po => po.Officer.Salary),
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id);

            return JsonConvert.SerializeObject(prisoners, Formatting.Indented);
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var prisonerNameArray = prisonersNames.Split(',');

            var prisoners = context.Prisoners.ToList()
                .Where(p => prisonerNameArray.Contains(p.FullName))
                .Select(p => new ExportPrisonerModel
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails.Select(m => new ExportMessageModel
                    {
                        Description = string.Join("", m.Description.Reverse()),
                    }).ToArray(),
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            var sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);
            var serializer = new XmlSerializer(typeof(ExportPrisonerModel[]), new XmlRootAttribute("Prisoners"));
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            serializer.Serialize(writer, prisoners, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}