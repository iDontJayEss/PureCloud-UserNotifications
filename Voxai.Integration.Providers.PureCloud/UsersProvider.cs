using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Model;

namespace Voxai.Integration.Providers.PureCloud;

/// <inheritdoc/>
public class UsersProvider : ProviderBase, IUsersProvider
{
    private IUsersApi ApiAccessor { get; }

    private ISearchApi SearchApiAccessor { get; }

    public UsersProvider(ILogger<UsersProvider> logger, IOptionsSnapshot<ProviderOptions> options, IUsersApi api, ISearchApi searchApi)
        : base(logger, options.Value)
    {
        ApiAccessor = api;
        SearchApiAccessor = searchApi;
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<User> GetAllUsersAsync()
        => GetPagedResourceAsync<User>(async (page, itemsPerPage) => await ApiAccessor.GetUsersAsync(pageSize: itemsPerPage, pageNumber: page));

    public async Task<User> GetUserByIdAsync(string userId)
    {
        var user = await ApiAccessor.GetUserAsync(userId);

        return user;
    }

    public async IAsyncEnumerable<User> GetUsersByIdAsync(params string[] ids)
    {
        if (!ids.Any())
            throw new ArgumentException("Must contain at least one value.", nameof(ids));

        if (ids.Length == 1)
        {
            yield return await GetUserByIdAsync(ids.First());
            yield break;
        }

        var users = GetPagedResourceAsync<User>(async (page, itemsPerPage) => await ApiAccessor.GetUsersAsync(pageSize: itemsPerPage, pageNumber: page, id: ids.ToList()));
        await foreach (var user in users)
            yield return user;

    }

    
    public async IAsyncEnumerable<User> GetUserByNameAsync(string userName)
    {
        if(string.IsNullOrWhiteSpace(userName)) 
            throw new ArgumentNullException(nameof(userName));

        var result = await SearchApiAccessor.PostUsersSearchAsync(new()
        {
            PageSize = 1,
            PageNumber = 1,
            Query = new()
            {
                new()
                {
                    Type = UserSearchCriteria.TypeEnum.Term,
                    Fields= new()
                    {
                        "email",
                        "username"
                    },
                    Value = userName
                }
            }
        });

        if(result is UsersSearchResponse response)
        {
            foreach (var user in response.Results)
                yield return user;
        }
        
    }
}
