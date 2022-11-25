using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MessageService.Domain
{
    [DataContract]
    public class FriendServer
    {
        
       
       
            [DataMember]
            public int idFriend { get; set; }

            [DataMember]
            public int gameFriend { get; set; }

            [DataMember]
            public DateTime creationDate { get; set; }

            [DataMember]
            public int ownerPlayer { get; set; }


        
    }
}
