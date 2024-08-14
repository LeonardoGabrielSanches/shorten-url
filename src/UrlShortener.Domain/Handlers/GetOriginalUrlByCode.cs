using UrlShortener.Domain.Common;
using UrlShortener.Domain.Repositories;

namespace UrlShortener.Domain.Handlers;

public sealed class GetOriginalUrlByCode(IUrlShortenerRepository urlShortenerRepository)
{
    public record GetOriginalUrlByCodeRequest(string Code);

    public record GetOriginalUrlByCodeResponse(string Url);

    public async Task<Result<GetOriginalUrlByCodeResponse>> Handle(GetOriginalUrlByCodeRequest request)
    {
        var url = await urlShortenerRepository.GetUrlByCodeAsync(request.Code);

        return url is null
            ? new Result<GetOriginalUrlByCodeResponse>(["Url not found."])
            : new Result<GetOriginalUrlByCodeResponse>(new GetOriginalUrlByCodeResponse(url.OriginalUrl));
    }
}