using Microsoft.Extensions.Logging;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Model;

namespace Voxai.Integration.Providers.PureCloud;

/// <inheritdoc/>
public class GroupProvider : IGroupsProvider
{
    private ILogger Log { get; }
    private IGroupsApi ApiAccessor { get; }

    public GroupProvider(ILogger<GroupProvider> logger, IGroupsApi api)
    {
        Log = logger;
        Log.LogInformation("Created new instance");
        ApiAccessor = api;
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<Group> GetAllGroupsAsync()
    {
        var currentPage = await ApiAccessor.GetGroupsAsync(pageSize: 50);

        // If this isn't the last page, queue up the next request.
        while (currentPage.PageNumber < currentPage.PageCount)
        {
            var nextPage = ApiAccessor.GetGroupsAsync(pageSize: 50, pageNumber: currentPage.PageNumber + 1);
            foreach (var group in currentPage.Entities)
                yield return group;
            currentPage = await nextPage;
        }

        // Return the last page's entities.
        foreach (var group in currentPage.Entities)
            yield return group;
    }
    /// <summary>
    ///Get Group details by Group Id.
    /// </summary>
    /// <param name="GroupID">Group Id.</param>
    /// <returns>Group Object</returns>
    public async Task<Group> GetGroupAsync(string groupId)
    {
        var groupInfo = await ApiAccessor.GetGroupAsync(groupId);
        return groupInfo;
    }
    /// <summary>
    ///Get Group details by Group Name.
    /// </summary>
    /// <param name="GroupID">Group Id.</param>
    /// <returns>Group Object</returns>
    public async Task<Group> GetGroupByNameAsync(string groupName)
    {
        var groupInfo = await ApiAccessor.GetGroupsSearchAsync(groupName);
        return groupInfo.Results.FirstOrDefault();
    }
}
