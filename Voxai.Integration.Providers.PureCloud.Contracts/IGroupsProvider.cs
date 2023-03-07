using PureCloudPlatform.Client.V2.Model;

namespace Voxai.Integration.Providers.PureCloud;

/// <summary>
/// Exposes read operations on Genesys PureCloud Groups.
/// </summary>
public interface IGroupsProvider
{
    /// <summary>
    /// Retrieves all Groups in PureCloud.
    /// </summary>
    /// <returns>A collection of all Groups.</returns>
    IAsyncEnumerable<Group> GetAllGroupsAsync();
    Task<Group> GetGroupAsync(string groupID);
    Task<Group> GetGroupByNameAsync(string groupName);


}
