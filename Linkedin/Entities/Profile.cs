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
        public string current_work_email { get; set; }
        public string current_personal_email { get; set; }
    }
}
