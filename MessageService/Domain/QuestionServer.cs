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
    public class QuestionServer
    {

        [DataMember]
        public int idQuestion { get; set; }
        [DataMember]
        public string question { get; set; }
        [DataMember]
        public string questionClass { get; set; }
        [DataMember]
        public List<AnswerServer> answers { get; set; }
    }
}
