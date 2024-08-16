using UrlShortener.Api.Settings;
using UrlShortener.Domain.Handlers;
using UrlShortener.Domain.Repositories;
using UrlShortener.Infra.Data.MongoDb.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUrlShortenerRepository, MongoDbUrlRepository>();
builder.Services.AddScoped<CreateUrlShortener>();
builder.Services.AddScoped<GetOriginalUrlByCode>();

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/shorten-url",
        async (CreateUrlShortener.CreateUrlShortenerRequest request, CreateUrlShortener createUrlShortener) =>
        {
            var result = await createUrlShortener.Handle(request);

            return result.Success
                ? Results.Ok(result.Data)
                : Results.BadRequest(new { errors = result.ErrorMessages.ToList() });
        })
    .WithName("UrlShortener")
    .WithOpenApi();

app.MapGet("/{code}",
        async (string code, GetOriginalUrlByCode getOriginalUrlByCode) =>
        {
            var result = await getOriginalUrlByCode.Handle(new GetOriginalUrlByCode.GetOriginalUrlByCodeRequest(code));

            return result.Success
                ? Results.Redirect(result.Data?.Url ?? string.Empty)
                : Results.NotFound();
        })
    .WithName("UrlShortenerByCode")
    .WithOpenApi();

app.Run();