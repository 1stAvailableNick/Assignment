using System.Text.Json;

namespace Assignment.Converters
{
    public interface IConverter
    {
        object Convert(JsonElement value);
    }
}
