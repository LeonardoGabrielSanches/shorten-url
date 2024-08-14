using Moq;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Handlers;
using UrlShortener.Domain.Repositories;

namespace UrlShortener.Tests.Handlers;

public class GetOriginalUrlByCodeTest
{
    private readonly Mock<IUrlShortenerRepository> _urlShortenerRepositoryMock;
    private readonly GetOriginalUrlByCode _getOriginalUrlByCode;

    public GetOriginalUrlByCodeTest()
    {
        _urlShortenerRepositoryMock = new Mock<IUrlShortenerRepository>();

        _getOriginalUrlByCode = new GetOriginalUrlByCode(_urlShortenerRepositoryMock.Object);
    }

    [Fact(DisplayName = "It should return the original url")]
    public async Task Should_ReturnOriginalUrl()
    {
        _urlShortenerRepositoryMock.Setup(x => x.GetUrlByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(new Url("https://example.com", "Code"));

        var request = new GetOriginalUrlByCode.GetOriginalUrlByCodeRequest("Code");

        var response = await _getOriginalUrlByCode.Handle(request);

        Assert.NotNull(response);
        Assert.Equal("https://example.com", response.Data?.Url);
    }

    [Fact(DisplayName = "It should not return the original url when code is not found")]
    public async Task ShouldNot_ReturnOriginalUrl_WhenCodeIsNotFound()
    {
        var request = new GetOriginalUrlByCode.GetOriginalUrlByCodeRequest("Code");

        var response = await _getOriginalUrlByCode.Handle(request);

        Assert.NotNull(response);
        Assert.False(response.Success);
    }
}