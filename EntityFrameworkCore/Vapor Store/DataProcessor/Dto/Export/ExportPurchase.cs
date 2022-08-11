using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("Purchase")]
    public class ExportPurchase
    {
        public string Card { get; set; }
        public string Cvc { get; set; }
        public string Date { get; set; }
        public ExportGame Game { get; set; }
    }
}
