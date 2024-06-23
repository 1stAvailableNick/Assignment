using System.Text.Json;
using MessagePack;

namespace Assignment.Converters
{
    public class MessagePackConverter : IConverter
    {
        public object Convert(JsonElement value)
        {
            return MessagePackSerializer.ConvertFromJson(
                value.GetRawText(),
                MessagePack.Resolvers.ContractlessStandardResolver.Options);
        }
    }
}
