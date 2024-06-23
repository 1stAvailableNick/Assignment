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
        private readonly ICachedStorage _storage;
        private readonly IConverter _xmlConverter;
        private readonly IConverter _msgpackConverter;

        public DocumentsController(
            ICachedStorage storage,
            [FromKeyedServices(XmlConverter.DocumentCode)] IConverter xmlConverter,
            [FromKeyedServices(MsgPackConverter.DocumentCode)] IConverter msgpackConverter)
        {
            _storage = storage;
            _xmlConverter = xmlConverter;
            _msgpackConverter = msgpackConverter;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([Required][FromRoute] Guid id)
        {
            var document = _storage.Load(id);
            if (!document.HasValue)
            {
                return NotFound($"Document {id} was not found");
            }
            
            if (Request.Headers.Accept.Contains(XmlConverter.DocumentCode))
            {
                return Ok(_xmlConverter.Convert(document.Value));
            }
            else if (Request.Headers.Accept.Contains(MsgPackConverter.DocumentCode))
            {

                return Ok(_msgpackConverter.Convert(document.Value));
            }
            else
            {
                return BadRequest($"None from specified formats is supported");
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
