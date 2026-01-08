using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WEB_253551_KORZUN.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string NormalizedName { get; set; } = string.Empty;

        public List<CarPart> CarParts { get; set; } = new List<CarPart>();
    }
}
