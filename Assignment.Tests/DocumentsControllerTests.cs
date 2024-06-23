using System.Text.Json;
using Assignment.Controllers;
using Assignment.Converters;
using Assignment.Storage;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NSubstitute;

namespace Assignment.Tests
{
    public class DocumentsControllerTests
    {
        private readonly DocumentsController _sut;
        private readonly IConverter _xmlConverter;
        private readonly IConverter _messagePackConverter;
        private readonly ICachedStorage _storage;

        public DocumentsControllerTests()
        {
            _xmlConverter = Substitute.For<IConverter>();
            _messagePackConverter = Substitute.For<IConverter>();
            _storage = Substitute.For<ICachedStorage>();
            _sut = new DocumentsController(
                _storage,
                _xmlConverter,
                _messagePackConverter)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        public void Get_ReturnsBadRequest_WhenUnknownFormat()
        {
            _storage.Load(Guid.Empty).Returns(new JsonElement());

            var response = _sut.Get(Guid.Empty);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Get_ReturnsXml_WhenAskingForSavedData()
        {
            var content = new JsonElement();
            _storage.Load(Guid.Empty).Returns(content);
            _xmlConverter.Convert(content).Returns("<Root></Root>");

            _sut.Request.Headers.Accept = new StringValues("application/xml");
            var response = _sut.Get(Guid.Empty);

            response
                .Should().BeOfType<OkObjectResult>()
                .Subject.Value.Should().Be("<Root></Root>");
        }
    }
}