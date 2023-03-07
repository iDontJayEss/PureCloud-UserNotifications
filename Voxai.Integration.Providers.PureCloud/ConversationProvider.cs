using Microsoft.Extensions.Logging;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Model;

namespace Voxai.Integration.Providers.PureCloud;
public class ConversationProvider : IConversationProvider
{
    private ILogger Log { get; }
    private IConversationsApi ApiAccessor { get; }

    public ConversationProvider(ILogger<ConversationProvider> logger, IConversationsApi api)
    {
        Log = logger;
        Log.LogInformation("Created new instance");
        ApiAccessor = api;
    }
    public async Task<ConversationEntityListing> GetAllConversationAsync()
    {
        var currentPage = await ApiAccessor.GetConversationsAsync();

        return currentPage;


    }
    public async Task<Conversation> GetConversationAsync(string conversationId)
    {
        var conversation = await ApiAccessor.GetConversationAsync(conversationId);
        return conversation;
    }
    public AnalyticsConversationWithoutAttributesMultiGetResponse GetConversationsbyIds(List<string> conversationId)
    {
        var conversation = ApiAccessor.GetAnalyticsConversationsDetails(conversationId);
        return conversation;
    }
}
