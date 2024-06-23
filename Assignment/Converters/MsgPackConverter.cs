using System.Text.Json;
using MessagePack;

namespace Assignment.Converters
{
    public class MsgPackConverter : IConverter
    {
        public const string DocumentCode = "application/msgpack";

        public object Convert(JsonElement value)
        {
            return MessagePackSerializer.ConvertFromJson(
                value.GetRawText(),
                MessagePack.Resolvers.ContractlessStandardResolver.Options);
        }
    }
}
