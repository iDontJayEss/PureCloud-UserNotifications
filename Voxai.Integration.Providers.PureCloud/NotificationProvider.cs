using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Extensions.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Voxai.Integration.Providers.PureCloud;
public class NotificationProvider : ProviderBase, INotificationProvider
{
    private NotificationHandler Handler { get; } = new();

    public NotificationProvider(ILogger<NotificationProvider> logger, IOptionsSnapshot<ProviderOptions> options) 
        : base(logger, options.Value)
    {
    }

    public event NotificationHandler.NotificationReceivedHandler Received 
    { 
        add => Handler.NotificationReceived += value; 
        remove => Handler.NotificationReceived -= value; 
    }

    public void AddSubscription<TModel>(string topicTemplate, params string[] parameters)
    {
        Log.LogDebug($"Subscribing to topic {topicTemplate} for {string.Join(", ", parameters)}");
        Handler.AddSubscription(string.Format(topicTemplate, parameters), typeof(TModel));
    }
}
