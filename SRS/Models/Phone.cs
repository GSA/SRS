using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Models
{
    public class Phone
    {
        public string HomeCell { get; internal set; }
        public string HomePhone { get; set; }
        public string PersonalCell { get; set; }
        public string WorkCell { get; set; }
        public object WorkFax { get; internal set; }
        public string WorkPhone { get; internal set; }
        public string WorkTextTelephone { get; internal set; }
    }
}

