using PureCloudPlatform.Client.V2.Client;

namespace Voxai.Integration.Providers.PureCloud;

public interface IPureCloudClient
{
    Configuration ClientConfig { get; }
}