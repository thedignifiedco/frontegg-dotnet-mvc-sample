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
FRONTEGG_APP_ID="your-app-id-here"
```

**Important**: The `FRONTEGG_APP_ID` is different from the `FRONTEGG_CLIENT_ID`. You can find your clientId in the [Keys & Domains](https://developers.frontegg.com/api/overview#performing-your-first-api-call) of the Frontegg Portal. Your appId can be found in the [Applications](https://developers.frontegg.com/guides/management/multi-apps/overview) section of your Frontegg Portal.

3) In Frontegg Portal → Authentication → Login Method → Hosted Login:
- Allowed Redirect URIs: add the ones you will use locally, e.g.
  - `http://localhost:5057/signin-oidc`
- Post-logout Redirect URIs:
  - `http://localhost:5057/signout-callback-oidc`

#### Authentication Configuration
The application includes enhanced Frontegg authentication with:

- **App ID Parameter**: The appId is automatically added to the authorization request (different from clientId)
- **Client ID Parameter**: The clientId is also included for OAuth client identification
- **Automatic Token Refresh**: Tokens are automatically refreshed every 30 minutes
- **Token Lifetime Management**: Uses token lifetime from Frontegg
- **HTTPS Enforcement**: Requires HTTPS metadata
- **Audience Validation**: Validates token audience against Client ID
- **Event Logging**: Logs successful token validations

#### Frontegg Parameters Explained
- **appId**: Identifies the specific application instance in Frontegg (found in OAuth URLs)
- **clientId**: OAuth client identifier used for authentication flow
- **clientSecret**: OAuth client secret for secure communication

#### Optional Configuration
You can add additional parameters to the authentication request:

```csharp
// In Program.cs, uncomment and modify as needed:
context.ProtocolMessage.SetParameter("tenant_id", "your-tenant-id");
context.ProtocolMessage.SetParameter("custom_param", "custom_value");
```

### Features

#### Authentication
- **Sign In/Sign Out**: Full OIDC authentication flow with Frontegg
- **User Claims**: View user information and claims after authentication
- **Dashboard**: Secure dashboard showing authenticated user data
- **Token Management**: Automatic token refresh and lifetime management

#### Admin Portal
- **Secure Admin Portal Access**: Direct link to the Frontegg hosted Admin Portal
- **Dynamic URL Construction**: Admin Portal URL is automatically built from `FRONTEGG_AUTHORITY` environment variable
- **User Management**: Manage users, roles, and permissions
- **Security Policies**: Configure security and compliance settings

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

### Navigation
- **Home**: Landing page with sign-in option
- **Dashboard**: View user claims and information (requires authentication)
- **Admin Portal**: Access Frontegg Admin Portal in new tab (requires authentication)

### Security Features
- **X-Frame-Options Compliance**: Admin Portal opens in new tab to prevent clickjacking
- **Secure Authentication**: Full OIDC flow with proper session management
- **Environment Variable Security**: Sensitive credentials stored securely
- **No Iframe Embedding**: Avoids security vulnerabilities from embedded content
- **Token Validation**: Comprehensive token validation with audience checking
- **HTTPS Enforcement**: Requires secure connections for metadata

### Notes
- Target framework: `net9.0`
- Packages: `Microsoft.AspNetCore.Authentication.OpenIdConnect` (9.0.x), `Microsoft.IdentityModel.Tokens` (8.0.x)
- The application reads Frontegg credentials from environment variables first, with fallback to `appsettings.json` for local development
- The `.env` file is automatically loaded for local development using the `DotNetEnv` package
- **Important**: Never commit your `.env` file to version control as it contains sensitive credentials
- **Security**: Admin Portal opens in new tab to ensure full functionality and security compliance
- **Token Management**: Automatic refresh ensures continuous authentication without user intervention
- **Frontegg Integration**: Uses both appId and clientId for proper Frontegg integration
