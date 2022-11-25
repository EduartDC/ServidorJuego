using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MessageService.Domain
{
    [DataContract]
    public class AnswerServer
    {
        [DataMember]
        public int idAnswer { get; set; }
        [DataMember]
        public string answer { get; set; }
        [DataMember]
        public int score { get; set; }
        [DataMember]
        public int place { get; set; }
    }
}
