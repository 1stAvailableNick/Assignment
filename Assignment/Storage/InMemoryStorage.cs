using System.Text.Json;

namespace Assignment.Storage
{
    public class InMemoryStorage : IStorage
    {
        private readonly Dictionary<Guid, JsonElement> _objects;

        public InMemoryStorage()
        {
            _objects = [];
        }

        public JsonElement? Load(Guid id)
        {
            return _objects.TryGetValue(id, out JsonElement o)
                ? o : null;
        }

        public void Save(Guid id, JsonElement o)
        {
            _objects[id] = o;
        }
    }
}
