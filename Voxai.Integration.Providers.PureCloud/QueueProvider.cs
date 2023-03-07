using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Model;

namespace Voxai.Integration.Providers.PureCloud;

/// <inheritdoc/>
public class QueueProvider : ProviderBase, IQueueProvider
{
    private IRoutingApi ApiAccessor { get; }

    public QueueProvider(ILogger<QueueProvider> logger, IOptionsSnapshot<ProviderOptions> options, IRoutingApi api)
        : base(logger, options.Value) => ApiAccessor = api;

    /// <inheritdoc/>
    public IAsyncEnumerable<Queue> GetAllQueuesAsync()
        => GetPagedResourceAsync<Queue>(async (page, itemsPerPage) => await ApiAccessor.GetRoutingQueuesAsync(pageNumber: page, pageSize: itemsPerPage));

    /// <inheritdoc/>
    public async Task<Queue> GetQueueByIdAsync(string queueId)
    {
        var queue = await ApiAccessor.GetRoutingQueueAsync(queueId);

        return queue;
    }

    public async IAsyncEnumerable<Queue> GetQueuesByIdAsync(params string[] ids)
    {
        if (!ids.Any())
            throw new ArgumentException("Must contain at least one value.", nameof(ids));

        if (ids.Length == 1)
        {
            yield return await GetQueueByIdAsync(ids.First());
            yield break;
        }

        var queues = GetPagedResourceAsync<Queue>(async (page, itemsPerPage) => await ApiAccessor.GetRoutingQueuesAsync(pageSize: itemsPerPage, pageNumber: page, id: ids.ToList()));
        await foreach (var queue in queues)
            yield return queue;

    }

    ///// <inheritdoc/>
    //public async IAsyncEnumerable<Queue> GetQueueByNameAsync(string queueName)
    //{
    //    throw new NotImplementedException();
    //}

    /// <inheritdoc/>
    public IAsyncEnumerable<WrapupCode> GetQueueWrapUpCodesAsync(string queueId)
        => GetPagedResourceAsync<WrapupCode>(async (page, itemsPerPage) => await ApiAccessor.GetRoutingQueueWrapupcodesAsync(queueId, pageNumber: page, pageSize: itemsPerPage));


    /// <inheritdoc/>
    public IAsyncEnumerable<QueueMember> GetQueueUsersAsync(string queueId)
        => GetPagedResourceAsync<QueueMember>(async (page, itemsPerPage) => await ApiAccessor.GetRoutingQueueUsersAsync(queueId, pageNumber: page, pageSize: itemsPerPage));

}
