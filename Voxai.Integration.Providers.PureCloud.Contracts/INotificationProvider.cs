using PureCloudPlatform.Client.V2.Extensions.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxai.Integration.Providers.PureCloud;
public interface INotificationProvider
{
    event NotificationHandler.NotificationReceivedHandler Received;

    void AddSubscription<TModel>(string topicTemplate, params string[] parameters);
}
