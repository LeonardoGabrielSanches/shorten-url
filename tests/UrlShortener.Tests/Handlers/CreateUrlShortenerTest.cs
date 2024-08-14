using Microsoft.AspNetCore.Http;
using Moq;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Handlers;
using UrlShortener.Domain.Repositories;

namespace UrlShortener.Tests.Handlers;

public class CreateUrlShortenerTest
{
    private readonly Mock<IUrlShortenerRepository> _urlShortenerRepositoryMock;
    private readonly CreateUrlShortener _createUrlShortener;

    public CreateUrlShortenerTest()
    {
        _urlShortenerRepositoryMock = new Mock<IUrlShortenerRepository>();
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        httpContextAccessor.Setup(context => context.HttpContext.Request.Host)
            .Returns(new HostString("https://base-path.com/"));

        _createUrlShortener = new CreateUrlShortener(_urlShortenerRepositoryMock.Object, httpContextAccessor.Object);
    }

    [Fact(DisplayName = "It should not create a new shortener url when original url is invalid")]
    public async Task ShouldNot_CreateShortenerUrl_WhenOriginalUrlIsInvalid()
    {
        // Arrange
        var request = new CreateUrlShortener.CreateUrlShortenerRequest(string.Empty);

        // Act
        var response = await _createUrlShortener.Handle(request);

        // Assert
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Contains("Url must not be empty", response.ErrorMessages);
    }

    [Fact(DisplayName = "It should create a new shortener url")]
    public async Task Should_CreateShortenerUrl()
    {
        // Arrange
        var request = new CreateUrlShortener.CreateUrlShortenerRequest("https://example.com");

        _urlShortenerRepositoryMock
            .Setup(repo => repo.CreateShortUrlAsync(It.IsAny<Url>()));

        _urlShortenerRepositoryMock
            .SetupSequence(repo => repo.ExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        // Act
        var response = await _createUrlShortener.Handle(request);

        // Assert
        Assert.NotNull(response);
        Assert.Contains("https://base-path.com/", response.Data?.ShortUrl);

        _urlShortenerRepositoryMock.Verify(
            repository => repository.CreateShortUrlAsync(It.Is<Url>(u => u.OriginalUrl == request.Url)), Times.Once);
    }
}