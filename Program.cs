using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;
using Microsoft.Extensions.Logging;

// Load environment variables from .env file
Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Auth configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts =>
{
    opts.SlidingExpiration = true;
})
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    var cfg = builder.Configuration.GetSection("Authentication:Frontegg");

    // Read from environment variables first, fallback to configuration
    options.Authority = Environment.GetEnvironmentVariable("FRONTEGG_AUTHORITY") ?? cfg["Authority"];
    options.ClientId = Environment.GetEnvironmentVariable("FRONTEGG_CLIENT_ID") ?? cfg["ClientId"];
    options.ClientSecret = Environment.GetEnvironmentVariable("FRONTEGG_CLIENT_SECRET") ?? cfg["ClientSecret"];
    options.CallbackPath = cfg["CallbackPath"];                 // /signin-oidc
    options.SignedOutCallbackPath = cfg["SignedOutCallbackPath"];

    options.ResponseType = "code";
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;

    // Frontegg-specific configuration
    options.RequireHttpsMetadata = true;
    options.UseTokenLifetime = true;
    options.RefreshOnIssuerKeyNotFound = true;
    options.AutomaticRefreshInterval = TimeSpan.FromMinutes(30);

    // Add additional parameters that might be needed
    options.Events = new OpenIdConnectEvents
    {
        OnRedirectToIdentityProvider = context =>
        {
            // Add the appId parameter (different from clientId)
            // This should match the appId in your Frontegg OAuth URL
            var appId = Environment.GetEnvironmentVariable("FRONTEGG_APP_ID");
            if (!string.IsNullOrEmpty(appId))
            {
                context.ProtocolMessage.SetParameter("appId", appId);
            }
            
            // You can also add the clientId as a separate parameter if needed
            context.ProtocolMessage.SetParameter("clientId", Environment.GetEnvironmentVariable("FRONTEGG_CLIENT_ID") ?? cfg["ClientId"]);
            
            // Add tenant ID if you have one
            // context.ProtocolMessage.SetParameter("tenant_id", "your-tenant-id");
            
            return Task.CompletedTask;
        },
        
        OnTokenValidated = context =>
        {
            // Log successful token validation
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Token validated successfully for user: {User}", context.Principal?.Identity?.Name);
            return Task.CompletedTask;
        }
    };

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.Scope.Add("offline_access"); // optional

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = "roles",
        // Use the Client ID as the valid audience
        ValidAudience = Environment.GetEnvironmentVariable("FRONTEGG_CLIENT_ID") ?? cfg["ClientId"],
        // Disable audience validation if you want to accept any audience
        // ValidateAudience = false
    };
});

var app = builder.Build();

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
