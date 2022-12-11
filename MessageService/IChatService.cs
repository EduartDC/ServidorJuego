using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MessageService.Domain;

namespace MessageService
{
    [ServiceContract(CallbackContract = typeof(IChatServiceCallback), SessionMode = SessionMode.Required)]
    internal interface IChatService
    {
        [OperationContract(IsOneWay = true)]
        void Connect(PlayerServer player, string code);

        [OperationContract(IsOneWay = true)]
        void Say(int idMatch, MessageServer msg);

        [OperationContract(IsOneWay = true)]
        void Whisper(MessageServer msg, string player);

        [OperationContract(IsOneWay = true)]
        void Disconnect(PlayerServer player);
    }

    [ServiceContract]
    public interface IChatServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void RefreshClients(List<PlayerServer> players);

        [OperationContract(IsOneWay = true)]
        void Receive(MessageServer message);

        [OperationContract(IsOneWay = true)]
        void ReceiveWhisper(MessageServer msg);

        [OperationContract(IsOneWay = true)]
        void UserJoin(PlayerServer player);

        [OperationContract(IsOneWay = true)]
        void UserLeave(PlayerServer player);
    }




}
