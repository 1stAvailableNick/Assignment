using System.ComponentModel.DataAnnotations;
using Assignment.Converters;
using Assignment.Entities;
using Assignment.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{
    [ApiController]
    [Consumes("application/json; charset=UTF-8")]
    [Route("[controller]")]
    public class DocumentsController : Controller
    {
        public const string XmlCode = "xml";
        public const string MessagePackCode = "messagePack";
        private readonly ICachedStorage _storage;
        private readonly IConverter _xmlConverter;
        private readonly IConverter _messagePackConverter;

        public DocumentsController(
            ICachedStorage storage,
            [FromKeyedServices(XmlCode)] IConverter xmlConverter,
            [FromKeyedServices(MessagePackCode)] IConverter messagePackConverter)
        {
            _storage = storage;
            _xmlConverter = xmlConverter;
            _messagePackConverter = messagePackConverter;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(
            [Required][FromRoute] Guid id, 
            [Required][FromHeader] string documentCode)
        {
            var document = _storage.Load(id);
            if (!document.HasValue)
            {
                return NotFound($"Document {id} was not found");
            }

            return documentCode switch
            {
                XmlCode => Ok(_xmlConverter.Convert(document.Value)),
                MessagePackCode => Ok(_messagePackConverter.Convert(document.Value)),
                _ => BadRequest($"{documentCode} format is not supported")
            };
        }

        [HttpPost]
        public IActionResult Post([Required][FromBody] Document document)
        {
            _storage.Save(document.Id, document.Data);
            return Ok();
        }

        [HttpPut]
        public IActionResult Put([Required][FromBody] Document document)
        {
            _storage.Save(document.Id, document.Data);
            return Ok();
        }
    }
}
