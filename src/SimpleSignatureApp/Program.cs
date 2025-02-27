using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimpleSignature.Application.Commands;
using SimpleSignature.Application.Extensions;
using SimpleSignature.Application.Settings;
using SimpleSignature.Infrastructure.DAL;
using SimpleSignature.Infrastructure.DAL.Extensions;
using SimpleSignature.Infrastructure.FileStore.Extensions;
using SimpleSignature.Infrastructure.Telegram;
using SimpleSignature.Infrastructure.Telegram.Extensions;
using SimpleSignatureApp.Data;
using Telegram.Bot;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Время жизни сессии
    options.Cookie.HttpOnly = true; // Защита от доступа через JavaScript
    options.Cookie.IsEssential = true; // Необходимо для работы сессий
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});
builder.Services.AddServerSideBlazor();

builder.Services
    .AddApplication()
    .AddDal()
    .AddFileStore()
    .AddTelegramServices(builder.Configuration);

builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Expiration = TimeSpan.Zero;
    options.SuppressXFrameOptionsHeader = true;
});

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(x => { x.EnableAnnotations(); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

app.UseAntiforgery();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.UseSession();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.MapGet("/api/auth/telegram", async (HttpContext context, IMediator mediator, CancellationToken cancellationToken) =>
{
    var query = context.Request.Query; // Извлекаем данные пользователя
    var data = query.ToDictionary(k => k.Key, k => k.Value.ToString());


    await mediator.Send(new CreateUser(long.Parse(data["id"]), data["username"]), cancellationToken);
    var telegramUser = new TelegramUser
    {
        Id = data["id"],
        FirstName = data["first_name"],
        LastName = data.ContainsKey("last_name") ? data["last_name"] : null,
        Username = data.ContainsKey("username") ? data["username"] : null,
        PhotoUrl = data.ContainsKey("photo_url") ? data["photo_url"] : null,
        AuthDate = data["auth_date"]
    };

    // Сохраняем данные пользователя в сессии
    context.Session.SetString("TelegramUser", JsonSerializer.Serialize(telegramUser));

    // Редирект на Blazor-страницу /user
    return Results.Redirect("/user");
});

app.MapPost("/api/document",
        async ([FromForm] IFormFile file, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var newDocId = await mediator.Send(new CreateDocument(file.FileName, file.OpenReadStream()),
                cancellationToken);
            return Results.Ok(new { Id = newDocId });
        })
    .WithOpenApi()
    .DisableAntiforgery();

app.MapPost("/api/user/{id:long}/{documentId:guid}", async (
        [FromRoute] long id,
        [FromRoute] Guid documentId,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken) =>
    {
        await mediator.Send(new CreateSigningDocument(documentId, id), cancellationToken);
    })
    .WithOpenApi();

app.MapGet("/bot/setWebhook",
        async ([FromServices] TelegramBotClient bot, [FromServices] IOptions<TelegramOptions> telegramSettings) =>
        {
            var webhookUrl = telegramSettings.Value.WebhookUrl;
            await bot.SetWebhook(webhookUrl.AbsoluteUri);
            return $"Webhook set to {webhookUrl}";
        })
    .WithOpenApi();
app.MapPost("/bot", async (
    [FromBody] Update update,
    [FromServices] TelegramMessageHandler telegramMessageHandler,
    CancellationToken cancellationToken) =>
{
    await telegramMessageHandler.HandleUpdateAsync(update, cancellationToken);
});
app.MapPost("/", async (
    [FromBody] Update update,
    [FromServices] TelegramMessageHandler telegramMessageHandler,
    CancellationToken cancellationToken) =>
{
    await telegramMessageHandler.HandleUpdateAsync(update, cancellationToken);
});

app.Run();