using FluentValidation;
using Microsoft.AspNetCore.Http;
using UrlShortener.Domain.Common;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Repositories;
using UrlShortener.Domain.Utils;

namespace UrlShortener.Domain.Handlers;

public sealed class CreateUrlShortener(
    IUrlShortenerRepository urlShortenerRepository,
    IHttpContextAccessor httpContextAccessor)
{
    private class CreateUrlShortenerRequestValidator : AbstractValidator<CreateUrlShortenerRequest>
    {
        public CreateUrlShortenerRequestValidator()
        {
            RuleFor(x => x.Url).NotEmpty().WithMessage("Url must not be empty.");
        }
    }

    public record CreateUrlShortenerRequest(string Url);

    public record CreateUrlShortenerResponse(string Url);

    public async Task<Result<CreateUrlShortenerResponse>> Handle(CreateUrlShortenerRequest request)
    {
        var validationResult = await new CreateUrlShortenerRequestValidator().ValidateAsync(request);

        if (!validationResult.IsValid)
            return new Result<CreateUrlShortenerResponse>(validationResult.Errors.Select(x => x.ErrorMessage).ToList());

        var code = CodeGenerator.GenerateRandomCode(Random.Shared.Next(5, 10));

        while (await urlShortenerRepository.ExistsByCodeAsync(code))
            code = CodeGenerator.GenerateRandomCode(Random.Shared.Next(5, 10));

        var url = new Url(request.Url, code);

        await urlShortenerRepository.CreateShortUrlAsync(url);

        var shortUrl = $"https://{httpContextAccessor.HttpContext.Request.Host}/{url.Code}";

        return new Result<CreateUrlShortenerResponse>(new CreateUrlShortenerResponse(Url: shortUrl));
    }
}