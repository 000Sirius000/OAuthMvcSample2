using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using OAuthMvcSample.Data;
using OAuthMvcSample.Models;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

// =========================================================
// 1. EF + SQLite (äëÿ Identity & OpenIddict)
// =========================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
                        ?? "Data Source=app.db");
    options.UseOpenIddict();
});

// =========================================================
// 2. AppDbContext — 4 ïðîâàéäåðè ÁÄ
// =========================================================
var provider = builder.Configuration["Database:Provider"] ?? "Sqlite";

builder.Services.AddDbContext<AppDbContext>(options =>
{
    switch (provider)
    {
        case "SqlServer":
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
            break;

        case "Postgres":
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
            break;

        case "InMemory":
            options.UseInMemoryDatabase("AppInMemoryDb");
            break;

        default:
            options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
            break;
    }
});

// =========================================================
// 3. Identity
// =========================================================
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(opt =>
    {
        opt.Password.RequiredLength = 8;
        opt.Password.RequireDigit = true;
        opt.Password.RequireNonAlphanumeric = true;
        opt.Password.RequireUppercase = true;
        opt.Password.RequireLowercase = false;
        opt.Lockout.AllowedForNewUsers = false;
        opt.User.RequireUniqueEmail = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Account/Login";
    opt.AccessDeniedPath = "/Account/Login";
});

// =========================================================
// 4. OpenIddict
// =========================================================
builder.Services.AddOpenIddict()
    .AddCore(opt =>
    {
        opt.UseEntityFrameworkCore()
           .UseDbContext<ApplicationDbContext>();
    })
    .AddServer(opt =>
    {
        opt.AllowAuthorizationCodeFlow()
           .RequireProofKeyForCodeExchange();

        opt.SetAuthorizationEndpointUris("/connect/authorize")
           .SetTokenEndpointUris("/connect/token");

        opt.RegisterScopes(Scopes.OpenId, Scopes.Email, Scopes.Profile);

        opt.UseAspNetCore()
           .EnableAuthorizationEndpointPassthrough()
           .EnableTokenEndpointPassthrough()
           .EnableStatusCodePagesIntegration();

        opt.AddEphemeralEncryptionKey()
           .AddEphemeralSigningKey();

        opt.DisableAccessTokenEncryption();
    })
    .AddValidation(opt =>
    {
        opt.UseLocalServer();
        opt.UseAspNetCore();
    });

// =========================================================
// 5. MVC
// =========================================================
builder.Services.AddControllersWithViews();


// =========================================================
// 6. API Versioning + Explorer
// =========================================================
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Swagger — ÎÁÎÂ’ßÇÊÎÂÎ ÄÎ API Versioning Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var provider = builder.Services.BuildServiceProvider()
        .GetRequiredService<Asp.Versioning.ApiExplorer.IApiVersionDescriptionProvider>();

    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerDoc(
            description.GroupName,
            new OpenApiInfo()
            {
                Title = $"API {description.ApiVersion}",
                Version = description.ApiVersion.ToString()
            });
    }
});

// =========================================================
// 7. Build app
// =========================================================
var app = builder.Build();


// =========================================================
// 8. Swagger UI ç ï³äòðèìêîþ API Versioning
// =========================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        var providerApi = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in providerApi.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}


// =========================================================
// 9. Àâòî-ì³ãðàö³¿ + ñèä³íã OpenIddict êë³ºíòà
// =========================================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();

    var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
    const string clientId = "mvc-client";

    if (await manager.FindByClientIdAsync(clientId) is null)
    {
        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            DisplayName = "Demo MVC Client",
            RedirectUris = { new Uri("https://localhost:5001/signin-oidc") },
            PostLogoutRedirectUris = { new Uri("https://localhost:5001/signout-callback-oidc") },
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.ResponseTypes.Code,
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile
            },
            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            }
        });
    }
}


// =========================================================
// 10. Middleware pipeline
// =========================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
