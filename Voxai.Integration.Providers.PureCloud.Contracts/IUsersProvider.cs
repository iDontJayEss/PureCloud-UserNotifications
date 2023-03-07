using PureCloudPlatform.Client.V2.Model;

namespace Voxai.Integration.Providers.PureCloud;

/// <summary>
/// Exposes read operations on Genesys PureCloud Users.
/// </summary>
public interface IUsersProvider
{
    /// <summary>
    /// Retrieves all Users in PureCloud.
    /// </summary>
    /// <returns>A collection of all Users.</returns>
    IAsyncEnumerable<User> GetAllUsersAsync();

    /// <summary>
    /// Retrieves User by UserId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<User> GetUserByIdAsync(string userId);

    /// <summary>
    /// Retrieves User by UserName
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    IAsyncEnumerable<User> GetUserByNameAsync(string userName);

    /// <summary>
    /// Retrieves a collection of Users with the provided <paramref name="ids"/>.
    /// </summary>
    /// <param name="ids">The unique identifiers of each user.</param>
    /// <returns>A collection of Users matching the provided <paramref name="ids"/>.</returns>
    /// <exception cref="ArgumentException">Throws if <paramref name="ids"/> contains no values.</exception>
    IAsyncEnumerable<User> GetUsersByIdAsync(params string[] ids);
}