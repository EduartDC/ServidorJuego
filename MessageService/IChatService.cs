using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
//using DataBase;
using System.Runtime.Serialization;
using DataAcces;

namespace MessageService
{
    [ServiceContract(CallbackContract = typeof(IChatServiceCallback), SessionMode = SessionMode.Required)]
    internal interface IChatService
    {
        [OperationContract(IsOneWay = true)]
        void Connect(Player player, int idMatch);

        [OperationContract(IsOneWay = true)]
        void Say(int idMatch, Message msg);

        [OperationContract(IsOneWay = true)]
        void Whisper(Message msg, Player player);

        [OperationContract(IsOneWay = true)]
        void Disconnect(Player player);
    }

    [ServiceContract]
    public interface IChatServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void RefreshClients(List<Player> players);

        [OperationContract(IsOneWay = true)]
        void Receive(List<Message> messages);

        [OperationContract(IsOneWay = true)]
        void ReceiveWhisper(Message msg, Player player);

        [OperationContract(IsOneWay = true)]
        void UserJoin(Player player);

        [OperationContract(IsOneWay = true)]
        void UserLeave(Player player);
    }

    [DataContract]
    public class Message
    {
        private string _sender;
        private string _content;
        

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
 
    }


}
