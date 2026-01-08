using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253551_KORZUN.Domain.Entities
{
    public class CarPart
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        public string? Image { get; set; }
        public string? MimeType { get; set; }
        public Category? Category { get; set; }
    }
}
