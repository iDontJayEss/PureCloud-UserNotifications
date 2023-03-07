using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Extensions;

namespace Voxai.Integration.Providers.PureCloud;
public class PureCloudClient : IDisposable, IPureCloudClient
{
    public Configuration ClientConfig => Configuration.Default;

    private PureCloudClientOptions CurrentOptions { get; set; }
        = new PureCloudClientOptions();

    private ILogger Log { get; }

    public PureCloudClient(ILogger<PureCloudClient> logger, IOptionsMonitor<PureCloudClientOptions> optionsMonitor)
    {
        Log = logger;
        Log.LogInformation("Created new instance");
        CurrentOptions = optionsMonitor.CurrentValue;
        SetupClient(CurrentOptions);
        monitor = optionsMonitor.OnChange(UpdateOptions);
    }

    private static void SetupClient(PureCloudClientOptions options)
    {
        var config = Configuration.Default;
        if (options.MaxRetries > 0 || options.MaxRetrySeconds > 0)
        {
            config.ApiClient.RetryConfig = new ApiClient.RetryConfiguration
            {
                RetryMax = options.MaxRetries,
                MaxRetryTimeSec = options.MaxRetrySeconds,
            };
        }

        config.ApiClient.ClientId = options.ClientId;
        config.ApiClient.ClientSecret = options.ClientSecret;
        config.ApiClient.setBasePath(options.Region);
        config.ApiClient.PostToken(options.ClientId, options.ClientSecret);
    }

    private void UpdateOptions(PureCloudClientOptions options)
    {
        if (!CurrentOptions.Equals(options))
        {
            Log.LogInformation("Options updated");
            CurrentOptions = options;
            SetupClient(CurrentOptions);
        }
    }

    #region IDisposable Implementation

    private bool disposedValue;

    private readonly IDisposable monitor;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                monitor?.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion IDisposable Implementation
}
