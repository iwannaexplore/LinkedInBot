using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linkedin.Entities
{
    public class Profile
    {
        public int id { get; set; }
        public string name { get; set; }
        public string current_employer { get; set; }
        public string current_title { get; set; }
        public string linkedin_url { get; set; }
        public List<Email> Emails { get; set; }
    }
}
