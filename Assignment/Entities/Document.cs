using System.Text.Json;

namespace Assignment.Entities
{
    public class Document
    {
        public required Guid Id { get; set; }

        public required List<string> Tags { get; set; }

        public required JsonElement Data { get; set; }
    }
}
