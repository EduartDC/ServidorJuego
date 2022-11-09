using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DataBase;
using System.Runtime.Serialization;

namespace MessageService
{
    [ServiceContract(CallbackContract = typeof(IChatCallback), SessionMode = SessionMode.Required)]
    internal interface IChatService
    {
        [OperationContract(IsInitiating = true)]
        bool Connect(Player client);

        [OperationContract(IsOneWay = true)]
        void Say(Message msg);

        [OperationContract(IsOneWay = true)]
        void Whisper(Message msg, Player receiver);

        [OperationContract(IsOneWay = true, IsTerminating = true)]
        void Disconnect(Player client);
    }
    [ServiceContract]
    public interface IChatCallback
    {
        [OperationContract(IsOneWay = true)]
        void RefreshClients(List<Player> clients);

        [OperationContract(IsOneWay = true)]
        void Receive(Message msg);

        [OperationContract(IsOneWay = true)]
        void ReceiveWhisper(Message msg, Player receiver);

        [OperationContract(IsOneWay = true)]
        void UserJoin(Player client);

        [OperationContract(IsOneWay = true)]
        void UserLeave(Player client);
    }

    [DataContract]
    public class Message
    {
        private string _sender;
        private string _content;
        private DateTime _time;

        [DataMember]
        public string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }
        [DataMember]
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
        [DataMember]
        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }
    }


}
