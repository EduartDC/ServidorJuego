using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MessageService.Domain
{
    [DataContract]
    public class MatchServer
    {
        [DataMember]
        public int idMatch { get; set; }
        [DataMember]
        public int scorePlayerOne { get; set; }
        [DataMember]
        public int scorePlayerTwo { get; set; }
        [DataMember]
        public int playerWinner { get; set; }
        [DataMember]
        public string inviteCode { get; set; }
        [DataMember]
        public List<PlayerServer> players { get; set; }
    }
}
