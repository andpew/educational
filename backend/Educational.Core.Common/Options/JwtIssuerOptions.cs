namespace Educational.Core.Common.Options;

public class JwtIssuerOptions
{
    public string Key { get; set; } = null!;
    public TimeSpan ValidFor { get; set; }
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
}
