using PureCloudPlatform.Client.V2.Client;

namespace Voxai.Integration.Providers.PureCloud;

/// <summary>
/// Contains utilities for reflecting on PureCloud library types.
/// </summary>
public static class PureCloudReflectionExtensions
{
    public static IEnumerable<(Type implementation, IEnumerable<Type> contracts)> ApiContractMap
        => ApiImplementations.Select(t => (t, GetApiInterfaces(t)));

    /// <summary>
    /// Retrieves all <see cref="IApiAccessor"/> interfaces the provided <paramref name="type"/> implements.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>A collection of interface types the provided <paramref name="type"/> implements.</returns>
    public static IEnumerable<Type> GetApiInterfaces(this Type type)
        => type.GetInterfaces()
               .Where(typeof(IApiAccessor).IsAssignableFrom)
               .Where(t => !t.Equals(typeof(IApiAccessor)));

    public static IEnumerable<Type> ApiImplementations
        => typeof(IApiAccessor)
            .Assembly
            .GetTypes()
            .Where(typeof(IApiAccessor).IsAssignableFrom)
            .Where(t => !t.IsInterface);
}
