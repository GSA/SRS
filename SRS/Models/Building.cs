using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Models
{
    public class Building
    {
        public string BuildingLocationCode { get; set; }
        public object BuildingNumber { get; internal set; }
    }
}
