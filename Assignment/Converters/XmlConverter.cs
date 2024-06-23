using Newtonsoft.Json;
using System.Text.Json;
using System.Xml.Linq;

namespace Assignment.Converters
{
    public class XmlConverter : IConverter
    {
        public object Convert(JsonElement value)
        {
            var content = ConvertRecursive(value);
            var xml = new XElement("Root", content);
            return xml.ToString();
        }

        public object ConvertRecursive(JsonElement value)
        {
            if (value.ValueKind == JsonValueKind.Object)
            {
                string jsonString = value.GetRawText();
                if (JsonConvert.DeserializeXNode(jsonString, "Object") is not { } xDoc ||
                    xDoc.Root is null)
                {
                    return "";
                }
                return xDoc.Root;
            }
            // JsonConvert.DeserializeXNode does not work on non objects,
            // but "string", 15 or [0,1] is valid JSON as well,
            // therefore separate branches to handle them.
            else if (value.ValueKind == JsonValueKind.Array)
            {
                // Simulating behavior of JsonConvert.DeserializeXNode.
                // Each element of array must be separate XML element.
                return value
                    .EnumerateArray()
                    .Select(item => 
                        new XElement(
                            "Array", 
                            ConvertRecursive(item)));
            }
            else
            {
                // Works on strings, numbers, true, false, null.
                return value.ToString();
            }
        }
    }
}
