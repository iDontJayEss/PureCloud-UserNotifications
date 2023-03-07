using Microsoft.VisualBasic;
using PureCloudPlatform.Client.V2.Extensions.Notifications;
using PureCloudPlatform.Client.V2.Model;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Voxai.Integration.Providers.PureCloud;

namespace UserNotifications.ServiceHost;

public class Worker : BackgroundService
{
    private ILogger<Worker> Log { get; }

    private IServiceScope Scope { get; }

    private readonly ConcurrentBag<string> trackedConversations = new();

    private IServiceProvider ServiceProvider { get; } //Scope.ServiceProvider;

    private IUsersProvider UsersProvider { get; } // => ServiceProvider.GetRequiredService<IUsersProvider>();

    private INotificationProvider NotificationProvider { get; } //ServiceProvider.GetRequiredService<INotificationProvider>();

    public Worker(ILogger<Worker> logger, IServiceProvider provider)
    {
        Log = logger;
        Scope = provider.CreateScope();
        ServiceProvider = Scope.ServiceProvider;
        UsersProvider = ServiceProvider.GetRequiredService<IUsersProvider>();
        NotificationProvider = ServiceProvider.GetRequiredService<INotificationProvider>();
    }

    private async Task<IEnumerable<User>> GetUsersAsync(string username)
    {
        var result = new List<User>();
        await foreach(var user in UsersProvider.GetUserByNameAsync(username))
        {
            result.Add(user);
        }
        return result;
    }

    private static void SubscribeToUserNotifications(INotificationProvider provider, string userId)
    {
        provider.AddSubscription<AgentActivityChangedTopicAgentActivity>("v2.users.{0}.activity", userId);
        provider.AddSubscription<UserConversationsEventUserConversationSummary>("v2.users.{0}.conversationsummary", userId);
        provider.AddSubscription<PresenceEventUserPresence>("v2.users.{0}.presence", userId);
        provider.AddSubscription<UserRoutingStatusUserRoutingStatus>("v2.users.{0}.routingStatus", userId);
        provider.AddSubscription<UserStationChangeTopicUserStations>("v2.users.{0}.station", userId);
        provider.AddSubscription<ConversationEventTopicConversation>("v2.users.{0}.conversations", userId);
    }

    private void SubscribeToConversationNotifications(INotificationProvider provider, string conversationId)
    {
        if (!trackedConversations.Contains(conversationId))
        {
            provider.AddSubscription<UserStartDetailEventTopicUserStartEvent>("v2.detail.events.conversation.{0}.user.start", conversationId);
            provider.AddSubscription<UserEndDetailEventTopicUserEndEvent>("v2.detail.events.conversation.{0}.user.end", conversationId);
            trackedConversations.Add(conversationId);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var user = (await GetUsersAsync("ischlarman@voxai.com")).Single();
        Log.LogDebug($"User {user.Username} retrieved: Id = {user.Id}");
        SubscribeToUserNotifications(NotificationProvider, user.Id);
        NotificationProvider.Received += HandleUserNotifications;

        while (!stoppingToken.IsCancellationRequested)
        {
            Log.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(10000, stoppingToken);
        }

        NotificationProvider.Received -= HandleUserNotifications;
    }

    private void HandleUserNotifications(INotificationData notificationData)
    {
        // NotificationData<T> : INotificationData
        //  public T EventBody {get;}

        dynamic notification = notificationData;
        switch (notification.EventBody)
        {
            case AgentActivityChangedTopicAgentActivity agentActivity:
                Log.LogDebug($"Agent Activity: {JsonSerializer.Serialize(agentActivity)}");
                break;

            case UserConversationsEventUserConversationSummary conversationSummary:
                Log.LogDebug($"Agent Conversation Summary: {JsonSerializer.Serialize(conversationSummary)}");
                break;

            case PresenceEventUserPresence presence:
                Log.LogDebug($"User Presence: {JsonSerializer.Serialize(presence)}");
                break;

            case UserRoutingStatusUserRoutingStatus routingStatus:
                Log.LogDebug($"Routing Status: {JsonSerializer.Serialize(routingStatus)}");
                break;

            case UserStationChangeTopicUserStations station:
                Log.LogDebug($"User Station: {JsonSerializer.Serialize(station)}");
                break;

            case ConversationEventTopicConversation conversation:
                Log.LogDebug($"User Conversations: {JsonSerializer.Serialize(conversation)}");
                SubscribeToConversationNotifications(NotificationProvider, conversation.Id);
                break;

            case UserStartDetailEventTopicUserStartEvent conversationStart:
                Log.LogDebug($"Conversation User Start: {JsonSerializer.Serialize(conversationStart)}");
                break;
            case UserEndDetailEventTopicUserEndEvent conversationEnd:
                Log.LogDebug($"Conversation User End: {JsonSerializer.Serialize(conversationEnd)}");
                break;
        }

        //if(GetEventBody(notificationData) is object instance)
        //{
        //    switch (instance)
        //    {
        //        case AgentActivityChangedTopicAgentActivity agentActivity:
        //            Log.LogDebug(JsonSerializer.Serialize(agentActivity));
        //            break;
        //    }
        //}
    }

    private static object GetEventBody(INotificationData notification)
        => EventBodyProperty.GetValue(notification)!;

    private static PropertyInfo EventBodyProperty { get; } = typeof(NotificationData<>).GetProperty("EventBody")!;
}
