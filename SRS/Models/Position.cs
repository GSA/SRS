using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Models
{
    public class Position
    {
        public string JobTitle { get; set; }
        public string Region { get; set; }
        public bool IsVirtual { get; set; }
        public bool VirtualRegion { get; set; }
        public string OfficeSymbol { get; set; }
        public string MajorOrg { get; set; }
        public object PositionControlNumber { get; internal set; }
        public object PositionOrganization { get; internal set; }
        public object SupervisoryStatus { get; internal set; }
        public object PositionSensitivity { get; internal set; }
        public object PayPlan { get; internal set; }
        public object DutyLocationCity { get; internal set; }
        public object DutyLocationCode { get; internal set; }
        public string DutyLocationState { get; internal set; }
        public object DutyLocationCounty { get; internal set; }
        public object PositionStartDate { get; internal set; }
        public object AgencyCodeSubelement { get; internal set; }
        public object WorkSchedule { get; internal set; }
        public object PayGrade { get; internal set; }
        public object JobSeries { get; internal set; }
    }
}

