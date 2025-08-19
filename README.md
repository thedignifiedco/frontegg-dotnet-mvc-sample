## ASP.NET Core MVC + Frontegg OIDC Sample

### Prerequisites
- .NET 9 SDK (or higher)
- Frontegg application (Client ID/Secret)

### Configuration
1) Copy `appsettings.json` and fill in your Frontegg details:
```json
{
  "Authentication": {
    "Frontegg": {
      "Authority": "https://YOUR-SUBDOMAIN.frontegg.com",
      "ClientId": "YOUR_CLIENT_ID",
      "ClientSecret": "YOUR_CLIENT_SECRET",
      "CallbackPath": "/signin-oidc",
      "SignedOutCallbackPath": "/signout-callback-oidc"
    }
  }
}
```

2) In Frontegg Portal → Authentication → Login Method → Hosted Login:
- Allowed Redirect URIs: add the ones you will use locally, e.g.
  - `http://localhost:5057/signin-oidc`
- Post-logout Redirect URIs:
  - `http://localhost:5057/signout-callback-oidc`

### Run locally
```bash
dotnet restore
dotnet run --urls http://localhost:5057
```
Then open `http://localhost:5057` and click Sign in.

Alternatively, to use HTTPS on the default ports from `Properties/launchSettings.json`:
```bash
dotnet run
```
Then open `https://localhost:5001`.

### Notes
- Target framework: `net9.0`
- Packages: `Microsoft.AspNetCore.Authentication.OpenIdConnect` (9.0.x), `Microsoft.IdentityModel.Tokens` (8.0.x)
