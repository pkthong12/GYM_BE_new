namespace API
{
    public class JwtToken
    {
        public required string SecretKey { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public int WebDaysOfLife { get; set; }
        public int WebMinutesOfLife { get; set; }
        public int MobileDaysOfLife { get; set; }
        public int RefreshTokenTTL { get; set; }
        public int RefreshTokenDaysOfLife { get; set; }
    }
    public class ConnectionStrings
    {
        public required string TestDb { get; set; }
        public required string CoreDb { get; set; }

    }
    public class StaticFolders
    {
		public required string Logs { get; set; }
        public required string Root { get; set; }
        public required string Avatars { get; set; }
        public required string Attachments { get; set; }
        public required string ExcelTemplates { get; set; }
        public required string ExcelReports { get; set; }
        public required string WordTemplates { get; set; }

    }

    public class SharedFolders
    {
        public required string Root { get; set; }
        public required string Images { get; set; }

    }

    public class RequestResponseLogger
    {
        public bool IsEnabled { get; set; }
        public required string Name { get; set; }
        public required string DateTimeFormat { get; set; }
        public required int DaysToKeep { get; set; }
    }

    public class Auth2OidcSetting
    {
        public bool IsEnabled { get; set; }
        public required string ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public required string TokenEndpoint { get; set; }
        public required string RedirectUri { get; set; }
    }

    //https://sso.vnsteel.vn/adfs/ls?wtrealm=https://qlns.vnsteel.vn&wa=wsignin1.0&wreply=https://localhost:44359/api/Authentication/HandleADFSResponse
    public class Saml2AdfsSetting
    {
        public bool IsEnabled { get; set; }
        public required string IdPUrl { get; set; }
        public required string SPUrl { get; set; }
    }

    public class Auth2Oidc
    {
        public List<Auth2OidcSetting>? Settings {  get; set; }
    }

    public class AppSettings
    {
        public required ConnectionStrings ConnectionStrings { get; set; }
        public required StaticFolders StaticFolders { get; set; }
        public required SharedFolders SharedFolders { get; set; }
        public required JwtToken JwtToken { get; set; }
        public required string CryptoKey { get; set; }
        public required RequestResponseLogger RequestResponseLogger { get; set; }
        public required Auth2Oidc Auth2Oidc { get; set; }
        public required Saml2AdfsSetting Saml2AdfsSetting { get; set; }
        public bool SupportMultipleTimeZones { get; set; }
        public required string SiteTimeZoneId { get; set; }

    }
}
