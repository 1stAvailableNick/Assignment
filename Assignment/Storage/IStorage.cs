using System.Text.Json;

namespace Assignment.Storage
{
    public interface IStorage
    {
        JsonElement? Load(Guid id);

        void Save(Guid id, JsonElement o);
    }
}
