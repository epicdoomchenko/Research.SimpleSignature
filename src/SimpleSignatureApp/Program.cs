using System.Text.Json;
using Microsoft.Extensions.Options;
using SimpleSignatureApp.Data;
using SimpleSignatureApp.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Время жизни сессии
    options.Cookie.HttpOnly = true; // Защита от доступа через JavaScript
    options.Cookie.IsEssential = true; // Необходимо для работы сессий
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.Configure<TelegramOptions>(builder.Configuration.GetSection(nameof(TelegramOptions)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.MapRazorPages();
app.UseSession(); // Добавьте эту строку перед app.MapGet

app.MapGet("/api/auth/telegram", (HttpContext context) =>
{
    var query = context.Request.Query; // Извлекаем данные пользователя
    var data = query.ToDictionary(k => k.Key, k => k.Value.ToString());
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

app.Run();
