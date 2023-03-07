using PureCloudPlatform.Client.V2.Model;

namespace Voxai.Integration.Providers.PureCloud;

/// <summary>
/// Exposes read operations on Genesys PureCloud Queues.
/// </summary>
public interface IQueueProvider
{
    /// <summary>
    /// Retrieves all Queues in PureCloud.
    /// </summary>
    /// <returns></returns>
    IAsyncEnumerable<Queue> GetAllQueuesAsync();

    /// <summary>
    /// Retrieves Queue by QueueId
    /// </summary>
    /// <param name="queueId"></param>
    /// <returns></returns>
    Task<Queue> GetQueueByIdAsync(string queueId);

    ///// <summary>
    ///// Retrieves Queue by QueueName
    ///// </summary>
    ///// <param name="queueName"></param>
    ///// <returns></returns>
    //IAsyncEnumerable<Queue> GetQueueByNameAsync(string queueName);

    /// <summary>
    /// Retrieves Users by QueueId
    /// </summary>
    /// <param name="queueId"></param>
    /// <returns></returns>
    IAsyncEnumerable<QueueMember> GetQueueUsersAsync(string queueId);

    /// <summary>
    /// Retrieves WrapUpCodes by QueueId
    /// </summary>
    /// <param name="queueId"></param>
    /// <returns></returns>
    IAsyncEnumerable<WrapupCode> GetQueueWrapUpCodesAsync(string queueId);

    /// <summary>
    /// Retrieves a collection of Queues with the provided <paramref name="ids"/>.
    /// </summary>
    /// <param name="ids">The unique identifiers of each Queue.</param>
    /// <returns>A collection of Queues matching the provided <paramref name="ids"/>.</returns>
    /// <exception cref="ArgumentException">Throws if <paramref name="ids"/> contains no values.</exception>
    IAsyncEnumerable<Queue> GetQueuesByIdAsync(params string[] ids);
}
