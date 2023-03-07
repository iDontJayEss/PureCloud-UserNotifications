using Microsoft.Extensions.Logging;
using PureCloudPlatform.Client.V2.Model;
using System.Runtime.CompilerServices;

namespace Voxai.Integration.Providers.PureCloud;
public abstract class ProviderBase
{
    protected ILogger Log { get; }

    protected ProviderOptions Options { get; }
        = new();

    protected ProviderBase(ILogger logger, ProviderOptions options)
    {
        Log = logger;
        Log.LogInformation("Created new instance");
        Options = options;
    }

    protected delegate Task<IPagedResource<TResource>> PagedResourceAccessor<TResource>(int page, int itemsPerPage);

    protected async IAsyncEnumerable<TResource> GetPagedResourceAsync<TResource>(PagedResourceAccessor<TResource> getByPageDelegate, [CallerMemberName] string callerName = "")

    {
        using var scope = Log.BeginScope(callerName);

        var currentPage = await getByPageDelegate(1, Options.Pagination.ItemsPerPage);
        Log.LogDebug(LogPageFormat, 1, currentPage.Entities.Count);
        var pages = currentPage.PageCount ?? 0;
        for (var i = currentPage.PageNumber ?? 1; i < pages; ++i)
        {
            var nextPage = getByPageDelegate(i, Options.Pagination.ItemsPerPage);
            foreach (var resource in currentPage.Entities)
                yield return resource;
            currentPage = await nextPage;
            Log.LogDebug(LogPageFormat, 1, currentPage.Entities.Count);
        }

        foreach (var resource in currentPage.Entities)
            yield return resource;
    }

    private const string LogPageFormat = "Retrieved Page {page} with {entries} Entries";
}
