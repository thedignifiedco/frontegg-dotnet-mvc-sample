## ASP.NET Core MVC + Frontegg OIDC Sample

### Prerequisites
- .NET 9 SDK (or higher)
- Frontegg application (Client ID/Secret)

### Configuration

#### Local Development
1) Copy `.env.example` to `.env` and fill in your Frontegg details:
```bash
cp .env.example .env
```

2) Edit `.env` with your actual Frontegg credentials:
```env
FRONTEGG_AUTHORITY="https://your-tenant.frontegg.com"
FRONTEGG_CLIENT_ID="your-client-id-here"
FRONTEGG_CLIENT_SECRET="your-client-secret-here"
```

3) In Frontegg Portal → Authentication → Login Method → Hosted Login:
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
- The application reads Frontegg credentials from environment variables first, with fallback to `appsettings.json` for local development
- The `.env` file is automatically loaded for local development using the `DotNetEnv` package
- **Important**: Never commit your `.env` file to version control as it contains sensitive credentials
