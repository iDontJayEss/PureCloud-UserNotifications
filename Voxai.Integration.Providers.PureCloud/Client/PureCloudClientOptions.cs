using PureCloudPlatform.Client.V2.Client;
using System.ComponentModel.DataAnnotations;

namespace Voxai.Integration.Providers.PureCloud;

/// <summary>
/// Describes options related to connecting to Genesys Cloud via their API.
/// </summary>
public class PureCloudClientOptions
{
    /// <summary>
    /// Client Id used for Client Credentials OAuth Grant.
    /// </summary>
    /// <remarks>
    /// This should be set in User Secrets or a secure store, rather than in a configuration file.
    /// Usage described here: https://developer.genesys.cloud/api/rest/authorization/use-client-credentials
    /// </remarks>
    [Required]
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Client Secret used for Client Credentials OAuth Grant.
    /// </summary>
    /// <remarks>
    /// This should be set in User Secrets or a secure store, rather than in a configuration file.
    /// Usage described here: https://developer.genesys.cloud/api/rest/authorization/use-client-credentials
    /// </remarks>
    [Required]
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// The API region the client should connect to.
    /// </summary>
    public PureCloudRegionHosts Region { get; set; } = PureCloudRegionHosts.us_east_1;

    /// <summary>
    /// Time in seconds to retry requests before returning an error.
    /// </summary>
    /// <remarks>
    /// Follows the backoff logic described here: https://developer.genesys.cloud/api/rest/rate_limits
    /// </remarks>
    public int MaxRetrySeconds { get; set; } = 10;

    /// <summary>
    /// Number of times the client will retry failed requests before returning an error.
    /// </summary>
    public int MaxRetries { get; set; } = 5;

    /// <summary>
    /// Section of configuration where these Options will reside.
    /// </summary>
    public const string ConfigSection = "PureCloud:Authorization";

    public override int GetHashCode()
        => HashCode.Combine(ClientId, ClientSecret, Region, MaxRetrySeconds, MaxRetries);

    public override bool Equals(object? obj)
        => obj is PureCloudClientOptions other
        && other.GetHashCode() == GetHashCode();

}
