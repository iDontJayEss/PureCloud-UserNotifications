namespace Voxai.Integration.Providers.PureCloud;

/// <summary>
/// Configuration options common to all Providers.
/// </summary>
public class ProviderOptions
{
    /// <summary>
    /// Pagination options for the provider.
    /// </summary>
    public PaginationOptions Pagination { get; set; } = new();

    public const string ConfigSection = "PureCloud:Providers";
}

/// <summary>
/// Configuration options related to paginated requests.
/// </summary>
public class PaginationOptions
{
    /// <summary>
    /// Number of items to retrieve at once from a paginated request.
    /// </summary>
    public int ItemsPerPage { get; set; } = 100;
}