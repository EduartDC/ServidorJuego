using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MessageService.Domain
{
    [DataContract]
    public class MessageServer
    {

        private string sender;
        private string content;


        [DataMember]
        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }
        [DataMember]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
    }
}
