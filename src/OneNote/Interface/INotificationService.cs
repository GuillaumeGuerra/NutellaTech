using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    [ServiceContract(CallbackContract = typeof(INotificationCallbackService))]
    public interface INotificationService
    {
        [OperationContract]
        [ApplyDataContractResolver]
        IEnumerable<Notification> GetNotificationsForTopic(Topic topic);

        [OperationContract]
        [ApplyDataContractResolver]
        void PushNotification(Notification notification, Topic topic);

        [OperationContract]
        [ApplyDataContractResolver]
        void RegisterListener();

        [OperationContract]
        [ApplyDataContractResolver]
        void UnregisterListener();

        [OperationContract]
        [ApplyDataContractResolver]
        List<Topic> GetAllTopics();

        [OperationContract]
        [ApplyDataContractResolver]
        void CreateTopic(Topic topic);

        [OperationContract]
        [ApplyDataContractResolver]
        void VoteOnNotification(VoteNotification voteNotification, bool voteFor, string sender);

        [OperationContract]
        [ApplyDataContractResolver]
        List<Voter> GetVoters();
    }
}
