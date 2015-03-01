using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    [ServiceContract]
    public interface INotificationCallbackService
    {
        [OperationContract(IsOneWay = true)]
        [ApplyDataContractResolver]
        void NewNotificationReceived(Notification notif, Topic topic, UpdateType updateType);

        [OperationContract(IsOneWay = true)]
        [ApplyDataContractResolver]
        void NewTopicCreated(Topic topic);
    }
}
