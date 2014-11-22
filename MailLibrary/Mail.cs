using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailLibrary
{
    public class Mail
    {
        public List<String> To { get; private set; }
        public List<String> Cc { get; private set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public Mail()
        {
           To = To?? new List<string>();
           Cc = Cc?? new List<string>();
        }

    }
}
