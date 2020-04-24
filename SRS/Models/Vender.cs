using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Models
{
    public class Vender
    {
        public Int64 GCIMSID { get; set; } //If Matched we set this        
        public string VenderName { get; set; }
        public string VenderShortName { get; set; }
        public string VenderEmail { get; set; }
    }
}
