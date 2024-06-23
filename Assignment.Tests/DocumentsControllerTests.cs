using System.Text.Json;
using Assignment.Controllers;
using Assignment.Converters;
using Assignment.Storage;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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
                _messagePackConverter);
        }

        [Fact]
        public void Get_ReturnsBadRequest_WhenUnknownFormat()
        {
            _storage.Load(Guid.Empty).Returns(new JsonElement());

            var response = _sut.Get(Guid.Empty, "unknown");

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Get_ReturnsXml_WhenAskingForSavedData()
        {
            var content = new JsonElement();
            _storage.Load(Guid.Empty).Returns(content);
            _xmlConverter.Convert(content).Returns("<Root></Root>");

            var response = _sut.Get(Guid.Empty, "xml");

            response
                .Should().BeOfType<OkObjectResult>()
                .Subject.Value.Should().Be("<Root></Root>");
        }
    }
}