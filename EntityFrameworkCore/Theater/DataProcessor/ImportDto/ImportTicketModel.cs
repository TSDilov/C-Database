using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTicketModel
    {
        [Required]
        [Range(typeof(decimal), "1.00", "100.00")]
        public decimal Price { get; set; }

        [Required]
        [Range(typeof(sbyte), "1", "10")]
        public sbyte RowNumber { get; set; }

        [Required]
        public int PlayId { get; set; }
    }
}
