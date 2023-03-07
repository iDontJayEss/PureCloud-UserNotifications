using PureCloudPlatform.Client.V2.Model;


namespace Voxai.Integration.Providers.PureCloud;
public interface IConversationProvider
{
    Task<Conversation> GetConversationAsync(string id);
    //IAsyncEnumerable<Conversation> get
    AnalyticsConversationWithoutAttributesMultiGetResponse GetConversationsbyIds(List<string> conversationId);
}
