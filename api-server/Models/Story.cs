using System.Collections.Generic;

namespace Sheep.IHeartFiction.ApiServer.Models
{
    public class Story : Entity
    {
        public string? Name { get; set; }
        public IEnumerable<string>? Chapters { get; set; }
    }
}
