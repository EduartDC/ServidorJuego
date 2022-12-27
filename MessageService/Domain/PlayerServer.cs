using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageService.Domain
{
    [DataContract]
    public class PlayerServer
    {
        [DataMember]
        public int idPlayer { get; set; }

        [DataMember]
        public string firstName { get; set; }

        [DataMember]
        public string lastName { get; set; }

        [DataMember]
        public string email { get; set; }

        [DataMember]
        public string userName { get; set; }

        [DataMember]
        public string password { get; set; }

        [DataMember]
        public bool status { get; set; }

        [DataMember]
        public IChatServiceCallback chatCallback { get; set; }

        [DataMember]
        public IMatchServiceCallBack matchCallBack { get; set; }

        [DataMember]
        public INotificationServiceCallback userCallBack { get; set; }
    }
}
