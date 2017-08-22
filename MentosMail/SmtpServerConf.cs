using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentosMail
{
    public class SmtpServerConf
    {
        public string Host { get; set; }
        public int Port { get; set; } = 587;
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
        public bool UseDefaultCredential { get; set; }
    }
}
